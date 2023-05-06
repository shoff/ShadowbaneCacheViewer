namespace Shadowbane.CacheViewer.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControlExtensions;
using Serilog;
using Shadowbane.Cache;
using Shadowbane.Cache.IO;
using Shadowbane.CacheViewer;
using Shadowbane.CacheViewer.Extensions;
using Shadowbane.CacheViewer.Services;

public partial class SBTreeControl : UserControl
{
    private readonly ICacheObjectBuilder objectBuilder;
    private readonly IStructureService structureService;
    public event EventHandler<ParseErrorEventArgs> OnParseError;
    public event EventHandler<LoadingMessageEventArgs> OnLoadingMessage;
    public event EventHandler<CacheObjectSelectedEventArgs> OnCacheObjectSelected;
    public event EventHandler<InvalidRenderIdEventArgs> OnInvalidRenderId;

    private TreeNode selectedNode = new();

    private readonly TreeNode simpleNode = new("Simple");
    private readonly TreeNode structureNode = new("Structures");
    private readonly TreeNode interactiveNode = new("Interactive");
    private readonly TreeNode equipmentNode = new("Equipment");
    private readonly TreeNode mobileNode = new("Mobiles");
    private readonly TreeNode deedNode = new("Deeds");
    private readonly TreeNode unknownNode = new("Unknown");
    private readonly TreeNode warrantNode = new("Warrants");
    private readonly TreeNode particleNode = new("Particles");

    private readonly List<TreeNode> simpleNodes = new();
    private readonly List<TreeNode> structureNodes = new();
    private readonly List<TreeNode> interactiveNodes = new();
    private readonly List<TreeNode> equipmentNodes = new();
    private readonly List<TreeNode> mobileNodes = new();
    private readonly List<TreeNode> deedNodes = new();
    private readonly List<TreeNode> warrantNodes = new();
    private readonly List<TreeNode> unknownNodes = new();
    private readonly List<TreeNode> particleNodes = new();
    private readonly BackgroundWorker parseObjectWorker;

    public int TotalCacheObject { get; }

    public bool ArchivesLoaded { get; private set; }

    public SBTreeControl() : this(null){}

    public SBTreeControl(
        IStructureService? structureService = null,
        ICacheObjectBuilder? cacheObjectBuilder = null)
    {
        this.InitializeComponent();
        // ReSharper disable once LocalizableElement
        this.ErrorLabel.Text = "Invalid cache object count: ";
        this.MessageLabel.Text = "";
        this.SaveTypeRadioButton1.Checked = true;

        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            this.objectBuilder = cacheObjectBuilder ?? new CacheObjectBuilder(new RenderableBuilder());
            this.structureService = structureService ?? new StructureService();

            this.SaveButton.Enabled = false;
            this.TotalCacheObject = GetCounts();
        }

        this.parseObjectWorker = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        this.parseObjectWorker.DoWork += this.ParseSelected!;
        this.parseObjectWorker.ProgressChanged += this.ParseObjectWorkerProgressChanged!;
        this.parseObjectWorker.RunWorkerCompleted += this.ParseObjectWorkerRunWorkerCompleted!;
    }
    private static int GetCounts()
    {
        return ArchiveLoader.ObjectArchive.IndexCount + ArchiveLoader.RenderArchive.IndexCount + ArchiveLoader.SoundArchive.IndexCount + ArchiveLoader.TextureArchive.IndexCount + ArchiveLoader.MeshArchive.IndexCount + ArchiveLoader.SkeletonArchive.IndexCount + ArchiveLoader.ZoneArchive.IndexCount + ArchiveLoader.VisualArchive.IndexCount;
    }

    private void SBTreeControl_Load(object sender, EventArgs e)
    {
        this.CacheObjectTreeView.Nodes.Add(this.simpleNode);
        this.CacheObjectTreeView.Nodes.Add(this.structureNode);
        this.CacheObjectTreeView.Nodes.Add(this.interactiveNode);
        this.CacheObjectTreeView.Nodes.Add(this.equipmentNode);
        this.CacheObjectTreeView.Nodes.Add(this.mobileNode);
        this.CacheObjectTreeView.Nodes.Add(this.deedNode);
        this.CacheObjectTreeView.Nodes.Add(this.unknownNode);
        this.CacheObjectTreeView.Nodes.Add(this.warrantNode);
        this.CacheObjectTreeView.Nodes.Add(this.particleNode);
    }

    private void SetVisibility(PictureBox pb, bool visible)
    {
        if (pb.InvokeRequired)
        {
            pb.BeginInvoke(new MethodInvoker(() => this.SetVisibility(pb, visible)));
        }
        else
        {
            pb.Visible = visible;
            pb.Refresh();
        }
    }

    public ICacheObject? SelectedCacheObject { get; set; }

    private async void SaveButtonClick(object sender, EventArgs e)
    {
        this.SaveButton.Enabled = false;
        this.SelectedCacheObject = (ICacheObject)this.CacheObjectTreeView.SelectedNode.Tag;

        try
        {
            await Task.Run(async () =>
            {
                await this.structureService.SaveAssembledModelAsync(this.SelectedCacheObject.Name.Replace(" ", ""),
                    this.SelectedCacheObject, this.SaveTypeRadioButton1.Checked);
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            this.MessageLabel.Text = ex.Message;
            throw;
        }
        this.SaveButton.Enabled = true;
    }

    private void CacheObjectTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        this.MessageLabel.Text = "";
        if (!this.ArchivesLoaded)
        {
            return;
        }

        if (e.Node == this.selectedNode)
        {
            return;
        }

        if (!this.parseObjectWorker.IsBusy)
        {
            if (e.Node != null)
            {
                this.selectedNode = e.Node;
            }
        }
        else
        {
            this.CacheObjectTreeView.SelectedNode = this.selectedNode;
            return;
        }

        // ok so right here, I need to determine what the type of object is 
        // well ok its going to be a cache object, but find the renderId and the
        // pertinent information from the renderId by validating the information
        // against the other archives. I will give each "archive" portion for each 
        // cacheObject a listView that ties all the information together at once.
        var cacheObject = (ICacheObject)this.CacheObjectTreeView.SelectedNode.Tag;

        // reparse it correctly

        var co = this.objectBuilder.CreateAndParse(cacheObject.Identity);
        // this.cacheObjectFactory.CreateAndParse(cacheObject.CacheIndex, true);

        if (co != null)
        {
            this.OnCacheObjectSelected.Raise(this, new CacheObjectSelectedEventArgs(co));
        }
        else
        {
            Log.Error($"object builder could not create cache object with identity {cacheObject.Identity}");
        }
        // this.parseObjectWorker.RunWorkerAsync(cacheObject);
    }

    private async void ParseSelected(object sender, DoWorkEventArgs e)
    {
        if (this.parseObjectWorker.CancellationPending)
        {
            e.Cancel = true;
            return;
        }

        var cacheObject = (ICacheObject?)e.Argument;
        if (cacheObject == null)
        {
            Log.Error($"ParseSelected could not cast event args to ICacheObject");
        }
        Log.Information($"Selected {cacheObject!.Identity} for parsing.");

        try
        {
            cacheObject.Parse();

            if (cacheObject.RenderCount == 0)
            {
                Log.Error(Messages.CouldNotFindRenderId, cacheObject.Identity);
                this.OnInvalidRenderId.Raise(this, new InvalidRenderIdEventArgs(cacheObject));
                this.parseObjectWorker.CancelAsync();
                return;
            }

            var realTimeModelService = new RealTimeModelService();

            this.OnLoadingMessage.Raise(this,
                new LoadingMessageEventArgs($"Generating model for {cacheObject.Identity}"));

            var models = await realTimeModelService.GenerateModelAsync(cacheObject.Identity);

            if (models == null || !models.Any())
            {
                Log.Error(
                    $"Unable to parse model for {cacheObject.Identity}: {cacheObject.Name}");

                var parseError = new ParseError
                {
                    CacheIndexIdentity = cacheObject.Identity,
                    CacheIndexOffset = cacheObject.CursorOffset,
                    CursorOffset = cacheObject.CursorOffset,
                    Data = cacheObject.Data.ToArray(),
                    Name = cacheObject.Name,
                    ObjectType = cacheObject.Flag,
                    RenderId = cacheObject.RenderId
                };

                this.selectedNode.ForeColor = Color.DarkRed;
                this.selectedNode.BackColor = Color.FromArgb(255, 204, 204);
                this.OnParseError.Raise(this, new ParseErrorEventArgs(parseError));
                throw new ApplicationException(
                    $"Unable to parse model for {cacheObject.Identity}: {cacheObject.Name}");
            }

            var eventArgs = new CacheObjectSelectedEventArgs(cacheObject);
            this.OnCacheObjectSelected.Raise(this, eventArgs);
            e.Result = cacheObject;
        }
        catch (Exception ex)
        {
            this.OnLoadingMessage.Raise(this, new LoadingMessageEventArgs(ex.Message));
            Log.Error(ex, ex.Message);
        }
    }

    private const string WORKER_EXCEPTION = "worker exception: ";

    private void ParseObjectWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Cancelled)
        {
            // Console.WriteLine("You canceled!");
        }
        else if (e.Error != null) // || e.Result == null)
        {
            var message = $@"{WORKER_EXCEPTION}{e.Error}";
            this.selectedNode.ForeColor = Color.DarkRed;
            this.selectedNode.BackColor = Color.FromArgb(255, 204, 204);
            this.MessageLabel.Text = message;
            Log.Error(message);
        }
        else
        {
            this.SelectedCacheObject = e.Result as ICacheObject;
            this.OnLoadingMessage.Raise(this,
                this.SelectedCacheObject == null
                    ? new LoadingMessageEventArgs("could not cast selection as a ICacheObject")
                    : new LoadingMessageEventArgs("Parsing complete"));
        }
    }

    private void ParseObjectWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        // Console.WriteLine("Reached " + e.ProgressPercentage + "%");
    }
    
    private async void LoadCacheButton_Click(object sender, EventArgs e)
    {
        await Task.Run(() => this.SetVisibility(this.LoadingPictureBox, true));

        await Task.Run(() =>
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
                        this.InvalidCount.SetText(invalidCacheObjects++.ToString());
                        continue;
                    }
                    string title = string.IsNullOrEmpty(cacheObject.Name) ?
                        ci.identity.ToString(CultureInfo.InvariantCulture) :
                        $"{ci.identity.ToString(CultureInfo.InvariantCulture)}-{cacheObject.Name}";

                    var node = new TreeNode(title)
                    {
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
                this.ValidCacheCount.SetText(validCacheObjects++.ToString());
            }
        });

        this.simpleNode.Nodes.AddRange(this.simpleNodes.ToArray());
        this.structureNode.Nodes.AddRange(this.structureNodes.ToArray());
        this.interactiveNode.Nodes.AddRange(this.interactiveNodes.ToArray());
        this.equipmentNode.Nodes.AddRange(this.equipmentNodes.ToArray());
        this.mobileNode.Nodes.AddRange(this.mobileNodes.ToArray());
        this.deedNode.Nodes.AddRange(this.deedNodes.ToArray());
        this.unknownNode.Nodes.AddRange(this.unknownNodes.ToArray());
        this.warrantNode.Nodes.AddRange(this.warrantNodes.ToArray());
        this.particleNode.Nodes.AddRange(this.particleNodes.ToArray());

        // what a pain in the ass this is Microsoft.
        this.LoadingPictureBox.SetVisible(false);
        this.LoadingPictureBox.Refresh();

        this.SaveButton.Enabled = true;
        this.ArchivesLoaded = true;
        this.OnLoadingMessage.Raise(this, new LoadingMessageEventArgs("Cache files loaded."));
    }
}

public class InvalidRenderIdEventArgs : EventArgs
{
    public InvalidRenderIdEventArgs(ICacheObject cacheObject)
    {
        this.CacheObject = cacheObject;
    }
    public ICacheObject CacheObject { get; }
}

public class CacheObjectSelectedEventArgs : EventArgs
{
    public CacheObjectSelectedEventArgs(ICacheObject cacheObject)
    {
        this.CacheObject = cacheObject;
    }
    public ICacheObject CacheObject { get; }
}

public class ParseErrorEventArgs : EventArgs
{
    public ParseError ParseError { get; }

    public ParseErrorEventArgs(ParseError parseError)
    {
        this.ParseError = parseError;
    }
}

public class LoadingMessageEventArgs : EventArgs
{
    public string Message { get; }

    public LoadingMessageEventArgs(string message)
    {
        this.Message = message;
    }
}

public class ParseError
{
    public uint CacheIndexIdentity { get; set; }
    public uint CacheIndexOffset { get; set; }
    public uint CursorOffset { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public uint InnerOffset { get; set; }
    public string Name { get; set; } = string.Empty;
    public ObjectType ObjectType { get; set; }
    public uint RenderId { get; set; }
}
