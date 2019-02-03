namespace MangoPublishPackage.Forms
{
    partial class FormCheckFileOccupy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDialog_api = new System.Windows.Forms.FolderBrowserDialog();
            this.tb_checkfolder_api = new System.Windows.Forms.TextBox();
            this.lbl_checkfolder_api = new System.Windows.Forms.Label();
            this.lbl_checkfolder_web = new System.Windows.Forms.Label();
            this.tb_checkfolder_web = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog_web = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_fbd_api = new System.Windows.Forms.Button();
            this.btn_fbd_web = new System.Windows.Forms.Button();
            this.lbl_infoList_api = new System.Windows.Forms.Label();
            this.lbl_infoList_Web = new System.Windows.Forms.Label();
            this.listBox_infolist_api = new System.Windows.Forms.ListBox();
            this.listBox_infolist_web = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // folderBrowserDialog_api
            // 
            this.folderBrowserDialog_api.SelectedPath = "C:\\webdir\\8006.wms.api.simdev\\bin";
            this.folderBrowserDialog_api.ShowNewFolderButton = false;
            // 
            // tb_checkfolder_api
            // 
            this.tb_checkfolder_api.Location = new System.Drawing.Point(89, 12);
            this.tb_checkfolder_api.Name = "tb_checkfolder_api";
            this.tb_checkfolder_api.Size = new System.Drawing.Size(269, 21);
            this.tb_checkfolder_api.TabIndex = 0;
            // 
            // lbl_checkfolder_api
            // 
            this.lbl_checkfolder_api.AutoSize = true;
            this.lbl_checkfolder_api.Location = new System.Drawing.Point(12, 15);
            this.lbl_checkfolder_api.Name = "lbl_checkfolder_api";
            this.lbl_checkfolder_api.Size = new System.Drawing.Size(71, 12);
            this.lbl_checkfolder_api.TabIndex = 1;
            this.lbl_checkfolder_api.Text = "Git.WMS.API";
            // 
            // lbl_checkfolder_web
            // 
            this.lbl_checkfolder_web.AutoSize = true;
            this.lbl_checkfolder_web.Location = new System.Drawing.Point(12, 42);
            this.lbl_checkfolder_web.Name = "lbl_checkfolder_web";
            this.lbl_checkfolder_web.Size = new System.Drawing.Size(71, 12);
            this.lbl_checkfolder_web.TabIndex = 3;
            this.lbl_checkfolder_web.Text = "Git.WMS.Web";
            // 
            // tb_checkfolder_web
            // 
            this.tb_checkfolder_web.Location = new System.Drawing.Point(89, 39);
            this.tb_checkfolder_web.Name = "tb_checkfolder_web";
            this.tb_checkfolder_web.Size = new System.Drawing.Size(269, 21);
            this.tb_checkfolder_web.TabIndex = 2;
            // 
            // folderBrowserDialog_web
            // 
            this.folderBrowserDialog_web.SelectedPath = "C:\\webdir\\8006.wms.web.simdev\\bin";
            this.folderBrowserDialog_web.ShowNewFolderButton = false;
            // 
            // btn_fbd_api
            // 
            this.btn_fbd_api.Location = new System.Drawing.Point(364, 10);
            this.btn_fbd_api.Name = "btn_fbd_api";
            this.btn_fbd_api.Size = new System.Drawing.Size(35, 23);
            this.btn_fbd_api.TabIndex = 4;
            this.btn_fbd_api.Text = "...";
            this.btn_fbd_api.UseVisualStyleBackColor = true;
            this.btn_fbd_api.Click += new System.EventHandler(this.btn_fbd_api_Click);
            // 
            // btn_fbd_web
            // 
            this.btn_fbd_web.Location = new System.Drawing.Point(364, 37);
            this.btn_fbd_web.Name = "btn_fbd_web";
            this.btn_fbd_web.Size = new System.Drawing.Size(35, 23);
            this.btn_fbd_web.TabIndex = 5;
            this.btn_fbd_web.Text = "...";
            this.btn_fbd_web.UseVisualStyleBackColor = true;
            this.btn_fbd_web.Click += new System.EventHandler(this.btn_fbd_web_Click);
            // 
            // lbl_infoList_api
            // 
            this.lbl_infoList_api.AutoSize = true;
            this.lbl_infoList_api.Location = new System.Drawing.Point(12, 97);
            this.lbl_infoList_api.Name = "lbl_infoList_api";
            this.lbl_infoList_api.Size = new System.Drawing.Size(71, 12);
            this.lbl_infoList_api.TabIndex = 9;
            this.lbl_infoList_api.Text = "Git.WMS.API";
            // 
            // lbl_infoList_Web
            // 
            this.lbl_infoList_Web.AutoSize = true;
            this.lbl_infoList_Web.Location = new System.Drawing.Point(712, 97);
            this.lbl_infoList_Web.Name = "lbl_infoList_Web";
            this.lbl_infoList_Web.Size = new System.Drawing.Size(71, 12);
            this.lbl_infoList_Web.TabIndex = 11;
            this.lbl_infoList_Web.Text = "Git.WMS.Web";
            // 
            // listBox_infolist_api
            // 
            this.listBox_infolist_api.FormattingEnabled = true;
            this.listBox_infolist_api.ItemHeight = 12;
            this.listBox_infolist_api.Location = new System.Drawing.Point(12, 121);
            this.listBox_infolist_api.Name = "listBox_infolist_api";
            this.listBox_infolist_api.Size = new System.Drawing.Size(643, 652);
            this.listBox_infolist_api.TabIndex = 12;
            // 
            // listBox_infolist_web
            // 
            this.listBox_infolist_web.FormattingEnabled = true;
            this.listBox_infolist_web.ItemHeight = 12;
            this.listBox_infolist_web.Location = new System.Drawing.Point(714, 121);
            this.listBox_infolist_web.Name = "listBox_infolist_web";
            this.listBox_infolist_web.Size = new System.Drawing.Size(609, 652);
            this.listBox_infolist_web.TabIndex = 13;
            // 
            // FormCheckFileOccupy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1335, 789);
            this.Controls.Add(this.listBox_infolist_web);
            this.Controls.Add(this.listBox_infolist_api);
            this.Controls.Add(this.lbl_infoList_Web);
            this.Controls.Add(this.lbl_infoList_api);
            this.Controls.Add(this.btn_fbd_web);
            this.Controls.Add(this.btn_fbd_api);
            this.Controls.Add(this.lbl_checkfolder_web);
            this.Controls.Add(this.tb_checkfolder_web);
            this.Controls.Add(this.lbl_checkfolder_api);
            this.Controls.Add(this.tb_checkfolder_api);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCheckFileOccupy";
            this.Text = "检查文件占用情况";
            this.Load += new System.EventHandler(this.FormCheckFileOccupy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_api;
        private System.Windows.Forms.TextBox tb_checkfolder_api;
        private System.Windows.Forms.Label lbl_checkfolder_api;
        private System.Windows.Forms.Label lbl_checkfolder_web;
        private System.Windows.Forms.TextBox tb_checkfolder_web;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_web;
        private System.Windows.Forms.Button btn_fbd_api;
        private System.Windows.Forms.Button btn_fbd_web;
        private System.Windows.Forms.Label lbl_infoList_api;
        private System.Windows.Forms.Label lbl_infoList_Web;
        private System.Windows.Forms.ListBox listBox_infolist_api;
        private System.Windows.Forms.ListBox listBox_infolist_web;
    }
}