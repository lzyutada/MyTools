namespace MangoPublishPackage
{
    partial class FormMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btn_checkfileoccupy = new System.Windows.Forms.Button();
            this.btn_publish = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnTFS = new System.Windows.Forms.Button();
            this.btnProjCheck = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(451, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 334);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(451, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btn_checkfileoccupy
            // 
            this.btn_checkfileoccupy.Location = new System.Drawing.Point(12, 55);
            this.btn_checkfileoccupy.Name = "btn_checkfileoccupy";
            this.btn_checkfileoccupy.Size = new System.Drawing.Size(98, 23);
            this.btn_checkfileoccupy.TabIndex = 2;
            this.btn_checkfileoccupy.Text = "检查文件占用";
            this.btn_checkfileoccupy.UseVisualStyleBackColor = true;
            this.btn_checkfileoccupy.Click += new System.EventHandler(this.btn_checkfileoccupy_Click);
            // 
            // btn_publish
            // 
            this.btn_publish.Location = new System.Drawing.Point(12, 84);
            this.btn_publish.Name = "btn_publish";
            this.btn_publish.Size = new System.Drawing.Size(98, 25);
            this.btn_publish.TabIndex = 3;
            this.btn_publish.Text = "编译和发布";
            this.btn_publish.UseVisualStyleBackColor = true;
            this.btn_publish.Click += new System.EventHandler(this.btn_publish_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(12, 28);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(98, 23);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnTFS
            // 
            this.btnTFS.Location = new System.Drawing.Point(12, 115);
            this.btnTFS.Name = "btnTFS";
            this.btnTFS.Size = new System.Drawing.Size(98, 23);
            this.btnTFS.TabIndex = 5;
            this.btnTFS.Text = "TFS管理";
            this.btnTFS.UseVisualStyleBackColor = true;
            this.btnTFS.Click += new System.EventHandler(this.btnTFS_Click);
            // 
            // btnProjCheck
            // 
            this.btnProjCheck.Location = new System.Drawing.Point(12, 144);
            this.btnProjCheck.Name = "btnProjCheck";
            this.btnProjCheck.Size = new System.Drawing.Size(98, 23);
            this.btnProjCheck.TabIndex = 6;
            this.btnProjCheck.Text = "检查项目(&P)";
            this.btnProjCheck.UseVisualStyleBackColor = true;
            this.btnProjCheck.Click += new System.EventHandler(this.btnProjCheck_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 356);
            this.Controls.Add(this.btnProjCheck);
            this.Controls.Add(this.btnTFS);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btn_publish);
            this.Controls.Add(this.btn_checkfileoccupy);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button btn_checkfileoccupy;
        private System.Windows.Forms.Button btn_publish;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnTFS;
        private System.Windows.Forms.Button btnProjCheck;
    }
}