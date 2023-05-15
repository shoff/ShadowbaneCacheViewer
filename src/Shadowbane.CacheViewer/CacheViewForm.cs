// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
namespace Shadowbane.CacheViewer;

using Shadowbane.Cache.Exporter.File;
using Shadowbane.Cache;
using Shadowbane.CacheViewer.Services;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Cache.IO;
using ControlExtensions;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shadowbane.CacheViewer.Config;

public partial class CacheViewForm : Form
{
    private readonly TreeNode simpleNode = new("Simple");
    private readonly TreeNode structureNode = new("Structures");
    private readonly TreeNode interactiveNode = new("Interactive");
    private readonly TreeNode equipmentNode = new("Equipment");
    private readonly TreeNode mobileNode = new("Mobiles");
    private readonly TreeNode deedNode = new("Deeds");
    private readonly TreeNode unknownNode = new("Unknown");
    private readonly TreeNode warrantNode = new("Warrants");
    private readonly TreeNode particleNode = new("Particles");

    // Archives
    private readonly CacheObjectBuilder cacheObjectBuilder = null!;
    private readonly IRenderableBuilder renderableBuilder = null!;
    private bool archivesLoaded;


    public CacheViewForm()
    {
        InitializeComponent();
        this.SaveButton.Enabled = false;
        this.CacheSaveButton.Enabled = false;

        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            Log.Debug("CacheViewForm created.");
            this.AcceptButton = this.CacheSaveButton;
            this.renderableBuilder = new RenderableBuilder();
            this.cacheObjectBuilder = new CacheObjectBuilder(this.renderableBuilder);
            // ReSharper disable once LocalizableElement
            this.TotalCacheLabel.Text = $"Total number of SB objects {ArchiveLoader.ObjectArchive.IndexCount}";
            this.TotalCacheLabel.Refresh();
        }
    }

    private async void LoadCacheButtonClick(object sender, EventArgs e)
    {
        this.LoadCacheButton.Enabled = false;
        this.LoadCacheButton.Refresh();

        // ReSharper disable once LocalizableElement
        //this.LoadLabel.Text = Messages.LoadTimeFromCache + " " +
        //    TimeSpan.FromTicks(this.cacheObjectBuilder.LoadTime).Seconds + Messages.Seconds;
        this.LoadLabel.Refresh();

        var simpleNodes = new List<TreeNode>();
        var structureNodes = new List<TreeNode>();
        var interactiveNodes = new List<TreeNode>();
        var equipmentNodes = new List<TreeNode>();
        var mobileNodes = new List<TreeNode>();
        var deedNodes = new List<TreeNode>();
        var warrantNodes = new List<TreeNode>();
        var unknownNodes = new List<TreeNode>();
        var particleNodes = new List<TreeNode>();

        // ReSharper disable once CSharpWarnings::CS4014
        await Task.Run(() => SetVisibility(this.LoadingPictureBox, true));

        await Task.Run(() =>
        {
            int cacheNumber = 0;
            foreach (var ci in ArchiveLoader.ObjectArchive.CacheIndices)
            {
                try
                {
                    var cacheObject = this.cacheObjectBuilder.CreateAndParse(ci.identity);
                    if (cacheObject is null)
                    {
                        throw new InvalidCacheObjectException(ci.identity);
                    }

                    cacheObject.CacheIndex = ci;
                    var title = string.IsNullOrWhiteSpace(cacheObject.Name)
                        ? ci.identity.ToString(CultureInfo.InvariantCulture)
                        : $"{ci.identity.ToString(CultureInfo.InvariantCulture)} - {cacheObject.Name}";

                    this.LoadingLabel.SetText($"Now loading number {cacheNumber++} : {title}");

                    var node = new TreeNode(title)
                    {
                        Tag = cacheObject,
                    };

                    switch (cacheObject.Flag)
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
        });

        this.simpleNode.Nodes.AddRange(simpleNodes.ToArray());
        this.structureNode.Nodes.AddRange(structureNodes.ToArray());
        this.interactiveNode.Nodes.AddRange(interactiveNodes.ToArray());
        this.equipmentNode.Nodes.AddRange(equipmentNodes.ToArray());
        this.mobileNode.Nodes.AddRange(mobileNodes.ToArray());
        this.deedNode.Nodes.AddRange(deedNodes.ToArray());
        this.unknownNode.Nodes.AddRange(unknownNodes.ToArray());
        this.warrantNode.Nodes.AddRange(warrantNodes.ToArray());
        this.particleNode.Nodes.AddRange(particleNodes.ToArray());

        // what a pain in the ass this is Microsoft.
        this.LoadingPictureBox.SetVisible(false);
        this.LoadingPictureBox.Refresh();

        ResetSaveButtons();
        Log.Information("CacheViewForm completed loading all cache archives.");
        this.archivesLoaded = true;
    }

    private void CacheViewFormLoad(object sender, EventArgs e)
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

    private async void SaveButtonClick(object sender, EventArgs e)
    {
        this.SaveButton.Enabled = false;
        this.CacheSaveButton.Enabled = false;
        ICacheRecord item = (ICacheRecord)this.CacheObjectTreeView.SelectedNode.Tag;

        try
        {
            await Task.Run(async () =>
            {
                var service = new StructureService();
                await service.SaveAssembledModelAsync(item.Name.Replace(" ", ""), item, this.SaveTypeRadioButton1.Checked);
            });
            ResetSaveButtons();
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            throw;
        }
    }

    private async void CacheObjectTreeViewAfterSelect(object sender, TreeViewEventArgs e)
    {
        if (!this.archivesLoaded)
        {
            return;
        }

        //this.PropertiesListView.Items.Clear();

        // ok so right here, I need to determine what the type of object is 
        // well ok its going to be a cache object, but find the renderId and the
        // pertinent information from the renderId by validating the information
        // against the other archives. I will give each "archive" portion for each 
        // cacheRecord a listView that ties all the information together at once.
        var item = (ICacheRecord)this.CacheObjectTreeView.SelectedNode.Tag;

        //await this.CacheIndexListView.Display(item);

        try
        {
            item.Parse();
            if (item.RenderId == 0)
            {
                Log.Error(Messages.CouldNotFindRenderId, item.Identity);
            }
            var realTimeModelService = new RealTimeModelService();
            var models = await realTimeModelService.GenerateModelAsync(item.Identity);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (models == null || models.Count == 0)
            {
                Log.Error($"Unable to parse model for {item.Identity}: {item.Name}");
                //ParseError parseError = new ParseError
                //{
                //    CacheIndexIdentity = item.Identity,
                //    CacheIndexOffset = item.CursorOffset,
                //    CursorOffset = item.CursorOffset,
                //    Data = item.Data.ToArray(),
                //    InnerOffset = 0,
                //    Name = item.Name,
                //    ObjectType = item.Flag,
                //    RenderId = item.RenderId
                //};
                //using (var context = new SbCacheViewerContext())
                //{
                //    context.ParseErrors.Add(parseError);
                //    await context.SaveChangesAsync();
                //}
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }
    }

    private void SetVisibility(PictureBox pb, bool visible)
    {
        if (pb.InvokeRequired)
        {
            pb.BeginInvoke(new MethodInvoker(() => SetVisibility(pb, visible)));
        }
        else
        {
            pb.Visible = visible;
            pb.Refresh();
        }
    }

    private async void CacheSaveButtonClick(object sender, EventArgs e)
    {
        using var serviceProvider = this.Services.BuildServiceProvider();
        IOptions<DirectoryOptions> directoryOptions =
            serviceProvider.GetRequiredService<IOptions<DirectoryOptions>>();

        string selectedFolder = directoryOptions.Value.CacheExportDirectory;

        this.SaveButton.Enabled = false;
        this.CacheSaveButton.Enabled = false;

        // this.PropertiesListView.Items.Clear();
        await Task.Run(() => SetVisibility(this.LoadingPictureBox, true));
        ICacheRecord item = (ICacheRecord)this.CacheObjectTreeView.SelectedNode.Tag;


        if (item is null || item.Data.Length == 0)
        {
            throw new NullCacheTreeNodeException(this.CacheObjectTreeView.SelectedNode.Text ?? "No text was in the selected node!");
        }

        // make the directory
        // TODO extract to it's own method
        string directory;

        if (string.IsNullOrEmpty(item.Name))
        {
            directory = selectedFolder + "\\ObjectCache\\" + item.Identity;
        }
        else
        {
            directory = selectedFolder + "\\ObjectCache\\" + item.Name + "_" + item.Identity;
        }

        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, true);
        }

        Directory.CreateDirectory(directory);
        await FileWriter.Writer.WriteAsync(item.Data.ToArray(), directory, $"{item.Name}.cache");

        try
        {
            // now try to create each render id if any are found
            if (item.RenderId == 0)
            {
                Log.Error(Messages.CouldNotFindRenderId, item.Identity);
                ResetSaveButtons();
                return;
            }

            // what a pain in the ass this is Microsoft.
            await Task.Run(() => SetVisibility(this.LoadingPictureBox, false));
            this.LoadingPictureBox.Visible = false;
            this.LoadingPictureBox.Refresh();

            ResetSaveButtons();
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            throw;
        }
    }

    private void ResetSaveButtons()
    {
        this.SaveButton.SetEnabled(true);
        this.SaveButton.Refresh();
        this.CacheSaveButton.SetEnabled(true);
        this.CacheSaveButton.Refresh();
    }

    private async Task SaveBinaryData(string directory, string fileName, ArraySegment<byte> data)
    {

        if (data.Count > 0)
        {
            await FileWriter.Writer.WriteAsync(data.ToArray(), directory, fileName);

            //if (asset.Item2.Count > 0)
            //{
            //    await
            //        FileWriter.Writer.WriteAsync(
            //            asset.Item2,
            //            Path.Combine(
            //                path,
            //                this.saveName + asset.CacheIndex2.identity.ToString(CultureInfo.InvariantCulture) + "_1.cache"));
            //}
        }
    }

    private void ShowEntityFormToolStripMenuItemClick(object sender, EventArgs e)
    {
        SBCacheObjectForm sBCacheObjectForm = new SBCacheObjectForm();
        sBCacheObjectForm.Show();
    }

    private void DatabaseFormToolStripMenuItemClick(object sender, EventArgs e)
    {
        // DatabaseForm dbf = new DatabaseForm();
        //dbf.Show();
    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    internal IServiceCollection Services { get; set; }
}