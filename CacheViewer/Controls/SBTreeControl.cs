namespace CacheViewer.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Globalization;
    using System.Threading.Tasks;
    using ControlExtensions;
    using Data;
    using Data.Entities;
    using Domain.Extensions;
    using Domain.Factories;
    using Domain.Models.Exportable;
    using Domain.Services;
    using Domain.Services.Prefabs;
    using Nito.ArraySegments;
    using NLog;

    public partial class SBTreeControl : UserControl
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private StructureService structureService;
        public event EventHandler<ParseErrorEventArgs> OnParseError;
        public event EventHandler<LoadingMessageEventArgs> OnLoadingMessage;
        public event EventHandler<CacheObjectSelectedEventArgs> OnCacheObjectSelected;
        public event EventHandler<InvalidRenderIdEventArgs> OnInvalidRenderId;

        private TreeNode selectedNode = new TreeNode();

        private readonly TreeNode simpleNode = new TreeNode("Simple");
        private readonly TreeNode structureNode = new TreeNode("Structures");
        private readonly TreeNode interactiveNode = new TreeNode("Interactive");
        private readonly TreeNode equipmentNode = new TreeNode("Equipment");
        private readonly TreeNode mobileNode = new TreeNode("Mobiles");
        private readonly TreeNode deedNode = new TreeNode("Deeds");
        private readonly TreeNode unknownNode = new TreeNode("Unknown");
        private readonly TreeNode warrantNode = new TreeNode("Warrants");
        private readonly TreeNode particleNode = new TreeNode("Particles");


        private readonly List<TreeNode> simpleNodes = new List<TreeNode>();
        private readonly List<TreeNode> structureNodes = new List<TreeNode>();
        private readonly List<TreeNode> interactiveNodes = new List<TreeNode>();
        private readonly List<TreeNode> equipmentNodes = new List<TreeNode>();
        private readonly List<TreeNode> mobileNodes = new List<TreeNode>();
        private readonly List<TreeNode> deedNodes = new List<TreeNode>();
        private readonly List<TreeNode> warrantNodes = new List<TreeNode>();
        private readonly List<TreeNode> unknownNodes = new List<TreeNode>();
        private readonly List<TreeNode> particleNodes = new List<TreeNode>();

        // Archives
        private readonly CacheObjectFactory cacheObjectFactory;
        private readonly RenderInformationFactory renderInformationFactory;
        private readonly BackgroundWorker parseObjectWorker;

        public int TotalCacheObject { get; }

        public bool ArchivesLoaded { get; private set; }

        public SBTreeControl()
        {
            this.InitializeComponent();
            this.MessageLabel.Text = "";
            this.SaveTypeRadioButton1.Checked = true;
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.SaveButton.Enabled = false;
                this.cacheObjectFactory = CacheObjectFactory.Instance;
                this.renderInformationFactory = RenderInformationFactory.Instance;
                this.structureService = new StructureService();
                this.TotalCacheObject = this.cacheObjectFactory.Indexes.Count;
            }

            this.parseObjectWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            this.parseObjectWorker.DoWork += this.ParseSelected;
            this.parseObjectWorker.ProgressChanged += this.ParseObjectWorkerProgressChanged;
            this.parseObjectWorker.RunWorkerCompleted += this.ParseObjectWorkerRunWorkerCompleted;
        }

        private async void SBTreeControl_Load(object sender, EventArgs e)
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

            await Task.Run(() => this.SetVisibility(this.LoadingPictureBox, true));

            await Task.Run(() =>
            {
                foreach (var ci in this.cacheObjectFactory.Indexes)
                {
                    try
                    {
                        var cacheObject = this.cacheObjectFactory.CreateOnly(ci);

                        string title = string.IsNullOrEmpty(cacheObject.Name) ?
                            ci.Identity.ToString(CultureInfo.InvariantCulture) :
                            $"{ci.Identity.ToString(CultureInfo.InvariantCulture)}-{cacheObject.Name}";

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
                        logger.Error(ex, ex.Message);
                    }
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

        public ICacheObject SelectedCacheObject { get; set; }

        private async void SaveButtonClick(object sender, EventArgs e)
        {
            this.SaveButton.Enabled = false;
            this.SelectedCacheObject = (ICacheObject)this.CacheObjectTreeView.SelectedNode.Tag;

            try
            {
                await Task.Run(async () =>
                {
                    var service = new StructureService();
                    await service.SaveAssembledModelAsync(this.SelectedCacheObject.Name.Replace(" ", ""),
                        this.SelectedCacheObject, this.SaveTypeRadioButton1.Checked);
                });
            }
            catch (Exception ex)
            {
                logger?.Error(ex);
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
                this.selectedNode = e.Node;
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

            var co = this.cacheObjectFactory.CreateAndParse(cacheObject.CacheIndex, true);
            this.OnCacheObjectSelected.Raise(this, new CacheObjectSelectedEventArgs(co));
            // this.parseObjectWorker.RunWorkerAsync(cacheObject);
        }

        private async void ParseSelected(object sender, DoWorkEventArgs e)
        {
            if (this.parseObjectWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            var cacheObject = (ICacheObject)e.Argument;
            logger?.Info($"Selected {cacheObject.CacheIndex} for parsing.");

            try
            {
                cacheObject.Parse();

                if (cacheObject.RenderCount == 0)
                {
                    logger?.Error(Messages.CouldNotFindRenderId, cacheObject.CacheIndex.Identity);
                    this.OnInvalidRenderId.Raise(this, new InvalidRenderIdEventArgs(cacheObject));
                    this.parseObjectWorker.CancelAsync();
                    return;
                }

                var realTimeModelService = new RealTimeModelService();

                this.OnLoadingMessage.Raise(this,
                    new LoadingMessageEventArgs($"Generating model for {cacheObject.CacheIndex.Identity}"));

                var models = await realTimeModelService.GenerateModelAsync(cacheObject.CacheIndex.Identity);

                if (models == null || models.Count == 0)
                {
                    logger?.Error(
                        $"Unable to parse model for {cacheObject.CacheIndex.Identity}: {cacheObject.Name}");

                    var parseError = new ParseError
                    {
                        CacheIndexIdentity = cacheObject.CacheIndex.Identity,
                        CacheIndexOffset = (int)cacheObject.CacheIndex.Offset,
                        CursorOffset = cacheObject.CursorOffset,
                        Data = cacheObject.Data.ToArray(),
                        InnerOffset = cacheObject.InnerOffset,
                        Name = cacheObject.Name,
                        ObjectType = cacheObject.Flag,
                        RenderId = (int)cacheObject.RenderId
                    };

                    this.selectedNode.ForeColor = Color.DarkRed;
                    this.selectedNode.BackColor = Color.FromArgb(255, 204, 204);
                    this.OnParseError.Raise(this, new ParseErrorEventArgs(parseError));
                    using (var context = new SbCacheViewerContext())
                    {
                        context.ParseErrors.Add(parseError);
                        await context.SaveChangesAsync();
                    }

                    throw new ApplicationException(
                        $"Unable to parse model for {cacheObject.CacheIndex.Identity}: {cacheObject.Name}");
                }

                var eventArgs = new CacheObjectSelectedEventArgs(cacheObject);
                this.OnCacheObjectSelected.Raise(this, eventArgs);
                e.Result = cacheObject;
            }
            catch (Exception ex)
            {
                this.OnLoadingMessage.Raise(this, new LoadingMessageEventArgs(ex.Message));
                logger?.Error(ex, ex.Message);
            }
        }

        private void ParseObjectWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // Console.WriteLine("You canceled!");
            }
            else if (e.Error != null) // || e.Result == null)
            {
                this.selectedNode.ForeColor = Color.DarkRed;
                this.selectedNode.BackColor = Color.FromArgb(255, 204, 204);
                this.MessageLabel.Text = "Worker exception: " + e.Error;
                logger?.Error("Worker exception: " + e.Error);
            }
            else
            {
                this.SelectedCacheObject = (ICacheObject)e.Result;
                this.OnLoadingMessage.Raise(this, new LoadingMessageEventArgs("Parsing complete"));
            }
        }

        private void ParseObjectWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           // Console.WriteLine("Reached " + e.ProgressPercentage + "%");
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
}

