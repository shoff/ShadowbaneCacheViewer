namespace Shadowbane.CacheViewer.Controls;

using System.ComponentModel;
using System.Windows.Forms;
using Cache;

public partial class CObjectControl : UserControl
{
    private readonly ICacheObject cacheObject;

    public CObjectControl(ICacheObject cacheObject)
    {
        this.cacheObject = cacheObject;
        InitializeComponent();
        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            this.CObjectHeadLabel.Text = this.cacheObject.Name ?? "WTF? No name";
        }
    }

}