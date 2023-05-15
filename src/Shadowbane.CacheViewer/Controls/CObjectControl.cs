namespace Shadowbane.CacheViewer.Controls;

using System.ComponentModel;
using System.Windows.Forms;
using Cache;

public partial class CObjectControl : UserControl
{
    private readonly ICacheRecord cacheRecord;

    public CObjectControl(ICacheRecord cacheRecord)
    {
        this.cacheRecord = cacheRecord;
        InitializeComponent();
        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            this.CObjectHeadLabel.Text = this.cacheRecord.Name ?? "WTF? No name";
        }
    }

}