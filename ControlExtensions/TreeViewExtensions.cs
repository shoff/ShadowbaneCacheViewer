namespace ControlExtensions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    public static class TreeViewExtensions
    {
        /// <summary>
        ///     Sets the border style.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="borderStyle">The border style.</param>
        public static void SetBorderStyle(this TreeView treeView, BorderStyle borderStyle)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetBorderStyle(borderStyle)));
            }
            else
            {
                treeView.BorderStyle = borderStyle;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the check boxes.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetCheckBoxes(this TreeView treeView, bool value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetCheckBoxes(value)));
            }
            else
            {
                treeView.CheckBoxes = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the draw mode.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetDrawMode(this TreeView treeView, TreeViewDrawMode value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetDrawMode(value)));
            }
            else
            {
                treeView.DrawMode = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the full row select.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetFullRowSelect(this TreeView treeView, bool value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetFullRowSelect(value)));
            }
            else
            {
                treeView.FullRowSelect = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the hide selection.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetHideSelection(this TreeView treeView, bool value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetHideSelection(value)));
            }
            else
            {
                treeView.HideSelection = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the hot tracking.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetHotTracking(this TreeView treeView, bool value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetHotTracking(value)));
            }
            else
            {
                treeView.HotTracking = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the index of the image.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetImageIndex(this TreeView treeView, int value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetImageIndex(value)));
            }
            else
            {
                treeView.ImageIndex = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the image key.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetImageKey(this TreeView treeView, string value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetImageKey(value)));
            }
            else
            {
                treeView.ImageKey = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the image list.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetImageList(this TreeView treeView, ImageList value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetImageList(value)));
            }
            else
            {
                treeView.ImageList = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the indent.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetIndent(this TreeView treeView, int value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetIndent(value)));
            }
            else
            {
                treeView.Indent = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the height of the item.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetItemHeight(this TreeView treeView, int value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetItemHeight(value)));
            }
            else
            {
                treeView.ItemHeight = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the label edit.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetLabelEdit(this TreeView treeView, bool value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetLabelEdit(value)));
            }
            else
            {
                treeView.LabelEdit = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the color of the line.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetLineColor(this TreeView treeView, Color value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetLineColor(value)));
            }
            else
            {
                treeView.LineColor = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the path separator.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetPathSeparator(this TreeView treeView, string value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetPathSeparator(value)));
            }
            else
            {
                treeView.PathSeparator = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the right to left layout.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetRightToLeftLayout(this TreeView treeView, bool value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetRightToLeftLayout(value)));
            }
            else
            {
                treeView.RightToLeftLayout = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the scrollable.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetScrollable(this TreeView treeView, bool value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetScrollable(value)));
            }
            else
            {
                treeView.Scrollable = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the index of the selected image.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectedImageIndex(this TreeView treeView, int value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetSelectedImageIndex(value)));
            }
            else
            {
                treeView.SelectedImageIndex = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the selected image key.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectedImageKey(this TreeView treeView, string value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetSelectedImageKey(value)));
            }
            else
            {
                treeView.SelectedImageKey = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the selected node.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectedNode(this TreeView treeView, TreeNode value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetSelectedNode(value)));
            }
            else
            {
                treeView.SelectedNode = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the show lines.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetShowLines(this TreeView treeView, bool value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetShowLines(value)));
            }
            else
            {
                treeView.ShowLines = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the show node tool tips.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetShowNodeToolTips(this TreeView treeView, bool value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetShowNodeToolTips(value)));
            }
            else
            {
                treeView.ShowNodeToolTips = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the show plus minus.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetShowPlusMinus(this TreeView treeView, bool value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetShowPlusMinus(value)));
            }
            else
            {
                treeView.ShowPlusMinus = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the show root lines.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetShowRootLines(this TreeView treeView, bool value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetShowRootLines(value)));
            }
            else
            {
                treeView.ShowRootLines = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the sorted.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetSorted(this TreeView treeView, bool value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetSorted(value)));
            }
            else
            {
                treeView.Sorted = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the state image list.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetStateImageList(this TreeView treeView, ImageList value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetStateImageList(value)));
            }
            else
            {
                treeView.StateImageList = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the top node.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetTopNode(this TreeView treeView, TreeNode value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetTopNode(value)));
            }
            else
            {
                treeView.TopNode = value;
                treeView.Refresh();
            }
        }

        /// <summary>
        ///     Sets the TreeView node sorter.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="value">The value.</param>
        public static void SetTreeViewNodeSorter(this TreeView treeView, IComparer value)
        {
            if (treeView.InvokeRequired)
            {
                treeView.BeginInvoke(new MethodInvoker(() => treeView.SetTreeViewNodeSorter(value)));
            }
            else
            {
                treeView.TreeViewNodeSorter = value;
                treeView.Refresh();
            }
        }
    }
}