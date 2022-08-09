namespace ControlExtensions
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    public static class ButtonBaseExtensions
    {
        /// <summary>
        /// Sets the color of the use visual style back.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetUseVisualStyleBackColor(this ButtonBase control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetUseVisualStyleBackColor(value)));
            }
            else
            {
                control.UseVisualStyleBackColor = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetText(this ButtonBase control, string value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetText(value)));
            }
            else
            {
                control.Text = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the text align.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetTextAlign(this ButtonBase control, ContentAlignment value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetTextAlign(value)));
            }
            else
            {
                control.TextAlign = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the text image relation.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetTextImageRelation(this ButtonBase control, TextImageRelation value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetTextImageRelation(value)));
            }
            else
            {
                control.TextImageRelation = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the use compatible text rendering.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetUseCompatibleTextRendering(this ButtonBase control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetUseCompatibleTextRendering(value)));
            }
            else
            {
                control.UseCompatibleTextRendering = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the use mnemonic.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetUseMnemonic(this ButtonBase control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetUseMnemonic(value)));
            }
            else
            {
                control.UseMnemonic = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the IME mode.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetImeMode(this ButtonBase control, ImeMode value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetImeMode(value)));
            }
            else
            {
                control.ImeMode = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the flat style.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetFlatStyle(this ButtonBase control, FlatStyle value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetFlatStyle(value)));
            }
            else
            {
                control.FlatStyle = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetImage(this ButtonBase control, Image value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetImage(value)));
            }
            else
            {
                control.Image = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the image align.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetImageAlign(this ButtonBase control, ContentAlignment value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetImageAlign(value)));
            }
            else
            {
                control.ImageAlign = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the index of the image.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetImageIndex(this ButtonBase control, int value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetImageIndex(value)));
            }
            else
            {
                control.ImageIndex = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the image key.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetImageKey(this ButtonBase control, string value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetImageKey(value)));
            }
            else
            {
                control.ImageKey = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the image list.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetImageList(this ButtonBase control, ImageList value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetImageList(value)));
            }
            else
            {
                control.ImageList = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the color of the back.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetBackColor(this ButtonBase control, Color value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetBackColor(value)));
            }
            else
            {
                control.BackColor = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the size of the automatic.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAutoSize(this ButtonBase control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoSize(value)));
            }
            else
            {
                control.AutoSize = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the automatic ellipsis.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAutoEllipsis(this ButtonBase control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoEllipsis(value)));
            }
            else
            {
                control.AutoEllipsis = value;
                control.Refresh();
            }
        }
    }

}