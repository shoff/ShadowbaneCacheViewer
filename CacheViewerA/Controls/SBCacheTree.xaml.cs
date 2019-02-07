using System.Windows.Controls;

namespace CacheViewerA.Controls
{
    /// <summary>
    /// Interaction logic for SBCacheTree.xaml
    /// </summary>
    public partial class SBCacheTree : UserControl
    {
        public SBCacheTree()
        {
            InitializeComponent();
            this.CacheItemsTree.Items.Add(new SBNode()
            {
            });
        }
    }
}
