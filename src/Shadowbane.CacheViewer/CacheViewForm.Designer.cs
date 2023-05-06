namespace Shadowbane.CacheViewer;

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
        this.LoadCacheButton = new Button();
        this.CacheObjectTreeView = new TreeView();
        this.LoadLabel = new Label();
        this.SaveButton = new Button();
        this.menuStrip1 = new MenuStrip();
        this.formsToolStripMenuItem = new ToolStripMenuItem();
        this.showEntityFormToolStripMenuItem = new ToolStripMenuItem();
        this.logViewerToolStripMenuItem = new ToolStripMenuItem();
        this.databaseFormToolStripMenuItem = new ToolStripMenuItem();
        this.LoadingPictureBox = new PictureBox();
        this.CacheSaveButton = new Button();
        this.TotalCacheLabel = new Label();
        this.StatusLabel = new Label();
        this.SaveTypeRadioButton2 = new RadioButton();
        this.SaveTypeRadioButton1 = new RadioButton();
        this.SaveToObjGroupBox = new GroupBox();
        this.folderBrowserDialog1 = new FolderBrowserDialog();
        this.LoadingLabel = new Label();
        this.menuStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.LoadingPictureBox).BeginInit();
        this.SaveToObjGroupBox.SuspendLayout();
        SuspendLayout();
        // 
        // LoadCacheButton
        // 
        this.LoadCacheButton.Location = new Point(28, 54);
        this.LoadCacheButton.Margin = new Padding(4, 3, 4, 3);
        this.LoadCacheButton.Name = "LoadCacheButton";
        this.LoadCacheButton.Size = new Size(253, 39);
        this.LoadCacheButton.TabIndex = 0;
        this.LoadCacheButton.Text = "Load Cache";
        this.LoadCacheButton.UseVisualStyleBackColor = true;
        this.LoadCacheButton.Click += LoadCacheButtonClick;
        // 
        // CacheObjectTreeView
        // 
        this.CacheObjectTreeView.Location = new Point(28, 100);
        this.CacheObjectTreeView.Margin = new Padding(4, 3, 4, 3);
        this.CacheObjectTreeView.Name = "CacheObjectTreeView";
        this.CacheObjectTreeView.Size = new Size(252, 554);
        this.CacheObjectTreeView.TabIndex = 1;
        this.CacheObjectTreeView.AfterSelect += CacheObjectTreeViewAfterSelect;
        // 
        // LoadLabel
        // 
        this.LoadLabel.AutoSize = true;
        this.LoadLabel.Location = new Point(124, 27);
        this.LoadLabel.Margin = new Padding(4, 0, 4, 0);
        this.LoadLabel.Name = "LoadLabel";
        this.LoadLabel.Size = new Size(126, 15);
        this.LoadLabel.TabIndex = 2;
        this.LoadLabel.Text = "Load time from cache:";
        // 
        // SaveButton
        // 
        this.SaveButton.Location = new Point(41, 33);
        this.SaveButton.Margin = new Padding(4, 3, 4, 3);
        this.SaveButton.Name = "SaveButton";
        this.SaveButton.Size = new Size(170, 39);
        this.SaveButton.TabIndex = 3;
        this.SaveButton.Text = "Save to .obj";
        this.SaveButton.UseVisualStyleBackColor = true;
        this.SaveButton.Click += SaveButtonClick;
        // 
        // menuStrip1
        // 
        this.menuStrip1.ImageScalingSize = new Size(20, 20);
        this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.formsToolStripMenuItem });
        this.menuStrip1.Location = new Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Padding = new Padding(5, 1, 0, 1);
        this.menuStrip1.Size = new Size(677, 24);
        this.menuStrip1.TabIndex = 10;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // formsToolStripMenuItem
        // 
        this.formsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.showEntityFormToolStripMenuItem, this.logViewerToolStripMenuItem, this.databaseFormToolStripMenuItem });
        this.formsToolStripMenuItem.Name = "formsToolStripMenuItem";
        this.formsToolStripMenuItem.Size = new Size(52, 22);
        this.formsToolStripMenuItem.Text = "&Forms";
        // 
        // showEntityFormToolStripMenuItem
        // 
        this.showEntityFormToolStripMenuItem.Name = "showEntityFormToolStripMenuItem";
        this.showEntityFormToolStripMenuItem.Size = new Size(156, 22);
        this.showEntityFormToolStripMenuItem.Text = "SB Object Form";
        this.showEntityFormToolStripMenuItem.Click += ShowEntityFormToolStripMenuItemClick;
        // 
        // logViewerToolStripMenuItem
        // 
        this.logViewerToolStripMenuItem.Name = "logViewerToolStripMenuItem";
        this.logViewerToolStripMenuItem.Size = new Size(156, 22);
        this.logViewerToolStripMenuItem.Text = "Log Viewer";
        // 
        // databaseFormToolStripMenuItem
        // 
        this.databaseFormToolStripMenuItem.Name = "databaseFormToolStripMenuItem";
        this.databaseFormToolStripMenuItem.Size = new Size(156, 22);
        this.databaseFormToolStripMenuItem.Text = "Database Form";
        this.databaseFormToolStripMenuItem.Click += DatabaseFormToolStripMenuItemClick;
        // 
        // LoadingPictureBox
        // 
        this.LoadingPictureBox.BackColor = SystemColors.Control;
        this.LoadingPictureBox.BackgroundImageLayout = ImageLayout.Center;
        this.LoadingPictureBox.Image = Resources.SbSpinner;
        this.LoadingPictureBox.InitialImage = null;
        this.LoadingPictureBox.Location = new Point(300, 564);
        this.LoadingPictureBox.Margin = new Padding(2);
        this.LoadingPictureBox.Name = "LoadingPictureBox";
        this.LoadingPictureBox.Size = new Size(104, 91);
        this.LoadingPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
        this.LoadingPictureBox.TabIndex = 13;
        this.LoadingPictureBox.TabStop = false;
        this.LoadingPictureBox.Visible = false;
        // 
        // CacheSaveButton
        // 
        this.CacheSaveButton.Location = new Point(29, 170);
        this.CacheSaveButton.Margin = new Padding(4, 3, 4, 3);
        this.CacheSaveButton.Name = "CacheSaveButton";
        this.CacheSaveButton.Size = new Size(191, 40);
        this.CacheSaveButton.TabIndex = 18;
        this.CacheSaveButton.Text = "Save to .cache";
        this.CacheSaveButton.UseVisualStyleBackColor = true;
        this.CacheSaveButton.Click += CacheSaveButtonClick;
        // 
        // TotalCacheLabel
        // 
        this.TotalCacheLabel.AutoSize = true;
        this.TotalCacheLabel.Location = new Point(24, 674);
        this.TotalCacheLabel.Margin = new Padding(2, 0, 2, 0);
        this.TotalCacheLabel.Name = "TotalCacheLabel";
        this.TotalCacheLabel.Size = new Size(166, 15);
        this.TotalCacheLabel.TabIndex = 19;
        this.TotalCacheLabel.Text = "Total number of cache objects";
        // 
        // StatusLabel
        // 
        this.StatusLabel.AutoSize = true;
        this.StatusLabel.Location = new Point(281, 674);
        this.StatusLabel.Margin = new Padding(2, 0, 2, 0);
        this.StatusLabel.Name = "StatusLabel";
        this.StatusLabel.Size = new Size(0, 15);
        this.StatusLabel.TabIndex = 20;
        // 
        // SaveTypeRadioButton2
        // 
        this.SaveTypeRadioButton2.AutoSize = true;
        this.SaveTypeRadioButton2.Checked = true;
        this.SaveTypeRadioButton2.Location = new Point(14, 127);
        this.SaveTypeRadioButton2.Margin = new Padding(4, 3, 4, 3);
        this.SaveTypeRadioButton2.Name = "SaveTypeRadioButton2";
        this.SaveTypeRadioButton2.Size = new Size(153, 19);
        this.SaveTypeRadioButton2.TabIndex = 21;
        this.SaveTypeRadioButton2.TabStop = true;
        this.SaveTypeRadioButton2.Text = "save as Indiviual meshes";
        this.SaveTypeRadioButton2.UseVisualStyleBackColor = true;
        // 
        // SaveTypeRadioButton1
        // 
        this.SaveTypeRadioButton1.AutoSize = true;
        this.SaveTypeRadioButton1.Location = new Point(14, 100);
        this.SaveTypeRadioButton1.Margin = new Padding(4, 3, 4, 3);
        this.SaveTypeRadioButton1.Name = "SaveTypeRadioButton1";
        this.SaveTypeRadioButton1.Size = new Size(117, 19);
        this.SaveTypeRadioButton1.TabIndex = 22;
        this.SaveTypeRadioButton1.Text = "save as one mesh";
        this.SaveTypeRadioButton1.UseVisualStyleBackColor = true;
        // 
        // SaveToObjGroupBox
        // 
        this.SaveToObjGroupBox.Controls.Add(this.CacheSaveButton);
        this.SaveToObjGroupBox.Controls.Add(this.SaveTypeRadioButton1);
        this.SaveToObjGroupBox.Controls.Add(this.SaveTypeRadioButton2);
        this.SaveToObjGroupBox.Controls.Add(this.SaveButton);
        this.SaveToObjGroupBox.Location = new Point(312, 54);
        this.SaveToObjGroupBox.Margin = new Padding(4, 3, 4, 3);
        this.SaveToObjGroupBox.Name = "SaveToObjGroupBox";
        this.SaveToObjGroupBox.Padding = new Padding(4, 3, 4, 3);
        this.SaveToObjGroupBox.Size = new Size(266, 227);
        this.SaveToObjGroupBox.TabIndex = 23;
        this.SaveToObjGroupBox.TabStop = false;
        this.SaveToObjGroupBox.Text = "3d export";
        // 
        // LoadingLabel
        // 
        this.LoadingLabel.AutoSize = true;
        this.LoadingLabel.Location = new Point(28, 712);
        this.LoadingLabel.Name = "LoadingLabel";
        this.LoadingLabel.Size = new Size(88, 15);
        this.LoadingLabel.TabIndex = 23;
        this.LoadingLabel.Text = "Waiting to load";
        this.LoadingLabel.Click += label1_Click;
        // 
        // CacheViewForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(677, 750);
        this.Controls.Add(this.LoadingLabel);
        this.Controls.Add(this.SaveToObjGroupBox);
        this.Controls.Add(this.StatusLabel);
        this.Controls.Add(this.TotalCacheLabel);
        this.Controls.Add(this.LoadingPictureBox);
        this.Controls.Add(this.LoadLabel);
        this.Controls.Add(this.CacheObjectTreeView);
        this.Controls.Add(this.LoadCacheButton);
        this.Controls.Add(this.menuStrip1);
        this.MainMenuStrip = this.menuStrip1;
        this.Margin = new Padding(4, 3, 4, 3);
        this.Name = "CacheViewForm";
        this.Text = "CacheViewForm";
        this.Load += CacheViewFormLoad;
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.LoadingPictureBox).EndInit();
        this.SaveToObjGroupBox.ResumeLayout(false);
        this.SaveToObjGroupBox.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.TreeView CacheObjectTreeView;
    private System.Windows.Forms.Label LoadLabel;
    private System.Windows.Forms.Button SaveButton;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.Button LoadCacheButton;
    private System.Windows.Forms.PictureBox LoadingPictureBox;
    private System.Windows.Forms.ToolStripMenuItem formsToolStripMenuItem;
    private System.Windows.Forms.Button CacheSaveButton;
    private System.Windows.Forms.Label TotalCacheLabel;
    private System.Windows.Forms.Label StatusLabel;
    private System.Windows.Forms.ToolStripMenuItem showEntityFormToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem logViewerToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem databaseFormToolStripMenuItem;
    private System.Windows.Forms.RadioButton SaveTypeRadioButton2;
    private System.Windows.Forms.RadioButton SaveTypeRadioButton1;
    private System.Windows.Forms.GroupBox SaveToObjGroupBox;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    private Label LoadingLabel;
}
