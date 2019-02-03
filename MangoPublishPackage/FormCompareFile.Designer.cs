namespace MangoPublishPackage
{
    partial class FormCompareFile
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tbLeftContent = new System.Windows.Forms.TextBox();
            this.tbLeftText = new System.Windows.Forms.TextBox();
            this.tbLeftFile = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.tbRightFile = new System.Windows.Forms.TextBox();
            this.tbRightText = new System.Windows.Forms.TextBox();
            this.tbRightContent = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnCompare);
            this.splitContainer1.Panel1.Controls.Add(this.tbLeftFile);
            this.splitContainer1.Panel1.Controls.Add(this.tbLeftText);
            this.splitContainer1.Panel1.Controls.Add(this.tbLeftContent);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbRightFile);
            this.splitContainer1.Panel2.Controls.Add(this.tbRightText);
            this.splitContainer1.Panel2.Controls.Add(this.tbRightContent);
            this.splitContainer1.Size = new System.Drawing.Size(1513, 746);
            this.splitContainer1.SplitterDistance = 779;
            this.splitContainer1.TabIndex = 0;
            // 
            // tbLeftContent
            // 
            this.tbLeftContent.Location = new System.Drawing.Point(12, 35);
            this.tbLeftContent.Multiline = true;
            this.tbLeftContent.Name = "tbLeftContent";
            this.tbLeftContent.ReadOnly = true;
            this.tbLeftContent.Size = new System.Drawing.Size(751, 352);
            this.tbLeftContent.TabIndex = 1;
            // 
            // tbLeftText
            // 
            this.tbLeftText.Location = new System.Drawing.Point(12, 391);
            this.tbLeftText.Multiline = true;
            this.tbLeftText.Name = "tbLeftText";
            this.tbLeftText.ReadOnly = true;
            this.tbLeftText.Size = new System.Drawing.Size(751, 352);
            this.tbLeftText.TabIndex = 2;
            // 
            // tbLeftFile
            // 
            this.tbLeftFile.Location = new System.Drawing.Point(12, 8);
            this.tbLeftFile.Name = "tbLeftFile";
            this.tbLeftFile.Size = new System.Drawing.Size(644, 21);
            this.tbLeftFile.TabIndex = 3;
            this.tbLeftFile.Text = "E:\\liuzy\\documents\\vs2017\\tfs\\mangoWMS\\Git.WMS.Web\\Git.WMS.API\\publishfolder\\bin\\" +
    "App_global.asax.dll";
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(688, 6);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(75, 23);
            this.btnCompare.TabIndex = 4;
            this.btnCompare.Text = "button1";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // tbRightFile
            // 
            this.tbRightFile.Location = new System.Drawing.Point(3, 6);
            this.tbRightFile.Name = "tbRightFile";
            this.tbRightFile.Size = new System.Drawing.Size(644, 21);
            this.tbRightFile.TabIndex = 6;
            this.tbRightFile.Text = "E:\\AppData\\VBox\\VirtualBox VMs\\Shares\\gitwms\\192.168.3.101\\git.wms.api\\Api\\bin\\Ap" +
    "p_global.asax.dll";
            // 
            // tbRightText
            // 
            this.tbRightText.Location = new System.Drawing.Point(3, 389);
            this.tbRightText.Multiline = true;
            this.tbRightText.Name = "tbRightText";
            this.tbRightText.ReadOnly = true;
            this.tbRightText.Size = new System.Drawing.Size(724, 352);
            this.tbRightText.TabIndex = 5;
            // 
            // tbRightContent
            // 
            this.tbRightContent.Location = new System.Drawing.Point(3, 33);
            this.tbRightContent.Multiline = true;
            this.tbRightContent.Name = "tbRightContent";
            this.tbRightContent.ReadOnly = true;
            this.tbRightContent.Size = new System.Drawing.Size(724, 352);
            this.tbRightContent.TabIndex = 4;
            // 
            // FormCompareFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1513, 746);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormCompareFile";
            this.Text = "Compare File";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox tbLeftContent;
        private System.Windows.Forms.TextBox tbLeftText;
        private System.Windows.Forms.TextBox tbLeftFile;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.TextBox tbRightFile;
        private System.Windows.Forms.TextBox tbRightText;
        private System.Windows.Forms.TextBox tbRightContent;
    }
}