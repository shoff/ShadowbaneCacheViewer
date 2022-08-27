namespace Shadowbane.CacheViewer.Controls;

using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cache;
using Serilog;

public partial class CObjectViewControl : UserControl
{
       
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
    public async Task Display(ICacheObject? cacheObject)
    {
        if (cacheObject == null)
        {
            Log.Error("Null ICacheObject sent to CObjectViewControl.Display");
            return;
        }

        await Task.Run(() =>
        {
            ListViewItem[] items = new ListViewItem[7];
            items[0] =
                new ListViewItem(new[] {$"name {cacheObject.Name}"});

            items[1] = new ListViewItem(new[] {$"identity {cacheObject.Identity}"});

            items[2] =
                new ListViewItem(new[] {$"offset {cacheObject.CursorOffset}"});
                
            items[3] =
                new ListViewItem(new[] {"uncompressed: Unknown Fix ICacheObject"});

            items[4] =
                new ListViewItem(new[] {"compressed Unknown Fix ICacheObject"});

            items[5] =
                new ListViewItem(new[]{$"render id {cacheObject.RenderId.ToString(CultureInfo.InvariantCulture)}"});
            items[6] =
                new ListViewItem(new[] {$"object type {cacheObject.Flag}"});

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