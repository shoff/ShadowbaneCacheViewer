namespace CacheViewer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using CacheViewer.Data;
    using Code;
    using Domain.Archive;
    using Domain.Exporters;
    using Domain.Extensions;
    using Domain.Factories;
    using Domain.Models;
    using Domain.Models.Exportable;
    using NLog;

    public partial class MainForm : Form
    {
        // data
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // Archives
        private readonly CacheObjectsCache cacheObjectsCache;
        private readonly ListViewColumnSorter columnSorter;
        private readonly TreeNode deedNode = new TreeNode("Deeds");
        private readonly TreeNode equipmentNode = new TreeNode("Equipment");
        private readonly TreeNode interactiveNode = new TreeNode("Interactive");
        private readonly MeshOnlyObjExporter meshExporter;
        private readonly MeshFactory meshFactory;
        private readonly TreeNode mobileNode = new TreeNode("Mobiles");
        private readonly TreeNode particleNode = new TreeNode("Particles");
        private readonly RenderInformationFactory renderInformationFactory;
        private readonly TreeNode simpleNode = new TreeNode("Simple");
        private readonly Stopwatch stopwatch;
        private readonly TreeNode structureNode = new TreeNode("Structures");
        private readonly TextureFactory textureFactory;
        private readonly TreeNode unknownNode = new TreeNode("Unknown");
        private readonly TreeNode warrantNode = new TreeNode("Warrants");


        private bool archivesLoaded;

        public MainForm()
        {
            this.InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.LoadLabel.Text = "Loading";
                this.stopwatch = new Stopwatch();
                this.stopwatch.Start();
                logger.Debug("CacheViewForm created.");
                //this.AcceptButton = this.CacheSaveButton;
                this.textureFactory = TextureFactory.Instance;
                this.cacheObjectsCache = CacheObjectsCache.Instance;
                this.renderInformationFactory = RenderInformationFactory.Instance;
                this.meshExporter = MeshOnlyObjExporter.Instance;
                this.meshFactory = MeshFactory.Instance;
                this.columnSorter = new ListViewColumnSorter();
                this.RenderInformationListView.ListViewItemSorter = this.columnSorter;

                //this.TotalCacheLabel.Text = "Total number of cache objects " + this.cacheObjectFactory.Indexes.Count;
                //this.TotalCacheLabel.Refresh();
            }
        }

        private async void CObjectsTabEnter(object sender, EventArgs e)
        {
            if (this.archivesLoaded)
            {
                return;
            }

            List<TreeNode> simpleNodes = new List<TreeNode>();
            List<TreeNode> structureNodes = new List<TreeNode>();
            List<TreeNode> interactiveNodes = new List<TreeNode>();
            List<TreeNode> equipmentNodes = new List<TreeNode>();
            List<TreeNode> mobileNodes = new List<TreeNode>();
            List<TreeNode> deedNodes = new List<TreeNode>();
            List<TreeNode> warrantNodes = new List<TreeNode>();
            List<TreeNode> unknownNodes = new List<TreeNode>();
            List<TreeNode> particleNodes = new List<TreeNode>();

            //await Task.Run(() => this.SetVisibility(this.LoadingPictureBox, true));

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
                        Tag = cacheObject
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
            // this.LoadingPictureBox.Visible = false;
            // this.LoadingPictureBox.Refresh();

            // never re-enable the load cache button!
            // this.LoadCacheButton.Enabled = true;
            // this.LoadCacheButton.Refresh();
            //ResetSaveButtons();
            logger.Info("CacheViewForm completed loading all cache archives.");
            this.archivesLoaded = true;
            this.stopwatch.Stop();
            // ReSharper disable once LocalizableElement
            this.LoadLabel.Text = Messages.LoadTimeFromCache + " " + this.stopwatch.ElapsedMilliseconds + " ms.";
            this.LoadLabel.Refresh();
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            this.CObjectTreeView.Nodes.Add(this.simpleNode);
            this.CObjectTreeView.Nodes.Add(this.structureNode);
            this.CObjectTreeView.Nodes.Add(this.interactiveNode);
            this.CObjectTreeView.Nodes.Add(this.equipmentNode);
            this.CObjectTreeView.Nodes.Add(this.mobileNode);
            this.CObjectTreeView.Nodes.Add(this.deedNode);
            this.CObjectTreeView.Nodes.Add(this.unknownNode);
            this.CObjectTreeView.Nodes.Add(this.warrantNode);
            this.CObjectTreeView.Nodes.Add(this.particleNode);
        }

        private async void CObjectTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!this.archivesLoaded)
            {
                return;
            }

            this.PropertiesListView.Items.Clear();
            this.CacheIndexListView.Clear();
            this.RenderInformationListView.Items.Clear();

            // ok so right here, I need to determine what the type of object is 
            // well ok its going to be a cache object, but find the renderId and the
            // pertinent information from the renderId by validating the information
            // against the other archives. I will give each "archive" portion for each 
            // cahceObject a listView that ties all the information together at once.
            ICacheObject item = (ICacheObject)this.CObjectTreeView.SelectedNode.Tag;

            await this.CacheIndexListView.Display(item);

            await this.FindRenderIds(item);

            try
            {
                item.Parse(item.Data);

                if (item.RenderId == 0)
                {
                    logger.Error(Messages.CouldNotFindRenderId, item.CacheIndex.Identity);
                }

                this.DisplayItemInformation(item);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
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
                string name = pi[i].Name;
                if (name == "Data")
                {
                    info = name + " : Length: " + ((ArraySegment<byte>)pi[i].GetValue(item, null)).Count + "\r\n";
                }
                else if ((name == "FourIntArray") || (name == "FourThousandInt"))
                {
                    continue;
                }
                else if (name == "StatArray")
                {
                    info = name;
                    List<uint> stats = (List<uint>)pi[i].GetValue(item, null);

                    var lvii = new ListViewItem(new[] { info });

                    if (stats.Count == 0)
                    {
                        lvii.SubItems.Add("none found");
                    }
                    else
                    {
                        foreach (var stat in stats)
                        {
                            lvii.SubItems.Add(stat.ToString());
                        }
                    }
                    this.PropertiesListView.Items.Add(lvii);
                    continue;
                }
                else
                {
                    info = name + " : " + pi[i].GetValue(item, null) + "\r\n";
                }
                var lvi = new ListViewItem(new[] { info });
                this.PropertiesListView.Items.Add(lvi);
            }
        }

        public async Task FindRenderIds(ICacheObject cacheObject)
        {
            if (cacheObject == null)
            {
                // ignore, this happens because of the way the tree is set up
                return;
            }

            this.RenderInformationListView.Items.Clear();
            this.RenderInformationListView.Refresh();
            //this.MeshListView.Items.Clear();
            //this.MeshListView.Refresh();
            //this.TexturePictureBox.Image = null;
            //this.TexturePictureBox.Refresh();

            await Task.Run(
                () =>
                {
                    // this just gets the length of the data 
                    int count = cacheObject.Data.Count;
                    using (var reader = cacheObject.Data.CreateBinaryReaderUtf32())
                    {
                        for (int offset = 25; offset < count - 4; offset++)
                        {
                            reader.BaseStream.Position = offset;

                            // we're testing each possible int in this array 
                            // to see if there is a corresponding id in the 
                            // index array from the RenderArchive
                            int id = reader.ReadInt32();

                            // Only test the int if it falls within a range determined by the item itself.
                            // The identityRange is just the lowest id in the cache and the highest id, if it 
                            // falls outside of those bounds, it's obviously wrong.
                            // we filter on 6 because it seems to be a common number that continues to come up for 
                            // lots of mobiles and other cache items.
                            if (id != 6 && id != 100 &&
                                id.TestRange(this.renderInformationFactory.IdentityRange.Item1, this.renderInformationFactory.IdentityRange.Item2))
                            {
                                // simple query to look up the id
                                var found = this.renderInformationFactory.IdentityArray.Any(x => x == id);

                                if (found)
                                {
                                    // ok here we need to actually parse this renderId to make sure it has a mesh associated to it.
                                    // this pulls the raw, uncompressed cacheObject from the renderArchive
                                    var renderAsset = this.renderInformationFactory.GetById(id);

                                    // this is the mess here
                                    bool hasmesh = this.FindModelId(renderAsset.Item1).Result;

                                    if (hasmesh)
                                    {
                                        this.SetRenderItem(this.RenderInformationListView, new[]
                                        {
                                            id.ToString(CultureInfo.InvariantCulture),
                                            offset.ToString(CultureInfo.InvariantCulture),
                                            "true",
                                            "renderinfo"
                                        });
                                    }
                                }
                            }
                        }
                    }
                });

            this.RenderInformationListView.Sort();
            this.RenderInformationListView.Refresh();
        }

        private async Task<bool> FindModelId(ArraySegment<byte> data)
        {
            bool result = false;

            await Task.Run(() =>
            {
                int count = data.Count;

                using (var reader = data.CreateBinaryReaderUtf32())
                {
                    // we start at 25 because all RenderInformation cache items
                    // have the same 25 starting information which is NOT child ids.
                    for (int i = 25; i < count - 4; i++)
                    {
                        reader.BaseStream.Position = i;
                        int id = reader.ReadInt32();

                        // the identityRange is just the lowest id in the cache and the highest id, if it falls outside of those
                        // bounds, it's obviously wrong.
                        if (id.TestRange(this.meshFactory.IdentityRange.Item1, this.meshFactory.IdentityRange.Item2))
                        {
                            // only set result to true.
                            var found = this.meshFactory.IdentityArray.Any(x => x == id);

                            if (found)
                            {
                                if (!result)
                                {
                                    // ok if result is still false, set it to true
                                    // to indicate that we found a matching mesh id,
                                    // we don't want to set it to false however as this
                                    // loop is executed many times.
                                    result = true;
                                }

                                // this.MeshesLabel.SetCrossThreadedMessage("Mesh Ids");
                                //this.SetRenderItem(this.MeshListView, new[] {
                                //    id.ToString(CultureInfo.InvariantCulture), i.ToString(CultureInfo.InvariantCulture), 
                                //    "true"});}
                                //else
                                //{
                                //    this.MeshesLabel.SetCrossThreadedMessage("Mesh Ids: " + Messages.UnableToFindMathchingMeshId);
                            }
                        }
                    }
                }
            });
            return result;
        }

        private void SetRenderItem(ListView control, string[] values)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => this.SetRenderItem(control, values)));
            }
            else
            {
                var count = control.Items.Count;
                bool hasMesh = bool.Parse(values[2]);

                if (count > 0)
                {
                    int lastOffset = int.Parse(control.Items[count - 1].SubItems[1].Text);
                    int thisOffSet = int.Parse(values[1]);
                    values[2] = (thisOffSet - lastOffset).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    // set it to blank so we don't display 'true' on the first row of every item.
                    values[2] = string.Empty;
                }

                var item = new ListViewItem(values);

                if (!hasMesh)
                {
                    item.BackColor = Color.LightSkyBlue;
                }

                control.Items.Add(item);
                control.Refresh();
            }
        }

        private void databaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<CacheObject> mobiles = new List<CacheObject>();

            foreach (TreeNode mobile in this.mobileNode.Nodes)
            {
                mobiles.Add((CacheObject)mobile.Tag);
            }
            DatabaseForm dbf = new DatabaseForm(mobiles);
            dbf.Show();
        }

        private void logViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogViewer logViewer = new LogViewer();
            logViewer.Show();
        }
    }
}