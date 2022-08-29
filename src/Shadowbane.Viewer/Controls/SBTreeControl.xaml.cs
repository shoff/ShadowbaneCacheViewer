using System.Threading;

namespace Shadowbane.Viewer.Controls;

using Shadowbane.Viewer.ViewModels;
using System.ComponentModel;
using Cache.IO;

public partial class SBTreeControl 
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly ICacheObjectBuilder objectBuilder = null!;
    private readonly CacheObjectViewModel viewModel;
    private readonly CacheObjectViewModelLoader loader;

    public SBTreeControl()
    {
        InitializeComponent();
        this.viewModel = new CacheObjectViewModel();
        DataContext = this.viewModel;
        this.loader = new CacheObjectViewModelLoader(this.viewModel);
        this.CacheObjectTreeView.ItemsSource = this.viewModel.Items;

        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            this.objectBuilder = new CacheObjectBuilder();

        }
    }

    public async void LoadCacheButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
         await this.loader.GetObjectsAsync(this.cancellationTokenSource.Token);
    }

}