using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MangoPublishPackage.Forms
{
    public partial class FormBuildAndPublish : Form
    {
        int _buildFailedCount = 0;
        string _buildLog = "";
        private List<FileInfo> _batFiles = null;
        public FormBuildAndPublish()
        {
            InitializeComponent();
        }

        delegate void _dlgtFunc_UpdateFormControl(object sender, string message);
        delegate void _dlgtFunc_UpdateFormControl1(object sender, List<string> message);


        /// <summary>
        /// 打开控制台执行拼接完成的批处理命令字符串
        /// </summary>
        /// <param name="inputAction">需要执行的命令委托方法：每次调用 <paramref name="inputAction"/> 中的参数都会执行一次</param>
        void ExecBatCommand(Action<Action<string>> inputAction)
        {
            Process pro = null;
            StreamWriter sIn = null;
            StreamReader sOut = null;

            try
            {
                pro = new Process();
                pro.StartInfo.FileName = "cmd.exe";
                pro.StartInfo.UseShellExecute = false;
                pro.StartInfo.CreateNoWindow = true;
                pro.StartInfo.RedirectStandardInput = true;
                pro.StartInfo.RedirectStandardOutput = true;
                pro.StartInfo.RedirectStandardError = true;

                pro.OutputDataReceived += (sender, e) => { BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), listBox_logs, e.Data); };
                pro.ErrorDataReceived += (sender, e) => { BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), listBox_logs, e.Data); };

                pro.Start();
                sIn = pro.StandardInput;
                sIn.AutoFlush = true;
                
                pro.BeginOutputReadLine();
                inputAction(value => { sIn.WriteLine(value); });

                pro.WaitForExit();
            }
            finally
            {
                if (pro != null && !pro.HasExited)
                    pro.Kill();

                if (sIn != null)
                    sIn.Close();
                if (sOut != null)
                    sOut.Close();
                if (pro != null)
                    pro.Close();
            }
        }

        protected void ThreadFunc_BuildAndPublish(object arg)
        {
            try
            {
                _buildLog = "";
                _buildFailedCount = 0;
                string batContent = "";
                using (FileStream fs = new FileStream(arg.ToString(), FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        batContent = sr.ReadToEnd();
                        BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdatetbBatContent), tbBatContent, batContent);
                        sr.Close();
                    }
                    fs.Close();
                }

                List<string> cmdLines = batContent.Split('\r', '\n').ToList();
                cmdLines.RemoveAll(cmd => string.IsNullOrEmpty(cmd));
                for(int i =0; i< cmdLines.Count;i++)
                {
                        int tmpIdx = cmdLines[i].IndexOf('>');
                    if (-1 != tmpIdx)
                    {
                        cmdLines[i] = cmdLines[i].Substring(0, tmpIdx);
                        continue;
                    }
                    tmpIdx = cmdLines[i].IndexOf("/OUT");
                    if (-1 != tmpIdx)
                    {
                        cmdLines[i] = cmdLines[i].Substring(0, tmpIdx);
                        continue;
                    }
                }

                if ("pause" == cmdLines.Last().ToLower()) cmdLines.RemoveAt(cmdLines.Count - 1);
                cmdLines.Add("exit 0");
                batContent = string.Join("\r\n", cmdLines);

                ExecBatCommand(p => { foreach (string cmd in cmdLines) { p(cmd); } p(@"exit 0"); });

                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateToolstripStatusLabel), toolStripStatusLabel_msg, string.Format("编译项目完成"));

                MessageBox.Show(string.Format("编译完成, 失败个数({0})\r\n编译输出: \r\n{1}", _buildFailedCount, _buildLog));
            }
            catch (Exception ex)
            {
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateToolstripStatusLabel), toolStripStatusLabel_msg, string.Format("编译项目发生异常: {0}", ex.Message));
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), listBox_logs, string.Format("编译项目异常调用堆栈: {0}", ex.StackTrace));
                return;
            }
        }

        protected void ThreadFunc_Publish(object arg)
        {
            FileInfo fiCsproj = arg as FileInfo;
            DirectoryInfo diTargetRoot = new DirectoryInfo(fiCsproj.Name.Contains("Git.WMS.Web") ? tbPublishPath_Web.Text : tbPublishPath_Api.Text);

            if (diTargetRoot.Exists)
            {
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, "开始备份源: " + diTargetRoot.FullName);
                Backup(diTargetRoot);
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, "备份源完成: " + diTargetRoot.FullName);
            }
            else
            {
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, "发布目标目录不存在: " + diTargetRoot.FullName);
                return;
            }


            List<string> pubFileList = GetPubFiles(fiCsproj);
            if (!pubFileList.EnumerableAny())
            {
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, "源文件不包含任何元素: " + fiCsproj.FullName);
                return;
            }
            else
            {
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdatetbBatContent), null, string.Format("发布源[{0}], 数量[{1}]:\r\n{2}", arg, pubFileList.Count, string.Join("\r\n", pubFileList)));
            }

            // package files
            foreach (string f in pubFileList)
            {
                try
                {
                    string tmpsource = f.Replace("%40", "@");
                    int tmpindex = tmpsource.IndexOf(fiCsproj.DirectoryName);
                    string tarfile = diTargetRoot.FullName + "\\" + tmpsource.Substring(tmpindex + fiCsproj.DirectoryName.Length + 1);
                    tmpindex = tarfile.LastIndexOf('\\');
                    if (!Directory.Exists(tarfile.Substring(0, tmpindex)))
                        Directory.CreateDirectory(tarfile.Substring(0, tmpindex));

                    File.Copy(tmpsource, tarfile);

                    using (FileStream fs = new FileStream(tarfile, FileMode.Open)) { fs.Close(); }
                    BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, "发布文件: " + tarfile);
                }
                catch (Exception ex)
                {
                    BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, string.Format("!ERROR, 发布文件[{0}]异常: {1}", f, ex.Message));
                }
            }

            BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, string.Format("发布[{0}]完成", arg));
        }

        void DlgtFunc_UpdateComboBoxBatFiles(object sender, string message)
        {
            if (null == comboBoxBatFile)
            {
                toolStripStatusLabel_msg.Text = "ComboBox控件[BAT Files]空引用";
                throw new NullReferenceException(toolStripStatusLabel_msg.Text);
            }
            else
                comboBoxBatFile.Items.Add(message);
        }

        void DlgtFunc_UpdateToolstripStatusLabel(object sender, string message)
        {
            toolStripStatusLabel_msg.Text = message;
        }

        void DlgtFunc_UpdateInfoList(object sender, string message)
        {
            if (null == message)
                return;

            if (message.Contains("，失败 ") && message.Contains("，失败 1 个")) 
            {
                toolStripStatusLabel_msg.Text = string.Format("编译错误, 失败个数: {0}", ++_buildFailedCount);
            }

            _buildLog += message + "\r\n";
            listBox_logs.Items.Add(message);
        }

        void DlgtFunc_UpdatetbBatContent(object sender, string message)
        {
            tbBatContent.Text = message;
        }

        void DlgtFunc_UpdatetbBatContentAppend(object sender, string message)
        {
            tbBatContent.Text += message;
        }

        void DlgtFunc_UpdateInfoListRange(object sender, List<string> message)
        {
            //lock (listBox_logs) {
            //    foreach (string msg in message)
            //    {
            //        listBox_logs.Items.Add(i);
            //    }
            //    //message.ForEach(i => listBox_logs.Items.Add(i));
            //}
        }

        protected void GetBatFiles(string pRoot)
        {
            List<FileInfo> retList = null;
            Utility.FindFile(pRoot, "*.bat", out retList);
            if (retList.EnumerableAny())
            {
                _batFiles = retList.ToList();
                _batFiles.ForEach(f =>
                {
                    BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateComboBoxBatFiles), comboBoxBatFile, f.FullName);
                });
            }
            else
            {
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateToolstripStatusLabel), null, string.Format("没有在根路径[{0}]下找到任何批处理文件", pRoot));
            }
            BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateToolstripStatusLabel), null, string.Format("在根路径[{0}]下搜索批处理文件完成, 共({1})个", pRoot, _batFiles.Count));
        }

        protected void Backup(DirectoryInfo pSourceDir)
        {
            string backupDir = string.Format("{0}\\backup\\{1}\\{2}", pSourceDir.Parent.FullName, DateTime.Now.ToString("yyyyMMdd_hhmmss"), pSourceDir.Name);
            DirectoryInfo diBackup = new DirectoryInfo(backupDir);
            diBackup.Create();

            int count = CopyDirsAndFiles(pSourceDir, diBackup);
            BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, string.Format("备份目标路径: {0}", diBackup?.FullName));
            BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, string.Format("备份[{0}]文件数量: {1}", pSourceDir?.FullName, count));

            ClearDir(pSourceDir);
            BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, string.Format("清除[{0}]文件数量: {1}", pSourceDir?.FullName, count));
        }

        protected int CopyDirsAndFiles(DirectoryInfo pSourceDir, DirectoryInfo pDestDir)
        {
            int retCount = 0;
            try
            {
                List<DirectoryInfo> subdirs = pSourceDir.GetDirectories().ToList();
                List<FileInfo> subFiles = pSourceDir.GetFiles().ToList();

                // except "writedir"
                DirectoryInfo diWritedir = subdirs.Find(d => d.Name.Equals("writedir", StringComparison.OrdinalIgnoreCase));
                if (null != diWritedir) subdirs.Remove(diWritedir);

                foreach (FileInfo file in subFiles)
                {
                    using (FileStream fs = new FileStream(file.FullName, FileMode.Open))
                    {
                        string destFile = pDestDir.FullName + "\\" + file.Name;
                        File.Copy(file.FullName, destFile);
                        using (FileStream destFs = new FileStream(destFile, FileMode.Open)) { destFs.Close(); }
                        fs.Close();
                    }
                    retCount++;
                }

                foreach (DirectoryInfo dir in subdirs)
                {
                    string subDirDest = pDestDir.FullName + "\\" + dir.Name + "\\";
                    DirectoryInfo diSubDirDest = new DirectoryInfo(subDirDest);
                    diSubDirDest.Create();
                    retCount += CopyDirsAndFiles(dir, diSubDirDest);
                }
            }
            catch (Exception ex)
            {
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, string.Format("ERROR, 拷贝目录发生异常, 源: {0}", pSourceDir?.FullName));
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, string.Format("ERROR, 拷贝目录发生异常, 目标: {0}", pDestDir?.FullName));
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, string.Format("ERROR, 拷贝目录发生异常, 异常信息: {0}", ex.Message));
            }
            return retCount;
        }

        protected int ClearDir(DirectoryInfo pSourceDir)
        {
            int retCount = 0;
            try
            {
                List<DirectoryInfo> subdirs = pSourceDir.GetDirectories().ToList();
                List<FileInfo> subFiles = pSourceDir.GetFiles().ToList();

                // except "writedir"
                DirectoryInfo diWritedir = subdirs.Find(d => d.Name.Equals("writedir", StringComparison.OrdinalIgnoreCase));
                if (null != diWritedir) subdirs.Remove(diWritedir);

                foreach (FileInfo file in subFiles)
                {
                    file.Delete();
                    retCount++;
                }

                foreach (DirectoryInfo dir in subdirs)
                {
                    retCount += ClearDir(dir);
                    dir.Delete();
                }
            }
            catch (Exception ex)
            {
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, string.Format("ERROR, 清除目录发生异常, 源: {0}", pSourceDir?.FullName));
                BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), null, string.Format("ERROR, 清除目录发生异常, 异常信息: {0}", ex.Message));
            }
            return retCount;
        }

        public List<string> GetPubFiles(FileInfo pCsproj)
        {
            if (null == pCsproj || !File.Exists(pCsproj.FullName))
            {
                //_logList.Add("csproj文件不存在: " + pCsproj.FullName);
                return null;
            }

            List<string> retList = new List<string>();

            // read from .csproject
            string csproj_content = "";
            using (FileStream fs = pCsproj.OpenRead())
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    csproj_content = sr.ReadToEnd();
                }
                fs.Close();
            }
            
            // get package file list

            // files of <Content Include=""/>
            Regex reg = new Regex("Content Include=(.*?)>", RegexOptions.IgnoreCase); // 搜索匹配的字符串
            var list1 = reg.Matches(csproj_content);
            //var files = new List<string>();
            foreach (var item in list1)
            {
                var file = pCsproj.DirectoryName + "\\" + item.ToString().Replace("Content Include=", "").Replace("/>", "").Replace(">", "").Replace(@"""", "").Trim();
                retList.Add(file);

            }

            // files in folder of '.\bin\'
            string binpath = pCsproj.DirectoryName + "\\bin\\";
            DirectoryInfo dibin = new DirectoryInfo(binpath);
            IEnumerable<string> binfiles = dibin.GetFiles("*.dll", SearchOption.AllDirectories).Select(fi => fi.FullName);
            retList.AddRange(binfiles);
            
            binfiles = dibin.GetFiles("*.pdb", SearchOption.AllDirectories).Select(fi => fi.FullName);
            retList.AddRange(binfiles);

            binfiles = dibin.GetFiles("*.xml", SearchOption.AllDirectories).Select(fi => fi.FullName);
            retList.AddRange(binfiles);

            //foreach (string li in retList) _logList.Add(li);
            BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdatetbBatContentAppend), null, (pCsproj.Name + ", 打包文件数量: " + retList.Count + "\r\n"));

            return retList;
        }

        private void FormBuildAndPublish_Load(object sender, EventArgs e)
        {
            tbPublishPath_Api.Text = @"\\192.168.3.101\webdir\8005.wms.api.simdev";
            tbPublishPath_Web.Text = @"\\192.168.3.101\webdir\8006.wms.web.simdev";
            tbRoot.Text = @"E:\liuzy\documents\vs2017\tfs\mangoWMS";
            _batFiles = new List<FileInfo>(3);
            toolStripStatusLabel_msg.Text = "";
            comboBoxBatFile.Items.Clear();
            Task.Factory.StartNew(() => { GetBatFiles(fbdBatRoot.SelectedPath); });
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string batFilePath = comboBoxBatFile.SelectedItem as string;
            if (!File.Exists(batFilePath))
            {
                toolStripStatusLabel_msg.Text = string.Format("!!批处理文件不存在: {0}", batFilePath);
            }
            else
            {
                Thread th = new Thread(new ParameterizedThreadStart(ThreadFunc_BuildAndPublish));
                th.Start(batFilePath);
            }
        }

        private void btnSelectRoot_Click(object sender, EventArgs e)
        {
            fbdBatRoot.Description = string.Format("选择批处理文件的根路径: {0}", tbRoot.Text);
            DialogResult dr = fbdBatRoot.ShowDialog(this);
            if (DialogResult.OK == dr)
            {
                tbRoot.Text = fbdBatRoot.SelectedPath;
                comboBoxBatFile.Items.Clear();
                Task.Factory.StartNew(() => { GetBatFiles(fbdBatRoot.SelectedPath); } );
            }
        }

        private void FormBuildAndPublish_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (null != _batFiles)
            {
                _batFiles.Clear();
            }
            //if (null != _logList)
            //{
            //    _logList.Clear();
            //}
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            tbBatContent.Text = "";
            listBox_logs.Items.Clear();

            #region 获取当前批处理文件的TFS路径.
            const string TFS_FLAG = "SET TFS_PATH=";
            string tfsPath = "";
            string batContent = "";
            using (FileStream fs = new FileStream(comboBoxBatFile.SelectedItem as string, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    batContent = sr.ReadToEnd();
                }
            }

            // find TFS path:
            int tmpIndex = batContent.IndexOf(TFS_FLAG);
            if (-1 == tmpIndex)
            {
                toolStripStatusLabel_msg.Text = string.Format("![ERROR Publis], 无法根据批处理文件({0})找到TFS本地路径.", comboBoxBatFile.SelectedItem);
                return;
            }
            else
            {
                tfsPath = batContent.Substring(tmpIndex + TFS_FLAG.Length);
                tmpIndex = tfsPath.IndexOf("\r\n");
                if (-1 == tmpIndex)
                {
                    toolStripStatusLabel_msg.Text = string.Format("![ERROR Publis], 无法根据批处理文件({0})找到TFS本地路径.", comboBoxBatFile.SelectedItem);
                    return;
                }
                else
                {
                    tfsPath = tfsPath.Substring(0, tmpIndex);
                }
            }
            #endregion // 获取当前批处理文件的TFS路径

            DirectoryInfo diTfs = new DirectoryInfo(tfsPath);
            if (!diTfs.Exists)
            {
                toolStripStatusLabel_msg.Text = string.Format("![ERROR Publis], 非法的TFS路径: {0}.", tfsPath);
                return;
            }

            #region find file of .csproj of Git.WMS.API
            {
                DirectoryInfo diProjPath = diTfs.GetDirectories("Git.WMS.Web*")[0].GetDirectories("Git.WMS.API")[0];
                FileInfo fiApiCsproj = diProjPath.GetFiles("Git.WMS.API.*csproj")[0];
                Thread th = new Thread(new ParameterizedThreadStart(ThreadFunc_Publish));
                th.Start(fiApiCsproj);
            }
            #endregion // find file of .csproj of Git.WMS.API
            #region find file of .csproj of Git.WMS.Web
            {
                DirectoryInfo diProjPath = diTfs.GetDirectories("Git.WMS.Web*")[0].GetDirectories("Git.WMS.Web")[0];
                FileInfo fiWebCsproj = diProjPath.GetFiles("Git.WMS.Web.*csproj")[0];
                Thread th = new Thread(new ParameterizedThreadStart(ThreadFunc_Publish));
                th.Start(fiWebCsproj);
            }
            #endregion // find file of .csproj of Git.WMS.Web
        }

        private void btnSelectPublishPath_API_Click(object sender, EventArgs e)
        {
            fbdPubApi.Description = string.Format("请选择发布API的目标路径: {0}", tbPublishPath_Api.Text);
            fbdPubApi.SelectedPath = tbPublishPath_Api.Text;
            DialogResult dr = fbdPubApi.ShowDialog(this);
            if (DialogResult.OK == dr)
            {
                tbPublishPath_Api.Text = fbdPubApi.SelectedPath;
            }
        }

        private void btnSelectPublishPath_Web_Click(object sender, EventArgs e)
        {
            fbdPubWeb.Description = string.Format("请选择发布API的目标路径: {0}", tbPublishPath_Web.Text);
            fbdPubWeb.SelectedPath = tbPublishPath_Web.Text;
            DialogResult dr = fbdPubWeb.ShowDialog(this);
            if (DialogResult.OK == dr)
            {
                tbPublishPath_Web.Text = fbdPubWeb.SelectedPath;
            }
        }
    }    
}
