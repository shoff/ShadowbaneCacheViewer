using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using CacheViewer.Domain.Models.Exportable;
using NLog;

namespace CacheViewer.Controls
{
    public partial class CObjectViewControl : UserControl
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="CObjectViewControl"/> class.
        /// </summary>
        public CObjectViewControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Displays the specified cache object.
        /// </summary>
        /// <param name="cacheObject">The cache object.</param>
        /// <returns></returns>
        public async Task Display(ICacheObject cacheObject)
        {
            if (cacheObject == null)
            {
                logger.Error("Null ICacheObject sent to CObjectViewControl.Display");
                return;
            }

            await Task.Run(() =>
            {
                ListViewItem[] items = new ListViewItem[7];
                items[0] =
                   new ListViewItem(new[] { string.Format("name {0}", cacheObject.Name) });

                items[1] = new ListViewItem(new[] { string.Format("identity {0}", cacheObject.CacheIndex.identity) });

                items[2] =
                    new ListViewItem(new[] { string.Format("offset {0}", cacheObject.CacheIndex.offset) });

                items[3] =
                    new ListViewItem(new[] { string.Format("uncompressed {0}", cacheObject.CacheIndex.unCompressedSize) });

                items[4] =
                    new ListViewItem(new[] { string.Format("compressed {0}", cacheObject.CacheIndex.compressedSize) });

                items[5] =
                    new ListViewItem(new[]{string.Format("render id {0}",
                        cacheObject.RenderId.ToString(CultureInfo.InvariantCulture))});
                items[6] =
                    new ListViewItem(new[] { string.Format("object type {0}", cacheObject.Flag) });

                this.SetRenderItem(this.CacheIndexListView, items);
            });

            //this.CacheIndexListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void SetRenderItem(ListView control, IEnumerable<ListViewItem> items)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => SetRenderItem(control, items)));
            }
            else
            {
                control.Items.Clear();
                control.Refresh();
                foreach (var item in items)
                {
                    control.Items.Add(item);
                }
                control.Refresh();
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.CacheIndexListView.Items.Clear();
        }
    }
}
