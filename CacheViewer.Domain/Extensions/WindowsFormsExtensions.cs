namespace CacheViewer.Domain.Extensions
{
    using System.Windows.Forms;

    public static class WindowsFormsExtensions
    {
        /// <summary>
        ///     Allows setting a cross threaded message to a control.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="message"></param>
        public static void SetCrossThreadedMessage(this Control control, string message)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => SetCrossThreadedMessage(control, message)));
            }
            else
            {
                control.Text = message;
            }
        }
    }
}