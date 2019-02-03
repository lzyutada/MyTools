namespace MangoPublishPackage.Forms
{
    partial class FormBuildAndPublish
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
            this.listBox_logs = new System.Windows.Forms.ListBox();
            this.comboBoxBatFile = new System.Windows.Forms.ComboBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.tbRoot = new System.Windows.Forms.TextBox();
            this.btnSelectRoot = new System.Windows.Forms.Button();
            this.fbdBatRoot = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_msg = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbBatContent = new System.Windows.Forms.TextBox();
            this.btnPublish = new System.Windows.Forms.Button();
            this.tbPublishPath_Api = new System.Windows.Forms.TextBox();
            this.btnSelectPublishPath_API = new System.Windows.Forms.Button();
            this.tbPublishPath_Web = new System.Windows.Forms.TextBox();
            this.btnSelectPublishPath_Web = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fbdPubApi = new System.Windows.Forms.FolderBrowserDialog();
            this.fbdPubWeb = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_logs
            // 
            this.listBox_logs.FormattingEnabled = true;
            this.listBox_logs.HorizontalScrollbar = true;
            this.listBox_logs.ItemHeight = 12;
            this.listBox_logs.Location = new System.Drawing.Point(515, 82);
            this.listBox_logs.Name = "listBox_logs";
            this.listBox_logs.Size = new System.Drawing.Size(686, 640);
            this.listBox_logs.TabIndex = 0;
            // 
            // comboBoxBatFile
            // 
            this.comboBoxBatFile.FormattingEnabled = true;
            this.comboBoxBatFile.Location = new System.Drawing.Point(11, 45);
            this.comboBoxBatFile.Name = "comboBoxBatFile";
            this.comboBoxBatFile.Size = new System.Drawing.Size(424, 20);
            this.comboBoxBatFile.TabIndex = 1;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(441, 43);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "(&R)运行";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tbRoot
            // 
            this.tbRoot.Location = new System.Drawing.Point(12, 12);
            this.tbRoot.Name = "tbRoot";
            this.tbRoot.Size = new System.Drawing.Size(383, 21);
            this.tbRoot.TabIndex = 3;
            // 
            // btnSelectRoot
            // 
            this.btnSelectRoot.Location = new System.Drawing.Point(401, 10);
            this.btnSelectRoot.Name = "btnSelectRoot";
            this.btnSelectRoot.Size = new System.Drawing.Size(34, 23);
            this.btnSelectRoot.TabIndex = 4;
            this.btnSelectRoot.Text = "...";
            this.btnSelectRoot.UseVisualStyleBackColor = true;
            this.btnSelectRoot.Click += new System.EventHandler(this.btnSelectRoot_Click);
            // 
            // fbdBatRoot
            // 
            this.fbdBatRoot.Description = "选择批处理文件的根路径";
            this.fbdBatRoot.SelectedPath = "E:\\liuzy\\documents\\vs2017\\tfs\\mangoWMS";
            this.fbdBatRoot.ShowNewFolderButton = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_msg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 751);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1213, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_msg
            // 
            this.toolStripStatusLabel_msg.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel_msg.Name = "toolStripStatusLabel_msg";
            this.toolStripStatusLabel_msg.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel_msg.Text = "toolStripStatusLabel1";
            // 
            // tbBatContent
            // 
            this.tbBatContent.Location = new System.Drawing.Point(12, 82);
            this.tbBatContent.Multiline = true;
            this.tbBatContent.Name = "tbBatContent";
            this.tbBatContent.ReadOnly = true;
            this.tbBatContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbBatContent.Size = new System.Drawing.Size(497, 640);
            this.tbBatContent.TabIndex = 6;
            // 
            // btnPublish
            // 
            this.btnPublish.Location = new System.Drawing.Point(1126, 10);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(75, 23);
            this.btnPublish.TabIndex = 7;
            this.btnPublish.Text = "(&P)发布";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // tbPublishPath_Api
            // 
            this.tbPublishPath_Api.Location = new System.Drawing.Point(680, 12);
            this.tbPublishPath_Api.Name = "tbPublishPath_Api";
            this.tbPublishPath_Api.Size = new System.Drawing.Size(383, 21);
            this.tbPublishPath_Api.TabIndex = 8;
            // 
            // btnSelectPublishPath_API
            // 
            this.btnSelectPublishPath_API.Location = new System.Drawing.Point(1069, 10);
            this.btnSelectPublishPath_API.Name = "btnSelectPublishPath_API";
            this.btnSelectPublishPath_API.Size = new System.Drawing.Size(34, 23);
            this.btnSelectPublishPath_API.TabIndex = 9;
            this.btnSelectPublishPath_API.Text = "...";
            this.btnSelectPublishPath_API.UseVisualStyleBackColor = true;
            this.btnSelectPublishPath_API.Click += new System.EventHandler(this.btnSelectPublishPath_API_Click);
            // 
            // tbPublishPath_Web
            // 
            this.tbPublishPath_Web.Location = new System.Drawing.Point(680, 45);
            this.tbPublishPath_Web.Name = "tbPublishPath_Web";
            this.tbPublishPath_Web.Size = new System.Drawing.Size(383, 21);
            this.tbPublishPath_Web.TabIndex = 10;
            // 
            // btnSelectPublishPath_Web
            // 
            this.btnSelectPublishPath_Web.Location = new System.Drawing.Point(1069, 43);
            this.btnSelectPublishPath_Web.Name = "btnSelectPublishPath_Web";
            this.btnSelectPublishPath_Web.Size = new System.Drawing.Size(34, 23);
            this.btnSelectPublishPath_Web.TabIndex = 11;
            this.btnSelectPublishPath_Web.Text = "...";
            this.btnSelectPublishPath_Web.UseVisualStyleBackColor = true;
            this.btnSelectPublishPath_Web.Click += new System.EventHandler(this.btnSelectPublishPath_Web_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(585, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "发布地址(API):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(585, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "发布地址(Web):";
            // 
            // fbdPubApi
            // 
            this.fbdPubApi.Description = "请选择发布API的目标路径: ";
            this.fbdPubApi.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.fbdPubApi.SelectedPath = "\\\\192.168.3.101\\webdir\\8005.wms.api.simdev";
            this.fbdPubApi.ShowNewFolderButton = false;
            // 
            // fbdPubWeb
            // 
            this.fbdPubWeb.Description = "请选择发布Web的目标目录: ";
            this.fbdPubWeb.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.fbdPubWeb.SelectedPath = "\\\\192.168.3.101\\webdir\\8005.wms.web.simdev";
            this.fbdPubWeb.ShowNewFolderButton = false;
            // 
            // FormBuildAndPublish
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 773);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectPublishPath_Web);
            this.Controls.Add(this.tbPublishPath_Web);
            this.Controls.Add(this.btnSelectPublishPath_API);
            this.Controls.Add(this.tbPublishPath_Api);
            this.Controls.Add(this.btnPublish);
            this.Controls.Add(this.tbBatContent);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnSelectRoot);
            this.Controls.Add(this.tbRoot);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.comboBoxBatFile);
            this.Controls.Add(this.listBox_logs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBuildAndPublish";
            this.Text = "编译, 发布";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormBuildAndPublish_FormClosed);
            this.Load += new System.EventHandler(this.FormBuildAndPublish_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_logs;
        private System.Windows.Forms.ComboBox comboBoxBatFile;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox tbRoot;
        private System.Windows.Forms.Button btnSelectRoot;
        private System.Windows.Forms.FolderBrowserDialog fbdBatRoot;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_msg;
        private System.Windows.Forms.TextBox tbBatContent;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.TextBox tbPublishPath_Api;
        private System.Windows.Forms.Button btnSelectPublishPath_API;
        private System.Windows.Forms.TextBox tbPublishPath_Web;
        private System.Windows.Forms.Button btnSelectPublishPath_Web;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog fbdPubApi;
        private System.Windows.Forms.FolderBrowserDialog fbdPubWeb;
    }
}