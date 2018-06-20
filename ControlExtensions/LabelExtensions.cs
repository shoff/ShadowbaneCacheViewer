namespace ControlExtensions
{
    using System.Windows.Forms;

    public static class LabelExtensions
    {
        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="text">The text.</param>
        public static void SetText(this Label label, string text)
        {
            if (label.InvokeRequired)
            {
                label.BeginInvoke(new MethodInvoker(() => label.SetText(text)));
            }
            else
            {
                label.Text = text;
                label.Refresh();
            }
        } 
    }
}