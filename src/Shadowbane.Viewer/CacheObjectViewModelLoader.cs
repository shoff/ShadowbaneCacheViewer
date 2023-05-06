namespace Shadowbane.Viewer;

using Shadowbane.Cache;
using Shadowbane.Cache.IO;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Serilog;
using Shadowbane.Viewer.ViewModels;

public class CacheObjectViewModelLoader
{
    private readonly CacheObjectViewModel viewModel;
    private readonly CacheObjectBuilder objectBuilder;
    private readonly SolidColorBrush itemBrush = new(Color.FromRgb(255, 255, 255));
    private readonly double itemFontSize = 14;
    public CacheObjectViewModelLoader(CacheObjectViewModel viewModel)
    {
        this.viewModel = viewModel;
        this.objectBuilder = new CacheObjectBuilder(new RenderableBuilder());
    }

    internal async Task GetObjectsAsync(CancellationToken cancellationToken)
    {
        foreach (var cacheIndex in ArchiveLoader.ObjectArchive.CacheIndices)
        {
            try
            {
                ICacheObject? cacheObject = this.objectBuilder.NameOnly(cacheIndex.identity);
                string title = cacheObject?.Name;
                var treeViewItem = new TreeViewItem()
                {
                    Header = title,
                    Tag = cacheObject,
                    Foreground = itemBrush,
                    FontSize = itemFontSize
                };

                switch (cacheObject?.Flag)
                {
                    case ObjectType.Sun:
                        break;
                    case ObjectType.Simple:
                        this.viewModel.SimpleNode.Items.Add(treeViewItem);
                        break;
                    case ObjectType.Structure:
                        this.viewModel.StructureNode.Items.Add(treeViewItem);
                        break;
                    case ObjectType.Interactive:
                        this.viewModel.InteractiveNode.Items.Add(treeViewItem);
                        break;
                    case ObjectType.Equipment:
                        this.viewModel.EquipmentNode.Items.Add(treeViewItem);
                        break;
                    case ObjectType.Mobile:
                        this.viewModel.MobileNode.Items.Add(treeViewItem);
                        break;
                    case ObjectType.Deed:
                        this.viewModel.DeedNode.Items.Add(treeViewItem);
                        break;
                    case ObjectType.Unknown:
                        this.viewModel.UnknownNode.Items.Add(treeViewItem);
                        break;
                    case ObjectType.Warrant:
                        this.viewModel.WarrantNode.Items.Add(treeViewItem);
                        break;
                    case ObjectType.Particle:
                        this.viewModel.ParticleNode.Items.Add(treeViewItem);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }
        await Task.CompletedTask;
    }
}