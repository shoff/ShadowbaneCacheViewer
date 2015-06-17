namespace CacheViewer.Domain.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models.Exportable;

    public class RenderListViewItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderListViewItem"/> class.
        /// </summary>
        public RenderListViewItem()
        {
            this.Meshes = new List<RenderListViewMeshItem>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string RenderId { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public string Offset { get; set; }

        /// <summary>
        /// Gets the meshes.
        /// </summary>
        /// <value>
        /// The meshes.
        /// </value>
        public List<RenderListViewMeshItem> Meshes { get; private set; }
    }

    public class RenderIdService
    {
        private readonly IModelIdService modelIdService;
        private readonly RenderFactory renderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderIdService"/> class.
        /// </summary>
        /// <param name="modelIdService">The model identifier service.</param>
        public RenderIdService(IModelIdService modelIdService = null)
        {
            this.modelIdService = modelIdService ?? new ModelIdService();
            this.renderFactory = RenderFactory.Instance;
        }

        /// <summary>
        /// Finds the render ids.
        /// </summary>
        /// <param name="cacheObject">The cache object.</param>
        /// <returns></returns>
        public async Task<ICollection<RenderListViewItem>> FindRenderIds(ICacheObject cacheObject)
        {
            if (cacheObject == null)
            {
                // ignore, this happens because of the way the tree is set up
                return new List<RenderListViewItem>();
            }
            return await GetAsync(cacheObject);
        }

        private async Task<ICollection<RenderListViewItem>> GetAsync(ICacheObject cacheObject)
        {
            List<RenderListViewItem> items = new List<RenderListViewItem>();

            await Task.Run(() =>
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
                        if (id.TestRange(this.renderFactory.IdentityRange.Item1, this.renderFactory.IdentityRange.Item2))
                        {
                            // simple query to look up the id 
                            var found = this.renderFactory.IdentityArray.Where(x => x == id).Any();

                            // if we found a possible RenderId, then we need to see if it has a mesh associated with it.
                            if (found)
                            {
                                // ok here we need to actually parse this renderId to make sure it has a mesh associated to it.
                                // this pulls the raw, uncompressed cacheObject from the renderArchive
                                var asset = this.renderFactory.GetById(id);

                                var renderItem = new RenderListViewItem
                                                        {
                                                            Offset = offset.ToString(),
                                                            RenderId = id.ToString()
                                                        };
                                //// this is the mess here
                                var meshesItem1 = this.modelIdService.FindModelId(asset.Item1);
                                renderItem.Meshes.AddRange(meshesItem1);

                                if (asset.Item2.Count > 0)
                                {
                                    var meshesItem2 = this.modelIdService.FindModelId(asset.Item2, 2);
                                    renderItem.Meshes.AddRange(meshesItem2);
                                }
                                items.Add(renderItem);
                            }
                        }
                    }
                }
            });
            return items;

            //this.RenderInformationListView.Sort();
            //this.RenderInformationListView.Refresh();
        }
    }
}