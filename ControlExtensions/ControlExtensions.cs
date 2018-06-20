namespace ControlExtensions
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public static class ControlExtensions
    {
        public static void SetAccessibleDefaultActionDescription(this Control control, string accessibleDescription)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAccessibleDefaultActionDescription(accessibleDescription)));
            }
            else
            {
                control.AccessibleDefaultActionDescription = accessibleDescription;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the accessible description.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="accessibleDescription">The accessible description.</param>
        public static void SetAccessibleDescription(this Control control, string accessibleDescription)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAccessibleDescription(accessibleDescription)));
            }
            else
            {
                control.AccessibleDescription = accessibleDescription;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the name of the accessible.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="accessibleName">Name of the accessible.</param>
        public static void SetAccessibleName(this Control control, string accessibleName)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAccessibleName(accessibleName)));
            }
            else
            {
                control.AccessibleName = accessibleName;
                control.Refresh();
            }
        }

        /// <summary>
        /// Sets the accessible role.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="accessibleRole">The accessible role.</param>
        public static void SetAccessibleRole(this Control control, AccessibleRole accessibleRole)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAccessibleRole(accessibleRole)));
            }
            else
            {
                control.AccessibleRole = accessibleRole;
                control.Refresh();
            }
        }
        
        /// <summary>
        ///     Sets the visibility.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        public static void SetVisibility(this Control control, bool visible)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetVisibility(visible)));
            }
            else
            {
                control.Visible = visible;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the size of the automatic.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="autoSize">if set to <c>true</c> [automatic size].</param>
        public static void SetAutoSize(this Control control, bool autoSize)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoSize(autoSize)));
            }
            else
            {
                control.AutoSize = autoSize;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the allow drop.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="allow">if set to <c>true</c> [allow].</param>
        public static void SetAllowDrop(this Control control, bool allow)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAllowDrop(allow)));
            }
            else
            {
                control.AllowDrop = allow;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the color of the back.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="color">The color.</param>
        public static void SetBackColor(this Control control, Color color)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetBackColor(color)));
            }
            else
            {
                control.BackColor = color;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the color of the fore.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="color">The color.</param>
        public static void SetForeColor(this Control control, Color color)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetForeColor(color)));
            }
            else
            {
                control.ForeColor = color;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the background image.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="image">The image.</param>
        public static void SetBackgroundImage(this Control control, Image image)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetBackgroundImage(image)));
            }
            else
            {
                control.BackgroundImage = image;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the background image layout.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="imageLayout">The image layout.</param>
        public static void SetBackgroundImageLayout(this Control control, ImageLayout imageLayout)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetBackgroundImageLayout(imageLayout)));
            }
            else
            {
                control.BackgroundImageLayout = imageLayout;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the bounds.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="bounds">The bounds.</param>
        public static void SetBounds(this Control control, Rectangle bounds)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetBounds(bounds)));
            }
            else
            {
                control.Bounds = bounds;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the binding context.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="bindingContext">The binding context.</param>
        public static void SetBindingContext(this Control control, BindingContext bindingContext)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetBindingContext(bindingContext)));
            }
            else
            {
                control.BindingContext = bindingContext;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="text">The text.</param>
        public static void SetUseWaitCursor(this Control control, string text)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetText(text)));
            }
            else
            {
                control.Text = text;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="text">The text.</param>
        public static void SetText(this Control control, string text)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetText(text)));
            }
            else
            {
                control.Text = text;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the top.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="top">The top.</param>
        public static void SetTop(this Control control, int top)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetTop(top)));
            }
            else
            {
                control.Top = top;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the tag.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="tag">The tag.</param>
        public static void SetTag(this Control control, object tag)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetTag(tag)));
            }
            else
            {
                control.Tag = tag;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the tab stop.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="stop">if set to <c>true</c> [stop].</param>
        public static void SetTabStop(this Control control, bool stop)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetTabStop(stop)));
            }
            else
            {
                control.TabStop = stop;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the cursor.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cursor">The cursor.</param>
        public static void SetCursor(this Control control, Cursor cursor)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetCursor(cursor)));
            }
            else
            {
                control.Cursor = cursor;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the context menu strip.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="contextMenuStrip">The context menu strip.</param>
        public static void SetContextMenuStrip(this Control control, ContextMenuStrip contextMenuStrip)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetContextMenuStrip(contextMenuStrip)));
            }
            else
            {
                control.ContextMenuStrip = contextMenuStrip;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the context menu.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="contextMenu">The context menu.</param>
        public static void SetContextMenu(this Control control, ContextMenu contextMenu)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetContextMenu(contextMenu)));
            }
            else
            {
                control.ContextMenu = contextMenu;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the dock.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="dockStyle">The dock style.</param>
        public static void SetDock(this Control control, DockStyle dockStyle)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetDock(dockStyle)));
            }
            else
            {
                control.Dock = dockStyle;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the enabled.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        public static void SetEnabled(this Control control, bool enabled)
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

        /// <summary>
        ///     Sets the causes validation.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        public static void SetCausesValidation(this Control control, bool enabled)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetCausesValidation(enabled)));
            }
            else
            {
                control.CausesValidation = enabled;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the capture.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="capture">if set to <c>true</c> [capture].</param>
        public static void SetCapture(this Control control, bool capture)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetCapture(capture)));
            }
            else
            {
                control.Capture = capture;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the index of the tab.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="tabIndex">Index of the tab.</param>
        public static void SetTabIndex(this Control control, int tabIndex)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetTabIndex(tabIndex)));
            }
            else
            {
                control.TabIndex = tabIndex;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the size.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="size">The size.</param>
        public static void SetSize(this Control control, Size size)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetSize(size)));
            }
            else
            {
                control.Size = size;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the site.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="site">The site.</param>
        public static void SetSite(this Control control, ISite site)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetSite(site)));
            }
            else
            {
                control.Site = site;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the right to left.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="rightToLeft">The right to left.</param>
        public static void SetRightToLeft(this Control control, RightToLeft rightToLeft)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetRightToLeft(rightToLeft)));
            }
            else
            {
                control.RightToLeft = rightToLeft;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the region.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="region">The region.</param>
        public static void SetRegion(this Control control, Region region)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetRegion(region)));
            }
            else
            {
                control.Region = region;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the parent.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="parent">The parent.</param>
        public static void SetParent(this Control control, Control parent)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetParent(parent)));
            }
            else
            {
                control.Parent = parent;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the name.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="name">The name.</param>
        public static void SetName(this Control control, string name)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetName(name)));
            }
            else
            {
                control.Name = name;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the minimum size.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="size">The size.</param>
        public static void SetMinimumSize(this Control control, Size size)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetMinimumSize(size)));
            }
            else
            {
                control.MinimumSize = size;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the size of the client.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="size">The size.</param>
        public static void SetClientSize(this Control control, Size size)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetClientSize(size)));
            }
            else
            {
                control.ClientSize = size;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the maximum size.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="size">The size.</param>
        public static void SetMaximumSize(this Control control, Size size)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetMaximumSize(size)));
            }
            else
            {
                control.MaximumSize = size;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the margin.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="padding">The padding.</param>
        public static void SetMargin(this Control control, Padding padding)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetMargin(padding)));
            }
            else
            {
                control.Padding = padding;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the location.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="point">The point.</param>
        public static void SetLocation(this Control control, Point point)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetLocation(point)));
            }
            else
            {
                control.Location = point;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the automatic scroll offset.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="point">The point.</param>
        public static void SetAutoScrollOffset(this Control control, Point point)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAutoScrollOffset(point)));
            }
            else
            {
                control.AutoScrollOffset = point;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the anchor.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="anchorStyles">The anchor styles.</param>
        public static void SetAnchor(this Control control, AnchorStyles anchorStyles)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetAnchor(anchorStyles)));
            }
            else
            {
                control.Anchor = anchorStyles;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the left.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="left">The left.</param>
        public static void SetLeft(this Control control, int left)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetLeft(left)));
            }
            else
            {
                control.Left = left;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the is accessible.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="isAccessible">if set to <c>true</c> [is accessible].</param>
        public static void SetIsAccessible(this Control control, bool isAccessible)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetIsAccessible(isAccessible)));
            }
            else
            {
                control.IsAccessible = isAccessible;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the IME mode.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="imeMode">The IME mode.</param>
        public static void SetImeMode(this Control control, ImeMode imeMode)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetImeMode(imeMode)));
            }
            else
            {
                control.ImeMode = imeMode;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the height.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="height">The height.</param>
        public static void SetHeight(this Control control, int height)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetHeight(height)));
            }
            else
            {
                control.Height = height;
                control.Refresh();
            }
        }

        /// <summary>
        ///     Sets the font.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="font">The font.</param>
        public static void SetFont(this Control control, Font font)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => control.SetFont(font)));
            }
            else
            {
                control.Font = font;
                control.Refresh();
            }
        }
    }
}