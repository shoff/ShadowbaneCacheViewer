namespace CacheViewer.Controls
{
    partial class RenderDxControl
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
            this.slimRenderControl1 = new CacheViewer.Controls.SlimRenderControl();
            this.SuspendLayout();
            // 
            // slimRenderControl1
            // 
            this.slimRenderControl1.Location = new System.Drawing.Point(60, 84);
            this.slimRenderControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.slimRenderControl1.Name = "slimRenderControl1";
            this.slimRenderControl1.Size = new System.Drawing.Size(483, 374);
            this.slimRenderControl1.TabIndex = 0;
            // 
            // RenderDxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.slimRenderControl1);
            this.Name = "RenderDxControl";
            this.ResumeLayout(false);

        }

        #endregion

        private SlimRenderControl slimRenderControl1;
    }
}
