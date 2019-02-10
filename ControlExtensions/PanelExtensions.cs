namespace ControlExtensions
{
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    public static class PanelExtensions
    {
        /// <summary>
        /// Sets the automatic size mode.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetAutoSizeMode(this Panel control, AutoSizeMode value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoSizeMode(value)));
            }
            else
            {
                control.AutoSizeMode = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the border style.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetBorderStyle(this Panel control, BorderStyle value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetBorderStyle(value)));
            }
            else
            {
                control.BorderStyle = value;
                control.Refresh();
            }
        }

    }
}