namespace CacheViewer
{
    partial class EntityInitializer
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
            this.AddRenderButton = new System.Windows.Forms.Button();
            this.CObjectsButton = new System.Windows.Forms.Button();
            this.TotalRenderLabel = new System.Windows.Forms.Label();
            this.CObjectsLabel = new System.Windows.Forms.Label();
            this.CurrentCObjectLabel = new System.Windows.Forms.Label();
            this.RenderLabel = new System.Windows.Forms.Label();
            this.MemoryLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddRenderButton
            // 
            this.AddRenderButton.Location = new System.Drawing.Point(13, 13);
            this.AddRenderButton.Name = "AddRenderButton";
            this.AddRenderButton.Size = new System.Drawing.Size(135, 31);
            this.AddRenderButton.TabIndex = 0;
            this.AddRenderButton.Text = "Add RenderEntities";
            this.AddRenderButton.UseVisualStyleBackColor = true;
            this.AddRenderButton.Click += new System.EventHandler(this.AddRenderButtonClick);
            // 
            // CObjectsButton
            // 
            this.CObjectsButton.Location = new System.Drawing.Point(13, 50);
            this.CObjectsButton.Name = "CObjectsButton";
            this.CObjectsButton.Size = new System.Drawing.Size(135, 31);
            this.CObjectsButton.TabIndex = 1;
            this.CObjectsButton.Text = "Add CObjects";
            this.CObjectsButton.UseVisualStyleBackColor = true;
            this.CObjectsButton.Click += new System.EventHandler(this.CObjectsButtonClick);
            // 
            // TotalRenderLabel
            // 
            this.TotalRenderLabel.AutoSize = true;
            this.TotalRenderLabel.Location = new System.Drawing.Point(154, 22);
            this.TotalRenderLabel.Name = "TotalRenderLabel";
            this.TotalRenderLabel.Size = new System.Drawing.Size(85, 13);
            this.TotalRenderLabel.TabIndex = 5;
            this.TotalRenderLabel.Text = "Render Count: 0";
            // 
            // CObjectsLabel
            // 
            this.CObjectsLabel.AutoSize = true;
            this.CObjectsLabel.Location = new System.Drawing.Point(24, 96);
            this.CObjectsLabel.Name = "CObjectsLabel";
            this.CObjectsLabel.Size = new System.Drawing.Size(0, 13);
            this.CObjectsLabel.TabIndex = 6;
            // 
            // CurrentCObjectLabel
            // 
            this.CurrentCObjectLabel.AutoSize = true;
            this.CurrentCObjectLabel.Location = new System.Drawing.Point(172, 59);
            this.CurrentCObjectLabel.Name = "CurrentCObjectLabel";
            this.CurrentCObjectLabel.Size = new System.Drawing.Size(0, 13);
            this.CurrentCObjectLabel.TabIndex = 7;
            // 
            // RenderLabel
            // 
            this.RenderLabel.AutoSize = true;
            this.RenderLabel.Location = new System.Drawing.Point(253, 22);
            this.RenderLabel.Name = "RenderLabel";
            this.RenderLabel.Size = new System.Drawing.Size(0, 13);
            this.RenderLabel.TabIndex = 9;
            // 
            // MemoryLabel
            // 
            this.MemoryLabel.AutoSize = true;
            this.MemoryLabel.Location = new System.Drawing.Point(12, 143);
            this.MemoryLabel.Name = "MemoryLabel";
            this.MemoryLabel.Size = new System.Drawing.Size(0, 13);
            this.MemoryLabel.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 87);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 31);
            this.button1.TabIndex = 11;
            this.button1.Text = "Add Textures";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // EntityInitializer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 165);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.MemoryLabel);
            this.Controls.Add(this.RenderLabel);
            this.Controls.Add(this.CurrentCObjectLabel);
            this.Controls.Add(this.CObjectsLabel);
            this.Controls.Add(this.TotalRenderLabel);
            this.Controls.Add(this.CObjectsButton);
            this.Controls.Add(this.AddRenderButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(472, 271);
            this.MinimizeBox = false;
            this.Name = "EntityInitializer";
            this.Text = "EntityInitializer";
            this.Load += new System.EventHandler(this.EntityInitializerLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddRenderButton;
        private System.Windows.Forms.Button CObjectsButton;
        private System.Windows.Forms.Label TotalRenderLabel;
        private System.Windows.Forms.Label CObjectsLabel;
        private System.Windows.Forms.Label CurrentCObjectLabel;
        private System.Windows.Forms.Label RenderLabel;
        private System.Windows.Forms.Label MemoryLabel;
        private System.Windows.Forms.Button button1;
    }
}