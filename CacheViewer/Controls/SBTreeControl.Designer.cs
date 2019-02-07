namespace CacheViewer.Controls
{
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
            this.SaveButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LoadingPictureBox = new System.Windows.Forms.PictureBox();
            this.SaveTypeRadioButton2 = new System.Windows.Forms.RadioButton();
            this.SaveTypeRadioButton1 = new System.Windows.Forms.RadioButton();
            this.CacheObjectTreeView = new System.Windows.Forms.TreeView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SaveButton.Location = new System.Drawing.Point(0, 52);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(350, 42);
            this.SaveButton.TabIndex = 0;
            this.SaveButton.Text = "Save Object";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LoadingPictureBox);
            this.panel1.Controls.Add(this.SaveTypeRadioButton2);
            this.panel1.Controls.Add(this.SaveTypeRadioButton1);
            this.panel1.Controls.Add(this.SaveButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 543);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 94);
            this.panel1.TabIndex = 1;
            // 
            // LoadingPictureBox
            // 
            this.LoadingPictureBox.Image = global::CacheViewer.Properties.Resources.SbSpinner;
            this.LoadingPictureBox.Location = new System.Drawing.Point(277, 5);
            this.LoadingPictureBox.Name = "LoadingPictureBox";
            this.LoadingPictureBox.Size = new System.Drawing.Size(51, 48);
            this.LoadingPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LoadingPictureBox.TabIndex = 3;
            this.LoadingPictureBox.TabStop = false;
            // 
            // SaveTypeRadioButton2
            // 
            this.SaveTypeRadioButton2.AutoSize = true;
            this.SaveTypeRadioButton2.Location = new System.Drawing.Point(18, 28);
            this.SaveTypeRadioButton2.Name = "SaveTypeRadioButton2";
            this.SaveTypeRadioButton2.Size = new System.Drawing.Size(143, 17);
            this.SaveTypeRadioButton2.TabIndex = 2;
            this.SaveTypeRadioButton2.TabStop = true;
            this.SaveTypeRadioButton2.Text = "save as Indiviual meshes";
            this.SaveTypeRadioButton2.UseVisualStyleBackColor = true;
            // 
            // SaveTypeRadioButton1
            // 
            this.SaveTypeRadioButton1.AutoSize = true;
            this.SaveTypeRadioButton1.Location = new System.Drawing.Point(18, 5);
            this.SaveTypeRadioButton1.Name = "SaveTypeRadioButton1";
            this.SaveTypeRadioButton1.Size = new System.Drawing.Size(111, 17);
            this.SaveTypeRadioButton1.TabIndex = 1;
            this.SaveTypeRadioButton1.TabStop = true;
            this.SaveTypeRadioButton1.Text = "save as one mesh";
            this.SaveTypeRadioButton1.UseVisualStyleBackColor = true;
            // 
            // CacheObjectTreeView
            // 
            this.CacheObjectTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CacheObjectTreeView.Location = new System.Drawing.Point(0, 0);
            this.CacheObjectTreeView.Name = "CacheObjectTreeView";
            this.CacheObjectTreeView.Size = new System.Drawing.Size(350, 543);
            this.CacheObjectTreeView.TabIndex = 0;
            this.CacheObjectTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.CacheObjectTreeView_AfterSelect);
            // 
            // SBTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CacheObjectTreeView);
            this.Controls.Add(this.panel1);
            this.MaximumSize = new System.Drawing.Size(350, 637);
            this.MinimumSize = new System.Drawing.Size(350, 637);
            this.Name = "SBTreeControl";
            this.Size = new System.Drawing.Size(350, 637);
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
    }
}
