namespace Shadowbane.CacheViewer.Controls;

using System.ComponentModel;
using System.Windows.Forms;


public partial class RenderInfoControl : UserControl
{

    public RenderInfoControl()
    {
        InitializeComponent();
        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            //this.modelIdService = new ModelIdService();
            //this.textureFactory = TextureFactory.Instance;
            //this.meshFactory = MeshFactory.Instance;
            //this.columnSorter = new ListViewColumnSorter();
            //this.RenderInformationListView.ListViewItemSorter = this.columnSorter;
        }
    }
}