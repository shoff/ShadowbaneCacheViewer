namespace Shadowbane.Viewer.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using Cache;
using Cache.IO;
using Serilog;
using UserControl = System.Windows.Controls.UserControl;

/// <summary>
/// Interaction logic for SBTreeControl.xaml
/// </summary>
public partial class SBTreeControl : UserControl
{
    private readonly ICacheObjectBuilder objectBuilder = null!;


    public event Func<string, string> InvalidObjectParsed;
    public event Func<string, string> ValidObjectParsed;

    private readonly TreeViewItem simpleNode = new() { Header = "Simple" };
    private readonly TreeViewItem structureNode = new() { Header = "Structures" };
    private readonly TreeViewItem interactiveNode = new() { Header = "Interactive" };
    private readonly TreeViewItem equipmentNode = new() { Header = "Equipment" };
    private readonly TreeViewItem mobileNode = new() { Header = "Mobiles" };
    private readonly TreeViewItem deedNode = new() { Header = "Deeds" };
    private readonly TreeViewItem unknownNode = new() { Header = "Unknown" };
    private readonly TreeViewItem warrantNode = new() { Header = "Warrants" };
    private readonly TreeViewItem particleNode = new() { Header = "Particles" };

    private readonly List<TreeViewItem> simpleNodes = new();
    private readonly List<TreeViewItem> structureNodes = new();
    private readonly List<TreeViewItem> interactiveNodes = new();
    private readonly List<TreeViewItem> equipmentNodes = new();
    private readonly List<TreeViewItem> mobileNodes = new();
    private readonly List<TreeViewItem> deedNodes = new();
    private readonly List<TreeViewItem> warrantNodes = new();
    private readonly List<TreeViewItem> unknownNodes = new();
    private readonly List<TreeViewItem> particleNodes = new();

    public SBTreeControl()
    {
        InitializeComponent();
        this.CacheObjectTreeView.Items.Add(simpleNode);
        this.CacheObjectTreeView.Items.Add(structureNode);
        this.CacheObjectTreeView.Items.Add(interactiveNode);
        this.CacheObjectTreeView.Items.Add(equipmentNode);
        this.CacheObjectTreeView.Items.Add(mobileNode);
        this.CacheObjectTreeView.Items.Add(deedNode);
        this.CacheObjectTreeView.Items.Add(unknownNode);
        this.CacheObjectTreeView.Items.Add(warrantNode);
        this.CacheObjectTreeView.Items.Add(particleNode);
        simpleNode.Items.Add(simpleNodes);
        structureNode.Items.Add(structureNodes);
        interactiveNode.Items.Add(interactiveNodes);
        equipmentNode.Items.Add(equipmentNodes);
        mobileNode.Items.Add(mobileNodes);
        deedNode.Items.Add(deedNodes);
        unknownNode.Items.Add(unknownNodes);
        warrantNode.Items.Add(warrantNodes);
        particleNode.Items.Add(particleNodes);

        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            this.objectBuilder = new CacheObjectBuilder();
            //this.ResetSaveButtons();
            //Serilog.Log.LogInforamtion.("CacheViewForm completed loading all cache archives.");
            //this.archivesLoaded = true;
        }
    }

    public async void LoadCacheButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        await GetObjectsAsync();
    }

    private Task GetObjectsAsync()
    {
        foreach (var cacheIndex in ArchiveLoader.ObjectArchive.CacheIndices)
        {
            try
            {
                ICacheObject? cacheObject = this.objectBuilder.NameOnly(cacheIndex.identity);
                string title = cacheObject?.Name;
                //    ?
                //cacheIndex.identity.ToString(CultureInfo.InvariantCulture) :
                //$"{cacheIndex.identity.ToString(CultureInfo.InvariantCulture)}-{cacheObject.Name}";

                var node = new TreeViewItem()
                {
                    Header = title,
                    Tag = cacheObject,
                };

                switch (cacheObject?.Flag)
                {
                    case ObjectType.Sun:
                        break;
                    case ObjectType.Simple:
                        simpleNodes.Add(node);
                        break;
                    case ObjectType.Structure:
                        structureNodes.Add(node);
                        break;
                    case ObjectType.Interactive:
                        interactiveNodes.Add(node);
                        break;
                    case ObjectType.Equipment:
                        equipmentNodes.Add(node);
                        break;
                    case ObjectType.Mobile:
                        mobileNodes.Add(node);
                        break;
                    case ObjectType.Deed:
                        deedNodes.Add(node);
                        break;
                    case ObjectType.Unknown:
                        unknownNodes.Add(node);
                        break;
                    case ObjectType.Warrant:
                        warrantNodes.Add(node);
                        break;
                    case ObjectType.Particle:
                        particleNodes.Add(node);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }


        simpleNodes.ForEach(s => this.simpleNode.Items.Add(s));
        structureNodes.ForEach(s => this.structureNode.Items.Add(s));
        interactiveNodes.ForEach(s => this.interactiveNode.Items.Add(s));
        equipmentNodes.ForEach(s => this.equipmentNode.Items.Add(s));
        mobileNodes.ForEach(s => this.mobileNode.Items.Add(s));
        deedNodes.ForEach(s => this.deedNode.Items.Add(s));
        unknownNodes.ForEach(s => this.unknownNode.Items.Add(s));
        warrantNodes.ForEach(s => this.warrantNode.Items.Add(s));
        particleNodes.ForEach(s => this.particleNode.Items.Add(s));
        return Task.CompletedTask;
        
    }
}