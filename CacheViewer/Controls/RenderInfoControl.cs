using System.ComponentModel;
using System.Windows.Forms;

namespace CacheViewer.Controls
{
    using CacheViewer.Domain.Factories;

    public partial class RenderInfoControl : UserControl
    {
        private RenderInformationFactory renderInformationFactory;

        public RenderInfoControl()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                //this.modelIdService = new ModelIdService();
                //this.textureFactory = TextureFactory.Instance;
                //this.meshFactory = MeshFactory.Instance;
                this.renderInformationFactory = RenderInformationFactory.Instance;
                //this.columnSorter = new ListViewColumnSorter();
                //this.RenderInformationListView.ListViewItemSorter = this.columnSorter;
            }
        }
    }
}
