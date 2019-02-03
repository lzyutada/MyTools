namespace MangoPublishPackage
{
    partial class FormWebPubPackage
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
            this.lblProjDir = new System.Windows.Forms.Label();
            this.tbProjDir = new System.Windows.Forms.TextBox();
            this.btnWebPub = new System.Windows.Forms.Button();
            this.listBoxPackList = new System.Windows.Forms.ListBox();
            this.btnLoadDir = new System.Windows.Forms.Button();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.lblSourceDir = new System.Windows.Forms.Label();
            this.tbTargetPath = new System.Windows.Forms.TextBox();
            this.tbCsprojFile = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblProjDir
            // 
            this.lblProjDir.AutoSize = true;
            this.lblProjDir.Location = new System.Drawing.Point(12, 9);
            this.lblProjDir.Name = "lblProjDir";
            this.lblProjDir.Size = new System.Drawing.Size(83, 12);
            this.lblProjDir.TabIndex = 0;
            this.lblProjDir.Text = "Project Dir: ";
            // 
            // tbProjDir
            // 
            this.tbProjDir.Location = new System.Drawing.Point(101, 6);
            this.tbProjDir.Name = "tbProjDir";
            this.tbProjDir.Size = new System.Drawing.Size(492, 21);
            this.tbProjDir.TabIndex = 1;
            this.tbProjDir.Text = "E:\\liuzy\\documents\\vs2017\\tfs\\mangoWMS\\Git.WMS.Web\\Git.WMS.API";
            // 
            // btnWebPub
            // 
            this.btnWebPub.Location = new System.Drawing.Point(740, 4);
            this.btnWebPub.Name = "btnWebPub";
            this.btnWebPub.Size = new System.Drawing.Size(96, 23);
            this.btnWebPub.TabIndex = 2;
            this.btnWebPub.Text = "Web Publish";
            this.btnWebPub.UseVisualStyleBackColor = true;
            this.btnWebPub.Click += new System.EventHandler(this.btnWebPub_Click);
            // 
            // listBoxPackList
            // 
            this.listBoxPackList.FormattingEnabled = true;
            this.listBoxPackList.ItemHeight = 12;
            this.listBoxPackList.Location = new System.Drawing.Point(14, 69);
            this.listBoxPackList.Name = "listBoxPackList";
            this.listBoxPackList.Size = new System.Drawing.Size(1513, 304);
            this.listBoxPackList.TabIndex = 3;
            // 
            // btnLoadDir
            // 
            this.btnLoadDir.Location = new System.Drawing.Point(615, 4);
            this.btnLoadDir.Name = "btnLoadDir";
            this.btnLoadDir.Size = new System.Drawing.Size(96, 23);
            this.btnLoadDir.TabIndex = 4;
            this.btnLoadDir.Text = "LoadDir";
            this.btnLoadDir.UseVisualStyleBackColor = true;
            this.btnLoadDir.Click += new System.EventHandler(this.btnLoadDir_Click);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(14, 378);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.Size = new System.Drawing.Size(1513, 338);
            this.tbOutput.TabIndex = 5;
            // 
            // lblSourceDir
            // 
            this.lblSourceDir.AutoSize = true;
            this.lblSourceDir.Location = new System.Drawing.Point(12, 36);
            this.lblSourceDir.Name = "lblSourceDir";
            this.lblSourceDir.Size = new System.Drawing.Size(377, 12);
            this.lblSourceDir.TabIndex = 6;
            this.lblSourceDir.Text = "E:\\liuzy\\documents\\vs2017\\tfs\\mangoWMS\\Git.WMS.Web\\Git.WMS.API";
            // 
            // tbTargetPath
            // 
            this.tbTargetPath.Location = new System.Drawing.Point(912, 6);
            this.tbTargetPath.Name = "tbTargetPath";
            this.tbTargetPath.Size = new System.Drawing.Size(615, 21);
            this.tbTargetPath.TabIndex = 7;
            this.tbTargetPath.Text = "E:\\AppData\\VBox\\VirtualBox VMs\\Shares\\gitwms\\local.singleproject.bin";
            // 
            // tbCsprojFile
            // 
            this.tbCsprojFile.Location = new System.Drawing.Point(426, 33);
            this.tbCsprojFile.Name = "tbCsprojFile";
            this.tbCsprojFile.Size = new System.Drawing.Size(167, 21);
            this.tbCsprojFile.TabIndex = 8;
            // 
            // FormWebPubPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1539, 728);
            this.Controls.Add(this.tbCsprojFile);
            this.Controls.Add(this.tbTargetPath);
            this.Controls.Add(this.lblSourceDir);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.btnLoadDir);
            this.Controls.Add(this.listBoxPackList);
            this.Controls.Add(this.btnWebPub);
            this.Controls.Add(this.tbProjDir);
            this.Controls.Add(this.lblProjDir);
            this.Name = "FormWebPubPackage";
            this.Text = "WebApp Publish Packager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosing_FormWebPubPackage);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProjDir;
        private System.Windows.Forms.TextBox tbProjDir;
        private System.Windows.Forms.Button btnWebPub;
        private System.Windows.Forms.ListBox listBoxPackList;
        private System.Windows.Forms.Button btnLoadDir;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.Label lblSourceDir;
        private System.Windows.Forms.TextBox tbTargetPath;
        private System.Windows.Forms.TextBox tbCsprojFile;
    }
}