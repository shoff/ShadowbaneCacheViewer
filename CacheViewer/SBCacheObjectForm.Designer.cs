namespace CacheViewer
{
    partial class SBCacheObjectForm
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
                this.sbTreeControl1.OnCacheObjectSelected -= CacheObjectSelected;
                this.sbTreeControl1.OnLoadingMessage -= LoadingMessageReceived;
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
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sbTreeControl1 = new CacheViewer.Controls.SBTreeControl();
            this.panelContainer1 = new CacheViewer.Code.PanelContainer();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.glControl1 = new OpenTK.GLControl();
            this.menuStrip1.SuspendLayout();
            this.panelContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1114, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseToolStripMenuItem1});
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.databaseToolStripMenuItem.Text = "&Forms";
            this.databaseToolStripMenuItem.Click += new System.EventHandler(this.databaseToolStripMenuItem_Click);
            // 
            // databaseToolStripMenuItem1
            // 
            this.databaseToolStripMenuItem1.Name = "databaseToolStripMenuItem1";
            this.databaseToolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
            this.databaseToolStripMenuItem1.Text = "&Database";
            this.databaseToolStripMenuItem1.Click += new System.EventHandler(this.databaseToolStripMenuItem1_Click);
            // 
            // sbTreeControl1
            // 
            this.sbTreeControl1.Location = new System.Drawing.Point(12, 64);
            this.sbTreeControl1.MaximumSize = new System.Drawing.Size(268, 637);
            this.sbTreeControl1.MinimumSize = new System.Drawing.Size(268, 637);
            this.sbTreeControl1.Name = "sbTreeControl1";
            this.sbTreeControl1.SelectedCacheObject = null;
            this.sbTreeControl1.Size = new System.Drawing.Size(268, 637);
            this.sbTreeControl1.TabIndex = 1;
            // 
            // panelContainer1
            // 
            this.panelContainer1.Controls.Add(this.glControl1);
            this.panelContainer1.Controls.Add(this.MessageLabel);
            this.panelContainer1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelContainer1.Location = new System.Drawing.Point(301, 24);
            this.panelContainer1.Name = "panelContainer1";
            this.panelContainer1.Size = new System.Drawing.Size(813, 696);
            this.panelContainer1.TabIndex = 0;
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessageLabel.Location = new System.Drawing.Point(13, 656);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(46, 18);
            this.MessageLabel.TabIndex = 0;
            this.MessageLabel.Text = "label1";
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(16, 62);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(772, 579);
            this.glControl1.TabIndex = 1;
            this.glControl1.VSync = false;
            // 
            // SBCacheObjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 720);
            this.Controls.Add(this.sbTreeControl1);
            this.Controls.Add(this.panelContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SBCacheObjectForm";
            this.Text = "SBCacheObjectForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelContainer1.ResumeLayout(false);
            this.panelContainer1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Code.PanelContainer panelContainer1;
        private Controls.SBTreeControl sbTreeControl1;
        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem1;
        private OpenTK.GLControl glControl1;
    }
}