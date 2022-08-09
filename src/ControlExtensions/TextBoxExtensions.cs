namespace ControlExtensions
{
    using System.Windows.Forms;

    /// <summary>
    /// </summary>
    public static class TextBoxExtensions
    {
        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="value">The value.</param>
        /// <example>
        ///   <code> this.textBox.SetText("Hello Async World");</code>
        /// </example>
        public static void SetText(this TextBox textBox, string value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke(new MethodInvoker(() => textBox.SetText(value)));
            }
            else
            {
                textBox.Text = value;
                textBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the accepts return.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAcceptsReturn(this TextBox textBox, bool value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke(new MethodInvoker(() => textBox.SetAcceptsReturn(value)));
            }
            else
            {
                textBox.AcceptsReturn = value;
                textBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the automatic complete custom source.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="value">The value.</param>
        public static void SetAutoCompleteCustomSource(this TextBox textBox, AutoCompleteStringCollection value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke(new MethodInvoker(() => textBox.SetAutoCompleteCustomSource(value)));
            }
            else
            {
                textBox.AutoCompleteCustomSource = value;
                textBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the automatic complete mode.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="value">The value.</param>
        public static void SetAutoCompleteMode(this TextBox textBox, AutoCompleteMode value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke(new MethodInvoker(() => textBox.SetAutoCompleteMode(value)));
            }
            else
            {
                textBox.AutoCompleteMode = value;
                textBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the automatic complete source.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="value">The value.</param>
        public static void SetAutoCompleteSource(this TextBox textBox, AutoCompleteSource value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke(new MethodInvoker(() => textBox.SetAutoCompleteSource(value)));
            }
            else
            {
                textBox.AutoCompleteSource = value;
                textBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the character casing.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="value">The value.</param>
        public static void SetCharacterCasing(this TextBox textBox, CharacterCasing value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke(new MethodInvoker(() => textBox.SetCharacterCasing(value)));
            }
            else
            {
                textBox.CharacterCasing = value;
                textBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the multiline.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public  static void SetMultiline(this TextBox textBox, bool value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke(new MethodInvoker(() => textBox.SetMultiline(value)));
            }
            else
            {
                textBox.Multiline = value;
                textBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the password character.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="value">The value.</param>
        public static void SetPasswordChar(this TextBox textBox, char value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke(new MethodInvoker(() => textBox.SetPasswordChar(value)));
            }
            else
            {
                textBox.PasswordChar = value;
                textBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the scroll bars.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="value">The value.</param>
        public static void SetScrollBars(this TextBox textBox, ScrollBars value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke(new MethodInvoker(() => textBox.SetScrollBars(value)));
            }
            else
            {
                textBox.ScrollBars = value;
                textBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the text align.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="value">The value.</param>
        public static void SetTextAlign(this TextBox textBox, HorizontalAlignment value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke(new MethodInvoker(() => textBox.SetTextAlign(value)));
            }
            else
            {
                textBox.TextAlign = value;
                textBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the use system password character.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetUseSystemPasswordChar(this TextBox textBox, bool value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke(new MethodInvoker(() => textBox.SetUseSystemPasswordChar(value)));
            }
            else
            {
                textBox.UseSystemPasswordChar = value;
                textBox.Refresh();
            }
        }


    }
}