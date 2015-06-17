namespace CacheViewer
{
    partial class TexturesForm
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
            this.ExportButton = new System.Windows.Forms.Button();
            this.NullCountLabel = new System.Windows.Forms.Label();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(12, 12);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(110, 36);
            this.ExportButton.TabIndex = 0;
            this.ExportButton.Text = "Export to File";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // NullCountLabel
            // 
            this.NullCountLabel.AutoSize = true;
            this.NullCountLabel.Location = new System.Drawing.Point(234, 12);
            this.NullCountLabel.Name = "NullCountLabel";
            this.NullCountLabel.Size = new System.Drawing.Size(65, 13);
            this.NullCountLabel.TabIndex = 1;
            this.NullCountLabel.Text = "null count: 0";
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Location = new System.Drawing.Point(12, 65);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(35, 13);
            this.MessageLabel.TabIndex = 2;
            this.MessageLabel.Text = "label1";
            // 
            // TexturesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 163);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.NullCountLabel);
            this.Controls.Add(this.ExportButton);
            this.Name = "TexturesForm";
            this.Text = "TexturesForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Label NullCountLabel;
        private System.Windows.Forms.Label MessageLabel;
    }
}