namespace Shadowbane.Viewer; 

using Shadowbane.Cache;
using Shadowbane.Cache.IO;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Serilog;
using Shadowbane.Viewer.ViewModels;

public class CacheObjectViewModelLoader
{
    private readonly CacheObjectViewModel viewModel;
    private readonly CacheObjectBuilder objectBuilder;

    public CacheObjectViewModelLoader(CacheObjectViewModel viewModel)
    {
        this.viewModel = viewModel;
        this.objectBuilder = new CacheObjectBuilder();
    }

    internal async Task GetObjectsAsync(CancellationToken cancellationToken)
    {
        //while (!cancellationToken.IsCancellationRequested)
        //{
            Parallel.ForEach(ArchiveLoader.ObjectArchive.CacheIndices, cacheIndex =>
                //foreach (var cacheIndex in ArchiveLoader.ObjectArchive.CacheIndices)
            {
                try
                {
                    ICacheObject? cacheObject = this.objectBuilder.NameOnly(cacheIndex.identity);
                    string title = cacheObject?.Name;
                    var treeViewItem = new TreeViewItem()
                    {
                        Header = title,
                        Tag = cacheObject,
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
            });
        //}

        //simpleNodes.ForEach(s => this.simpleNode.Items.Add(s));
        //structureNodes.ForEach(s => this.structureNode.Items.Add(s));
        //interactiveNodes.ForEach(s => this.interactiveNode.Items.Add(s));
        //equipmentNodes.ForEach(s => this.equipmentNode.Items.Add(s));
        //mobileNodes.ForEach(s => this.mobileNode.Items.Add(s));
        //deedNodes.ForEach(s => this.deedNode.Items.Add(s));
        //unknownNodes.ForEach(s => this.unknownNode.Items.Add(s));
        //warrantNodes.ForEach(s => this.warrantNode.Items.Add(s));
        //particleNodes.ForEach(s => this.particleNode.Items.Add(s));
        //
        await Task.CompletedTask;

    }
}