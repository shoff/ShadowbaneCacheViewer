using System;

namespace CacheViewer.Controls
{
    partial class SlimRenderControl
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
            try
            {
                if (dataStream != null)
                {
                    dataStream.Close();
                }

                if (!vertexBuffer.Disposed)
                {
                    vertexBuffer.Dispose();
                }

                if (!inputLayout.Disposed)
                {
                    inputLayout.Dispose();
                }

                vShaderSignature.Dispose();

                if (!vertexShader.Disposed)
                {
                    vertexShader.Dispose();
                }

                if (!cbChangePixelShader.Disposed)
                {
                    cbChangePixelShader.Dispose();
                }

                if (!this.renderTargetView.Disposed)
                {
                    this.renderTargetView.Dispose();
                }

                if (!swapChain.Disposed)
                {
                    swapChain.Dispose();
                }

                if (!device.Disposed)
                {
                    device.Dispose();
                }

                if (disposing && (components != null))
                {
                    components.Dispose();
                }

                base.Dispose(disposing);
            }
            catch (ObjectDisposedException)
            {
                /* nfi why this throws here */
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FormObject = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // gPanel
            // 
            this.FormObject.Location = new System.Drawing.Point(13, 13);
            this.FormObject.Name = "gPanel";
            this.FormObject.Size = new System.Drawing.Size(683, 538);
            this.FormObject.TabIndex = 0;
            // 
            // SlimRenderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 575);
            this.Controls.Add(this.FormObject);
            this.Name = "SlimRenderControl";
            this.Text = "SlimRenderControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel FormObject;
    }
}
