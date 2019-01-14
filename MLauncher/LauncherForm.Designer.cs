namespace MLauncher
{
    partial class LauncherForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.txtPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtDatetime = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tMenu_Setting = new System.Windows.Forms.ToolStripMenuItem();
            this.tMenu_OpenPath = new System.Windows.Forms.ToolStripMenuItem();
            this.tMenu_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip.SuspendLayout();
            this.contextNotify.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtPath,
            this.txtDatetime});
            this.statusStrip.Location = new System.Drawing.Point(0, 239);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(284, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // txtPath
            // 
            this.txtPath.AutoSize = false;
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(269, 17);
            this.txtPath.Spring = true;
            this.txtPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDatetime
            // 
            this.txtDatetime.Name = "txtDatetime";
            this.txtDatetime.Size = new System.Drawing.Size(0, 17);
            this.txtDatetime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextNotify;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "MLauncher";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // contextNotify
            // 
            this.contextNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitToolStripMenuItem});
            this.contextNotify.Name = "contextNotify";
            this.contextNotify.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextNotify.ShowImageMargin = false;
            this.contextNotify.Size = new System.Drawing.Size(74, 26);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(73, 22);
            this.ExitToolStripMenuItem.Text = "종료";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ContextNotify_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tMenu_Setting,
            this.tMenu_OpenPath,
            this.tMenu_Delete});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip.ShowImageMargin = false;
            this.contextMenuStrip.Size = new System.Drawing.Size(143, 70);
            // 
            // tMenu_Setting
            // 
            this.tMenu_Setting.Name = "tMenu_Setting";
            this.tMenu_Setting.Size = new System.Drawing.Size(142, 22);
            this.tMenu_Setting.Text = "설정(&S)";
            this.tMenu_Setting.Click += new System.EventHandler(this.tMenu_Click);
            // 
            // tMenu_OpenPath
            // 
            this.tMenu_OpenPath.Name = "tMenu_OpenPath";
            this.tMenu_OpenPath.Size = new System.Drawing.Size(142, 22);
            this.tMenu_OpenPath.Text = "폴더 바로가기(&O)";
            this.tMenu_OpenPath.Click += new System.EventHandler(this.tMenu_Click);
            // 
            // tMenu_Delete
            // 
            this.tMenu_Delete.Name = "tMenu_Delete";
            this.tMenu_Delete.Size = new System.Drawing.Size(142, 22);
            this.tMenu_Delete.Text = "삭제(&D)";
            this.tMenu_Delete.Click += new System.EventHandler(this.tMenu_Click);
            // 
            // LauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LauncherForm";
            this.Text = "MLauncher";
            this.Activated += new System.EventHandler(this.LauncherForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LauncherForm_FormClosing);
            this.Load += new System.EventHandler(this.LauncherForm_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.contextNotify.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel txtDatetime;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextNotify;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel txtPath;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tMenu_Setting;
        private System.Windows.Forms.ToolStripMenuItem tMenu_OpenPath;
        private System.Windows.Forms.ToolStripMenuItem tMenu_Delete;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

