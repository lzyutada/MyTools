namespace MangoPublishPackage
{
    partial class FormCheckProject
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
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTfsMapping = new System.Windows.Forms.TextBox();
            this.btnTfsMapping = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.fbdTfsMapping = new System.Windows.Forms.FolderBrowserDialog();
            this.btnCheck = new System.Windows.Forms.Button();
            this.listBoxMainlineFiles = new System.Windows.Forms.ListBox();
            this.listBoxBranchFiles = new System.Windows.Forms.ListBox();
            this.listBoxMainlineCsproj = new System.Windows.Forms.ListBox();
            this.listBoxBranchCsproj = new System.Windows.Forms.ListBox();
            this.lblTaskRsltMainlineFiles = new System.Windows.Forms.Label();
            this.lblTaskRsltBranchFiles = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbBCPath = new System.Windows.Forms.TextBox();
            this.btnSelBCPath = new System.Windows.Forms.Button();
            this.fbdBCPath = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(12, 72);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbOutput.Size = new System.Drawing.Size(432, 633);
            this.tbOutput.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "曹诗雨",
            "常晟",
            "董天一",
            "刘子煜"});
            this.comboBox1.Location = new System.Drawing.Point(95, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "选择开发人员";
            // 
            // tbTfsMapping
            // 
            this.tbTfsMapping.Location = new System.Drawing.Point(435, 9);
            this.tbTfsMapping.Name = "tbTfsMapping";
            this.tbTfsMapping.Size = new System.Drawing.Size(367, 21);
            this.tbTfsMapping.TabIndex = 3;
            // 
            // btnTfsMapping
            // 
            this.btnTfsMapping.Location = new System.Drawing.Point(808, 7);
            this.btnTfsMapping.Name = "btnTfsMapping";
            this.btnTfsMapping.Size = new System.Drawing.Size(33, 23);
            this.btnTfsMapping.TabIndex = 4;
            this.btnTfsMapping.Text = "...";
            this.btnTfsMapping.UseVisualStyleBackColor = true;
            this.btnTfsMapping.Click += new System.EventHandler(this.btnTfsMapping_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "选择代码目录(TFS本地映射): ";
            // 
            // fbdTfsMapping
            // 
            this.fbdTfsMapping.Description = "请选择本地代码路径";
            this.fbdTfsMapping.ShowNewFolderButton = false;
            this.fbdTfsMapping.Tag = "请选择本地代码路径";
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(14, 43);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(521, 23);
            this.btnCheck.TabIndex = 6;
            this.btnCheck.Text = "检查(&C)";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // listBoxMainlineFiles
            // 
            this.listBoxMainlineFiles.FormattingEnabled = true;
            this.listBoxMainlineFiles.HorizontalScrollbar = true;
            this.listBoxMainlineFiles.ItemHeight = 12;
            this.listBoxMainlineFiles.Location = new System.Drawing.Point(450, 72);
            this.listBoxMainlineFiles.Name = "listBoxMainlineFiles";
            this.listBoxMainlineFiles.ScrollAlwaysVisible = true;
            this.listBoxMainlineFiles.Size = new System.Drawing.Size(777, 364);
            this.listBoxMainlineFiles.TabIndex = 7;
            this.listBoxMainlineFiles.SelectedIndexChanged += new System.EventHandler(this.listBoxMainlineFiles_SelectedIndexChanged);
            this.listBoxMainlineFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxMainlineFiles_MouseDoubleClick);
            // 
            // listBoxBranchFiles
            // 
            this.listBoxBranchFiles.FormattingEnabled = true;
            this.listBoxBranchFiles.HorizontalScrollbar = true;
            this.listBoxBranchFiles.ItemHeight = 12;
            this.listBoxBranchFiles.Location = new System.Drawing.Point(1233, 72);
            this.listBoxBranchFiles.Name = "listBoxBranchFiles";
            this.listBoxBranchFiles.ScrollAlwaysVisible = true;
            this.listBoxBranchFiles.Size = new System.Drawing.Size(796, 364);
            this.listBoxBranchFiles.TabIndex = 8;
            this.listBoxBranchFiles.SelectedIndexChanged += new System.EventHandler(this.listBoxBranchFiles_SelectedIndexChanged);
            this.listBoxBranchFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxBranchFiles_MouseDoubleClick);
            // 
            // listBoxMainlineCsproj
            // 
            this.listBoxMainlineCsproj.FormattingEnabled = true;
            this.listBoxMainlineCsproj.HorizontalScrollbar = true;
            this.listBoxMainlineCsproj.ItemHeight = 12;
            this.listBoxMainlineCsproj.Location = new System.Drawing.Point(450, 461);
            this.listBoxMainlineCsproj.Name = "listBoxMainlineCsproj";
            this.listBoxMainlineCsproj.Size = new System.Drawing.Size(777, 244);
            this.listBoxMainlineCsproj.TabIndex = 9;
            this.listBoxMainlineCsproj.SelectedIndexChanged += new System.EventHandler(this.listBoxMainlineCsproj_SelectedIndexChanged);
            this.listBoxMainlineCsproj.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxMainlineCsproj_MouseDoubleClick);
            // 
            // listBoxBranchCsproj
            // 
            this.listBoxBranchCsproj.FormattingEnabled = true;
            this.listBoxBranchCsproj.HorizontalScrollbar = true;
            this.listBoxBranchCsproj.ItemHeight = 12;
            this.listBoxBranchCsproj.Location = new System.Drawing.Point(1233, 461);
            this.listBoxBranchCsproj.Name = "listBoxBranchCsproj";
            this.listBoxBranchCsproj.Size = new System.Drawing.Size(796, 244);
            this.listBoxBranchCsproj.TabIndex = 10;
            this.listBoxBranchCsproj.SelectedIndexChanged += new System.EventHandler(this.listBoxBranchCsproj_SelectedIndexChanged);
            this.listBoxBranchCsproj.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxBranchCsproj_MouseDoubleClick);
            // 
            // lblTaskRsltMainlineFiles
            // 
            this.lblTaskRsltMainlineFiles.AutoSize = true;
            this.lblTaskRsltMainlineFiles.Location = new System.Drawing.Point(551, 48);
            this.lblTaskRsltMainlineFiles.Name = "lblTaskRsltMainlineFiles";
            this.lblTaskRsltMainlineFiles.Size = new System.Drawing.Size(41, 12);
            this.lblTaskRsltMainlineFiles.TabIndex = 11;
            this.lblTaskRsltMainlineFiles.Text = "主线: ";
            // 
            // lblTaskRsltBranchFiles
            // 
            this.lblTaskRsltBranchFiles.AutoSize = true;
            this.lblTaskRsltBranchFiles.Location = new System.Drawing.Point(986, 48);
            this.lblTaskRsltBranchFiles.Name = "lblTaskRsltBranchFiles";
            this.lblTaskRsltBranchFiles.Size = new System.Drawing.Size(41, 12);
            this.lblTaskRsltBranchFiles.TabIndex = 12;
            this.lblTaskRsltBranchFiles.Text = "分支: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(861, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "BeyondCompare路径: ";
            // 
            // tbBCPath
            // 
            this.tbBCPath.Location = new System.Drawing.Point(975, 9);
            this.tbBCPath.Name = "tbBCPath";
            this.tbBCPath.Size = new System.Drawing.Size(367, 21);
            this.tbBCPath.TabIndex = 14;
            // 
            // btnSelBCPath
            // 
            this.btnSelBCPath.Location = new System.Drawing.Point(1348, 8);
            this.btnSelBCPath.Name = "btnSelBCPath";
            this.btnSelBCPath.Size = new System.Drawing.Size(33, 23);
            this.btnSelBCPath.TabIndex = 15;
            this.btnSelBCPath.Text = "...";
            this.btnSelBCPath.UseVisualStyleBackColor = true;
            // 
            // FormCheckProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2041, 718);
            this.Controls.Add(this.btnSelBCPath);
            this.Controls.Add(this.tbBCPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTaskRsltBranchFiles);
            this.Controls.Add(this.lblTaskRsltMainlineFiles);
            this.Controls.Add(this.listBoxBranchCsproj);
            this.Controls.Add(this.listBoxMainlineCsproj);
            this.Controls.Add(this.listBoxBranchFiles);
            this.Controls.Add(this.listBoxMainlineFiles);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnTfsMapping);
            this.Controls.Add(this.tbTfsMapping);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.tbOutput);
            this.Name = "FormCheckProject";
            this.Text = "检查项目";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormCheckProject_FormClosed);
            this.Load += new System.EventHandler(this.FormCheckProject_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTfsMapping;
        private System.Windows.Forms.Button btnTfsMapping;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog fbdTfsMapping;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.ListBox listBoxMainlineFiles;
        private System.Windows.Forms.ListBox listBoxBranchFiles;
        private System.Windows.Forms.ListBox listBoxMainlineCsproj;
        private System.Windows.Forms.ListBox listBoxBranchCsproj;
        private System.Windows.Forms.Label lblTaskRsltMainlineFiles;
        private System.Windows.Forms.Label lblTaskRsltBranchFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbBCPath;
        private System.Windows.Forms.Button btnSelBCPath;
        private System.Windows.Forms.FolderBrowserDialog fbdBCPath;
    }
}