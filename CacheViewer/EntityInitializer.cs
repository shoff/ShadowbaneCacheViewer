using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CacheViewer.Domain.Extensions;
using CacheViewer.Domain.Factories;
using CacheViewer.Domain.Models;
using CacheViewer.Domain.Models.Exportable;

namespace CacheViewer
{
    using CacheViewer.Data;
    using CacheViewer.Data.Entities;

    public partial class EntityInitializer : Form
    {
        private readonly SbCacheViewerContext sbCacheViewerContext;
        private readonly CacheObjectFactory cacheObjectFactory;// = CacheObjectFactory.Instance;
        private readonly RenderInformationFactory renderInformationFactory;// = RenderFactory.Instance;
        private readonly MeshFactory meshFactory;// = MeshFactory.Instance;
        private readonly TextureFactory textureFactory;// = TextureFactory.Instance;
        private readonly Process currentProc = Process.GetCurrentProcess();


        public EntityInitializer()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.sbCacheViewerContext = new SbCacheViewerContext();

                this.sbCacheViewerContext.Configuration.AutoDetectChangesEnabled = false;
                this.sbCacheViewerContext.ValidateOnSave = false;

                this.cacheObjectFactory = CacheObjectFactory.Instance;
                this.renderInformationFactory = RenderInformationFactory.Instance;
                this.meshFactory = MeshFactory.Instance;
                this.textureFactory = TextureFactory.Instance;

                this.renderInformationFactory.AppendModel = false;
            }

            this.InitializeComponent();
        }

        private async void AddRenderButtonClick(object sender, EventArgs e)
        {
            await Task.Run(this.AddRenderEntitiesAsync);
        }

        private async Task AddRenderEntitiesAsync()
        {
            int count = 0;
            int i = 0;
            int lastIdentity = 0;

            foreach (var cacheIndex in this.renderInformationFactory.Indexes)
            {
                count++;
                i++;
                if (cacheIndex.Identity == lastIdentity)
                {
                    lastIdentity = 0;
                    continue;
                }

                RenderInformation ri = this.renderInformationFactory.Create(cacheIndex);
                var re = BuildRenderEntity(ri);

                this.sbCacheViewerContext.RenderEntities.Add(re);

                if (ri.SharedId != null)
                {
                    lastIdentity = cacheIndex.Identity;
                    var re1 = BuildRenderEntity(ri.SharedId);
                    this.sbCacheViewerContext.RenderEntities.Add(re1);
                }

                this.SetMessage(this.RenderLabel, $"Processed RenderId {cacheIndex.Identity}");
                if (i > 1000)
                {
                    this.SetMessage(this.TotalRenderLabel, $"Render Count: {count}");

                    await this.sbCacheViewerContext.CommitAsync();
                    i = 0;
                }
            }
            await this.sbCacheViewerContext.CommitAsync();
        }

        private const string Vertice = "v {0} {1} {2}\r\n";
        private const string Normal = "vn {0} {1} {2}\r\n";
        private const string Texture = "vt {0} {1}\r\n";

        private void AddMeshEntities(SbCacheViewerContext context)
        {
            int i = 0;

            foreach (var cacheIndex in this.meshFactory.Indexes)
            {
                Mesh mesh = this.meshFactory.Create(cacheIndex);
                MeshEntity meshEntity = new MeshEntity
                {
                    CacheIndexIdentity = cacheIndex.Identity,
                    CompressedSize = (int)cacheIndex.CompressedSize,
                    UncompressedSize = (int)cacheIndex.CompressedSize,
                    FileOffset = (int)cacheIndex.Offset,
                    NormalsCount = (int)mesh.NormalsCount,
                    TexturesCount = (int)mesh.TextureCoordinatesCount,
                    VertexCount = (int)mesh.VertexCount,
                    Id = mesh.Id
                };
                StringBuilder sb = new StringBuilder();

                foreach (var v in mesh.Vertices)
                {
                    sb.AppendFormat(Vertice, v[0], v[1], v[2]);
                }

                meshEntity.Vertices = sb.ToString();
                sb.Clear();

                foreach (var vn in mesh.Normals)
                {
                    sb.AppendFormat(Normal, vn[0], vn[1], vn[2]);
                }
                meshEntity.Normals = sb.ToString();
                sb.Clear();

                foreach (var t in mesh.TextureVectors)
                {
                    sb.AppendFormat(Texture, t[0], t[1]);
                }

                meshEntity.TextureVectors = sb.ToString();

                i++;
                context.MeshEntities.Add(meshEntity);

                if (i > 1000)
                {
                    context.Commit();
                    i = 0;
                }
            }
            context.Commit();
        }

        private static RenderEntity BuildRenderEntity(RenderInformation ri)
        {
            RenderEntity re = new RenderEntity
            {
                ByteCount = ri.ByteCount,
                CacheIndexIdentity = ri.CacheIndex.Identity,
                CompressedSize = (int)ri.CacheIndex.CompressedSize,
                FileOffset = (int)ri.CacheIndex.Offset,
                HasMesh = ri.HasMesh,
                JointName = ri.JointName,
                MeshId = ri.MeshId,
                Notes = ri.Notes,
                Order = ri.Order,
                Position = ri.Position.ToString(),
                Scale = ri.Scale.ToString(),
                // Textures = ri.Textures,
                // TODO does not handle textures
                UncompressedSize = (int)ri.CacheIndex.UnCompressedSize,
                Children = ri.ChildRenderIdList.Map(x => new RenderChild
                {
                    RenderId = x
                }).ToList()
            };
            return re;
        }

        //public async Task FindWhereTheRenderIdIsHiding(ICacheObject entity)
        //{
        //    int count = entity.Data.Count;
        //    if (count == 0)
        //    {
        //        return;
        //    }

        //    using (var reader = entity.Data.CreateBinaryReaderUtf32())
        //    {
        //        for (int i = 25; i < count - 4; i++)
        //        {
        //            reader.BaseStream.Position = i;
        //            int id = reader.ReadInt32();

        //            if (this.TestRange(id, 1000, 77000300))
        //            {
        //                if (await this.Found(id))
        //                {
        //                    var item = this.dataContext.RenderAndOffsets.FirstOrDefault(x => x.Offset == i && x.RenderId == id);
        //                    if (item == null)
        //                    {
        //                        this.dataContext.RenderAndOffsets.Add(new RenderAndOffset
        //                        {
        //                            Offset = i,
        //                            RenderId = id,
        //                            CacheIndexIdentity = entity.CacheIndex.Identity
        //                        });

        //                        this.SetMessage(this.CObjectsLabel,
        //                            string.Format("Found matching renderId at position {0}, id is {1}", i, id));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    await this.dataContext.CommitAsync();
        //}

        //private async Task<bool> Found(int id)
        //{
        //    return await Task.Run(() =>
        //    {
        //        var renderId = this.renderInformationFactory.Indexes.FirstOrDefault(x => x.Identity == id);
        //        if (renderId.Identity > 0)
        //        {
        //            return true;
        //        }
        //        return false;
        //    });
        //}

        //private bool TestRange(int numberToCheck, int bottom, int top)
        //{
        //    return (numberToCheck > bottom && numberToCheck < top);
        //}

        private void SetMessage(Control control, string message)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => this.SetMessage(control, message)));
            }
            else
            {
                control.Text = message;
                control.Refresh();
            }
        }

        private async void CObjectsButtonClick(object sender, EventArgs e)
        {
            foreach (var cacheIndex in this.cacheObjectFactory.Indexes)
            {
                this.SetMessage(this.CurrentCObjectLabel,
                    string.Format("Now finding RenderIds for CObject at Index {0}",
                    cacheIndex.Identity));

                ICacheObject cacheObject = this.cacheObjectFactory.CreateAndParse(cacheIndex);

                this.sbCacheViewerContext.CacheObjectEntities.Add(new CacheObjectEntity
                {
                    CacheIndexIdentity = cacheIndex.Identity,
                    CompressedSize = (int)cacheIndex.CompressedSize,
                    UncompressedSize = (int)cacheIndex.UnCompressedSize,
                    FileOffset = cacheObject.InnerOffset,
                    Name = cacheObject.Name,
                    ObjectType = cacheObject.Flag,
                    ObjectTypeDescription =
                        cacheObject.Flag == ObjectType.Simple ? "Simple" :
                        cacheObject.Flag == ObjectType.Mobile ? "Mobile" :
                        cacheObject.Flag == ObjectType.Equipment ? "Equipment" :
                        cacheObject.Flag == ObjectType.Interactive ? "Interactive" :
                        cacheObject.Flag == ObjectType.Structure ? "Structure" :
                        cacheObject.Flag == ObjectType.Warrant ? "Warrant" : "",
                });

                //if (((int)cacheObject.Flag > 3) && ((int)cacheObject.Flag != 17))
                //{
                //    await FindWhereTheRenderIdIsHiding(cacheObject);
                //}
            }
            await this.sbCacheViewerContext.CommitAsync();
        }

        private void EntityInitializerLoad(object sender, EventArgs e)
        {
        }

        private void UpdateMemoryLabel(object state)
        {
            long memoryUsed = this.currentProc.WorkingSet64;
            this.SetMessage(this.MemoryLabel, string.Format("Current memory use: {0} mb.", memoryUsed > 0 ? memoryUsed / 1048576 : 0));
        }
    }
}
