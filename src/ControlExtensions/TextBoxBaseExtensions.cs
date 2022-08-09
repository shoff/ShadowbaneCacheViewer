namespace ControlExtensions
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// </summary>
    public static class TextBoxBaseExtensions
    {
        /// <summary>
        /// Sets the accepts tab.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAcceptsTab(this TextBoxBase textBoxBase, bool value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetAcceptsTab(value)));
            }
            else
            {
                textBoxBase.AcceptsTab = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the size of the automatic.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAutoSize(this TextBoxBase textBoxBase, bool value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetAutoSize(value)));
            }
            else
            {
                textBoxBase.AutoSize = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the color of the back.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">The value.</param>
        public static void SetBackColor(this TextBoxBase textBoxBase, Color value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetBackColor(value)));
            }
            else
            {
                textBoxBase.BackColor = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the background image.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">The value.</param>
        public static void SetBackgroundImage(this TextBoxBase textBoxBase, Image value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetBackgroundImage(value)));
            }
            else
            {
                textBoxBase.BackgroundImage = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the background image layout.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">The value.</param>
        public static void SetBackgroundImageLayout(this TextBoxBase textBoxBase, ImageLayout value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetBackgroundImageLayout(value)));
            }
            else
            {
                textBoxBase.BackgroundImageLayout = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the border style.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">The value.</param>
        public static void SetBorderStyle(this TextBoxBase textBoxBase, BorderStyle value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetBorderStyle(value)));
            }
            else
            {
                textBoxBase.BorderStyle = value;
                textBoxBase.Refresh();
            }
        }


        /// <summary>
        /// Sets the color of the fore.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">The value.</param>
        public static void SetForeColor(this TextBoxBase textBoxBase, Color value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetForeColor(value)));
            }
            else
            {
                textBoxBase.ForeColor = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the hide selection.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetHideSelection(this TextBoxBase textBoxBase, bool value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetHideSelection(value)));
            }
            else
            {
                textBoxBase.HideSelection = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the lines.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">The value.</param>
        public static void SetLines(this TextBoxBase textBoxBase, IEnumerable<string> value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetLines(value)));
            }
            else
            {
                textBoxBase.Lines = value.ToArray();
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the maximum length.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">The value.</param>
        public static void SetMaxLength(this TextBoxBase textBoxBase, int value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetMaxLength(value)));
            }
            else
            {
                textBoxBase.MaxLength = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the modified.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetModified(this TextBoxBase textBoxBase, bool value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetModified(value)));
            }
            else
            {
                textBoxBase.Modified = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the multiline.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetMultiline(this TextBoxBase textBoxBase, bool value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetMultiline(value)));
            }
            else
            {
                textBoxBase.Multiline = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the padding.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">The value.</param>
        public static void SetPadding(this TextBoxBase textBoxBase, Padding value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetPadding(value)));
            }
            else
            {
                textBoxBase.Padding = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the read only.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetReadOnly(this TextBoxBase textBoxBase, bool value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetReadOnly(value)));
            }
            else
            {
                textBoxBase.ReadOnly = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the selected text.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectedText(this TextBoxBase textBoxBase, string value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetSelectedText(value)));
            }
            else
            {
                textBoxBase.SelectedText = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the length of the selection.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionLength(this TextBoxBase textBoxBase, int value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetSelectionLength(value)));
            }
            else
            {
                textBoxBase.SelectionLength = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the selection start.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionStart(this TextBoxBase textBoxBase, int value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetSelectionStart(value)));
            }
            else
            {
                textBoxBase.SelectionStart = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the shortcuts enabled.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetShortcutsEnabled(this TextBoxBase textBoxBase, bool value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetShortcutsEnabled(value)));
            }
            else
            {
                textBoxBase.ShortcutsEnabled = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">The value.</param>
        public static void SetText(this TextBoxBase textBoxBase, string value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetText(value)));
            }
            else
            {
                textBoxBase.Text = value;
                textBoxBase.Refresh();
            }
        }

        /// <summary>
        /// Sets the word wrap.
        /// </summary>
        /// <param name="textBoxBase">The text box base.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetWordWrap(this TextBoxBase textBoxBase, bool value)
        {
            if (textBoxBase.InvokeRequired)
            {
                textBoxBase.BeginInvoke(new MethodInvoker(() => textBoxBase.SetWordWrap(value)));
            }
            else
            {
                textBoxBase.WordWrap = value;
                textBoxBase.Refresh();
            }
        }

    }
}