using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MangoPublishPackage.FileControl
{
    public partial class ControlFileDetail : UserControl
    {
        public ControlFileDetail()
        {
            InitializeComponent();
        }

        public void SetFileName(string val){lblFileName.Text = "FileName: " +val;}
        public void SetAttribute(string val) { lblAttr.Text = "Attribute: " + val; }
        public void SetUpdateTime(string val) { lblUpdateTime.Text = "UpdateTime: " + val; }
        public void SetSize(string val) { lblSize.Text = "Size: " + val; }
        public void SetFileVersion(string val) { lblFileVersion.Text = "File Version: " + val; }
        public void SetProductVersion(string val) { lblProdVersion.Text = "Product Version: " + val; }
        public void SetCopyright(string val) { lblCopyright.Text = "@Copyright: " + val; }

        public void SetFileContent(string content)
        {
            textBoxFileContent.Text = content;
        }
    }
}
