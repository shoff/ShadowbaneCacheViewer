namespace Shadowbane.Viewer.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Cache;
using Cache.IO;
using CacheViewer.Controls;
using CacheViewer.Extensions;
using CacheViewer.Services;
using Serilog;
using UserControl = System.Windows.Controls.UserControl;

/// <summary>
/// Interaction logic for SBTreeControl.xaml
/// </summary>
public partial class SBTreeControl : UserControl
{
    private readonly ICacheObjectBuilder objectBuilder = null!;
    private readonly IStructureService structureService = null!;
    public event EventHandler<ParseErrorEventArgs> OnParseError = null!;
    public event EventHandler<LoadingMessageEventArgs> OnLoadingMessage = null!;
    public event EventHandler<CacheObjectSelectedEventArgs> OnCacheObjectSelected = null!;
    public event EventHandler<InvalidRenderIdEventArgs> OnInvalidRenderId = null!;

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
    private readonly BackgroundWorker parseObjectWorker;

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
            this.structureService = new StructureService();

        }

        this.parseObjectWorker = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        //this.parseObjectWorker.DoWork += this.ParseSelected!;
        //this.parseObjectWorker.ProgressChanged += this.ParseObjectWorkerProgressChanged!;
        //this.parseObjectWorker.RunWorkerCompleted += this.ParseObjectWorkerRunWorkerCompleted!;

    }

    private void LoadCacheButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        //await Task.Run(() => this.SetVisibility(this.LoadingPictureBox, true));

        // await Task.Run(() =>
        //{
        var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        Task.Factory.StartNew(GetObjects, CancellationToken.None, 
            TaskCreationOptions.None, scheduler);
    }

    public void GetObjects()
    {
        try
        {
            int validCacheObjects = 0;
            int invalidCacheObjects = 0;

            foreach (var ci in ArchiveLoader.ObjectArchive.CacheIndices)
            {
                try
                {
                    var cacheObject = this.objectBuilder.CreateAndParse(ci.identity);
                    if (cacheObject == null)
                    {
                        string message = $"Unable to create and parse cache object with identity {ci.identity}";
                        Log.Error(message);
                        this.InvalidObjectParsed?.Invoke(invalidCacheObjects++.ToString());
                        continue;
                    }

                    string title = string.IsNullOrEmpty(cacheObject.Name) ?
                        ci.identity.ToString(CultureInfo.InvariantCulture) :
                        $"{ci.identity.ToString(CultureInfo.InvariantCulture)}-{cacheObject.Name}";

                    var node = new TreeViewItem
                    {
                        Header = title,
                        Tag = cacheObject,
                    };

                    switch (cacheObject.Flag)
                    {
                        case ObjectType.Sun:
                            break;
                        case ObjectType.Simple:
                            this.simpleNodes.Add(node);
                            break;
                        case ObjectType.Structure:
                            this.structureNodes.Add(node);
                            break;
                        case ObjectType.Interactive:
                            this.interactiveNodes.Add(node);
                            break;
                        case ObjectType.Equipment:
                            this.equipmentNodes.Add(node);
                            break;
                        case ObjectType.Mobile:
                            this.mobileNodes.Add(node);
                            break;
                        case ObjectType.Deed:
                            this.deedNodes.Add(node);
                            break;
                        case ObjectType.Unknown:
                            this.unknownNodes.Add(node);
                            break;
                        case ObjectType.Warrant:
                            this.warrantNodes.Add(node);
                            break;
                        case ObjectType.Particle:
                            this.particleNodes.Add(node);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                    throw;
                }

                this.ValidObjectParsed?.Invoke(validCacheObjects++.ToString());
            }
            // });

            this.simpleNodes.ForEach(s => this.simpleNode.Items.Add(s));
            this.structureNodes.ForEach(s => this.structureNode.Items.Add(s));
            this.interactiveNodes.ForEach(s => this.interactiveNode.Items.Add(s));
            this.equipmentNodes.ForEach(s => this.equipmentNode.Items.Add(s));
            this.mobileNodes.ForEach(s => this.mobileNode.Items.Add(s));
            this.deedNodes.ForEach(s => this.deedNode.Items.Add(s));
            this.unknownNodes.ForEach(s => this.unknownNode.Items.Add(s));
            this.warrantNodes.ForEach(s => this.warrantNode.Items.Add(s));
            this.particleNodes.ForEach(s => this.particleNode.Items.Add(s));

            // what a pain in the ass this is Microsoft.
            this.ArchivesLoaded = true;
            this.OnLoadingMessage.Raise(this, new LoadingMessageEventArgs("Cache files loaded."));
        }
        catch { }

    }

    public bool ArchivesLoaded { get; set; }
}
