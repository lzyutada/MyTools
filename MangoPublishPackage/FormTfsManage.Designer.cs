namespace MangoPublishPackage
{
    partial class FormTfsManage
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
            this.tbTfsUri = new System.Windows.Forms.TextBox();
            this.btnConnectTfs = new System.Windows.Forms.Button();
            this.listBoxProjects = new System.Windows.Forms.ListBox();
            this.listBoxTfsProjInfo = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // tbTfsUri
            // 
            this.tbTfsUri.Location = new System.Drawing.Point(72, 12);
            this.tbTfsUri.Name = "tbTfsUri";
            this.tbTfsUri.Size = new System.Drawing.Size(549, 21);
            this.tbTfsUri.TabIndex = 0;
            // 
            // btnConnectTfs
            // 
            this.btnConnectTfs.Location = new System.Drawing.Point(627, 12);
            this.btnConnectTfs.Name = "btnConnectTfs";
            this.btnConnectTfs.Size = new System.Drawing.Size(75, 23);
            this.btnConnectTfs.TabIndex = 1;
            this.btnConnectTfs.Text = "&Connect";
            this.btnConnectTfs.UseVisualStyleBackColor = true;
            this.btnConnectTfs.Click += new System.EventHandler(this.btnConnectTfs_Click);
            // 
            // listBoxProjects
            // 
            this.listBoxProjects.FormattingEnabled = true;
            this.listBoxProjects.HorizontalScrollbar = true;
            this.listBoxProjects.ItemHeight = 12;
            this.listBoxProjects.Location = new System.Drawing.Point(12, 60);
            this.listBoxProjects.Name = "listBoxProjects";
            this.listBoxProjects.ScrollAlwaysVisible = true;
            this.listBoxProjects.Size = new System.Drawing.Size(288, 568);
            this.listBoxProjects.TabIndex = 2;
            this.listBoxProjects.SelectedIndexChanged += new System.EventHandler(this.listBoxProjects_SelectedIndexChanged);
            // 
            // listBoxTfsProjInfo
            // 
            this.listBoxTfsProjInfo.FormattingEnabled = true;
            this.listBoxTfsProjInfo.ItemHeight = 12;
            this.listBoxTfsProjInfo.Location = new System.Drawing.Point(306, 60);
            this.listBoxTfsProjInfo.Name = "listBoxTfsProjInfo";
            this.listBoxTfsProjInfo.Size = new System.Drawing.Size(424, 196);
            this.listBoxTfsProjInfo.TabIndex = 3;
            // 
            // FormTfsManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 657);
            this.Controls.Add(this.listBoxTfsProjInfo);
            this.Controls.Add(this.listBoxProjects);
            this.Controls.Add(this.btnConnectTfs);
            this.Controls.Add(this.tbTfsUri);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTfsManage";
            this.Text = "TFS工具";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTfsManage_Closed);
            this.Load += new System.EventHandler(this.FormTfsManage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbTfsUri;
        private System.Windows.Forms.Button btnConnectTfs;
        private System.Windows.Forms.ListBox listBoxProjects;
        private System.Windows.Forms.ListBox listBoxTfsProjInfo;
    }
}