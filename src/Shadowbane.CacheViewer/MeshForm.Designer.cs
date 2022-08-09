namespace Shadowbane.CacheViewer
{
    partial class MeshForm
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.LoadModelButton = new System.Windows.Forms.Button();
            this.CacheIndexesDataGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Offset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RawSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SaveSelectedButton = new System.Windows.Forms.Button();
            this.SaveAllButton = new System.Windows.Forms.Button();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.CacheIndexesDataGrid)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoadModelButton
            // 
            this.LoadModelButton.Enabled = false;
            this.LoadModelButton.Location = new System.Drawing.Point(360, 56);
            this.LoadModelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LoadModelButton.Name = "LoadModelButton";
            this.LoadModelButton.Size = new System.Drawing.Size(206, 35);
            this.LoadModelButton.TabIndex = 0;
            this.LoadModelButton.TabStop = false;
            this.LoadModelButton.UseVisualStyleBackColor = true;
            this.LoadModelButton.Click += new System.EventHandler(this.LoadModelButton_Click);
            // 
            // CacheIndexesDataGrid
            // 
            this.CacheIndexesDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CacheIndexesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CacheIndexesDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Id,
            this.Offset,
            this.RawSize,
            this.Size});
            this.CacheIndexesDataGrid.Location = new System.Drawing.Point(18, 106);
            this.CacheIndexesDataGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CacheIndexesDataGrid.Name = "CacheIndexesDataGrid";
            this.CacheIndexesDataGrid.Size = new System.Drawing.Size(548, 554);
            this.CacheIndexesDataGrid.TabIndex = 6;
            this.CacheIndexesDataGrid.SelectionChanged += new System.EventHandler(this.CacheIndexesDataGrid_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Width = 30;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.Width = 60;
            // 
            // Offset
            // 
            this.Offset.HeaderText = "Offset";
            this.Offset.Name = "Offset";
            this.Offset.Width = 75;
            // 
            // RawSize
            // 
            this.RawSize.HeaderText = "Raw Size";
            this.RawSize.Name = "RawSize";
            this.RawSize.Width = 60;
            // 
            // Size
            // 
            this.Size.HeaderText = "Size";
            this.Size.Name = "Size";
            this.Size.Width = 60;
            // 
            // SaveSelectedButton
            // 
            this.SaveSelectedButton.Location = new System.Drawing.Point(18, 56);
            this.SaveSelectedButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveSelectedButton.Name = "SaveSelectedButton";
            this.SaveSelectedButton.Size = new System.Drawing.Size(178, 35);
            this.SaveSelectedButton.TabIndex = 7;
            this.SaveSelectedButton.Text = "Save selected to .obj";
            this.SaveSelectedButton.UseVisualStyleBackColor = true;
            this.SaveSelectedButton.Click += new System.EventHandler(this.SaveSelectedButton_Click);
            // 
            // SaveAllButton
            // 
            this.SaveAllButton.Location = new System.Drawing.Point(204, 56);
            this.SaveAllButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveAllButton.Name = "SaveAllButton";
            this.SaveAllButton.Size = new System.Drawing.Size(148, 35);
            this.SaveAllButton.TabIndex = 8;
            this.SaveAllButton.Text = "Save all to .obj";
            this.SaveAllButton.UseVisualStyleBackColor = true;
            this.SaveAllButton.Click += new System.EventHandler(this.SaveAllButton_Click);
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Location = new System.Drawing.Point(24, 71);
            this.MessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(0, 20);
            this.MessageLabel.TabIndex = 9;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1368, 33);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);

            // 
            // MeshForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1368, 678);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.SaveAllButton);
            this.Controls.Add(this.SaveSelectedButton);
            this.Controls.Add(this.CacheIndexesDataGrid);
            this.Controls.Add(this.LoadModelButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MeshForm";
            this.Text = "Mesh Viewer Form";
            this.Load += new System.EventHandler(this.MeshFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.CacheIndexesDataGrid)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadModelButton;
        private System.Windows.Forms.DataGridView CacheIndexesDataGrid;
        private System.Windows.Forms.Button SaveSelectedButton;
        private System.Windows.Forms.Button SaveAllButton;
        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Offset;
        private System.Windows.Forms.DataGridViewTextBoxColumn RawSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn Size;
        //private SlimRenderControl slimRenderControl1;
    }
}

