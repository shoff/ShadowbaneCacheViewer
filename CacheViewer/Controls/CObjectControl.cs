using System.Windows.Forms;

namespace CacheViewer.Controls
{
    using System.ComponentModel;
    using Domain.Models.Exportable;

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
}
