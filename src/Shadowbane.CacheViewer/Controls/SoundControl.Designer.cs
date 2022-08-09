namespace Shadowbane.CacheViewer.Controls
{
    partial class SoundControl
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
            this.SaveToWavButton = new System.Windows.Forms.Button();
            this.SaveAllSoundsButton = new System.Windows.Forms.Button();
            this.SavingSoundFileLabel = new System.Windows.Forms.Label();
            this.PlaySoundFileButton = new System.Windows.Forms.Button();
            this.SelectedSoundLabel = new System.Windows.Forms.Label();
            this.FrequencyLabel = new System.Windows.Forms.Label();
            this.BitrateLabel = new System.Windows.Forms.Label();
            this.LengthLabel = new System.Windows.Forms.Label();
            this.SoundsDataGrid = new System.Windows.Forms.DataGridView();
            this.Location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Identity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bitrate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Frequency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumChans = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SongObject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadCacheButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SoundsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // SaveToWavButton
            // 
            this.SaveToWavButton.Location = new System.Drawing.Point(2, 635);
            this.SaveToWavButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveToWavButton.Name = "SaveToWavButton";
            this.SaveToWavButton.Size = new System.Drawing.Size(180, 43);
            this.SaveToWavButton.TabIndex = 1;
            this.SaveToWavButton.Text = "Save To File";
            this.SaveToWavButton.UseVisualStyleBackColor = true;
            this.SaveToWavButton.Click += new System.EventHandler(this.SaveToWavButtonClick);
            // 
            // SaveAllSoundsButton
            // 
            this.SaveAllSoundsButton.Location = new System.Drawing.Point(192, 635);
            this.SaveAllSoundsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveAllSoundsButton.Name = "SaveAllSoundsButton";
            this.SaveAllSoundsButton.Size = new System.Drawing.Size(180, 43);
            this.SaveAllSoundsButton.TabIndex = 2;
            this.SaveAllSoundsButton.Text = "Save All";
            this.SaveAllSoundsButton.UseVisualStyleBackColor = true;
            this.SaveAllSoundsButton.Click += new System.EventHandler(this.SaveAllSoundsButtonClick);
            // 
            // SavingSoundFileLabel
            // 
            this.SavingSoundFileLabel.AutoSize = true;
            this.SavingSoundFileLabel.Location = new System.Drawing.Point(380, 646);
            this.SavingSoundFileLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SavingSoundFileLabel.Name = "SavingSoundFileLabel";
            this.SavingSoundFileLabel.Size = new System.Drawing.Size(0, 20);
            this.SavingSoundFileLabel.TabIndex = 3;
            // 
            // PlaySoundFileButton
            // 
            this.PlaySoundFileButton.Location = new System.Drawing.Point(4, 504);
            this.PlaySoundFileButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PlaySoundFileButton.Name = "PlaySoundFileButton";
            this.PlaySoundFileButton.Size = new System.Drawing.Size(180, 43);
            this.PlaySoundFileButton.TabIndex = 4;
            this.PlaySoundFileButton.Text = "Play Sound";
            this.PlaySoundFileButton.UseVisualStyleBackColor = true;
            this.PlaySoundFileButton.Click += new System.EventHandler(this.PlaySoundFileButtonClick);
            // 
            // SelectedSoundLabel
            // 
            this.SelectedSoundLabel.AutoSize = true;
            this.SelectedSoundLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectedSoundLabel.Location = new System.Drawing.Point(192, 508);
            this.SelectedSoundLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SelectedSoundLabel.Name = "SelectedSoundLabel";
            this.SelectedSoundLabel.Size = new System.Drawing.Size(238, 29);
            this.SelectedSoundLabel.TabIndex = 5;
            this.SelectedSoundLabel.Text = "Selected Sound File:";
            // 
            // FrequencyLabel
            // 
            this.FrequencyLabel.AutoSize = true;
            this.FrequencyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FrequencyLabel.Location = new System.Drawing.Point(572, 578);
            this.FrequencyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FrequencyLabel.Name = "FrequencyLabel";
            this.FrequencyLabel.Size = new System.Drawing.Size(139, 29);
            this.FrequencyLabel.TabIndex = 6;
            this.FrequencyLabel.Text = "Frequency: ";
            // 
            // BitrateLabel
            // 
            this.BitrateLabel.AutoSize = true;
            this.BitrateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BitrateLabel.Location = new System.Drawing.Point(572, 609);
            this.BitrateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BitrateLabel.Name = "BitrateLabel";
            this.BitrateLabel.Size = new System.Drawing.Size(88, 29);
            this.BitrateLabel.TabIndex = 7;
            this.BitrateLabel.Text = "Bitrate:";
            // 
            // LengthLabel
            // 
            this.LengthLabel.AutoSize = true;
            this.LengthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LengthLabel.Location = new System.Drawing.Point(572, 639);
            this.LengthLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LengthLabel.Name = "LengthLabel";
            this.LengthLabel.Size = new System.Drawing.Size(208, 29);
            this.LengthLabel.TabIndex = 8;
            this.LengthLabel.Text = "Length ( in bytes ):";
            // 
            // SoundsDataGrid
            // 
            this.SoundsDataGrid.AllowUserToDeleteRows = false;
            this.SoundsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SoundsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Location,
            this.Identity,
            this.Bitrate,
            this.Frequency,
            this.NumChans,
            this.DataLength,
            this.SongObject});
            this.SoundsDataGrid.Location = new System.Drawing.Point(4, 28);
            this.SoundsDataGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SoundsDataGrid.Name = "SoundsDataGrid";
            this.SoundsDataGrid.ReadOnly = true;
            this.SoundsDataGrid.Size = new System.Drawing.Size(1170, 451);
            this.SoundsDataGrid.TabIndex = 9;
            this.SoundsDataGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.SoundsDataGridRowEnter);
            // 
            // Location
            // 
            this.Location.HeaderText = "Location";
            this.Location.Name = "Location";
            this.Location.ReadOnly = true;
            // 
            // Identity
            // 
            this.Identity.HeaderText = "Identity";
            this.Identity.Name = "Identity";
            this.Identity.ReadOnly = true;
            // 
            // Bitrate
            // 
            this.Bitrate.HeaderText = "Bitrate";
            this.Bitrate.Name = "Bitrate";
            this.Bitrate.ReadOnly = true;
            // 
            // Frequency
            // 
            this.Frequency.HeaderText = "Frequency";
            this.Frequency.Name = "Frequency";
            this.Frequency.ReadOnly = true;
            // 
            // NumChans
            // 
            this.NumChans.HeaderText = "No. Channels";
            this.NumChans.Name = "NumChans";
            this.NumChans.ReadOnly = true;
            // 
            // DataLength
            // 
            this.DataLength.HeaderText = "Length";
            this.DataLength.Name = "DataLength";
            this.DataLength.ReadOnly = true;
            // 
            // SongObject
            // 
            this.SongObject.HeaderText = "";
            this.SongObject.Name = "SongObject";
            this.SongObject.ReadOnly = true;
            this.SongObject.Visible = false;
            // 
            // LoadCacheButton
            // 
            this.LoadCacheButton.Location = new System.Drawing.Point(961, 504);
            this.LoadCacheButton.Name = "LoadCacheButton";
            this.LoadCacheButton.Size = new System.Drawing.Size(213, 43);
            this.LoadCacheButton.TabIndex = 10;
            this.LoadCacheButton.Text = "Load Cache";
            this.LoadCacheButton.UseVisualStyleBackColor = true;
            this.LoadCacheButton.Click += new System.EventHandler(this.LoadCacheButton_Click);
            // 
            // SoundControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LoadCacheButton);
            this.Controls.Add(this.SoundsDataGrid);
            this.Controls.Add(this.LengthLabel);
            this.Controls.Add(this.BitrateLabel);
            this.Controls.Add(this.FrequencyLabel);
            this.Controls.Add(this.SelectedSoundLabel);
            this.Controls.Add(this.PlaySoundFileButton);
            this.Controls.Add(this.SavingSoundFileLabel);
            this.Controls.Add(this.SaveAllSoundsButton);
            this.Controls.Add(this.SaveToWavButton);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SoundControl";
            this.Size = new System.Drawing.Size(1204, 689);
            ((System.ComponentModel.ISupportInitialize)(this.SoundsDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveToWavButton;
        private System.Windows.Forms.Button SaveAllSoundsButton;
        private System.Windows.Forms.Label SavingSoundFileLabel;
        private System.Windows.Forms.Button PlaySoundFileButton;
        private System.Windows.Forms.Label SelectedSoundLabel;
        private System.Windows.Forms.Label FrequencyLabel;
        private System.Windows.Forms.Label BitrateLabel;
        private System.Windows.Forms.Label LengthLabel;
        private System.Windows.Forms.DataGridView SoundsDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Location;
        private System.Windows.Forms.DataGridViewTextBoxColumn Identity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bitrate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Frequency;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumChans;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn SongObject;
        private System.Windows.Forms.Button LoadCacheButton;

    }
}
