namespace ControlExtensions
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// </summary>
    public static class PictureBoxExtensions
    {

        public static void SetVisible(this PictureBox pictureBox, bool visible)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.BeginInvoke(new MethodInvoker(() => pictureBox.Visible = visible));
            }
            else
            {
                pictureBox.Visible = visible;
                pictureBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the border style.
        /// </summary>
        /// <param name="pictureBox">The picture box.</param>
        /// <param name="value">The value.</param>
        public static void SetBorderStyle(this PictureBox pictureBox, BorderStyle value)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.BeginInvoke(new MethodInvoker(() => pictureBox.SetBorderStyle(value)));
            }
            else
            {
                pictureBox.BorderStyle = value;
                pictureBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the causes validation.
        /// </summary>
        /// <param name="pictureBox">The picture box.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetCausesValidation(this PictureBox pictureBox, bool value)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.BeginInvoke(new MethodInvoker(() => pictureBox.SetCausesValidation(value)));
            }
            else
            {
                pictureBox.CausesValidation = value;
                pictureBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the error image.
        /// </summary>
        /// <param name="pictureBox">The picture box.</param>
        /// <param name="value">The value.</param>
        public static void SetErrorImage(this PictureBox pictureBox, Image value)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.BeginInvoke(new MethodInvoker(() => pictureBox.SetErrorImage(value)));
            }
            else
            {
                pictureBox.ErrorImage = value;
                pictureBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="pictureBox">The picture box.</param>
        /// <param name="value">The value.</param>
        public static void SetImage(this PictureBox pictureBox, Image value)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.BeginInvoke(new MethodInvoker(() => pictureBox.SetImage(value)));
            }
            else
            {
                pictureBox.Image = value;
                pictureBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the image location.
        /// </summary>
        /// <param name="pictureBox">The picture box.</param>
        /// <param name="value">The value.</param>
        public static void SetImageLocation(this PictureBox pictureBox, string value)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.BeginInvoke(new MethodInvoker(() => pictureBox.SetImageLocation(value)));
            }
            else
            {
                pictureBox.ImageLocation = value;
                pictureBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the IME mode.
        /// </summary>
        /// <param name="pictureBox">The picture box.</param>
        /// <param name="value">The value.</param>
        public static void SetImeMode(this PictureBox pictureBox, ImeMode value)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.BeginInvoke(new MethodInvoker(() => pictureBox.SetImeMode(value)));
            }
            else
            {
                pictureBox.ImeMode = value;
                pictureBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the initial image.
        /// </summary>
        /// <param name="pictureBox">The picture box.</param>
        /// <param name="value">The value.</param>
        public static void SetInitialImage(this PictureBox pictureBox, Image value)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.BeginInvoke(new MethodInvoker(() => pictureBox.SetInitialImage(value)));
            }
            else
            {
                pictureBox.InitialImage = value;
                pictureBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the size mode.
        /// </summary>
        /// <param name="pictureBox">The picture box.</param>
        /// <param name="value">The value.</param>
        public static void SetSizeMode(this PictureBox pictureBox, PictureBoxSizeMode value)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.BeginInvoke(new MethodInvoker(() => pictureBox.SetSizeMode(value)));
            }
            else
            {
                pictureBox.SizeMode = value;
                pictureBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the wait on load.
        /// </summary>
        /// <param name="pictureBox">The picture box.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetWaitOnLoad(this PictureBox pictureBox, bool value)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.BeginInvoke(new MethodInvoker(() => pictureBox.SetWaitOnLoad(value)));
            }
            else
            {
                pictureBox.WaitOnLoad = value;
                pictureBox.Refresh();
            }
        }

    }
}