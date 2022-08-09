namespace ControlExtensions
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    public static class ContainerControlExtensions
    {
        /// <summary>
        /// Sets the active control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetActiveControl(this ContainerControl control, Control value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetActiveControl(value)));
            }
            else
            {
                control.ActiveControl = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the automatic scale dimensions.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetAutoScaleDimensions(this ContainerControl control, SizeF value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoScaleDimensions(value)));
            }
            else
            {
                control.AutoScaleDimensions = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the automatic scale mode.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetAutoScaleMode(this ContainerControl control, AutoScaleMode value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoScaleMode(value)));
            }
            else
            {
                control.AutoScaleMode = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the automatic validate.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetAutoValidate(this ContainerControl control, AutoValidate value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoValidate(value)));
            }
            else
            {
                control.AutoValidate = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the binding context.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetBindingContext(this ContainerControl control, BindingContext value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetBindingContext(value)));
            }
            else
            {
                control.BindingContext = value;
                control.Refresh();
            }
        }
    }
}