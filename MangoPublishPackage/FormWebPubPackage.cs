using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace MangoPublishPackage
{
    public partial class FormWebPubPackage : Form
    {
        string _projdir = "";
        List<string> _targetfiles = new List<string>(5);

        public FormWebPubPackage()
        {
            InitializeComponent();
        }

        delegate void _dlgtFunc_Publish_CopyFile(string fullname, string targetdir);
        delegate void _dlgtFunc_newthread_copyfile(object arg);
        void DlgtFunc_Publish_CopyFile(string fullname, string targetdir)
        {
            tbOutput.Text += fullname;
        }

        void DlgtFunc_newthread_copyfile(object args)
        {
            // package files
            foreach (string f in _targetfiles)
            {
                //BeginInvoke(new _dlgtFunc_Publish_CopyFile(DlgtFunc_Publish_CopyFile), f, targetdir);
                try
                {
                    string tmpsource = f.Replace("%40", "@");
                    int tmpindex = tmpsource.IndexOf(_projdir);
                    string tarfile = args.ToString() + "\\" + tmpsource.Substring(tmpindex + _projdir.Length + 1);
                    tmpindex = tarfile.LastIndexOf('\\');
                    if (!Directory.Exists(tarfile.Substring(0, tmpindex)))
                        Directory.CreateDirectory(tarfile.Substring(0, tmpindex));

                    File.Copy(tmpsource, tarfile);
                    BeginInvoke(new _dlgtFunc_Publish_CopyFile(DlgtFunc_Publish_CopyFile), ("打包文件: " + tarfile + "\r\n"), "");
                }
                catch (Exception ex)
                {
                    BeginInvoke(new _dlgtFunc_Publish_CopyFile(DlgtFunc_Publish_CopyFile), string.Format("打包文件[{0}]发生异常: {1}", f, ex.Message), "");
                }
                //tbOutput.Text += "打包文件: " + tarfile + "\r\n";
            }
            BeginInvoke(new _dlgtFunc_Publish_CopyFile(DlgtFunc_Publish_CopyFile), "打包文件完成", "");
        }

        private void btnLoadDir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblSourceDir.Text))
            {
                tbOutput.Text = "invalid project directory!";
                return;
            }

            _projdir = lblSourceDir.Text = tbProjDir.Text;

            // get file of *.csproj
            int tmpIdx = _projdir.LastIndexOf('\\');
            if (string.IsNullOrEmpty(tbCsprojFile.Text))
            {
                tbOutput.Text = string.Format("invalid project directory! {0}", tbCsprojFile.Text);
                return;
            }
            else
            {
                _targetfiles.Clear();
                listBoxPackList.Items.Clear();
            }
            string csprojfile = _projdir + "\\" + tbCsprojFile.Text + ".csproj";

            // read from .csproject
            string csproj_content = "";
            using (FileStream fs = new FileStream(csprojfile, FileMode.Open))
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
                var file = _projdir + "\\" + item.ToString().Replace("Content Include=", "").Replace("/>", "").Replace(">", "").Replace(@"""", "").Trim();
                _targetfiles.Add(file);

                listBoxPackList.Items.Add(file);
            }

            // files in folder of '.\bin\'
            string binpath = _projdir + "\\bin\\";
            DirectoryInfo dibin = new DirectoryInfo(binpath);
            IEnumerable<string> binfiles = dibin.GetFiles("*.dll", SearchOption.AllDirectories).Select(fi => fi.FullName);
            _targetfiles.AddRange(binfiles);
            listBoxPackList.Items.AddRange(binfiles.ToArray());

            binfiles = dibin.GetFiles("*.pdb", SearchOption.AllDirectories).Select(fi => fi.FullName);
            _targetfiles.AddRange(binfiles);
            listBoxPackList.Items.AddRange(binfiles.ToArray());
        }

        private void btnWebPub_Click(object sender, EventArgs e)
        {
            // TODO: makesure target folder exist
            string targetdir = tbTargetPath.Text;
            DirectoryInfo destDi = null;
            if (!Directory.Exists(targetdir))
            {
                destDi = Directory.CreateDirectory(targetdir);
            }

            System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(DlgtFunc_newthread_copyfile));
            th.Start(targetdir);
        }

        private void FormClosing_FormWebPubPackage(object sender, FormClosingEventArgs e)
        {
            _targetfiles.Clear();
        }
    }
}
