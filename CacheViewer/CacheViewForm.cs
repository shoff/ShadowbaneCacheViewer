

namespace CacheViewer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Domain.Exporters;
    using Domain.Factories;
    using Domain.Models;
    using Domain.Models.Exportable;
    using NLog;
    using System.IO;
    using Data;
    using Domain.Services;
    using Domain.Services.Prefabs;

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
        private readonly CacheObjectsCache cacheObjectsCache;
        private readonly MeshOnlyObjExporter meshExporter;
        private readonly RenderInformationFactory renderInformationFactory;
        private readonly TextureFactory textureFactory;
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
                this.textureFactory = TextureFactory.Instance;
                this.cacheObjectsCache = CacheObjectsCache.Instance;
                this.renderInformationFactory = RenderInformationFactory.Instance;
                this.meshExporter = MeshOnlyObjExporter.Instance;
                this.TotalCacheLabel.Text = "Total number of cache objects " + this.cacheObjectsCache.Indexes.Count;
                this.TotalCacheLabel.Refresh();
            }
        }

        private async void LoadCacheButtonClick(object sender, EventArgs e)
        {
            this.LoadCacheButton.Enabled = false;
            this.LoadCacheButton.Refresh();

            // ReSharper disable once LocalizableElement
            this.LoadLabel.Text = Messages.LoadTimeFromCache + " " +
                TimeSpan.FromTicks(this.cacheObjectsCache.LoadTime).Seconds + Messages.Seconds;
            this.LoadLabel.Refresh();

            List<TreeNode> simpleNodes = new List<TreeNode>();
            List<TreeNode> structureNodes = new List<TreeNode>();
            List<TreeNode> interactiveNodes = new List<TreeNode>();
            List<TreeNode> equipmentNodes = new List<TreeNode>();
            List<TreeNode> mobileNodes = new List<TreeNode>();
            List<TreeNode> deedNodes = new List<TreeNode>();
            List<TreeNode> warrantNodes = new List<TreeNode>();
            List<TreeNode> unknownNodes = new List<TreeNode>();
            List<TreeNode> particleNodes = new List<TreeNode>();

            // ReSharper disable once CSharpWarnings::CS4014
            await Task.Run(() => this.SetVisibility(this.LoadingPictureBox, true));

            await Task.Run(() =>
            {
                foreach (var ci in this.cacheObjectsCache.Indexes)
                {
                    // this is not populating the cache array?
                    ICacheObject cacheObject = this.cacheObjectsCache.CreateAndParse(ci);

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
            this.LoadingPictureBox.Visible = false;
            this.LoadingPictureBox.Refresh();

            // never re-enable the load cache button!
            // this.LoadCacheButton.Enabled = true;
            // this.LoadCacheButton.Refresh();
            this.ResetSaveButtons();
            logger.Info("CacheViewForm completed loading all cache archives.");
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

            this.PropertiesListView.Items.Clear();
            ICacheObject item = (ICacheObject)this.CacheObjectTreeView.SelectedNode.Tag;
            await this.CacheIndexListView.Display(item);

            try
            {
                await Task.Run(async () =>
                {
                    StructureService service = new StructureService();
                    await service.SaveAll(item.Name.Replace(" ", ""), item.Name, item.Flag);
                });
                this.ResetSaveButtons();
                this.DisplayItemInformation(item);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }


        }
        private async void CacheObjectTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!this.archivesLoaded)
            {
                return;
            }

            this.PropertiesListView.Items.Clear();
            
            // ok so right here, I need to determine what the type of object is 
            // well ok its going to be a cache object, but find the renderId and the
            // pertinent information from the renderId by validating the information
            // against the other archives. I will give each "archive" portion for each 
            // cahceObject a listView that ties all the information together at once.
            ICacheObject item = (ICacheObject)this.CacheObjectTreeView.SelectedNode.Tag;

            await this.CacheIndexListView.Display(item);

            try
            {
                item.Parse(item.Data);

                if (item.RenderId == 0)
                {
                    logger.Error(Messages.CouldNotFindRenderId, item.CacheIndex.Identity);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }

            this.DisplayItemInformation(item);
            // goes and tries to discover the possible renderId's when this cache item is selected.
            // this REALLY should be a core part of the parser and NOT in the render control which 
            // should only have the responsibility of displaying the information.

            await this.renderControl1.FindRenderIds(item);
        }

        private void DisplayItemInformation(ICacheObject item)
        {
            if (item == null)
            {
                // just ignore
                return;
            }

            PropertyInfo[] pi = item.GetType().GetProperties();

            for (int i = 0; i < pi.Length; i++)
            {
                if (pi[i].Name == "CacheIndex")
                {
                    continue;
                }
                string info;
                if (pi[i].Name == "Data")
                {
                    info = pi[i].Name + " : Length: " + ((ArraySegment<byte>)pi[i].GetValue(item, null)).Count + "\r\n";
                }

                else
                {
                    info = pi[i].Name + " : " + pi[i].GetValue(item, null) + "\r\n";
                }
                var lvi = new ListViewItem(new[] { info });
                this.PropertiesListView.Items.Add(lvi);
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
            this.SaveButton.Enabled = false;
            this.CacheSaveButton.Enabled = false;

            // this.PropertiesListView.Items.Clear();
            await Task.Run(() => this.SetVisibility(this.LoadingPictureBox, true));

            ICacheObject item = (ICacheObject)this.CacheObjectTreeView.SelectedNode.Tag;
            await this.CacheIndexListView.Display(item);

            if (item.Data.Count == 0)
            {
                throw new ApplicationException();
            }
            // make the directory

            // TODO extract to it's own method
            string directory;

            if (string.IsNullOrEmpty(item.Name))
            {
                directory = AppDomain.CurrentDomain.BaseDirectory +"\\ObjectCache\\"+ item.CacheIndex.Identity;
            }
            else
            {
                directory = AppDomain.CurrentDomain.BaseDirectory + "\\ObjectCache\\" + item.Name + "_" + item.CacheIndex.Identity;
            }

            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }

            Directory.CreateDirectory(directory);
            // await SaveBinaryData(directory + "\\cobject.cache", item.Data);
            FileWriter.Writer.Write(item.Data, directory + "\\cobject.cache");

            try
            {
                if (item.RenderId == 0)
                {
                    logger.Error(Messages.CouldNotFindRenderId, item.CacheIndex.Identity);
                    this.ResetSaveButtons();
                    return;
                }

                var render = this.renderInformationFactory.Create((int)item.RenderId, addByteData: true);

                if (render.BinaryAsset.Item1.Count > 0)
                {
                    await this.SaveBinaryData(directory + "\\" + render.CacheIndex.Name, render.BinaryAsset.Item1);
                }
                if (render.BinaryAsset.Item2.Count > 0)
                {
                    await this.SaveBinaryData(directory + "\\" + render.BinaryAsset.CacheIndex2.Name, render.BinaryAsset.Item1);
               }
                // what a pain in the ass this is Microsoft.
                this.LoadingPictureBox.Visible = false;
                this.LoadingPictureBox.Refresh();
                this.ResetSaveButtons();

                // await this.meshExporter.ExportAsync(render.Mesh, item.Name);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        private void ResetSaveButtons()
        {
            this.SaveButton.Enabled = true;
            this.SaveButton.Refresh();
            this.CacheSaveButton.Enabled = true;
            this.CacheSaveButton.Refresh();
        }

        private async Task SaveBinaryData(string fileName, ArraySegment<byte> data)
        {

            if (data.Count > 0)
            {
                await FileWriter.Writer.WriteAsync(data, fileName);

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
            EntityInitializer ei = new EntityInitializer();
            ei.Show();
        }

        private void LogViewerToolStripMenuItemClick(object sender, EventArgs e)
        {
            LogViewer logViewer = new LogViewer();
            logViewer.Show();
        }

        private void DatabaseFormToolStripMenuItemClick(object sender, EventArgs e)
        {
            List<CacheObject> mobiles = new List<CacheObject>();

            foreach (TreeNode mobile in this.mobileNode.Nodes)
            {
                mobiles.Add((CacheObject)mobile.Tag);
            }
            DatabaseForm dbf = new DatabaseForm(mobiles);
            dbf.Show();
        }

    }
}
