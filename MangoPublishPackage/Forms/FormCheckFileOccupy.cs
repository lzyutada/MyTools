using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MangoPublishPackage.Forms
{
    public partial class FormCheckFileOccupy : Form
    {
        string _folder_path_api = "";
        string _folder_path_web = "";

        delegate void _dlgtFunc_newthread_check(object arg);
        delegate void _dlgtFunc_UpdateFormControl(object sender, string message);

        void DlgtFunc_newthread_check(object args)
        {
            TCheckObject checkObj = args as TCheckObject;
            if (null == checkObj || !Directory.Exists(checkObj._checkfolder) || null == checkObj._infoview)
            {
                throw new NullReferenceException(string.Format("检查文件占用情况发生异常, 子线程传入空引用参数: args={0}", args));
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(checkObj._checkfolder);
                DirectoryInfo[] diList = di.GetDirectories();
                foreach (var diitem in diList)
                {
                    System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(DlgtFunc_newthread_check));
                    th.Start(new TCheckObject(checkObj._checkgroup, di.FullName, checkObj._infoview));
                }

                FileInfo[] files = di.GetFiles("*.dll");
                foreach (var fi in files)
                {
                    try
                    {
                        using (FileStream fs = fi.OpenRead())
                        {
                            BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), checkObj._infoview, string.Format("{0}: 打开文件[{1}]成功", checkObj._checkgroup, fi.Name));
                            fs.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateInfoList), checkObj._infoview, string.Format("ERROR!! {0}: 打开文件[{1}]发生异常, {2}", checkObj._checkgroup, fi.Name, ex.Message));
                    }
                }
            }
        }

        void DlgtFunc_UpdateInfoList(object sender, string message)
        {
            ListBox lb = sender as ListBox;
            if (null != lb)
            {
                lb.Items.Add(message);
            }
            else
            {
                throw new NullReferenceException("更新InfoList控件异常, 空引用");
            }
        }

        public FormCheckFileOccupy()
        {
            InitializeComponent();
        }

        private void btn_fbd_api_Click(object sender, EventArgs e)
        {
            folderBrowserDialog_api.SelectedPath = tb_checkfolder_api.Text;
            DialogResult dlgRslt = folderBrowserDialog_api.ShowDialog();
            if (DialogResult.OK == dlgRslt)
            {

                System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(DlgtFunc_newthread_check));
                th.Start(new TCheckObject("Git.WMS.API", folderBrowserDialog_api.SelectedPath, listBox_infolist_api));
            }
        }

        private void btn_fbd_web_Click(object sender, EventArgs e)
        {
            folderBrowserDialog_web.SelectedPath = tb_checkfolder_web.Text;
            DialogResult dlgRslt = folderBrowserDialog_web.ShowDialog();
            if (DialogResult.OK == dlgRslt)
            {
                System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(DlgtFunc_newthread_check));
                th.Start(new TCheckObject("Git.WMS.Web", folderBrowserDialog_web.SelectedPath, listBox_infolist_web));
            }
        }

        private void FormCheckFileOccupy_Load(object sender, EventArgs e)
        {
            _folder_path_api = @"\\192.168.3.101\webdir\8005.wms.api.simdev\bin";
            _folder_path_web = @"\\192.168.3.101\webdir\8006.wms.web.simdev\bin";

            tb_checkfolder_api.Text = _folder_path_api;
            tb_checkfolder_web.Text = _folder_path_web;
        }
    }

    class TCheckObject
    {
        public string _checkgroup { get; protected set; }
        public string _checkfolder { get; protected set; }
        public ListBox _infoview { get; protected set; }
        public TCheckObject(string pCheckGroup, string pCheckFolder, ListBox pInfoView)
        {
            _checkgroup = pCheckGroup;
            _checkfolder = pCheckFolder;
            _infoview = pInfoView;
        }
    }
}
