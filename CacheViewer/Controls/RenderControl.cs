
namespace CacheViewer.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using CacheViewer.Code;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models;
    using CacheViewer.Domain.Models.Exportable;
    using CacheViewer.Domain.Services;
    using NLog;

    /// <summary>
    /// </summary>
    public partial class RenderControl : UserControl
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable

        private readonly ListViewColumnSorter columnSorter;
        private readonly RenderInformationFactory renderInformationFactory;
        private readonly MeshFactory meshFactory;
        private readonly IModelIdService modelIdService;

        // ReSharper disable once NotAccessedField.Local
        /// <summary>
        /// </summary>
        private readonly TextureFactory textureFactory;

        /// <summary>
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        public RenderControl()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.modelIdService = new ModelIdService();
                this.textureFactory = TextureFactory.Instance;
                this.meshFactory = MeshFactory.Instance;
                this.renderInformationFactory = RenderInformationFactory.Instance;
                this.columnSorter = new ListViewColumnSorter();
                this.RenderInformationListView.ListViewItemSorter = this.columnSorter;

            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void RenderInformationListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.RenderInformationListView.SelectedItems.Count == 0)
            {
                return;
            }

            this.MeshListView.Items.Clear();
            this.MeshListView.Refresh();

            var item = this.RenderInformationListView.SelectedItems[0].SubItems[0];

            // ok get the renderIndex from cache
            var id = int.Parse(item.Text);

            if (this.renderInformationFactory.IdentityArray.Any(x => x == id))
            {
                var cacheAsset = this.renderInformationFactory.GetById(id);

                // TODO THIS IS FUCKED HERE 4/21/2015
                // there is something Fucked in the get children part.


                // TODO this is still fucked but only here, the getting part if fixed, I think
                // 5/13/2015
                var renderInformation = this.renderInformationFactory.Create(id, addByteData: true);

                // await this.FindModelId(cacheAsset.Item1);
                var idList = this.modelIdService.FindModelId(cacheAsset.Item1);

                if (idList.Any())
                {
                    this.MeshesLabel.SetCrossThreadedMessage("Mesh Ids");
                    foreach (var meshId in idList)
                    {
                        this.SetRenderItem(this.MeshListView, new[]
                        {
                            meshId.Id.ToString(CultureInfo.InvariantCulture), 
                            meshId.Offset.ToString(CultureInfo.InvariantCulture), 
                            "true"
                        });
                    }
                }
                else
                {
                    this.MeshesLabel.SetCrossThreadedMessage("CacheAsset 1 Mesh Ids: " + Messages.UnableToFindMathchingMeshId);
                }

                if (cacheAsset.Item2.Count > 0)
                {
                    // await this.FindModelId(cacheAsset.Item2);
                    var idList1 = this.modelIdService.FindModelId(cacheAsset.Item2, 2);

                    if (idList1.Any())
                    {
                        this.MeshesLabel.SetCrossThreadedMessage("Mesh Ids");
                        foreach (var meshId in idList1.Where(x => x.ItemNumber == 2))
                        {
                            this.SetRenderItem(this.MeshListView, new[]
                            {
                                meshId.Id.ToString(CultureInfo.InvariantCulture), 
                                meshId.Offset.ToString(CultureInfo.InvariantCulture), 
                                "true"
                            });
                        }
                    }
                    else
                    {
                        this.MeshesLabel.SetCrossThreadedMessage("CacheAsset 2 Mesh Ids: " + Messages.UnableToFindMathchingMeshId);
                    }
                }
            }
            else
            {
                logger.Error(Messages.RenderFactoryDoesNotHaveMatchingId, id);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="control">
        /// </param>
        /// <param name="values">
        /// </param>
        private void SetRenderItem(ListView control, string[] values)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => SetRenderItem(control, values)));
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

        /// <summary>
        /// </summary>
        /// <param name="cacheObject">
        /// </param>
        /// <returns>
        /// </returns>
        public async Task FindRenderIds(ICacheObject cacheObject)
        {
            if (cacheObject == null)
            {
                // ignore, this happens because of the way the tree is set up
                return;
            }

            this.RenderInformationListView.Items.Clear();
            this.RenderInformationListView.Refresh();
            this.MeshListView.Items.Clear();
            this.MeshListView.Refresh();
            this.TexturePictureBox.Image = null;
            this.TexturePictureBox.Refresh();

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
                            if (id.TestRange(this.renderInformationFactory.IdentityRange.Item1, this.renderInformationFactory.IdentityRange.Item2))
                            {
                                // simple query to look up the id
                                var found = this.renderInformationFactory.IdentityArray.Where(x => x == id).Any();

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
                                                offset.ToString(CultureInfo.InvariantCulture), "true"
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

        /// <summary>
        /// </summary>
        /// <param name="data">
        /// </param>
        /// <returns>
        /// </returns>
        private async Task<bool> FindModelId(ArraySegment<byte> data)
        {
            bool result = false;

            await Task.Run(
                () =>
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
                                var found = this.meshFactory.IdentityArray.Where(x => x == id).Any();

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

                                    this.MeshesLabel.SetCrossThreadedMessage("Mesh Ids");
                                    this.SetRenderItem(
                                        this.MeshListView,
                                        new[]
                                                {
                                                    id.ToString(CultureInfo.InvariantCulture), i.ToString(CultureInfo.InvariantCulture), 
                                                    "true"
                                                });
                                }
                                else
                                {
                                    this.MeshesLabel.SetCrossThreadedMessage("Mesh Ids: " + Messages.UnableToFindMathchingMeshId);
                                }
                            }
                        }
                    }
                });
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void MeshListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.MeshListView.SelectedItems.Count == 0)
            {
                return;
            }

            this.TexturePictureBox.Image = null;
            this.TexturePictureBox.Refresh();
            var item = this.MeshListView.SelectedItems[0].SubItems[0];

            // ok get the renderIndex from cache
            var id = int.Parse(item.Text);

            if (this.textureFactory.IdentityArray.Any(x => x == id))
            {
                Texture texture = this.textureFactory.Build(id);
                var image = texture.TextureMap(this.textureFactory.GetById(id).Item1);
                this.TexturePictureBox.Image = image;
                this.TexturePictureBox.Refresh();
            }
            else
            {
                logger.Error(Messages.RenderFactoryDoesNotHaveMatchingId, id);
            }
        }
    }
}
