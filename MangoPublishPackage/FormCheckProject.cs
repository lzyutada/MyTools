using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MangoPublishPackage.Configs;
using MangoPublishPackage.WorkFlow;

namespace MangoPublishPackage
{
    public partial class FormCheckProject : FormWorkFlowBase
    {
        public FormCheckProject()
        {
            InitializeComponent();
        }

        protected void HandleTaskBegin(object sender, ITaskItem task)
        {
            tbOutput.Text += "> 任务[" + task.GetType().Name + "]开始:\r\n";
        }

        protected void HandleTaskFinished(object sender, ITaskItem task, TTaskResult Result)
        {
            tbOutput.Text += " 运行结果: " + Result + "\r\n";
            tbOutput.Text += " " + task.ToString() + "\r\n";

            try
            {
                if ((task is TaskItem_CheckProj_CheckCsprojFiles))
                {
                    if ((task as TaskItem_CheckProj_CheckFiles).DismatchList.EnumerableAny())
                    {
                        (task as TaskItem_CheckProj_CheckFiles).DismatchList.ForEach(f => {
                            if (!string.IsNullOrEmpty(f?.PathMainline?.FullName))
                                listBoxMainlineCsproj.Items.Add(f?.PathMainline?.FullName);
                        });
                        (task as TaskItem_CheckProj_CheckFiles).DismatchList.ForEach(f => {
                            if (!string.IsNullOrEmpty(f?.PathBranch?.FullName))
                                listBoxBranchCsproj.Items.Add(f.PathBranch?.FullName);
                        });
                    }
                }
                else if ((task is TaskItem_CheckProj_CheckFiles))
                {
                    if ((task as TaskItem_CheckProj_CheckFiles).DismatchList.EnumerableAny())
                    {
                        (task as TaskItem_CheckProj_CheckFiles).DismatchList.ForEach(f => { 
                            if (!string.IsNullOrEmpty(f?.PathMainline?.FullName))
                                listBoxMainlineFiles.Items.Add(f?.PathMainline?.FullName);
                        });
                        (task as TaskItem_CheckProj_CheckFiles).DismatchList.ForEach(f => {
                            if (!string.IsNullOrEmpty(f?.PathBranch?.FullName))
                                listBoxBranchFiles.Items.Add(f.PathBranch?.FullName);
                        });
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                tbOutput.Text += "\r\n";
                tbOutput.Text += " 处理任务结果发生异常: " + ex.Message + "\r\n";
                tbOutput.Text += " 调用堆栈: " + ex.StackTrace + "\r\n";
            }

            tbOutput.Text += "> 任务[" + task.GetType().Name + "]结束\r\n\r\n";
        }

        private void FormCheckProject_Load(object sender, EventArgs e)
        {
            #region init contorl displaying.
            // select developer
            comboBox1.SelectedItem = (string.IsNullOrEmpty(MyConfig.Instance.Setting.CHECKPRO_TEAMDEVELOPER) ? "刘子煜" : MyConfig.Instance.Setting.CHECKPRO_TEAMDEVELOPER);

            // select TFS path
            if (Directory.Exists(MyConfig.Instance.Setting.CHECKPROJ_TFSMAPPING))
            {
                fbdTfsMapping.SelectedPath = tbTfsMapping.Text = MyConfig.Instance.Setting.CHECKPROJ_TFSMAPPING;
            }
            else if (Directory.Exists(tbTfsMapping.Text))
            {
                fbdTfsMapping.SelectedPath = MyConfig.Instance.Setting.CHECKPROJ_TFSMAPPING = tbTfsMapping.Text;
            }
            else
            {
                fbdTfsMapping.SelectedPath = MyConfig.Instance.Setting.CHECKPROJ_TFSMAPPING = tbTfsMapping.Text = @"E:\liuzy\documents\vs2017\tfs\mangoWMS\";
            }

            // select BeyondCompare path.
            if (Directory.Exists(MyConfig.Instance.Setting.CHECKPRO_BEYONDCOMPARE_PATH))
            {
                fbdBCPath.SelectedPath = tbBCPath.Text = MyConfig.Instance.Setting.CHECKPRO_BEYONDCOMPARE_PATH;
            }
            else if (Directory.Exists(tbBCPath.Text))
            {
                fbdBCPath.SelectedPath = MyConfig.Instance.Setting.CHECKPRO_BEYONDCOMPARE_PATH = tbBCPath.Text;
            }
            else
            {
                fbdBCPath.SelectedPath = MyConfig.Instance.Setting.CHECKPRO_BEYONDCOMPARE_PATH = tbBCPath.Text = @"E:\Program Files (x86)\Beyond Compare";
            }
            #endregion // init contorl displaying.

            // set handler for notification of task item events.
            SetHandleWorkTaskBegin(HandleTaskBegin);
            SetHandleWorkTaskFinished(HandleTaskFinished);
        }

        private void FormCheckProject_FormClosed(object sender, FormClosedEventArgs e)
        {
            // save settings
            if (null != MyConfig.Instance)
            {
                MyConfig.Instance.Setting.CHECKPRO_TEAMDEVELOPER = comboBox1.SelectedItem.ToString();
                MyConfig.Instance.Setting.CHECKPROJ_TFSMAPPING = fbdTfsMapping.SelectedPath;
                MyConfig.Instance.Setting.CHECKPRO_BEYONDCOMPARE_PATH = fbdBCPath.SelectedPath;
                MyConfig.Instance.Save();
            }
        }

        private void btnTfsMapping_Click(object sender, EventArgs e)
        {
            fbdTfsMapping.SelectedPath = (Directory.Exists(tbTfsMapping.Text)) ? tbTfsMapping.Text : MyConfig.Instance.Setting.CHECKPRO_TEAMDEVELOPER;
            DialogResult dr = fbdTfsMapping.ShowDialog(this);
            if (DialogResult.OK == dr)
            {
                // save settings
                MyConfig.Instance.Setting.CHECKPROJ_TFSMAPPING = tbTfsMapping.Text = fbdTfsMapping.SelectedPath;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyConfig.Instance.SetTeamDeveloper(comboBox1.SelectedText);
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            tbOutput.Text = "";
            listBoxBranchFiles.Items.Clear();
            listBoxMainlineFiles.Items.Clear();
            listBoxBranchCsproj.Items.Clear();
            listBoxMainlineCsproj.Items.Clear();

            #region register workflow items.
            AddTask(new TaskItem_CheckProj_CheckDeveloper(comboBox1.SelectedItem.ToString())); // check developer
            AddTask(new TaskItem_CheckProj_CheckTfsMapping(tbTfsMapping.Text)); // check TFS mapping

            var taskGetMainlineFiles = new TaskItem_CheckProj_GetMainlineFiles(tbTfsMapping.Text);
            AddTask(taskGetMainlineFiles); // search for all files in mainline

            var taskGetBranchFiles = new TaskItem_CheckProj_GetBranchFiles(tbTfsMapping.Text, comboBox1.SelectedItem.ToString());
            AddTask(taskGetBranchFiles); // search for all files in branches

            // comparing of project files(except csproj, \bin, \obj, \writedir, \UploadFile, \.vs, \data_dll
            AddTask(new TaskItem_CheckProj_CheckFiles(taskGetMainlineFiles, taskGetBranchFiles));

            // comparing of csproj files.
            AddTask(new TaskItem_CheckProj_CheckCsprojFiles(taskGetMainlineFiles, taskGetBranchFiles));
            #endregion // register workflow items.

            StartWorkFlow();
        }

        private void listBoxMainlineFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var task = new TaskItem_CheckProj_GetBranchFiles(tbTfsMapping.Text, comboBox1.SelectedItem.ToString()))
            {
                string bcFile = listBoxMainlineFiles.SelectedItem.ToString().Substring(tbTfsMapping.Text.Length+1); // $mango_WMS/Git.WMS.Web/xxxx -or- $mango_WMS/Git.Framework/xxxx
                string ModuleName = bcFile.Substring(0, bcFile.IndexOf("\\")); // "Git.WMS.Web" -or- "Git.Framework"
                string RelativePath = bcFile.Substring(bcFile.IndexOf("\\") + 1); // rest descriptor without "Git.WMS.Web" -or- "Git.Framework"
                string bcFileRight = tbTfsMapping.Text + task.DeveloperDirGroup + ModuleName + ".branch\\" + RelativePath;

                int selIndex = listBoxBranchFiles.Items.IndexOf(bcFileRight);
                if (0 <= selIndex) listBoxBranchFiles.SetSelected(selIndex, true);
            }
        }

        private void listBoxBranchFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var task = new TaskItem_CheckProj_GetBranchFiles(tbTfsMapping.Text, comboBox1.SelectedItem.ToString()))
            {
                // [$mango_WMS/branch/dev/???/]Git.WMS.Web.branch/xxxx -or- [$mango_WMS/branch/dev/???/]Git.Framework.branch/xxxx
                string bcFile = listBoxBranchFiles.SelectedItem.ToString().Substring(tbTfsMapping.Text.Length + task.DeveloperDirGroup.Length + 1);

                string ModuleName = bcFile.Substring(0, bcFile.IndexOf("\\")).Replace(".branch", ""); // "Git.WMS.Web.branch" -or- "Git.Framework.branch"
                string RelativePath = bcFile.Substring(bcFile.IndexOf("\\") + 1); // rest descriptor without "Git.WMS.Web.branch" -or- "Git.Framework.branch"
                string bcFileLeft = tbTfsMapping.Text + ModuleName + RelativePath;

                int selIndex = listBoxMainlineFiles.Items.IndexOf(bcFileLeft);
                if (0 <= selIndex) listBoxMainlineFiles.SetSelected(selIndex, true);
            }
        }
        
        void CompareByBeyondCompare() // string left, string right)
        {
            FileInfo fiBC = new FileInfo(tbBCPath.Text + @"\BCompare.exe");
            if (fiBC.Exists && null != listBoxMainlineFiles.SelectedItem && null != listBoxBranchFiles.SelectedItem)
            {
                string left = listBoxMainlineFiles.SelectedItem.ToString();
                string right = listBoxBranchFiles.SelectedItem.ToString();
                System.Diagnostics.Process.Start(fiBC.FullName, left + " " + right);
            }
        }

        private void listBoxMainlineFiles_MouseDoubleClick(object sender, MouseEventArgs e){CompareByBeyondCompare();}

        private void listBoxBranchFiles_MouseDoubleClick(object sender, MouseEventArgs e){CompareByBeyondCompare();}

        private void listBoxMainlineCsproj_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var task = new TaskItem_CheckProj_GetBranchFiles(tbTfsMapping.Text, comboBox1.SelectedItem.ToString()))
            {
                FileInfo fiBcLeft = new FileInfo(listBoxMainlineCsproj.SelectedItem.ToString());
                string bcFileRight = task.TfsMapping + task.DeveloperDirGroup + fiBcLeft.Directory.Parent.Name + ".branch\\" + fiBcLeft.Directory.Name + "\\" + fiBcLeft.Directory.Name + ".branch." + task.Developer + fiBcLeft.Extension;

                int selIndex = listBoxBranchCsproj.Items.IndexOf(bcFileRight);
                if (0 <= selIndex) listBoxBranchCsproj.SetSelected(selIndex, true);
            }
        }

        private void listBoxMainlineCsproj_MouseDoubleClick(object sender, MouseEventArgs e){ CompareCsprojByBeyondCompare();}

        private void listBoxBranchCsproj_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var task = new TaskItem_CheckProj_GetBranchFiles(tbTfsMapping.Text, comboBox1.SelectedItem.ToString()))
            {
                FileInfo fiBcRight = new FileInfo(listBoxBranchCsproj.SelectedItem.ToString());
                string bcFileRight = tbTfsMapping.Text + fiBcRight.Directory.Parent.Name.Replace(".branch", "") + "\\" + fiBcRight.Directory.Name + "\\" + fiBcRight.Name;

                int selIndex = listBoxBranchCsproj.Items.IndexOf(bcFileRight);
                if (0 <= selIndex) listBoxBranchCsproj.SetSelected(selIndex, true);
            }
        }

        private void listBoxBranchCsproj_MouseDoubleClick(object sender, MouseEventArgs e){ CompareCsprojByBeyondCompare();}

        void CompareCsprojByBeyondCompare() // string left, string right)
        {
            FileInfo fiBC = new FileInfo(tbBCPath.Text + @"\BCompare.exe");
            if (fiBC.Exists)
            {
                string left = listBoxMainlineCsproj.SelectedItem.ToString();
                string right = listBoxBranchCsproj.SelectedItem.ToString();
                System.Diagnostics.Process.Start(fiBC.FullName, left + " " + right);
            }
        }
    }

    class TaskItem_CheckProj_CheckDeveloper : ITaskItem
    {
        string[] TeamMember = new string[] { "常晟", "董天一", "曹诗雨", "刘子煜" };
        string Developer = "";

        public TaskItem_CheckProj_CheckDeveloper(string pDeveloper)
        {
            Developer = pDeveloper;
        }

        public int Start() { return 1; }
        public TTaskResult Finished() { return (TeamMember.Contains(Developer)) ? TTaskResult.ESuccess : TTaskResult.EFailed; }

        public override string ToString()
        {
            return "[TaskItem_CheckProj_CheckDeveloper("+ Developer + ")]";
        }
    }

    class TaskItem_CheckProj_CheckTfsMapping : ITaskItem
    {
        string TfsMapping = "";

        public TaskItem_CheckProj_CheckTfsMapping(string pMapping) {TfsMapping = pMapping;}

        public override string ToString()
        {
            return "[TaskItem_CheckProj_CheckTfsMapping(" + TfsMapping + ")]";
        }

        public int Start() { return 1; }
        public TTaskResult Finished() { return (new DirectoryInfo(TfsMapping).Exists) ? TTaskResult.ESuccess : TTaskResult.EFailed; }
    }

    class TaskItem_CheckProj_GetMainlineFiles : ITaskItem, IDisposable
    {
        public string TfsMapping { get; private set; }
        public List<FileInfo> Files { get; private set; }

        public TaskItem_CheckProj_GetMainlineFiles(string pTfsMapping){TfsMapping = pTfsMapping;}

        public void Dispose(){if (null != Files) Files.Clear();}

        public override string ToString()
        {
            return string.Format("[TaskItem_CheckProj_GetMainlineFiles(\r\nTfsMapping: {0}\r\nFiles?.Count: {1})]", TfsMapping, Files?.Count);
        }
        virtual public int Start()
        {
            GetTfsFiles(TfsMapping + "\\Git.WMS.Web\\", TfsMapping + "\\Git.Framework\\");
            return 1;
        }

        virtual public TTaskResult Finished() { return (!Files.EnumerableAny()) ? TTaskResult.EFailed : TTaskResult.ESuccess; }

        protected void GetTfsFiles(string PathWeb, string PathFramework)
        {
            string[] IgnoreFolders = new string[] { "bin", "obj", "writedir", "data_dll", "UploadFile", ".vs", "PublishProfiles" , "packages" };
            string[] IgnoreFiles = new string[] { "packages.config" };
            DirectoryInfo diGitWeb = new DirectoryInfo(PathWeb);
            DirectoryInfo diGitFramework = new DirectoryInfo(PathFramework);

            // TODO: skip folders, bin, obj, writedir, data_dll, UploadFile, .vs, \PublishProfiles
            List<FileInfo> files = null;
            if (diGitWeb.Exists)
            {
                Utility.FindFile(diGitWeb.FullName
                    , "*"
                    , IgnoreFolders
                    , IgnoreFiles
                    , out files);
                if (files.EnumerableAny())
                {
                    if (null == Files) Files = new List<FileInfo>(files.Count);
                    Files.AddRange(files);
                    if (null != files) files.Clear();
                }
            }
            if (diGitFramework.Exists)
            {
                Utility.FindFile(diGitFramework.FullName
                    , "*"
                    , IgnoreFolders
                    , IgnoreFiles
                    , out files);
                if (files.EnumerableAny())
                {
                    if (null == Files) Files = new List<FileInfo>(files.Count);
                    Files.AddRange(files);
                    if (null != files) files.Clear();
                }
            }
        }
    }

    class TaskItem_CheckProj_GetBranchFiles : TaskItem_CheckProj_GetMainlineFiles
    {
        public string Developer { get; set; }
        public string DeveloperDirGroup { get; protected set; }
        public TaskItem_CheckProj_GetBranchFiles(string pTfsMapping, string pDeveloper) : base(pTfsMapping)
        {
            switch (pDeveloper)
            {
                case "曹诗雨": { Developer = "caoshiyu"; break; }
                case "常晟": { Developer = "changsheng"; break; }
                case "董天一": { Developer = "dongtianyi"; break; }
                case "刘子煜": { Developer = "liuziyu"; break; }
                default: { Developer = ""; break; }
            }

            DeveloperDirGroup = (string.IsNullOrEmpty(Developer)) ? "" : string.Format("\\branch\\dev\\{0}\\", Developer);
        }

        public override string ToString()
        {
            return string.Format("[TaskItem_CheckProj_GetBranchFiles(\r\nTfsMapping: {0}\r\nFiles?.Count: {1})\r\nDeveloper: {2}\r\nDeveloperDirGroup: {3}]", TfsMapping, Files?.Count, Developer, DeveloperDirGroup);
        }

        override public int Start()
        {
            int err = -1;
            if (!string.IsNullOrEmpty(Developer))
            {
                GetTfsFiles(
                    TfsMapping + DeveloperDirGroup + "\\Git.WMS.Web.branch\\"
                    , TfsMapping + DeveloperDirGroup + "\\Git.Framework.branch\\"
                    );
                err = 1;
            }
            return err;
        }
    }

    class TaskItem_CheckProj_CheckFiles : ITaskItem, IDisposable
    {
        public class CompareResult : IDisposable
        {
            public string TfsMapping { get; set; }
            public string Developer { get; set; }

            public FileInfo PathMainline { get; set; }
            public FileInfo PathBranch { get; set; }
            public long Equal { get { return Compare(); } }
            
            public void Dispose()
            {
            }

            public override string ToString()
            {
                return string.Format("[CompareResult(\r\nTfsMapping: {0}\r\nDeveloper: {1})\r\nPathMainline: {2}\r\nPathBranch: {3}\r\nEqual: {4}]", TfsMapping, Developer, PathMainline, PathBranch, Equal);
            }

            long Compare()
            {
                if (null == PathMainline || null == PathBranch)
                    return -1;
                else
                {
                    string mRelPath = GetRelativePath(PathMainline.FullName, TfsMapping);
                    string bRelPath = GetRelativePath(PathBranch.FullName, TfsMapping + "\\branch\\dev\\" + Developer);
                    bRelPath = bRelPath.Replace(".branch", "");
                    bRelPath = bRelPath.Replace("." + Developer, "");

                    if (0 != mRelPath.ToLower().CompareTo(bRelPath.ToLower()))
                    {
                        throw new Exception(string.Format("对比文件不匹配.\r\nleft: {0}\r\nright: {1}", mRelPath, bRelPath));
                    }
                    else
                    {
                        byte[] ContentMainline = null;
                        byte[] ContentBranch = null;
                        Utility.ReadFile(PathMainline?.FullName, out ContentMainline);
                        Utility.ReadFile(PathBranch?.FullName, out ContentBranch);
                        long ret = Utility.CompareBytes(ContentMainline, ContentBranch); // compare content.
                        return ret;
                    }
                }
            }
            public string GetRelativePath(string pFullPath, string pRootPath)
            {
                return (!string.IsNullOrEmpty(pFullPath) && !string.IsNullOrEmpty(pRootPath) && 0 == pFullPath.IndexOf(pRootPath)) ? pFullPath.Substring(pRootPath.Length) : "";
            }
        } // class CompareResult

        protected TaskItem_CheckProj_GetMainlineFiles PrevTaskMainline = null;
        protected TaskItem_CheckProj_GetBranchFiles PrevTaskBranch = null;
        public List<CompareResult> ResultList { get; protected set; }
        public List<CompareResult> DismatchList { get; protected set; }

        public TaskItem_CheckProj_CheckFiles(TaskItem_CheckProj_GetMainlineFiles pTask1, TaskItem_CheckProj_GetBranchFiles pTask2)
        {
            PrevTaskMainline = pTask1;
            PrevTaskBranch = pTask2;
        }

        public void Dispose()
        {
            if (null != DismatchList) DismatchList.Clear();
            if (null != ResultList) ResultList.Clear();
            if (null != PrevTaskMainline) PrevTaskMainline.Dispose();
            if (null != PrevTaskBranch) PrevTaskBranch.Dispose();
        }

        public override string ToString()
        {
            return string.Format("[TaskItem_CheckProj_CheckFiles(\r\nPrevTaskMainline: {0}\r\nPrevTaskBranch: {1})\r\nResultList?.Count: {2}\r\nDismatchList?.Count: {3}]", PrevTaskMainline, PrevTaskBranch, ResultList?.Count, DismatchList?.Count);
        }

        virtual public int Start()
        {
            if (null != PrevTaskMainline && null != PrevTaskBranch)
            {
                ResultList = PrevTaskMainline.Files.Select(fi =>
                 {
                     return new CompareResult()
                     {
                         PathMainline = new FileInfo(fi.FullName),
                         PathBranch = null,
                         TfsMapping = PrevTaskMainline.TfsMapping,
                         Developer = PrevTaskBranch.Developer
                     };
                 }).ToList();

                int i = 0;
                foreach (FileInfo file in PrevTaskBranch.Files)
                {
                    CompareResult result = null;
                    if (null != (result = ResultList.Find(r =>
                    {
                        //if (null == r?.PathMainline) System.Diagnostics.Trace.WriteLine(string.Format("ERROR: 读取主线文件时出现了空引用, r={0}, b={1}, m={2}", r, r?.PathBranch, r?.PathMainline));
                        return 0 == CompareGitWmsProjFile(r.PathMainline, file);
                    })))
                    {
                        result.PathBranch = file; // update compare result.
                    }
                    else
                    {
                        ResultList.Add(new CompareResult() { PathBranch = file, PathMainline = null, TfsMapping = PrevTaskMainline.TfsMapping, Developer = PrevTaskBranch.Developer }); // add new compare result for branch only files
                    }
                    i++;
                } // foreach (... // compare with branch for each file.
            }

            return 1;
        }

        virtual public TTaskResult Finished()
        {
            DismatchList = ResultList.Where(r => 0 != r.Equal)?.ToList();
            return (DismatchList.EnumerableAny()) ? TTaskResult.EFailed : TTaskResult.ESuccess;
        }

        virtual public int CompareGitWmsProjFile(FileInfo pMainlineFile, FileInfo pBranchFile)
        {
            try
            {
                if (null == pMainlineFile && null == pBranchFile) return 0;
                else if (null == pMainlineFile) return -1;
                else if (null == pBranchFile) return 1;

                string mRelPath = GetRelativePath(pMainlineFile.FullName, PrevTaskMainline.TfsMapping);
                string bRelPath = GetRelativePath(pBranchFile.FullName, PrevTaskMainline.TfsMapping + "\\branch\\dev\\" + PrevTaskBranch.Developer + "\\");
                {
                    string ModuleName = bRelPath.Substring(0, bRelPath.IndexOf("\\"));
                    ModuleName = ModuleName.Replace(".branch", "");
                    string tmpPath = bRelPath.Substring(bRelPath.IndexOf("\\") + 1);
                    bRelPath = "\\" + ModuleName + "\\" + tmpPath;
                }

                return mRelPath.ToLower().CompareTo(bRelPath.ToLower());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("发生异常: " + ex.Message + ", 调用堆栈: " + ex.StackTrace);
                return -1;
            }
        }

        public string GetRelativePath(string pFullPath, string pRootPath)
        {
            return (!string.IsNullOrEmpty(pFullPath) && !string.IsNullOrEmpty(pRootPath) && 0 == pFullPath.IndexOf(pRootPath)) ? pFullPath.Substring(pRootPath.Length) : "";
        }
    }

    class TaskItem_CheckProj_CheckCsprojFiles : TaskItem_CheckProj_CheckFiles
    {
        public TaskItem_CheckProj_CheckCsprojFiles(TaskItem_CheckProj_GetMainlineFiles pTask1, TaskItem_CheckProj_GetBranchFiles pTask2)
            : base(pTask1, pTask2)
        {
        }

        override public int Start()
        {
            if (null != PrevTaskMainline && null != PrevTaskBranch)
            {
                ResultList = PrevTaskMainline.Files.Where(tmp => tmp.Extension.Equals(".csproj")).Select(fi =>
                  {
                      return new CompareResult()
                      {
                          PathMainline = new FileInfo(fi.FullName),
                          PathBranch = null,
                          TfsMapping = PrevTaskMainline.TfsMapping,
                          Developer = PrevTaskBranch.Developer
                      };
                  }).ToList();

                int i = 0;
                IEnumerable<FileInfo> csprojFiles = PrevTaskBranch.Files.Where(tmp => tmp.Extension.Equals(".csproj"));
                foreach (FileInfo file in csprojFiles)
                {
                    CompareResult result = null;
                    if (null != (result = ResultList.Find(r =>
                    {
                        //if (null == r?.PathMainline) System.Diagnostics.Trace.WriteLine(string.Format("ERROR: 读取主线文件时出现了空引用, r={0}, b={1}, m={2}", r, r?.PathBranch, r?.PathMainline));
                        return 0 == CompareGitWmsProjFile(r.PathMainline, file);
                    })))
                    {
                        result.PathBranch = file; // update compare result.
                    }
                    else
                    {
                        ResultList.Add(new CompareResult() { PathBranch = file, PathMainline = null, TfsMapping = PrevTaskMainline.TfsMapping, Developer = PrevTaskBranch.Developer }); // add new compare result for branch only files
                    }
                    i++;
                } // foreach (... // compare with branch for each file.
            }

            return 1;
        }
        //override public TTaskResult Finished()
        //{
        //    return TTaskResult.EFailed; // not implement
        //}

        override public int CompareGitWmsProjFile(FileInfo pMainlineFile, FileInfo pBranchFile)
        {
            string mRelPath = GetRelativePath(pMainlineFile.FullName, PrevTaskMainline.TfsMapping);
            string bRelPath = GetRelativePath(pBranchFile.FullName, PrevTaskMainline.TfsMapping + "\\branch\\dev\\" + PrevTaskBranch.Developer);
            bRelPath = bRelPath.Replace(".branch", "");
            bRelPath = bRelPath.Replace("." + PrevTaskBranch.Developer, "");

            return mRelPath.ToLower().CompareTo(bRelPath.ToLower());
        }
    }
}
