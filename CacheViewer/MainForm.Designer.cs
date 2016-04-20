using CacheViewer.Controls;

namespace CacheViewer
{
    partial class MainForm
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
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.CObjectsTab = new System.Windows.Forms.TabPage();
            this.RenderInformationListView = new System.Windows.Forms.ListView();
            this.RenderIds = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OffsetHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DistanceHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PropertiesListView = new System.Windows.Forms.ListView();
            this.PropertiesColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CacheIndexListView = new CacheViewer.Controls.CObjectViewControl();
            this.CObjectTreeView = new System.Windows.Forms.TreeView();
            this.RenderInfoTab = new System.Windows.Forms.TabPage();
            this.SoundPage = new System.Windows.Forms.TabPage();
            this.soundControl1 = new CacheViewer.Controls.SoundControl();
            this.LoadLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.formsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainTabControl.SuspendLayout();
            this.CObjectsTab.SuspendLayout();
            this.SoundPage.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTabControl
            // 
            this.MainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTabControl.Controls.Add(this.CObjectsTab);
            this.MainTabControl.Controls.Add(this.RenderInfoTab);
            this.MainTabControl.Controls.Add(this.SoundPage);
            this.MainTabControl.Location = new System.Drawing.Point(0, 40);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(849, 671);
            this.MainTabControl.TabIndex = 0;
            // 
            // CObjectsTab
            // 
            this.CObjectsTab.Controls.Add(this.RenderInformationListView);
            this.CObjectsTab.Controls.Add(this.PropertiesListView);
            this.CObjectsTab.Controls.Add(this.CacheIndexListView);
            this.CObjectsTab.Controls.Add(this.CObjectTreeView);
            this.CObjectsTab.Location = new System.Drawing.Point(4, 22);
            this.CObjectsTab.Name = "CObjectsTab";
            this.CObjectsTab.Padding = new System.Windows.Forms.Padding(3);
            this.CObjectsTab.Size = new System.Drawing.Size(841, 645);
            this.CObjectsTab.TabIndex = 0;
            this.CObjectsTab.Text = "CObjects";
            this.CObjectsTab.UseVisualStyleBackColor = true;
            this.CObjectsTab.Enter += new System.EventHandler(this.CObjectsTabEnter);
            // 
            // RenderInformationListView
            // 
            this.RenderInformationListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RenderIds,
            this.OffsetHeader,
            this.DistanceHeader,
            this.Type});
            this.RenderInformationListView.GridLines = true;
            this.RenderInformationListView.Location = new System.Drawing.Point(530, 5);
            this.RenderInformationListView.Name = "RenderInformationListView";
            this.RenderInformationListView.Size = new System.Drawing.Size(307, 558);
            this.RenderInformationListView.TabIndex = 3;
            this.RenderInformationListView.UseCompatibleStateImageBehavior = false;
            this.RenderInformationListView.View = System.Windows.Forms.View.Details;
            // 
            // RenderIds
            // 
            this.RenderIds.Text = "Ids";
            this.RenderIds.Width = 84;
            // 
            // OffsetHeader
            // 
            this.OffsetHeader.Text = "Offset";
            this.OffsetHeader.Width = 84;
            // 
            // DistanceHeader
            // 
            this.DistanceHeader.Text = "Distance";
            this.DistanceHeader.Width = 79;
            // 
            // Type
            // 
            this.Type.Text = "Type";
            // 
            // PropertiesListView
            // 
            this.PropertiesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PropertiesColumn});
            this.PropertiesListView.FullRowSelect = true;
            this.PropertiesListView.GridLines = true;
            this.PropertiesListView.Location = new System.Drawing.Point(263, 175);
            this.PropertiesListView.Margin = new System.Windows.Forms.Padding(2);
            this.PropertiesListView.Name = "PropertiesListView";
            this.PropertiesListView.Size = new System.Drawing.Size(261, 388);
            this.PropertiesListView.TabIndex = 2;
            this.PropertiesListView.UseCompatibleStateImageBehavior = false;
            this.PropertiesListView.View = System.Windows.Forms.View.Details;
            // 
            // PropertiesColumn
            // 
            this.PropertiesColumn.Text = "Item Properties";
            this.PropertiesColumn.Width = 260;
            // 
            // CacheIndexListView
            // 
            this.CacheIndexListView.Location = new System.Drawing.Point(263, 5);
            this.CacheIndexListView.Margin = new System.Windows.Forms.Padding(1);
            this.CacheIndexListView.Name = "CacheIndexListView";
            this.CacheIndexListView.Size = new System.Drawing.Size(261, 166);
            this.CacheIndexListView.TabIndex = 1;
            // 
            // CObjectTreeView
            // 
            this.CObjectTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.CObjectTreeView.Location = new System.Drawing.Point(3, 3);
            this.CObjectTreeView.Name = "CObjectTreeView";
            this.CObjectTreeView.Size = new System.Drawing.Size(255, 639);
            this.CObjectTreeView.TabIndex = 0;
            this.CObjectTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.CObjectTreeViewAfterSelect);
            // 
            // RenderInfoTab
            // 
            this.RenderInfoTab.Location = new System.Drawing.Point(4, 22);
            this.RenderInfoTab.Name = "RenderInfoTab";
            this.RenderInfoTab.Padding = new System.Windows.Forms.Padding(3);
            this.RenderInfoTab.Size = new System.Drawing.Size(841, 645);
            this.RenderInfoTab.TabIndex = 1;
            this.RenderInfoTab.Text = "Render Information";
            this.RenderInfoTab.UseVisualStyleBackColor = true;
            // 
            // SoundPage
            // 
            this.SoundPage.Controls.Add(this.soundControl1);
            this.SoundPage.Location = new System.Drawing.Point(4, 22);
            this.SoundPage.Margin = new System.Windows.Forms.Padding(2);
            this.SoundPage.Name = "SoundPage";
            this.SoundPage.Padding = new System.Windows.Forms.Padding(2);
            this.SoundPage.Size = new System.Drawing.Size(841, 645);
            this.SoundPage.TabIndex = 2;
            this.SoundPage.Text = "Sounds";
            this.SoundPage.UseVisualStyleBackColor = true;
            // 
            // soundControl1
            // 
            this.soundControl1.Location = new System.Drawing.Point(23, 47);
            this.soundControl1.Name = "soundControl1";
            this.soundControl1.Size = new System.Drawing.Size(803, 448);
            this.soundControl1.TabIndex = 0;
            // 
            // LoadLabel
            // 
            this.LoadLabel.AutoSize = true;
            this.LoadLabel.Location = new System.Drawing.Point(705, 24);
            this.LoadLabel.Name = "LoadLabel";
            this.LoadLabel.Size = new System.Drawing.Size(57, 13);
            this.LoadLabel.TabIndex = 1;
            this.LoadLabel.Text = "LoadLabel";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formsToolStripMenuItem,
            this.logViewerToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(849, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // formsToolStripMenuItem
            // 
            this.formsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseToolStripMenuItem});
            this.formsToolStripMenuItem.Name = "formsToolStripMenuItem";
            this.formsToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.formsToolStripMenuItem.Text = "&Forms";
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.databaseToolStripMenuItem.Text = "&Database";
            this.databaseToolStripMenuItem.Click += new System.EventHandler(this.databaseToolStripMenuItem_Click);
            // 
            // logViewerToolStripMenuItem
            // 
            this.logViewerToolStripMenuItem.Name = "logViewerToolStripMenuItem";
            this.logViewerToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.logViewerToolStripMenuItem.Text = "&LogViewer";
            this.logViewerToolStripMenuItem.Click += new System.EventHandler(this.logViewerToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 711);
            this.Controls.Add(this.LoadLabel);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.MainTabControl.ResumeLayout(false);
            this.CObjectsTab.ResumeLayout(false);
            this.SoundPage.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage CObjectsTab;
        private System.Windows.Forms.TabPage RenderInfoTab;
        private System.Windows.Forms.TreeView CObjectTreeView;
        private System.Windows.Forms.Label LoadLabel;
        private Controls.CObjectViewControl CacheIndexListView;
        private System.Windows.Forms.ListView PropertiesListView;
        private System.Windows.Forms.ColumnHeader PropertiesColumn;
        private System.Windows.Forms.ListView RenderInformationListView;
        private System.Windows.Forms.ColumnHeader RenderIds;
        private System.Windows.Forms.ColumnHeader OffsetHeader;
        private System.Windows.Forms.ColumnHeader DistanceHeader;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem formsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.TabPage SoundPage;
        private SoundControl soundControl1;
        private System.Windows.Forms.ToolStripMenuItem logViewerToolStripMenuItem;
    }
}