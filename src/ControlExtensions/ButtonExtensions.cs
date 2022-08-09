namespace ControlExtensions
{
    using System.Windows.Forms;

    public static class ButtonExtensions
    {
        /// <summary>
        /// Sets the dialog result.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetDialogResult(this Button control, DialogResult value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetDialogResult(value)));
            }
            else
            {
                control.DialogResult = value;
                control.Refresh();
            }
        }

        public static void SetEnabled(this Button control, bool enabled)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetEnabled(enabled)));
            }
            else
            {
                control.Enabled = enabled;
                control.Refresh();
            }
        }
    }
}