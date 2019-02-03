namespace MangoPublishPackage.FileControl
{
    partial class ControlFileDetail
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxFileContent = new System.Windows.Forms.TextBox();
            this.lblFileName = new System.Windows.Forms.Label();
            this.lblAttr = new System.Windows.Forms.Label();
            this.lblUpdateTime = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblFileVersion = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblProdVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxFileContent
            // 
            this.textBoxFileContent.Location = new System.Drawing.Point(3, 122);
            this.textBoxFileContent.Multiline = true;
            this.textBoxFileContent.Name = "textBoxFileContent";
            this.textBoxFileContent.ReadOnly = true;
            this.textBoxFileContent.Size = new System.Drawing.Size(1167, 615);
            this.textBoxFileContent.TabIndex = 0;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(13, 9);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(59, 12);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "FileName:";
            // 
            // lblAttr
            // 
            this.lblAttr.AutoSize = true;
            this.lblAttr.Location = new System.Drawing.Point(13, 30);
            this.lblAttr.Name = "lblAttr";
            this.lblAttr.Size = new System.Drawing.Size(65, 12);
            this.lblAttr.TabIndex = 2;
            this.lblAttr.Text = "Attribute:";
            // 
            // lblUpdateTime
            // 
            this.lblUpdateTime.AutoSize = true;
            this.lblUpdateTime.Location = new System.Drawing.Point(13, 54);
            this.lblUpdateTime.Name = "lblUpdateTime";
            this.lblUpdateTime.Size = new System.Drawing.Size(71, 12);
            this.lblUpdateTime.TabIndex = 3;
            this.lblUpdateTime.Text = "UpdateTime:";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(13, 79);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(35, 12);
            this.lblSize.TabIndex = 4;
            this.lblSize.Text = "Szie:";
            // 
            // lblFileVersion
            // 
            this.lblFileVersion.AutoSize = true;
            this.lblFileVersion.Location = new System.Drawing.Point(305, 9);
            this.lblFileVersion.Name = "lblFileVersion";
            this.lblFileVersion.Size = new System.Drawing.Size(89, 12);
            this.lblFileVersion.TabIndex = 5;
            this.lblFileVersion.Text = "File Version: ";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(305, 30);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(59, 12);
            this.lblCopyright.TabIndex = 6;
            this.lblCopyright.Text = "CopyRight";
            // 
            // lblProdVersion
            // 
            this.lblProdVersion.AutoSize = true;
            this.lblProdVersion.Location = new System.Drawing.Point(305, 54);
            this.lblProdVersion.Name = "lblProdVersion";
            this.lblProdVersion.Size = new System.Drawing.Size(107, 12);
            this.lblProdVersion.TabIndex = 7;
            this.lblProdVersion.Text = "Product Version: ";
            // 
            // ControlFileDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblProdVersion);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblFileVersion);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.lblUpdateTime);
            this.Controls.Add(this.lblAttr);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.textBoxFileContent);
            this.Name = "ControlFileDetail";
            this.Size = new System.Drawing.Size(1173, 740);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFileContent;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblAttr;
        private System.Windows.Forms.Label lblUpdateTime;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblFileVersion;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblProdVersion;
    }
}
