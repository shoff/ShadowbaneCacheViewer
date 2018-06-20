namespace ControlExtensions
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    public static class RadioButtonExtensions
    {
        /// <summary>
        /// Sets the appearance.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetAppearance(this RadioButton control, Appearance value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAppearance(value)));
            }
            else
            {
                control.Appearance = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the automatic check.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAutoCheck(this RadioButton control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoCheck(value)));
            }
            else
            {
                control.AutoCheck = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the check align.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetCheckAlign(this RadioButton control, ContentAlignment value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetCheckAlign(value)));
            }
            else
            {
                control.CheckAlign = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the checked.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetChecked(this RadioButton control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetChecked(value)));
            }
            else
            {
                control.Checked = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the text align.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetTextAlign(this RadioButton control, ContentAlignment value)
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
        /// Sets the tab stop.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetTabStop(this RadioButton control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetTabStop(value)));
            }
            else
            {
                control.TabStop = value;
                control.Refresh();
            }
        }
    }
}