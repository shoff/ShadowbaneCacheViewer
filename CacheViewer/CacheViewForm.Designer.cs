namespace CacheViewer
{
    partial class CacheViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CacheViewForm));
            this.LoadCacheButton = new System.Windows.Forms.Button();
            this.CacheObjectTreeView = new System.Windows.Forms.TreeView();
            this.LoadLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.PropertiesListView = new System.Windows.Forms.ListView();
            this.PropertiesColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.formsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showEntityFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PropertiesLabel = new System.Windows.Forms.Label();
            this.LoadingPictureBox = new System.Windows.Forms.PictureBox();
            this.CacheSaveButton = new System.Windows.Forms.Button();
            this.TotalCacheLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.renderControl1 = new CacheViewer.Controls.RenderControl();
            this.CacheIndexListView = new CacheViewer.Controls.CObjectViewControl();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadCacheButton
            // 
            this.LoadCacheButton.Location = new System.Drawing.Point(9, 54);
            this.LoadCacheButton.Name = "LoadCacheButton";
            this.LoadCacheButton.Size = new System.Drawing.Size(89, 34);
            this.LoadCacheButton.TabIndex = 0;
            this.LoadCacheButton.Text = "Load Cache";
            this.LoadCacheButton.UseVisualStyleBackColor = true;
            this.LoadCacheButton.Click += new System.EventHandler(this.LoadCacheButtonClick);
            // 
            // CacheObjectTreeView
            // 
            this.CacheObjectTreeView.Location = new System.Drawing.Point(175, 47);
            this.CacheObjectTreeView.Name = "CacheObjectTreeView";
            this.CacheObjectTreeView.Size = new System.Drawing.Size(217, 481);
            this.CacheObjectTreeView.TabIndex = 1;
            this.CacheObjectTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.CacheObjectTreeViewAfterSelect);
            // 
            // LoadLabel
            // 
            this.LoadLabel.AutoSize = true;
            this.LoadLabel.Location = new System.Drawing.Point(172, 23);
            this.LoadLabel.Name = "LoadLabel";
            this.LoadLabel.Size = new System.Drawing.Size(112, 13);
            this.LoadLabel.TabIndex = 2;
            this.LoadLabel.Text = "Load time from cache:";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(9, 93);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(89, 34);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.Text = "Save to .obj";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // PropertiesListView
            // 
            this.PropertiesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PropertiesColumn});
            this.PropertiesListView.FullRowSelect = true;
            this.PropertiesListView.GridLines = true;
            this.PropertiesListView.Location = new System.Drawing.Point(402, 47);
            this.PropertiesListView.Margin = new System.Windows.Forms.Padding(2);
            this.PropertiesListView.Name = "PropertiesListView";
            this.PropertiesListView.Size = new System.Drawing.Size(241, 180);
            this.PropertiesListView.TabIndex = 0;
            this.PropertiesListView.UseCompatibleStateImageBehavior = false;
            this.PropertiesListView.View = System.Windows.Forms.View.Details;
            // 
            // PropertiesColumn
            // 
            this.PropertiesColumn.Text = "Item Properties";
            this.PropertiesColumn.Width = 200;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(978, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // formsToolStripMenuItem
            // 
            this.formsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showEntityFormToolStripMenuItem,
            this.logViewerToolStripMenuItem,
            this.databaseFormToolStripMenuItem});
            this.formsToolStripMenuItem.Name = "formsToolStripMenuItem";
            this.formsToolStripMenuItem.Size = new System.Drawing.Size(52, 22);
            this.formsToolStripMenuItem.Text = "&Forms";
            // 
            // showEntityFormToolStripMenuItem
            // 
            this.showEntityFormToolStripMenuItem.Name = "showEntityFormToolStripMenuItem";
            this.showEntityFormToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.showEntityFormToolStripMenuItem.Text = "Show Entity Form";
            this.showEntityFormToolStripMenuItem.Click += new System.EventHandler(this.ShowEntityFormToolStripMenuItemClick);
            // 
            // logViewerToolStripMenuItem
            // 
            this.logViewerToolStripMenuItem.Name = "logViewerToolStripMenuItem";
            this.logViewerToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.logViewerToolStripMenuItem.Text = "Log Viewer";
            this.logViewerToolStripMenuItem.Click += new System.EventHandler(this.LogViewerToolStripMenuItemClick);
            // 
            // databaseFormToolStripMenuItem
            // 
            this.databaseFormToolStripMenuItem.Name = "databaseFormToolStripMenuItem";
            this.databaseFormToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.databaseFormToolStripMenuItem.Text = "Database Form";
            this.databaseFormToolStripMenuItem.Click += new System.EventHandler(this.DatabaseFormToolStripMenuItemClick);
            // 
            // PropertiesLabel
            // 
            this.PropertiesLabel.AutoSize = true;
            this.PropertiesLabel.Location = new System.Drawing.Point(402, 23);
            this.PropertiesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PropertiesLabel.Name = "PropertiesLabel";
            this.PropertiesLabel.Size = new System.Drawing.Size(87, 13);
            this.PropertiesLabel.TabIndex = 11;
            this.PropertiesLabel.Text = "Object properties";
            // 
            // LoadingPictureBox
            // 
            this.LoadingPictureBox.BackColor = System.Drawing.SystemColors.Control;
            this.LoadingPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.LoadingPictureBox.Image = global::CacheViewer.Properties.Resources.SbSpinner;
            this.LoadingPictureBox.InitialImage = null;
            this.LoadingPictureBox.Location = new System.Drawing.Point(9, 422);
            this.LoadingPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.LoadingPictureBox.Name = "LoadingPictureBox";
            this.LoadingPictureBox.Size = new System.Drawing.Size(89, 79);
            this.LoadingPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.LoadingPictureBox.TabIndex = 13;
            this.LoadingPictureBox.TabStop = false;
            this.LoadingPictureBox.Visible = false;
            // 
            // CacheSaveButton
            // 
            this.CacheSaveButton.Location = new System.Drawing.Point(9, 133);
            this.CacheSaveButton.Name = "CacheSaveButton";
            this.CacheSaveButton.Size = new System.Drawing.Size(88, 35);
            this.CacheSaveButton.TabIndex = 18;
            this.CacheSaveButton.Text = "Save to .cache";
            this.CacheSaveButton.UseVisualStyleBackColor = true;
            this.CacheSaveButton.Click += new System.EventHandler(this.CacheSaveButtonClick);
            // 
            // TotalCacheLabel
            // 
            this.TotalCacheLabel.AutoSize = true;
            this.TotalCacheLabel.Location = new System.Drawing.Point(172, 531);
            this.TotalCacheLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TotalCacheLabel.Name = "TotalCacheLabel";
            this.TotalCacheLabel.Size = new System.Drawing.Size(151, 13);
            this.TotalCacheLabel.TabIndex = 19;
            this.TotalCacheLabel.Text = "Total number of cache objects";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(402, 531);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 13);
            this.StatusLabel.TabIndex = 20;
            // 
            // renderControl1
            // 
            this.renderControl1.Location = new System.Drawing.Point(395, 227);
            this.renderControl1.Margin = new System.Windows.Forms.Padding(1);
            this.renderControl1.Name = "renderControl1";
            this.renderControl1.Size = new System.Drawing.Size(497, 306);
            this.renderControl1.TabIndex = 17;
            // 
            // CacheIndexListView
            // 
            this.CacheIndexListView.Location = new System.Drawing.Point(654, 47);
            this.CacheIndexListView.Margin = new System.Windows.Forms.Padding(1);
            this.CacheIndexListView.Name = "CacheIndexListView";
            this.CacheIndexListView.Size = new System.Drawing.Size(239, 171);
            this.CacheIndexListView.TabIndex = 16;
            // 
            // CacheViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 551);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.TotalCacheLabel);
            this.Controls.Add(this.CacheSaveButton);
            this.Controls.Add(this.renderControl1);
            this.Controls.Add(this.CacheIndexListView);
            this.Controls.Add(this.LoadingPictureBox);
            this.Controls.Add(this.PropertiesLabel);
            this.Controls.Add(this.PropertiesListView);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.LoadLabel);
            this.Controls.Add(this.CacheObjectTreeView);
            this.Controls.Add(this.LoadCacheButton);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CacheViewForm";
            this.Text = "CacheViewForm";
            this.Load += new System.EventHandler(this.CacheViewFormLoad);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView CacheObjectTreeView;
        private System.Windows.Forms.Label LoadLabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.ListView PropertiesListView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Label PropertiesLabel;
        private System.Windows.Forms.Button LoadCacheButton;
        private System.Windows.Forms.PictureBox LoadingPictureBox;
        private System.Windows.Forms.ColumnHeader PropertiesColumn;
        private Controls.CObjectViewControl CacheIndexListView;
        private Controls.RenderControl renderControl1;
        private System.Windows.Forms.ToolStripMenuItem formsToolStripMenuItem;
        private System.Windows.Forms.Button CacheSaveButton;
        private System.Windows.Forms.Label TotalCacheLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.ToolStripMenuItem showEntityFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseFormToolStripMenuItem;
    }
}