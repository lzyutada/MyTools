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

namespace MangoPublishPackage
{
    public partial class FormCompareFile : Form
    {
        public FormCompareFile()
        {
            InitializeComponent();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (!File.Exists(tbLeftFile.Text) || !File.Exists(tbRightFile.Text))
            {
                MessageBox.Show("file not exist!");
                return;
            }

            tbLeftContent.Text = "";
            tbLeftText.Text = "";
            tbRightContent.Text = "";
            tbRightText.Text = "";
            string leftText = "";
            string rightText = "";
            byte[] leftcontent = null;
            byte[] rightcontent = null;

            // get content of left
            GetFileContent(tbLeftFile.Text, out leftcontent, out leftText);
            //tbLeftText.Text = leftText;
            leftcontent.ToList().ForEach(b => tbLeftContent.Text += (" 0x" + b.ToString("X2")));

            leftText.ToCharArray().ToList().ForEach(ch => tbLeftText.Text += (ch));
            //var leftBytes = System.Text.Encoding.ASCII.GetBytes(leftText);
            //leftText.ToCharArray().ToList().ForEach(ch => tbLeftContent.Text += ((int)ch).ToString() + "\r\n");
            //foreach (char ch in leftText)
            //{
            //    tbLeftContent.Text += ((int)ch).ToString() + "\r\n";
            //}

            // TODO: get content of right
            GetFileContent(tbRightFile.Text, out rightcontent, out rightText);
            rightcontent.ToList().ForEach(b => tbRightContent.Text += (" 0x" + b.ToString("X2")));
            rightText.ToCharArray().ToList().ForEach(ch => tbRightText.Text += (ch));
            //tbRightFile.Text = rightText;
            //var rightBytes = System.Text.Encoding.ASCII.GetBytes(rightText);
            //rightBytes.ToList().ForEach(b => tbRightContent.Text += b.ToString("X2"));
            //foreach (char ch in rightText)
            //{
            //    tbRightContent.Text += ((int)ch).ToString() + "\r\n";
            //}
        }

        void GetFileContent(string pFn, out byte[] pOutBytes, out string pOutTxt)
        {
            using (FileStream fs = new FileStream(pFn, FileMode.Open))
            {
                int len = (int)fs.Length;
                pOutBytes = new byte[len];
                fs.Read(pOutBytes, 0, len);
                pOutTxt = Encoding.ASCII.GetString(pOutBytes);
            }
        }

        //void GetFileContent(string pFn, out byte[] pOutTxt)
        //{
        //}
    }
}
