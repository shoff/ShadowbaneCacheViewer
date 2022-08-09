namespace Shadowbane.CacheViewer.Controls
{
    partial class CObjectControl
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
            this.CObjectsHeadPanel = new System.Windows.Forms.Panel();
            this.CObjectHeadLabel = new System.Windows.Forms.Label();
            this.CObjectsHeadPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CObjectsHeadPanel
            // 
            this.CObjectsHeadPanel.BackColor = System.Drawing.Color.White;
            this.CObjectsHeadPanel.Controls.Add(this.CObjectHeadLabel);
            this.CObjectsHeadPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CObjectsHeadPanel.Location = new System.Drawing.Point(0, 0);
            this.CObjectsHeadPanel.Name = "CObjectsHeadPanel";
            this.CObjectsHeadPanel.Size = new System.Drawing.Size(393, 38);
            this.CObjectsHeadPanel.TabIndex = 0;
            // 
            // CObjectHeadLabel
            // 
            this.CObjectHeadLabel.AutoSize = true;
            this.CObjectHeadLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CObjectHeadLabel.Location = new System.Drawing.Point(21, 9);
            this.CObjectHeadLabel.Name = "CObjectHeadLabel";
            this.CObjectHeadLabel.Size = new System.Drawing.Size(46, 18);
            this.CObjectHeadLabel.TabIndex = 0;
            this.CObjectHeadLabel.Text = "label1";
            // 
            // CObjectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.Controls.Add(this.CObjectsHeadPanel);
            this.Name = "CObjectControl";
            this.Size = new System.Drawing.Size(393, 222);
            this.CObjectsHeadPanel.ResumeLayout(false);
            this.CObjectsHeadPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel CObjectsHeadPanel;
        private System.Windows.Forms.Label CObjectHeadLabel;
    }
}
