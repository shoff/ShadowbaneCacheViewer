namespace Shadowbane.CacheViewer.Controls
{
    partial class CObjectViewControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CacheIndexListView = new System.Windows.Forms.ListView();
            this.CacheIndexColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // CacheIndexListView
            // 
            this.CacheIndexListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CacheIndexColumnHeader});
            this.CacheIndexListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CacheIndexListView.FullRowSelect = true;
            this.CacheIndexListView.GridLines = true;
            this.CacheIndexListView.Location = new System.Drawing.Point(0, 0);
            this.CacheIndexListView.Margin = new System.Windows.Forms.Padding(2);
            this.CacheIndexListView.MultiSelect = false;
            this.CacheIndexListView.Name = "CacheIndexListView";
            this.CacheIndexListView.Size = new System.Drawing.Size(239, 193);
            this.CacheIndexListView.TabIndex = 0;
            this.CacheIndexListView.TabStop = false;
            this.CacheIndexListView.UseCompatibleStateImageBehavior = false;
            this.CacheIndexListView.View = System.Windows.Forms.View.Details;
            // 
            // CacheIndexColumnHeader
            // 
            this.CacheIndexColumnHeader.Text = "CacheIndex";
            this.CacheIndexColumnHeader.Width = 250;
            // 
            // CObjectViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CacheIndexListView);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CObjectViewControl";
            this.Size = new System.Drawing.Size(239, 193);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView CacheIndexListView;
        private System.Windows.Forms.ColumnHeader CacheIndexColumnHeader;

    }
}
