namespace CacheViewer
{
    partial class SBCacheObjectForm
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
                this.sbTreeControl1.OnCacheObjectSelected -= CacheObjectSelected;
                this.sbTreeControl1.OnLoadingMessage -= LoadingMessageReceived;
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
            this.sbTreeControl1 = new CacheViewer.Controls.SBTreeControl();
            this.panelContainer1 = new CacheViewer.Code.PanelContainer();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.insideMover1 = new CacheViewer.Code.InsideMover(this.components);
            this.panelContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sbTreeControl1
            // 
            this.sbTreeControl1.Location = new System.Drawing.Point(12, 12);
            this.sbTreeControl1.MaximumSize = new System.Drawing.Size(268, 637);
            this.sbTreeControl1.MinimumSize = new System.Drawing.Size(268, 637);
            this.sbTreeControl1.Name = "sbTreeControl1";
            this.sbTreeControl1.SelectedCacheObject = null;
            this.sbTreeControl1.Size = new System.Drawing.Size(268, 637);
            this.sbTreeControl1.TabIndex = 1;
            // 
            // panelContainer1
            // 
            this.panelContainer1.Controls.Add(this.MessageLabel);
            this.panelContainer1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelContainer1.Location = new System.Drawing.Point(301, 0);
            this.panelContainer1.Name = "panelContainer1";
            this.panelContainer1.Size = new System.Drawing.Size(813, 681);
            this.panelContainer1.TabIndex = 0;
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessageLabel.Location = new System.Drawing.Point(13, 656);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(46, 18);
            this.MessageLabel.TabIndex = 0;
            this.MessageLabel.Text = "label1";
            // 
            // insideMover1
            // 
            this.insideMover1.ControlToMove = null;
            // 
            // SBCacheObjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 681);
            this.Controls.Add(this.sbTreeControl1);
            this.Controls.Add(this.panelContainer1);
            this.Name = "SBCacheObjectForm";
            this.Text = "SBCacheObjectForm";
            this.panelContainer1.ResumeLayout(false);
            this.panelContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Code.InsideMover insideMover1;
        private Code.PanelContainer panelContainer1;
        private Controls.SBTreeControl sbTreeControl1;
        private System.Windows.Forms.Label MessageLabel;
    }
}