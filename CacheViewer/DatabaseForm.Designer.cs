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
            this.CreateElvenChurchButton = new System.Windows.Forms.Button();
            this.CreateChurchLabel = new System.Windows.Forms.Label();
            this.RangerBlindButton = new System.Windows.Forms.Button();
            this.RangerBlindLabel = new System.Windows.Forms.Label();
            this.LizardManTempleLabel = new System.Windows.Forms.Label();
            this.LizardManTempleButton = new System.Windows.Forms.Button();
            this.vector3EntityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AssociateTexturesLabel = new System.Windows.Forms.Label();
            this.AssociateTexturesButton = new System.Windows.Forms.Button();
            this.ValidRangeValidationLabel = new System.Windows.Forms.Label();
            this.RangeLabel = new System.Windows.Forms.Label();
            this.RangeTextBox = new System.Windows.Forms.TextBox();
            this.ClearDatabaseLabel = new System.Windows.Forms.Label();
            this.ClearDataButton = new System.Windows.Forms.Button();
            this.SaveMeshesCheckbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RenderInfoCheckbox = new System.Windows.Forms.CheckBox();
            this.CObjectsCheckbox = new System.Windows.Forms.CheckBox();
            this.RenderCheckbox = new System.Windows.Forms.CheckBox();
            this.TexturesCheckbox = new System.Windows.Forms.CheckBox();
            this.cacheObjectEntityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.vector3EntityBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cacheObjectEntityBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // SaveTexturesButton
            // 
            this.SaveTexturesButton.Location = new System.Drawing.Point(98, 68);
            this.SaveTexturesButton.Name = "SaveTexturesButton";
            this.SaveTexturesButton.Size = new System.Drawing.Size(189, 34);
            this.SaveTexturesButton.TabIndex = 6;
            this.SaveTexturesButton.Text = "Save Textures To Database";
            this.SaveTexturesButton.UseVisualStyleBackColor = true;
            this.SaveTexturesButton.Click += new System.EventHandler(this.SaveTexturesButton_Click);
            // 
            // TextureSaveLabel
            // 
            this.TextureSaveLabel.AutoSize = true;
            this.TextureSaveLabel.Location = new System.Drawing.Point(296, 79);
            this.TextureSaveLabel.Name = "TextureSaveLabel";
            this.TextureSaveLabel.Size = new System.Drawing.Size(61, 13);
            this.TextureSaveLabel.TabIndex = 7;
            this.TextureSaveLabel.Text = "Not Started";
            // 
            // SaveMeshesButton
            // 
            this.SaveMeshesButton.Enabled = false;
            this.SaveMeshesButton.Location = new System.Drawing.Point(98, 111);
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
            this.SaveMeshesLabel.Location = new System.Drawing.Point(296, 119);
            this.SaveMeshesLabel.Name = "SaveMeshesLabel";
            this.SaveMeshesLabel.Size = new System.Drawing.Size(61, 13);
            this.SaveMeshesLabel.TabIndex = 9;
            this.SaveMeshesLabel.Text = "Not Started";
            // 
            // SaveCacheButton
            // 
            this.SaveCacheButton.Enabled = false;
            this.SaveCacheButton.Location = new System.Drawing.Point(98, 198);
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
            this.SaveCacheLabel.Location = new System.Drawing.Point(296, 209);
            this.SaveCacheLabel.Name = "SaveCacheLabel";
            this.SaveCacheLabel.Size = new System.Drawing.Size(61, 13);
            this.SaveCacheLabel.TabIndex = 11;
            this.SaveCacheLabel.Text = "Not Started";
            // 
            // AssociateRenderButton
            // 
            this.AssociateRenderButton.Enabled = false;
            this.AssociateRenderButton.Location = new System.Drawing.Point(98, 241);
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
            this.AssociateRenderLabel.Location = new System.Drawing.Point(296, 252);
            this.AssociateRenderLabel.Name = "AssociateRenderLabel";
            this.AssociateRenderLabel.Size = new System.Drawing.Size(61, 13);
            this.AssociateRenderLabel.TabIndex = 13;
            this.AssociateRenderLabel.Text = "Not Started";
            // 
            // RenderButton
            // 
            this.RenderButton.Enabled = false;
            this.RenderButton.Location = new System.Drawing.Point(98, 154);
            this.RenderButton.Name = "RenderButton";
            this.RenderButton.Size = new System.Drawing.Size(189, 34);
            this.RenderButton.TabIndex = 14;
            this.RenderButton.Text = "Save RenderInfo To Database";
            this.RenderButton.UseVisualStyleBackColor = true;
            this.RenderButton.Click += new System.EventHandler(this.RenderButton_Click);
            // 
            // RenderLabel
            // 
            this.RenderLabel.AutoSize = true;
            this.RenderLabel.Location = new System.Drawing.Point(296, 165);
            this.RenderLabel.Name = "RenderLabel";
            this.RenderLabel.Size = new System.Drawing.Size(61, 13);
            this.RenderLabel.TabIndex = 15;
            this.RenderLabel.Text = "Not Started";
            // 
            // CreateElvenChurchButton
            // 
            this.CreateElvenChurchButton.Location = new System.Drawing.Point(25, 38);
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
            this.CreateChurchLabel.Location = new System.Drawing.Point(223, 49);
            this.CreateChurchLabel.Name = "CreateChurchLabel";
            this.CreateChurchLabel.Size = new System.Drawing.Size(61, 13);
            this.CreateChurchLabel.TabIndex = 17;
            this.CreateChurchLabel.Text = "Not Started";
            // 
            // RangerBlindButton
            // 
            this.RangerBlindButton.Location = new System.Drawing.Point(25, 85);
            this.RangerBlindButton.Name = "RangerBlindButton";
            this.RangerBlindButton.Size = new System.Drawing.Size(189, 34);
            this.RangerBlindButton.TabIndex = 18;
            this.RangerBlindButton.Text = "Create Ranger Blind";
            this.RangerBlindButton.UseVisualStyleBackColor = true;
            this.RangerBlindButton.Click += new System.EventHandler(this.RangerBlindButton_Click);
            // 
            // RangerBlindLabel
            // 
            this.RangerBlindLabel.AutoSize = true;
            this.RangerBlindLabel.Location = new System.Drawing.Point(223, 96);
            this.RangerBlindLabel.Name = "RangerBlindLabel";
            this.RangerBlindLabel.Size = new System.Drawing.Size(61, 13);
            this.RangerBlindLabel.TabIndex = 19;
            this.RangerBlindLabel.Text = "Not Started";
            // 
            // LizardManTempleLabel
            // 
            this.LizardManTempleLabel.AutoSize = true;
            this.LizardManTempleLabel.Location = new System.Drawing.Point(221, 143);
            this.LizardManTempleLabel.Name = "LizardManTempleLabel";
            this.LizardManTempleLabel.Size = new System.Drawing.Size(61, 13);
            this.LizardManTempleLabel.TabIndex = 21;
            this.LizardManTempleLabel.Text = "Not Started";
            // 
            // LizardManTempleButton
            // 
            this.LizardManTempleButton.Location = new System.Drawing.Point(25, 132);
            this.LizardManTempleButton.Name = "LizardManTempleButton";
            this.LizardManTempleButton.Size = new System.Drawing.Size(189, 34);
            this.LizardManTempleButton.TabIndex = 20;
            this.LizardManTempleButton.Text = "Create Lizardman Temple";
            this.LizardManTempleButton.UseVisualStyleBackColor = true;
            this.LizardManTempleButton.Click += new System.EventHandler(this.LizardManTempleButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.CreateElvenChurchButton);
            this.groupBox1.Controls.Add(this.LizardManTempleLabel);
            this.groupBox1.Controls.Add(this.CreateChurchLabel);
            this.groupBox1.Controls.Add(this.LizardManTempleButton);
            this.groupBox1.Controls.Add(this.RangerBlindButton);
            this.groupBox1.Controls.Add(this.RangerBlindLabel);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Location = new System.Drawing.Point(12, 384);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(503, 194);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Test Objects";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.TexturesCheckbox);
            this.groupBox2.Controls.Add(this.RenderCheckbox);
            this.groupBox2.Controls.Add(this.CObjectsCheckbox);
            this.groupBox2.Controls.Add(this.RenderInfoCheckbox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.SaveMeshesCheckbox);
            this.groupBox2.Controls.Add(this.AssociateTexturesLabel);
            this.groupBox2.Controls.Add(this.AssociateTexturesButton);
            this.groupBox2.Controls.Add(this.ValidRangeValidationLabel);
            this.groupBox2.Controls.Add(this.RangeLabel);
            this.groupBox2.Controls.Add(this.RangeTextBox);
            this.groupBox2.Controls.Add(this.RenderButton);
            this.groupBox2.Controls.Add(this.SaveTexturesButton);
            this.groupBox2.Controls.Add(this.RenderLabel);
            this.groupBox2.Controls.Add(this.TextureSaveLabel);
            this.groupBox2.Controls.Add(this.SaveMeshesButton);
            this.groupBox2.Controls.Add(this.AssociateRenderLabel);
            this.groupBox2.Controls.Add(this.SaveMeshesLabel);
            this.groupBox2.Controls.Add(this.AssociateRenderButton);
            this.groupBox2.Controls.Add(this.SaveCacheButton);
            this.groupBox2.Controls.Add(this.SaveCacheLabel);
            this.groupBox2.Location = new System.Drawing.Point(12, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(503, 356);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Importers";
            // 
            // AssociateTexturesLabel
            // 
            this.AssociateTexturesLabel.AutoSize = true;
            this.AssociateTexturesLabel.Location = new System.Drawing.Point(296, 295);
            this.AssociateTexturesLabel.Name = "AssociateTexturesLabel";
            this.AssociateTexturesLabel.Size = new System.Drawing.Size(61, 13);
            this.AssociateTexturesLabel.TabIndex = 20;
            this.AssociateTexturesLabel.Text = "Not Started";
            // 
            // AssociateTexturesButton
            // 
            this.AssociateTexturesButton.Enabled = false;
            this.AssociateTexturesButton.Location = new System.Drawing.Point(98, 284);
            this.AssociateTexturesButton.Name = "AssociateTexturesButton";
            this.AssociateTexturesButton.Size = new System.Drawing.Size(189, 34);
            this.AssociateTexturesButton.TabIndex = 19;
            this.AssociateTexturesButton.Text = "Associate Textures";
            this.AssociateTexturesButton.UseVisualStyleBackColor = true;
            this.AssociateTexturesButton.Click += new System.EventHandler(this.AssociateTexturesButton_Click);
            // 
            // ValidRangeValidationLabel
            // 
            this.ValidRangeValidationLabel.AutoSize = true;
            this.ValidRangeValidationLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ValidRangeValidationLabel.Location = new System.Drawing.Point(366, 42);
            this.ValidRangeValidationLabel.Name = "ValidRangeValidationLabel";
            this.ValidRangeValidationLabel.Size = new System.Drawing.Size(0, 13);
            this.ValidRangeValidationLabel.TabIndex = 18;
            // 
            // RangeLabel
            // 
            this.RangeLabel.AutoSize = true;
            this.RangeLabel.Location = new System.Drawing.Point(165, 42);
            this.RangeLabel.Name = "RangeLabel";
            this.RangeLabel.Size = new System.Drawing.Size(117, 13);
            this.RangeLabel.TabIndex = 17;
            this.RangeLabel.Text = "Valid Range Difference";
            this.RangeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RangeTextBox
            // 
            this.RangeTextBox.Location = new System.Drawing.Point(288, 39);
            this.RangeTextBox.Name = "RangeTextBox";
            this.RangeTextBox.Size = new System.Drawing.Size(69, 20);
            this.RangeTextBox.TabIndex = 16;
            this.RangeTextBox.Text = "5000";
            this.RangeTextBox.TextChanged += new System.EventHandler(this.RangeTextBox_TextChanged);
            // 
            // ClearDatabaseLabel
            // 
            this.ClearDatabaseLabel.AutoSize = true;
            this.ClearDatabaseLabel.Location = new System.Drawing.Point(235, 47);
            this.ClearDatabaseLabel.Name = "ClearDatabaseLabel";
            this.ClearDatabaseLabel.Size = new System.Drawing.Size(0, 13);
            this.ClearDatabaseLabel.TabIndex = 24;
            // 
            // ClearDataButton
            // 
            this.ClearDataButton.Location = new System.Drawing.Point(311, 591);
            this.ClearDataButton.Name = "ClearDataButton";
            this.ClearDataButton.Size = new System.Drawing.Size(178, 34);
            this.ClearDataButton.TabIndex = 25;
            this.ClearDataButton.Text = "Clear Data";
            this.ClearDataButton.UseVisualStyleBackColor = true;
            this.ClearDataButton.Click += new System.EventHandler(this.ClearDataButton_Click);
            // 
            // SaveMeshesCheckbox
            // 
            this.SaveMeshesCheckbox.AutoSize = true;
            this.SaveMeshesCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SaveMeshesCheckbox.Location = new System.Drawing.Point(17, 118);
            this.SaveMeshesCheckbox.Name = "SaveMeshesCheckbox";
            this.SaveMeshesCheckbox.Size = new System.Drawing.Size(58, 17);
            this.SaveMeshesCheckbox.TabIndex = 28;
            this.SaveMeshesCheckbox.Text = "enable";
            this.SaveMeshesCheckbox.UseVisualStyleBackColor = true;
            this.SaveMeshesCheckbox.CheckedChanged += new System.EventHandler(this.SaveMeshesCheckbox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "enable execution";
            // 
            // RenderInfoCheckbox
            // 
            this.RenderInfoCheckbox.AutoSize = true;
            this.RenderInfoCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RenderInfoCheckbox.Location = new System.Drawing.Point(17, 164);
            this.RenderInfoCheckbox.Name = "RenderInfoCheckbox";
            this.RenderInfoCheckbox.Size = new System.Drawing.Size(58, 17);
            this.RenderInfoCheckbox.TabIndex = 30;
            this.RenderInfoCheckbox.Text = "enable";
            this.RenderInfoCheckbox.UseVisualStyleBackColor = true;
            this.RenderInfoCheckbox.CheckedChanged += new System.EventHandler(this.RenderInfoCheckbox_CheckedChanged);
            // 
            // CObjectsCheckbox
            // 
            this.CObjectsCheckbox.AutoSize = true;
            this.CObjectsCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CObjectsCheckbox.Location = new System.Drawing.Point(17, 208);
            this.CObjectsCheckbox.Name = "CObjectsCheckbox";
            this.CObjectsCheckbox.Size = new System.Drawing.Size(58, 17);
            this.CObjectsCheckbox.TabIndex = 31;
            this.CObjectsCheckbox.Text = "enable";
            this.CObjectsCheckbox.UseVisualStyleBackColor = true;
            this.CObjectsCheckbox.CheckedChanged += new System.EventHandler(this.CObjectsCheckbox_CheckedChanged);
            // 
            // RenderCheckbox
            // 
            this.RenderCheckbox.AutoSize = true;
            this.RenderCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RenderCheckbox.Location = new System.Drawing.Point(17, 252);
            this.RenderCheckbox.Name = "RenderCheckbox";
            this.RenderCheckbox.Size = new System.Drawing.Size(58, 17);
            this.RenderCheckbox.TabIndex = 32;
            this.RenderCheckbox.Text = "enable";
            this.RenderCheckbox.UseVisualStyleBackColor = true;
            this.RenderCheckbox.CheckedChanged += new System.EventHandler(this.RenderCheckbox_CheckedChanged);
            // 
            // TexturesCheckbox
            // 
            this.TexturesCheckbox.AutoSize = true;
            this.TexturesCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TexturesCheckbox.Location = new System.Drawing.Point(17, 294);
            this.TexturesCheckbox.Name = "TexturesCheckbox";
            this.TexturesCheckbox.Size = new System.Drawing.Size(58, 17);
            this.TexturesCheckbox.TabIndex = 33;
            this.TexturesCheckbox.Text = "enable";
            this.TexturesCheckbox.UseVisualStyleBackColor = true;
            this.TexturesCheckbox.CheckedChanged += new System.EventHandler(this.TexturesCheckbox_CheckedChanged);
            // 
            // cacheObjectEntityBindingSource
            // 
            this.cacheObjectEntityBindingSource.DataSource = typeof(CacheViewer.Data.Entities.CacheObjectEntity);
            // 
            // DatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 630);
            this.Controls.Add(this.ClearDataButton);
            this.Controls.Add(this.ClearDatabaseLabel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DatabaseForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "DatabaseForm";
            this.Load += new System.EventHandler(this.DatabaseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vector3EntityBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.Button RangerBlindButton;
        private System.Windows.Forms.Label RangerBlindLabel;
        private System.Windows.Forms.Label LizardManTempleLabel;
        private System.Windows.Forms.Button LizardManTempleButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label RangeLabel;
        private System.Windows.Forms.TextBox RangeTextBox;
        private System.Windows.Forms.Label ValidRangeValidationLabel;
        private System.Windows.Forms.Label ClearDatabaseLabel;
        private System.Windows.Forms.Button ClearDataButton;
        private System.Windows.Forms.Label AssociateTexturesLabel;
        private System.Windows.Forms.Button AssociateTexturesButton;
        private System.Windows.Forms.CheckBox TexturesCheckbox;
        private System.Windows.Forms.CheckBox RenderCheckbox;
        private System.Windows.Forms.CheckBox CObjectsCheckbox;
        private System.Windows.Forms.CheckBox RenderInfoCheckbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox SaveMeshesCheckbox;
    }
}