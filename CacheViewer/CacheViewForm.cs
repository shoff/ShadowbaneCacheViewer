// ReSharper disable LocalizableElement
namespace CacheViewer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Domain.Factories;
    using Domain.Models.Exportable;
    using NLog;
    using System.IO;
    using ControlExtensions;
    using Data;
    using Data.Entities;
    using Domain.Services;
    using Domain.Services.Prefabs;
    using Nito.ArraySegments;

    public partial class CacheViewForm : Form
    {
        private readonly TreeNode simpleNode = new TreeNode("Simple");
        private readonly TreeNode structureNode = new TreeNode("Structures");
        private readonly TreeNode interactiveNode = new TreeNode("Interactive");
        private readonly TreeNode equipmentNode = new TreeNode("Equipment");
        private readonly TreeNode mobileNode = new TreeNode("Mobiles");
        private readonly TreeNode deedNode = new TreeNode("Deeds");
        private readonly TreeNode unknownNode = new TreeNode("Unknown");
        private readonly TreeNode warrantNode = new TreeNode("Warrants");
        private readonly TreeNode particleNode = new TreeNode("Particles");

        // Archives
        private readonly CacheObjectFactory cacheObjectFactory;
        private readonly RenderInformationFactory renderInformationFactory;
        private bool archivesLoaded;

        // data
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public CacheViewForm()
        {
            this.InitializeComponent();
            this.SaveButton.Enabled = false;
            this.CacheSaveButton.Enabled = false;
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                logger.Debug("CacheViewForm created.");
                this.AcceptButton = this.CacheSaveButton;
                this.cacheObjectFactory = CacheObjectFactory.Instance;
                this.renderInformationFactory = RenderInformationFactory.Instance;
                this.TotalCacheLabel.Text = "Total number of cache objects " + this.cacheObjectFactory.Indexes.Count;
                this.TotalCacheLabel.Refresh();
            }
        }

        private async void LoadCacheButtonClick(object sender, EventArgs e)
        {
            this.LoadCacheButton.Enabled = false;
            this.LoadCacheButton.Refresh();

            // ReSharper disable once LocalizableElement
            this.LoadLabel.Text = Messages.LoadTimeFromCache + " " +
                TimeSpan.FromTicks(this.cacheObjectFactory.LoadTime).Seconds + Messages.Seconds;
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
            await Task.Run(() => this.SetVisibility(this.LoadingPictureBox, true));

            await Task.Run(() =>
            {
                logger?.Info($"In LoadCacheButtonClick found {this.cacheObjectFactory.Indexes.Count}");
                foreach (var ci in this.cacheObjectFactory.Indexes)
                {
                    try
                    {
                        var cacheObject = this.cacheObjectFactory.CreateOnly(ci);
                        logger?.Debug($"Loaded cachObject {cacheObject.Name}");

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
                        logger?.Error(ex, ex.Message);
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

            this.ResetSaveButtons();
            logger?.Info("CacheViewForm completed loading all cache archives.");
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
            ICacheObject item = (ICacheObject)this.CacheObjectTreeView.SelectedNode.Tag;

            try
            {
                await Task.Run(async () =>
                {
                    var service = new StructureService();
                    await service.SaveAllAsync(item.Name.Replace(" ", ""), item.Name, item.Flag, this.SaveTypeRadioButton1.Checked);
                });
                this.ResetSaveButtons();
            }
            catch (Exception ex)
            {
                logger?.Error(ex);
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
            // cacheObject a listView that ties all the information together at once.
            var item = (ICacheObject)this.CacheObjectTreeView.SelectedNode.Tag;

            //await this.CacheIndexListView.Display(item);

            try
            {
                item.Parse(item.Data);
                if (item.RenderId == 0)
                {
                    logger?.Error(Messages.CouldNotFindRenderId, item.CacheIndex.Identity);
                }
                var realTimeModelService = new RealTimeModelService();
                var models = await realTimeModelService.GenerateModelAsync(item.CacheIndex.Identity);

                if (models == null || models.Count == 0)
                {
                    logger?.Error($"Unable to parse model for {item.CacheIndex.Identity}: {item.Name}");
                    ParseError parseError = new ParseError
                    {
                        CacheIndexIdentity = item.CacheIndex.Identity,
                        CacheIndexOffset = (int) item.CacheIndex.Offset,
                        CursorOffset = item.CursorOffset,
                        Data = item.Data.ToArray(),
                        InnerOffset = item.InnerOffset,
                        Name = item.Name,
                        ObjectType = item.Flag,
                        RenderId = (int) item.RenderId
                    };
                    using (var context = new SbCacheViewerContext())
                    {
                        context.ParseErrors.Add(parseError);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.Error(ex, ex.Message);
            }
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

        private async void CacheSaveButtonClick(object sender, EventArgs e)
        {
            string selectedFolder = ConfigurationManager.AppSettings["CacheExport"];
                
            //    = AppDomain.CurrentDomain.BaseDirectory;

            //using (this.folderBrowserDialog1 = new FolderBrowserDialog())
            //{
            //    DialogResult result = folderBrowserDialog1.ShowDialog();
            //    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath))
            //    {
            //        selectedFolder = folderBrowserDialog1.SelectedPath;
            //    }
            //}

            this.SaveButton.Enabled = false;
            this.CacheSaveButton.Enabled = false;

            // this.PropertiesListView.Items.Clear();
            await Task.Run(() => this.SetVisibility(this.LoadingPictureBox, true));
            ICacheObject item = (ICacheObject)this.CacheObjectTreeView.SelectedNode.Tag;

            if (item.Data.Count == 0)
            {
                throw new ApplicationException();
            }
            // make the directory
            // TODO extract to it's own method
            string directory;

            if (string.IsNullOrEmpty(item.Name))
            {
                directory = selectedFolder + "\\ObjectCache\\" + item.CacheIndex.Identity;
            }
            else
            {
                directory = selectedFolder + "\\ObjectCache\\" + item.Name + "_" + item.CacheIndex.Identity;
            }

            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }

            Directory.CreateDirectory(directory);
            await FileWriter.Writer.WriteAsync(item.Data.ToArray(), directory, "cobject.cache");

            try
            {

                // now try to create each render id if any are found
                if (item.RenderId == 0)
                {
                    logger.Error(Messages.CouldNotFindRenderId, item.CacheIndex.Identity);
                    this.ResetSaveButtons();
                    return;
                }

                var render = this.renderInformationFactory.Create((int)item.RenderId, addByteData: true);

                if (render.BinaryAsset.Item1.Count > 0)
                {
                    await this.SaveBinaryData(directory, render.CacheIndex.Name, render.BinaryAsset.Item1);
                }

                if (render.BinaryAsset.Item2.Count > 0)
                {
                    await this.SaveBinaryData(directory, render.BinaryAsset.CacheIndex2.Name,render.BinaryAsset.Item1);
                }

                // what a pain in the ass this is Microsoft.
                await Task.Run(() => this.SetVisibility(this.LoadingPictureBox, false));
                this.LoadingPictureBox.Visible = false;
                this.LoadingPictureBox.Refresh();
                this.ResetSaveButtons();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
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

        private void LogViewerToolStripMenuItemClick(object sender, EventArgs e)
        {
            LogViewer logViewer = new LogViewer();
            logViewer.Show();
        }

        private void DatabaseFormToolStripMenuItemClick(object sender, EventArgs e)
        {
            DatabaseForm dbf = new DatabaseForm();
            dbf.Show();
        }
    }
}
