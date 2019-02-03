using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MangoPublishPackage.BuildingControl;
using MangoPublishPackage.FileControl;
using System.Web;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using MangoPublishPackage.Forms;

namespace MangoPublishPackage
{
    public partial class FormMain : Form
    {

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        private void btn_checkfileoccupy_Click(object sender, EventArgs e)
        {
            FormCheckFileOccupy f = new FormCheckFileOccupy();
            f.ShowDialog();
        }

        private void btn_publish_Click(object sender, EventArgs e)
        {
            FormBuildAndPublish f = new FormBuildAndPublish();
            f.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTFS_Click(object sender, EventArgs e)
        {
            FormTfsManage f = new FormTfsManage();
            f.ShowDialog();
        }

        private void btnProjCheck_Click(object sender, EventArgs e)
        {
            FormCheckProject f = new FormCheckProject();
            f.ShowDialog();
        }
    }
}
