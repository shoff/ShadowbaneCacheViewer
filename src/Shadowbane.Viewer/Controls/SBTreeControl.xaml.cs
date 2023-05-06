using System.Threading;

namespace Shadowbane.Viewer.Controls;

using Shadowbane.Viewer.ViewModels;
using System.ComponentModel;
using Cache.IO;
using System;

public partial class SBTreeControl 
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly ICacheObjectBuilder objectBuilder = null!;
    public CacheObjectViewModel ViewModel { get; }

    public SBTreeControl()
    {
        InitializeComponent();
        this.ViewModel = new CacheObjectViewModel();
        DataContext = this.ViewModel;
        // this.loader = new CacheObjectViewModelLoader(this.ViewModel);
        this.CacheObjectTreeView.ItemsSource = this.ViewModel.Items;

        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            this.objectBuilder = new CacheObjectBuilder(new RenderableBuilder());

        }
    }

    public async void LoadCacheButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        throw new NotImplementedException();
        // await this.loader.GetObjectsAsync(this.cancellationTokenSource.Token);
    }

}