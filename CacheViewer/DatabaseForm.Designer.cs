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
            this.components = new System.ComponentModel.Container();
            this.SaveTexturesButton = new System.Windows.Forms.Button();
            this.TextureSaveLabel = new System.Windows.Forms.Label();
            this.SaveMeshesButton = new System.Windows.Forms.Button();
            this.SaveMeshesLabel = new System.Windows.Forms.Label();
            this.SaveCacheButton = new System.Windows.Forms.Button();
            this.SaveCacheLabel = new System.Windows.Forms.Label();
            this.AssociateRenderButton = new System.Windows.Forms.Button();
            this.AssociateRenderLabel = new System.Windows.Forms.Label();
            this.RenderButton = new System.Windows.Forms.Button();
            this.RenderLabel = new System.Windows.Forms.Label();
            this.vector3EntityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cacheObjectEntityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CreateElvenChurchButton = new System.Windows.Forms.Button();
            this.CreateChurchLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.vector3EntityBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cacheObjectEntityBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // SaveTexturesButton
            // 
            this.SaveTexturesButton.Location = new System.Drawing.Point(14, 12);
            this.SaveTexturesButton.Name = "SaveTexturesButton";
            this.SaveTexturesButton.Size = new System.Drawing.Size(190, 34);
            this.SaveTexturesButton.TabIndex = 6;
            this.SaveTexturesButton.Text = "Save Textures To Database";
            this.SaveTexturesButton.UseVisualStyleBackColor = true;
            this.SaveTexturesButton.Click += new System.EventHandler(this.SaveTexturesButton_Click);
            // 
            // TextureSaveLabel
            // 
            this.TextureSaveLabel.AutoSize = true;
            this.TextureSaveLabel.Location = new System.Drawing.Point(210, 23);
            this.TextureSaveLabel.Name = "TextureSaveLabel";
            this.TextureSaveLabel.Size = new System.Drawing.Size(61, 13);
            this.TextureSaveLabel.TabIndex = 7;
            this.TextureSaveLabel.Text = "Not Started";
            // 
            // SaveMeshesButton
            // 
            this.SaveMeshesButton.Location = new System.Drawing.Point(14, 59);
            this.SaveMeshesButton.Name = "SaveMeshesButton";
            this.SaveMeshesButton.Size = new System.Drawing.Size(189, 34);
            this.SaveMeshesButton.TabIndex = 8;
            this.SaveMeshesButton.Text = "Save Meshes To Database";
            this.SaveMeshesButton.UseVisualStyleBackColor = true;
            this.SaveMeshesButton.Click += new System.EventHandler(this.SaveMeshesButton_Click);
            // 
            // SaveMeshesLabel
            // 
            this.SaveMeshesLabel.AutoSize = true;
            this.SaveMeshesLabel.Location = new System.Drawing.Point(210, 70);
            this.SaveMeshesLabel.Name = "SaveMeshesLabel";
            this.SaveMeshesLabel.Size = new System.Drawing.Size(61, 13);
            this.SaveMeshesLabel.TabIndex = 9;
            this.SaveMeshesLabel.Text = "Not Started";
            // 
            // SaveCacheButton
            // 
            this.SaveCacheButton.Location = new System.Drawing.Point(14, 106);
            this.SaveCacheButton.Name = "SaveCacheButton";
            this.SaveCacheButton.Size = new System.Drawing.Size(189, 34);
            this.SaveCacheButton.TabIndex = 10;
            this.SaveCacheButton.Text = "Save Cache Objects To Database";
            this.SaveCacheButton.UseVisualStyleBackColor = true;
            this.SaveCacheButton.Click += new System.EventHandler(this.SaveCacheButton_Click);
            // 
            // SaveCacheLabel
            // 
            this.SaveCacheLabel.AutoSize = true;
            this.SaveCacheLabel.Location = new System.Drawing.Point(210, 117);
            this.SaveCacheLabel.Name = "SaveCacheLabel";
            this.SaveCacheLabel.Size = new System.Drawing.Size(61, 13);
            this.SaveCacheLabel.TabIndex = 11;
            this.SaveCacheLabel.Text = "Not Started";
            // 
            // AssociateRenderButton
            // 
            this.AssociateRenderButton.Enabled = false;
            this.AssociateRenderButton.Location = new System.Drawing.Point(12, 200);
            this.AssociateRenderButton.Name = "AssociateRenderButton";
            this.AssociateRenderButton.Size = new System.Drawing.Size(189, 34);
            this.AssociateRenderButton.TabIndex = 12;
            this.AssociateRenderButton.Text = "Associate Render and Offsets";
            this.AssociateRenderButton.UseVisualStyleBackColor = true;
            this.AssociateRenderButton.Click += new System.EventHandler(this.AssociateRenderButton_Click);
            // 
            // AssociateRenderLabel
            // 
            this.AssociateRenderLabel.AutoSize = true;
            this.AssociateRenderLabel.Location = new System.Drawing.Point(208, 211);
            this.AssociateRenderLabel.Name = "AssociateRenderLabel";
            this.AssociateRenderLabel.Size = new System.Drawing.Size(61, 13);
            this.AssociateRenderLabel.TabIndex = 13;
            this.AssociateRenderLabel.Text = "Not Started";
            // 
            // RenderButton
            // 
            this.RenderButton.Location = new System.Drawing.Point(14, 153);
            this.RenderButton.Name = "RenderButton";
            this.RenderButton.Size = new System.Drawing.Size(187, 34);
            this.RenderButton.TabIndex = 14;
            this.RenderButton.Text = "Save RenderInfo To Database";
            this.RenderButton.UseVisualStyleBackColor = true;
            this.RenderButton.Click += new System.EventHandler(this.RenderButton_Click);
            // 
            // RenderLabel
            // 
            this.RenderLabel.AutoSize = true;
            this.RenderLabel.Location = new System.Drawing.Point(210, 164);
            this.RenderLabel.Name = "RenderLabel";
            this.RenderLabel.Size = new System.Drawing.Size(61, 13);
            this.RenderLabel.TabIndex = 15;
            this.RenderLabel.Text = "Not Started";
            // 
            // vector3EntityBindingSource
            // 
            this.vector3EntityBindingSource.DataSource = typeof(CacheViewer.Data.Entities.Vector3Entity);
            // 
            // cacheObjectEntityBindingSource
            // 
            this.cacheObjectEntityBindingSource.DataSource = typeof(CacheViewer.Data.Entities.CacheObjectEntity);
            // 
            // CreateElvenChurchButton
            // 
            this.CreateElvenChurchButton.Location = new System.Drawing.Point(14, 249);
            this.CreateElvenChurchButton.Name = "CreateElvenChurchButton";
            this.CreateElvenChurchButton.Size = new System.Drawing.Size(189, 34);
            this.CreateElvenChurchButton.TabIndex = 16;
            this.CreateElvenChurchButton.Text = "Create Elven Church (validates data)";
            this.CreateElvenChurchButton.UseVisualStyleBackColor = true;
            this.CreateElvenChurchButton.Click += new System.EventHandler(this.CreateElvenChurchButton_Click);
            // 
            // CreateChurchLabel
            // 
            this.CreateChurchLabel.AutoSize = true;
            this.CreateChurchLabel.Location = new System.Drawing.Point(210, 260);
            this.CreateChurchLabel.Name = "CreateChurchLabel";
            this.CreateChurchLabel.Size = new System.Drawing.Size(61, 13);
            this.CreateChurchLabel.TabIndex = 17;
            this.CreateChurchLabel.Text = "Not Started";
            // 
            // DatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 345);
            this.Controls.Add(this.CreateChurchLabel);
            this.Controls.Add(this.CreateElvenChurchButton);
            this.Controls.Add(this.RenderLabel);
            this.Controls.Add(this.RenderButton);
            this.Controls.Add(this.AssociateRenderLabel);
            this.Controls.Add(this.AssociateRenderButton);
            this.Controls.Add(this.SaveCacheLabel);
            this.Controls.Add(this.SaveCacheButton);
            this.Controls.Add(this.SaveMeshesLabel);
            this.Controls.Add(this.SaveMeshesButton);
            this.Controls.Add(this.TextureSaveLabel);
            this.Controls.Add(this.SaveTexturesButton);
            this.Name = "DatabaseForm";
            this.Text = "DatabaseForm";
            ((System.ComponentModel.ISupportInitialize)(this.vector3EntityBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cacheObjectEntityBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingSource vector3EntityBindingSource;
        private System.Windows.Forms.BindingSource cacheObjectEntityBindingSource;
        private System.Windows.Forms.Button SaveTexturesButton;
        private System.Windows.Forms.Label TextureSaveLabel;
        private System.Windows.Forms.Button SaveMeshesButton;
        private System.Windows.Forms.Label SaveMeshesLabel;
        private System.Windows.Forms.Button SaveCacheButton;
        private System.Windows.Forms.Label SaveCacheLabel;
        private System.Windows.Forms.Button AssociateRenderButton;
        private System.Windows.Forms.Label AssociateRenderLabel;
        private System.Windows.Forms.Button RenderButton;
        private System.Windows.Forms.Label RenderLabel;
        private System.Windows.Forms.Button CreateElvenChurchButton;
        private System.Windows.Forms.Label CreateChurchLabel;
    }
}