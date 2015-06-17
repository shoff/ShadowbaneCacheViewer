namespace CacheViewer
{
    partial class DatabaseForm
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
            this.SaveAllToDatabaseButton = new System.Windows.Forms.Button();
            this.SaveMobilesButton = new System.Windows.Forms.Button();
            this.SaveMobilesLabel = new System.Windows.Forms.Label();
            this.SaveSkelButton = new System.Windows.Forms.Button();
            this.SkeletonLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SaveAllToDatabaseButton
            // 
            this.SaveAllToDatabaseButton.Location = new System.Drawing.Point(20, 20);
            this.SaveAllToDatabaseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveAllToDatabaseButton.Name = "SaveAllToDatabaseButton";
            this.SaveAllToDatabaseButton.Size = new System.Drawing.Size(165, 52);
            this.SaveAllToDatabaseButton.TabIndex = 0;
            this.SaveAllToDatabaseButton.Text = "Save to database";
            this.SaveAllToDatabaseButton.UseVisualStyleBackColor = true;
            this.SaveAllToDatabaseButton.Click += new System.EventHandler(this.SaveAllToDatabaseButtonClick);
            // 
            // SaveMobilesButton
            // 
            this.SaveMobilesButton.Location = new System.Drawing.Point(21, 82);
            this.SaveMobilesButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveMobilesButton.Name = "SaveMobilesButton";
            this.SaveMobilesButton.Size = new System.Drawing.Size(164, 52);
            this.SaveMobilesButton.TabIndex = 1;
            this.SaveMobilesButton.Text = "Save Mobiles";
            this.SaveMobilesButton.UseVisualStyleBackColor = true;
            this.SaveMobilesButton.Click += new System.EventHandler(this.SaveMobilesButtonClick);
            // 
            // SaveMobilesLabel
            // 
            this.SaveMobilesLabel.AutoSize = true;
            this.SaveMobilesLabel.Location = new System.Drawing.Point(194, 98);
            this.SaveMobilesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SaveMobilesLabel.Name = "SaveMobilesLabel";
            this.SaveMobilesLabel.Size = new System.Drawing.Size(0, 20);
            this.SaveMobilesLabel.TabIndex = 2;
            // 
            // SaveSkelButton
            // 
            this.SaveSkelButton.Location = new System.Drawing.Point(21, 143);
            this.SaveSkelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveSkelButton.Name = "SaveSkelButton";
            this.SaveSkelButton.Size = new System.Drawing.Size(164, 52);
            this.SaveSkelButton.TabIndex = 3;
            this.SaveSkelButton.Text = "Save Skeletons";
            this.SaveSkelButton.UseVisualStyleBackColor = true;
            this.SaveSkelButton.Click += new System.EventHandler(this.SaveSkelButtonClick);
            // 
            // SkeletonLabel
            // 
            this.SkeletonLabel.AutoSize = true;
            this.SkeletonLabel.Location = new System.Drawing.Point(194, 159);
            this.SkeletonLabel.Name = "SkeletonLabel";
            this.SkeletonLabel.Size = new System.Drawing.Size(0, 20);
            this.SkeletonLabel.TabIndex = 4;
            // 
            // DatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 291);
            this.Controls.Add(this.SkeletonLabel);
            this.Controls.Add(this.SaveSkelButton);
            this.Controls.Add(this.SaveMobilesLabel);
            this.Controls.Add(this.SaveMobilesButton);
            this.Controls.Add(this.SaveAllToDatabaseButton);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DatabaseForm";
            this.Text = "DatabaseForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveAllToDatabaseButton;
        private System.Windows.Forms.Button SaveMobilesButton;
        private System.Windows.Forms.Label SaveMobilesLabel;
        private System.Windows.Forms.Button SaveSkelButton;
        private System.Windows.Forms.Label SkeletonLabel;
    }
}