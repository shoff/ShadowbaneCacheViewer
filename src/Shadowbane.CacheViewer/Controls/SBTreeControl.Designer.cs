namespace Shadowbane.CacheViewer.Controls
{
    using Shadowbane.CacheViewer;

    partial class SBTreeControl
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
            this.SaveButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.InvalidCount = new System.Windows.Forms.Label();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.LoadingPictureBox = new System.Windows.Forms.PictureBox();
            this.SaveTypeRadioButton2 = new System.Windows.Forms.RadioButton();
            this.SaveTypeRadioButton1 = new System.Windows.Forms.RadioButton();
            this.CacheObjectTreeView = new System.Windows.Forms.TreeView();
            this.LoadCacheButton = new System.Windows.Forms.Button();
            this.ValidCacheCount = new System.Windows.Forms.Label();
            this.ValidCacheLable = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SaveButton.Location = new System.Drawing.Point(0, 124);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(408, 38);
            this.SaveButton.TabIndex = 0;
            this.SaveButton.Text = "Save Object";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ValidCacheCount);
            this.panel1.Controls.Add(this.ValidCacheLable);
            this.panel1.Controls.Add(this.LoadCacheButton);
            this.panel1.Controls.Add(this.InvalidCount);
            this.panel1.Controls.Add(this.ErrorLabel);
            this.panel1.Controls.Add(this.MessageLabel);
            this.panel1.Controls.Add(this.LoadingPictureBox);
            this.panel1.Controls.Add(this.SaveTypeRadioButton2);
            this.panel1.Controls.Add(this.SaveTypeRadioButton1);
            this.panel1.Controls.Add(this.SaveButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 573);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 162);
            this.panel1.TabIndex = 1;
            // 
            // InvalidCount
            // 
            this.InvalidCount.AutoSize = true;
            this.InvalidCount.Location = new System.Drawing.Point(176, 70);
            this.InvalidCount.Name = "InvalidCount";
            this.InvalidCount.Size = new System.Drawing.Size(13, 15);
            this.InvalidCount.TabIndex = 6;
            this.InvalidCount.Text = "0";
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.Location = new System.Drawing.Point(21, 70);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(149, 15);
            this.ErrorLabel.TabIndex = 5;
            this.ErrorLabel.Text = "invalid cache object count:";
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Location = new System.Drawing.Point(21, 54);
            this.MessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(38, 15);
            this.MessageLabel.TabIndex = 4;
            this.MessageLabel.Text = "label1";
            // 
            // LoadingPictureBox
            // 
            this.LoadingPictureBox.Image = global::Shadowbane.CacheViewer.Resources.SbSpinner;
            this.LoadingPictureBox.Location = new System.Drawing.Point(323, 6);
            this.LoadingPictureBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LoadingPictureBox.Name = "LoadingPictureBox";
            this.LoadingPictureBox.Size = new System.Drawing.Size(59, 55);
            this.LoadingPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LoadingPictureBox.TabIndex = 3;
            this.LoadingPictureBox.TabStop = false;
            // 
            // SaveTypeRadioButton2
            // 
            this.SaveTypeRadioButton2.AutoSize = true;
            this.SaveTypeRadioButton2.Location = new System.Drawing.Point(21, 32);
            this.SaveTypeRadioButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.SaveTypeRadioButton2.Name = "SaveTypeRadioButton2";
            this.SaveTypeRadioButton2.Size = new System.Drawing.Size(153, 19);
            this.SaveTypeRadioButton2.TabIndex = 2;
            this.SaveTypeRadioButton2.TabStop = true;
            this.SaveTypeRadioButton2.Text = "save as Indiviual meshes";
            this.SaveTypeRadioButton2.UseVisualStyleBackColor = true;
            // 
            // SaveTypeRadioButton1
            // 
            this.SaveTypeRadioButton1.AutoSize = true;
            this.SaveTypeRadioButton1.Location = new System.Drawing.Point(21, 6);
            this.SaveTypeRadioButton1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.SaveTypeRadioButton1.Name = "SaveTypeRadioButton1";
            this.SaveTypeRadioButton1.Size = new System.Drawing.Size(117, 19);
            this.SaveTypeRadioButton1.TabIndex = 1;
            this.SaveTypeRadioButton1.TabStop = true;
            this.SaveTypeRadioButton1.Text = "save as one mesh";
            this.SaveTypeRadioButton1.UseVisualStyleBackColor = true;
            // 
            // CacheObjectTreeView
            // 
            this.CacheObjectTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CacheObjectTreeView.Location = new System.Drawing.Point(0, 0);
            this.CacheObjectTreeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CacheObjectTreeView.Name = "CacheObjectTreeView";
            this.CacheObjectTreeView.Size = new System.Drawing.Size(408, 573);
            this.CacheObjectTreeView.TabIndex = 0;
            this.CacheObjectTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.CacheObjectTreeView_AfterSelect);
            // 
            // LoadCacheButton
            // 
            this.LoadCacheButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.LoadCacheButton.Location = new System.Drawing.Point(21, 90);
            this.LoadCacheButton.Name = "LoadCacheButton";
            this.LoadCacheButton.Size = new System.Drawing.Size(360, 33);
            this.LoadCacheButton.TabIndex = 2;
            this.LoadCacheButton.Text = "Load Cache";
            this.LoadCacheButton.UseVisualStyleBackColor = true;
            this.LoadCacheButton.Click += new System.EventHandler(this.LoadCacheButton_Click);
            // 
            // ValidCacheCount
            // 
            this.ValidCacheCount.AutoSize = true;
            this.ValidCacheCount.Location = new System.Drawing.Point(368, 70);
            this.ValidCacheCount.Name = "ValidCacheCount";
            this.ValidCacheCount.Size = new System.Drawing.Size(13, 15);
            this.ValidCacheCount.TabIndex = 8;
            this.ValidCacheCount.Text = "0";
            // 
            // ValidCacheLable
            // 
            this.ValidCacheLable.AutoSize = true;
            this.ValidCacheLable.Location = new System.Drawing.Point(213, 70);
            this.ValidCacheLable.Name = "ValidCacheLable";
            this.ValidCacheLable.Size = new System.Drawing.Size(139, 15);
            this.ValidCacheLable.TabIndex = 7;
            this.ValidCacheLable.Text = "valid cache object count:";
            // 
            // SBTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CacheObjectTreeView);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximumSize = new System.Drawing.Size(408, 735);
            this.MinimumSize = new System.Drawing.Size(408, 735);
            this.Name = "SBTreeControl";
            this.Size = new System.Drawing.Size(408, 735);
            this.Load += new System.EventHandler(this.SBTreeControl_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton SaveTypeRadioButton1;
        private System.Windows.Forms.RadioButton SaveTypeRadioButton2;
        private System.Windows.Forms.PictureBox LoadingPictureBox;
        private System.Windows.Forms.TreeView CacheObjectTreeView;
        private System.Windows.Forms.Label MessageLabel;
        private Label ErrorLabel;
        private Label InvalidCount;
        private Button LoadCacheButton;
        private Label ValidCacheCount;
        private Label ValidCacheLable;
    }
}
