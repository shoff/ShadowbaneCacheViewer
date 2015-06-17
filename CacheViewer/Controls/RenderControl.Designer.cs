namespace CacheViewer.Controls
{
    partial class RenderControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RenderIdLabel = new System.Windows.Forms.Label();
            this.RenderInformationListView = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MeshListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MeshesLabel = new System.Windows.Forms.Label();
            this.TextureIdLabel = new System.Windows.Forms.Label();
            this.TexturePictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.TexturePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // RenderIdLabel
            // 
            this.RenderIdLabel.AutoSize = true;
            this.RenderIdLabel.Location = new System.Drawing.Point(3, 3);
            this.RenderIdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.RenderIdLabel.Name = "RenderIdLabel";
            this.RenderIdLabel.Size = new System.Drawing.Size(59, 13);
            this.RenderIdLabel.TabIndex = 0;
            this.RenderIdLabel.Text = "Render Ids";
            // 
            // RenderInformationListView
            // 
            this.RenderInformationListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.RenderInformationListView.FullRowSelect = true;
            this.RenderInformationListView.GridLines = true;
            this.RenderInformationListView.Location = new System.Drawing.Point(5, 18);
            this.RenderInformationListView.Margin = new System.Windows.Forms.Padding(2);
            this.RenderInformationListView.Name = "RenderInformationListView";
            this.RenderInformationListView.Size = new System.Drawing.Size(241, 284);
            this.RenderInformationListView.TabIndex = 1;
            this.RenderInformationListView.UseCompatibleStateImageBehavior = false;
            this.RenderInformationListView.View = System.Windows.Forms.View.Details;
            this.RenderInformationListView.SelectedIndexChanged += new System.EventHandler(this.RenderInformationListViewSelectedIndexChanged);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Render Id";
            this.columnHeader7.Width = 65;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Offset";
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Distance";
            // 
            // MeshListView
            // 
            this.MeshListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.MeshListView.FullRowSelect = true;
            this.MeshListView.GridLines = true;
            this.MeshListView.Location = new System.Drawing.Point(255, 18);
            this.MeshListView.Margin = new System.Windows.Forms.Padding(2);
            this.MeshListView.Name = "MeshListView";
            this.MeshListView.Size = new System.Drawing.Size(241, 126);
            this.MeshListView.TabIndex = 2;
            this.MeshListView.UseCompatibleStateImageBehavior = false;
            this.MeshListView.View = System.Windows.Forms.View.Details;
            this.MeshListView.SelectedIndexChanged += new System.EventHandler(this.MeshListViewSelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Offset";
            this.columnHeader2.Width = 90;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Distance";
            // 
            // MeshesLabel
            // 
            this.MeshesLabel.AutoSize = true;
            this.MeshesLabel.Location = new System.Drawing.Point(253, 3);
            this.MeshesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.MeshesLabel.Name = "MeshesLabel";
            this.MeshesLabel.Size = new System.Drawing.Size(50, 13);
            this.MeshesLabel.TabIndex = 3;
            this.MeshesLabel.Text = "Mesh Ids";
            // 
            // TextureIdLabel
            // 
            this.TextureIdLabel.AutoSize = true;
            this.TextureIdLabel.Location = new System.Drawing.Point(253, 155);
            this.TextureIdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TextureIdLabel.Name = "TextureIdLabel";
            this.TextureIdLabel.Size = new System.Drawing.Size(60, 13);
            this.TextureIdLabel.TabIndex = 5;
            this.TextureIdLabel.Text = "Texture Ids";
            // 
            // TexturePictureBox
            // 
            this.TexturePictureBox.Location = new System.Drawing.Point(256, 172);
            this.TexturePictureBox.Name = "TexturePictureBox";
            this.TexturePictureBox.Size = new System.Drawing.Size(238, 130);
            this.TexturePictureBox.TabIndex = 6;
            this.TexturePictureBox.TabStop = false;
            // 
            // RenderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TexturePictureBox);
            this.Controls.Add(this.TextureIdLabel);
            this.Controls.Add(this.MeshesLabel);
            this.Controls.Add(this.MeshListView);
            this.Controls.Add(this.RenderInformationListView);
            this.Controls.Add(this.RenderIdLabel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RenderControl";
            this.Size = new System.Drawing.Size(497, 307);
            ((System.ComponentModel.ISupportInitialize)(this.TexturePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RenderIdLabel;
        private System.Windows.Forms.ListView RenderInformationListView;
        private System.Windows.Forms.ListView MeshListView;
        private System.Windows.Forms.Label MeshesLabel;
        private System.Windows.Forms.Label TextureIdLabel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.PictureBox TexturePictureBox;
    }
}
