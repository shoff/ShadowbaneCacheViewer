namespace CacheViewer
{
    partial class LogViewer
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
                this.logRepository.Dispose();
                this.dataContext.Dispose();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.LogsGridView = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateCreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DebugCheckBox = new System.Windows.Forms.CheckBox();
            this.InfoCheckBox = new System.Windows.Forms.CheckBox();
            this.WarnCheckBox = new System.Windows.Forms.CheckBox();
            this.ErrorCheckBox = new System.Windows.Forms.CheckBox();
            this.FatalCheckBox = new System.Windows.Forms.CheckBox();
            this.GetLogsButton = new System.Windows.Forms.Button();
            this.DeleteLogsButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LogsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // LogsGridView
            // 
            this.LogsGridView.AllowUserToAddRows = false;
            this.LogsGridView.AllowUserToDeleteRows = false;
            this.LogsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LogsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.DateCreated,
            this.LogLevel,
            this.Message});
            this.LogsGridView.Location = new System.Drawing.Point(146, 0);
            this.LogsGridView.Name = "LogsGridView";
            this.LogsGridView.Size = new System.Drawing.Size(959, 449);
            this.LogsGridView.TabIndex = 0;
            // 
            // Id
            // 
            this.Id.HeaderText = "id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // DateCreated
            // 
            dataGridViewCellStyle1.Format = "d";
            dataGridViewCellStyle1.NullValue = null;
            this.DateCreated.DefaultCellStyle = dataGridViewCellStyle1;
            this.DateCreated.HeaderText = "Created";
            this.DateCreated.Name = "DateCreated";
            // 
            // LogLevel
            // 
            this.LogLevel.HeaderText = "Log Level";
            this.LogLevel.Name = "LogLevel";
            // 
            // Message
            // 
            this.Message.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Message.HeaderText = "Message";
            this.Message.Name = "Message";
            // 
            // DebugCheckBox
            // 
            this.DebugCheckBox.AutoSize = true;
            this.DebugCheckBox.Location = new System.Drawing.Point(12, 38);
            this.DebugCheckBox.Name = "DebugCheckBox";
            this.DebugCheckBox.Size = new System.Drawing.Size(64, 17);
            this.DebugCheckBox.TabIndex = 1;
            this.DebugCheckBox.Text = "DEBUG";
            this.DebugCheckBox.UseVisualStyleBackColor = true;
            // 
            // InfoCheckBox
            // 
            this.InfoCheckBox.AutoSize = true;
            this.InfoCheckBox.Location = new System.Drawing.Point(12, 61);
            this.InfoCheckBox.Name = "InfoCheckBox";
            this.InfoCheckBox.Size = new System.Drawing.Size(51, 17);
            this.InfoCheckBox.TabIndex = 2;
            this.InfoCheckBox.Text = "INFO";
            this.InfoCheckBox.UseVisualStyleBackColor = true;
            // 
            // WarnCheckBox
            // 
            this.WarnCheckBox.AutoSize = true;
            this.WarnCheckBox.Location = new System.Drawing.Point(12, 85);
            this.WarnCheckBox.Name = "WarnCheckBox";
            this.WarnCheckBox.Size = new System.Drawing.Size(60, 17);
            this.WarnCheckBox.TabIndex = 3;
            this.WarnCheckBox.Text = "WARN";
            this.WarnCheckBox.UseVisualStyleBackColor = true;
            // 
            // ErrorCheckBox
            // 
            this.ErrorCheckBox.AutoSize = true;
            this.ErrorCheckBox.Checked = true;
            this.ErrorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ErrorCheckBox.Location = new System.Drawing.Point(13, 109);
            this.ErrorCheckBox.Name = "ErrorCheckBox";
            this.ErrorCheckBox.Size = new System.Drawing.Size(65, 17);
            this.ErrorCheckBox.TabIndex = 4;
            this.ErrorCheckBox.Text = "ERROR";
            this.ErrorCheckBox.UseVisualStyleBackColor = true;
            // 
            // FatalCheckBox
            // 
            this.FatalCheckBox.AutoSize = true;
            this.FatalCheckBox.Location = new System.Drawing.Point(12, 133);
            this.FatalCheckBox.Name = "FatalCheckBox";
            this.FatalCheckBox.Size = new System.Drawing.Size(59, 17);
            this.FatalCheckBox.TabIndex = 5;
            this.FatalCheckBox.Text = "FATAL";
            this.FatalCheckBox.UseVisualStyleBackColor = true;
            // 
            // GetLogsButton
            // 
            this.GetLogsButton.Location = new System.Drawing.Point(12, 198);
            this.GetLogsButton.Name = "GetLogsButton";
            this.GetLogsButton.Size = new System.Drawing.Size(116, 23);
            this.GetLogsButton.TabIndex = 6;
            this.GetLogsButton.Text = "Get Logs";
            this.GetLogsButton.UseVisualStyleBackColor = true;
            this.GetLogsButton.Click += new System.EventHandler(this.GetLogsButtonClick);
            // 
            // DeleteLogsButton
            // 
            this.DeleteLogsButton.Location = new System.Drawing.Point(13, 356);
            this.DeleteLogsButton.Name = "DeleteLogsButton";
            this.DeleteLogsButton.Size = new System.Drawing.Size(115, 23);
            this.DeleteLogsButton.TabIndex = 7;
            this.DeleteLogsButton.Text = "Delete All Logs";
            this.DeleteLogsButton.UseVisualStyleBackColor = true;
            this.DeleteLogsButton.Click += new System.EventHandler(this.DeleteLogsButtonClick);
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 449);
            this.Controls.Add(this.DeleteLogsButton);
            this.Controls.Add(this.GetLogsButton);
            this.Controls.Add(this.FatalCheckBox);
            this.Controls.Add(this.ErrorCheckBox);
            this.Controls.Add(this.WarnCheckBox);
            this.Controls.Add(this.InfoCheckBox);
            this.Controls.Add(this.DebugCheckBox);
            this.Controls.Add(this.LogsGridView);
            this.Name = "LogViewer";
            this.Text = "LogViewer";
            ((System.ComponentModel.ISupportInitialize)(this.LogsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView LogsGridView;
        private System.Windows.Forms.CheckBox DebugCheckBox;
        private System.Windows.Forms.CheckBox InfoCheckBox;
        private System.Windows.Forms.CheckBox WarnCheckBox;
        private System.Windows.Forms.CheckBox ErrorCheckBox;
        private System.Windows.Forms.CheckBox FatalCheckBox;
        private System.Windows.Forms.Button GetLogsButton;
        private System.Windows.Forms.Button DeleteLogsButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateCreated;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
    }
}