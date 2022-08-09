namespace ControlExtensions
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// </summary>
    public static class RichTextBoxExtensions
    {
        /// <summary>
        /// Sets the allow drop.
        /// </summary>
        /// <param name="richTextBox">The control.</param>
        /// <param name="value">if set to <c>true</c> [allow].</param>
        public static void SetAllowDrop(this RichTextBox richTextBox, bool value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetAllowDrop(value)));
            }
            else
            {
                richTextBox.AllowDrop = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the automatic word selection.
        /// </summary>
        /// <param name="richTextBox">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAutoWordSelection(this RichTextBox richTextBox, bool value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetAutoWordSelection(value)));
            }
            else
            {
                richTextBox.AutoWordSelection = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the bullet indent.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetBulletIndent(this RichTextBox richTextBox, int value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetBulletIndent(value)));
            }
            else
            {
                richTextBox.BulletIndent = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the detect urls.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetDetectUrls(this RichTextBox richTextBox, bool value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetDetectUrls(value)));
            }
            else
            {
                richTextBox.DetectUrls = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the enable automatic drag drop.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetEnableAutoDragDrop(this RichTextBox richTextBox, bool value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetEnableAutoDragDrop(value)));
            }
            else
            {
                richTextBox.EnableAutoDragDrop = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetFont(this RichTextBox richTextBox, Font value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetFont(value)));
            }
            else
            {
                richTextBox.Font = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the color of the fore.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetForeColor(this RichTextBox richTextBox, Color value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetForeColor(value)));
            }
            else
            {
                richTextBox.ForeColor = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the language option.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetLanguageOption(this RichTextBox richTextBox, RichTextBoxLanguageOptions value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetLanguageOption(value)));
            }
            else
            {
                richTextBox.LanguageOption = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the maximum length.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetMaxLength(this RichTextBox richTextBox, int value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetMaxLength(value)));
            }
            else
            {
                richTextBox.MaxLength = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the rich text shortcuts enabled.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetRichTextShortcutsEnabled(this RichTextBox richTextBox, bool value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetRichTextShortcutsEnabled(value)));
            }
            else
            {
                richTextBox.RichTextShortcutsEnabled = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the right margin.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetRightMargin(this RichTextBox richTextBox, int value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetRightMargin(value)));
            }
            else
            {
                richTextBox.RightMargin = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the RTF.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetRtf(this RichTextBox richTextBox, string value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetRtf(value)));
            }
            else
            {
                richTextBox.Rtf = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the scroll bars.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetScrollBars(this RichTextBox richTextBox, RichTextBoxScrollBars value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetScrollBars(value)));
            }
            else
            {
                richTextBox.ScrollBars = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the selected RTF.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectedRtf(this RichTextBox richTextBox, string value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectedRtf(value)));
            }
            else
            {
                richTextBox.SelectedRtf = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the selected text.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectedText(this RichTextBox richTextBox, string value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectedText(value)));
            }
            else
            {
                richTextBox.SelectedText = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the selection alignment.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionAlignment(this RichTextBox richTextBox, HorizontalAlignment value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectionAlignment(value)));
            }
            else
            {
                richTextBox.SelectionAlignment = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the color of the selection back.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionBackColor(this RichTextBox richTextBox, Color value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectionBackColor(value)));
            }
            else
            {
                richTextBox.SelectionBackColor = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the selection bullet.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetSelectionBullet(this RichTextBox richTextBox, bool value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectionBullet(value)));
            }
            else
            {
                richTextBox.SelectionBullet = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the selection character offset.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionCharOffset(this RichTextBox richTextBox, int value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectionCharOffset(value)));
            }
            else
            {
                richTextBox.SelectionCharOffset = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the color of the selection.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionColor(this RichTextBox richTextBox, Color value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectionColor(value)));
            }
            else
            {
                richTextBox.SelectionColor = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the selection font.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionFont(this RichTextBox richTextBox, Font value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectionFont(value)));
            }
            else
            {
                richTextBox.SelectionFont = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the selection hanging indent.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionHangingIndent(this RichTextBox richTextBox, int value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectionHangingIndent(value)));
            }
            else
            {
                richTextBox.SelectionHangingIndent = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the selection indent.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionIndent(this RichTextBox richTextBox, int value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectionIndent(value)));
            }
            else
            {
                richTextBox.SelectionIndent = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the length of the selection.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionLength(this RichTextBox richTextBox, int value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectionLength(value)));
            }
            else
            {
                richTextBox.SelectionLength = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the selection protected.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetSelectionProtected(this RichTextBox richTextBox, bool value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectionProtected(value)));
            }
            else
            {
                richTextBox.SelectionProtected = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the selection right indent.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionRightIndent(this RichTextBox richTextBox, int value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectionRightIndent(value)));
            }
            else
            {
                richTextBox.SelectionRightIndent = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the selection tabs.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectionTabs(this RichTextBox richTextBox, int[] value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetSelectionTabs(value)));
            }
            else
            {
                richTextBox.SelectionTabs = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the show selection margin.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetShowSelectionMargin(this RichTextBox richTextBox, bool value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetShowSelectionMargin(value)));
            }
            else
            {
                richTextBox.ShowSelectionMargin = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetText(this RichTextBox richTextBox, string value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetText(value)));
            }
            else
            {
                richTextBox.Text = value;
                richTextBox.Refresh();
            }
        }

        /// <summary>
        /// Sets the zoom factor.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="value">The value.</param>
        public static void SetZoomFactor(this RichTextBox richTextBox, float value)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(() => richTextBox.SetZoomFactor(value)));
            }
            else
            {
                richTextBox.ZoomFactor = value;
                richTextBox.Refresh();
            }
        }
    }
}