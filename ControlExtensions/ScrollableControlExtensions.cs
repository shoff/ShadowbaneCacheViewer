namespace ControlExtensions
{
    using System.Drawing;
    using System.Windows.Forms;

    public static class ScrollableControlExtensions
    {
        /// <summary>
        /// Sets the automatic scroll.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
         public static void SetAutoScroll(this ScrollableControl control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoScroll(value)));
            }
            else
            {
                control.AutoScroll = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the automatic scroll margin.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetAutoScrollMargin(this ScrollableControl control, Size value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoScrollMargin(value)));
            }
            else
            {
                control.AutoScrollMargin = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the minimum size of the automatic scroll.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetAutoScrollMinSize(this ScrollableControl control, Size value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoScrollMinSize(value)));
            }
            else
            {
                control.AutoScrollMinSize = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the automatic scroll position.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetAutoScrollPosition(this ScrollableControl control, Point value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoScrollPosition(value)));
            }
            else
            {
                control.AutoScrollPosition = value;
                control.Refresh();
            }
        }
    }
}