namespace ControlExtensions
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    public static class ToolStripExtensions
    {
        /// <summary>
        /// Sets the allow drop.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAllowDrop(this ToolStrip control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAllowDrop(value)));
            }
            else
            {
                control.AllowDrop = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the allow item reorder.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAllowItemReorder(this ToolStrip control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAllowItemReorder(value)));
            }
            else
            {
                control.AllowItemReorder = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the allow merge.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAllowMerge(this ToolStrip control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAllowMerge(value)));
            }
            else
            {
                control.AllowMerge = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the anchor.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetAnchor(this ToolStrip control, AnchorStyles value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAnchor(value)));
            }
            else
            {
                control.Anchor = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the automatic scroll.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAutoScroll(this ToolStrip control, bool value)
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
        public static void SetAutoScrollMargin(this ToolStrip control, Size value)
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
        public static void SetAutoScrollMinSize(this ToolStrip control, Size value)
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
        public static void SetAutoScrollPosition(this ToolStrip control, Point value)
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

        /// <summary>
        /// Sets the size of the automatic.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetAutoSize(this ToolStrip control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoSize(value)));
            }
            else
            {
                control.AutoSize = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the color of the back.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetBackColor(this ToolStrip control, Color value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetBackColor(value)));
            }
            else
            {
                control.BackColor = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the binding context.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetBindingContext(this ToolStrip control, BindingContext value)
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

        /// <summary>
        /// Sets the can overflow.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetCanOverflow(this ToolStrip control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetCanOverflow(value)));
            }
            else
            {
                control.CanOverflow = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the causes validation.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetCausesValidation(this ToolStrip control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetCausesValidation(value)));
            }
            else
            {
                control.CausesValidation = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the cursor.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetCursor(this ToolStrip control, Cursor value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetCursor(value)));
            }
            else
            {
                control.Cursor = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the default drop down direction.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetDefaultDropDownDirection(this ToolStrip control, ToolStripDropDownDirection value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetDefaultDropDownDirection(value)));
            }
            else
            {
                control.DefaultDropDownDirection = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the dock.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetDock(this ToolStrip control, DockStyle value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetDock(value)));
            }
            else
            {
                control.Dock = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetFont(this ToolStrip control, Font value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetFont(value)));
            }
            else
            {
                control.Font = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the color of the fore.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetForeColor(this ToolStrip control, Color value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetForeColor(value)));
            }
            else
            {
                control.ForeColor = value;
                control.Refresh();
            }
        }


        /// <summary>
        /// Sets the grip margin.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetGripMargin(this ToolStrip control, Padding value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetGripMargin(value)));
            }
            else
            {
                control.GripMargin = value;
                control.Refresh();
            }
        }


        /// <summary>
        /// Sets the grip style.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetGripStyle(this ToolStrip control, ToolStripGripStyle value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetGripStyle(value)));
            }
            else
            {
                control.GripStyle = value;
                control.Refresh();
            }
        }


        /// <summary>
        /// Sets the image list.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetImageList(this ToolStrip control, ImageList value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetImageList(value)));
            }
            else
            {
                control.ImageList = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the size of the image scaling.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetImageScalingSize(this ToolStrip control, Size value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetImageScalingSize(value)));
            }
            else
            {
                control.ImageScalingSize = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the layout settings.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetLayoutSettings(this ToolStrip control, LayoutSettings value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetLayoutSettings(value)));
            }
            else
            {
                control.LayoutSettings = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the layout style.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetLayoutStyle(this ToolStrip control, ToolStripLayoutStyle value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetLayoutStyle(value)));
            }
            else
            {
                control.LayoutStyle = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the renderer.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetRenderer(this ToolStrip control, ToolStripRenderer value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetRenderer(value)));
            }
            else
            {
                control.Renderer = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the render mode.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetRenderMode(this ToolStrip control, ToolStripRenderMode value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetRenderMode(value)));
            }
            else
            {
                control.RenderMode = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the show item tool tips.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetShowItemToolTips(this ToolStrip control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetShowItemToolTips(value)));
            }
            else
            {
                control.ShowItemToolTips = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the stretch.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetStretch(this ToolStrip control, bool value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetStretch(value)));
            }
            else
            {
                control.Stretch = value;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the tab stop.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetTabStop(this ToolStrip control, bool value)
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

        /// <summary>
        /// Sets the text direction.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        public static void SetTextDirection(this ToolStrip control, ToolStripTextDirection value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetTextDirection(value)));
            }
            else
            {
                control.TextDirection = value;
                control.Refresh();
            }
        }
    }
}