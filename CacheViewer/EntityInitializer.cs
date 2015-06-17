using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CacheViewer.Domain.Data;
using CacheViewer.Domain.Data.Entities;
using CacheViewer.Domain.Extensions;
using CacheViewer.Domain.Factories;
using CacheViewer.Domain.Models;
using CacheViewer.Domain.Models.Exportable;

namespace CacheViewer
{
    public partial class EntityInitializer : Form
    {
        private readonly DataContext dataContext;
        private readonly CacheObjectFactory cacheObjectFactory;// = CacheObjectFactory.Instance;
        private readonly RenderFactory renderFactory;// = RenderFactory.Instance;
        private readonly MeshFactory meshFactory;// = MeshFactory.Instance;
        private readonly TextureFactory textureFactory;// = TextureFactory.Instance;
        private readonly Process currentProc = Process.GetCurrentProcess();


        public EntityInitializer()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.dataContext = new DataContext();

                this.dataContext.Configuration.AutoDetectChangesEnabled = false;
                this.dataContext.ValidateOnSave = false;

                this.cacheObjectFactory = CacheObjectFactory.Instance;
                this.renderFactory = RenderFactory.Instance;
                this.meshFactory = MeshFactory.Instance;
                this.textureFactory = TextureFactory.Instance;

                this.renderFactory.AppendModel = false;
            }
            InitializeComponent();
        }

        private async void AddRenderButtonClick(object sender, EventArgs e)
        {
            await Task.Run(() => this.AddRenderEntitiesAsync());
        }

        private async Task AddRenderEntitiesAsync()
        {
            int count = 0;
            int i = 0;
            int lastIdentity = 0;

            foreach (var cacheIndex in this.renderFactory.Indexes)
            {
                count++;
                i++;
                if (cacheIndex.identity == lastIdentity)
                {
                    lastIdentity = 0;
                    continue;
                }

                RenderInformation ri = renderFactory.Create(cacheIndex);
                var re = BuildRenderEntity(ri);

                dataContext.RenderEntities.Add(re);

                if (ri.SharedId != null)
                {
                    lastIdentity = cacheIndex.identity;
                    var re1 = BuildRenderEntity(ri.SharedId);
                    dataContext.RenderEntities.Add(re1);
                }
                SetMessage(this.RenderLabel, string.Format("Processed RenderId {0}", cacheIndex.identity));
                if (i > 1000)
                {
                    SetMessage(this.TotalRenderLabel, string.Format("Render Count: {0}", count));

                    await dataContext.CommitAsync();
                    i = 0;
                }
            }
            await dataContext.CommitAsync();
        }

        private void AddTextureEntities(DataContext context)
        {
            int i = 0;
            foreach (var cacheIndex in this.textureFactory.Indexes)
            {
                i++;
                Texture texture = textureFactory.Build(cacheIndex.identity, false);
                context.Textures.Add(texture);

                if (i > 1000)
                {
                    context.Commit();
                    i = 0;
                }
            }
            context.Commit();
        }

        private const string Vertice = "v {0} {1} {2}\r\n";
        private const string Normal = "vn {0} {1} {2}\r\n";
        private const string Texture = "vt {0} {1}\r\n";

        private void AddMeshEntities(DataContext context)
        {
            int i = 0;

            foreach (var cacheIndex in this.meshFactory.Indexes)
            {
                Mesh mesh = this.meshFactory.Create(cacheIndex);
                MeshEntity meshEntity = new MeshEntity
                {
                    CacheIndexIdentity = cacheIndex.identity,
                    CompressedSize = (int)cacheIndex.compressedSize,
                    UncompressedSize = (int)cacheIndex.compressedSize,
                    FileOffset = (int)cacheIndex.offset,
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
                CacheIndexIdentity = ri.CacheIndex.identity,
                CompressedSize = (int)ri.CacheIndex.compressedSize,
                FileOffset = (int)ri.CacheIndex.offset,
                HasMesh = ri.HasMesh,
                JointName = ri.JointName,
                MeshId = ri.MeshId,
                Notes = ri.Notes,
                Order = ri.Order,
                Position = ri.Position.ToString(),
                Scale = ri.Scale.ToString(),
                TextureId = ri.TextureId,

                UncompressedSize = (int)ri.CacheIndex.unCompressedSize,
                Children = ri.ChildRenderIdList.Map(x => new RenderChild
                {
                    RenderId = x
                }).ToList()
            };
            return re;
        }

        public async Task FindWhereTheRenderIdIsHiding(ICacheObject entity)
        {
            int count = entity.Data.Count;
            if (count == 0)
            {
                return;
            }

            using (var reader = entity.Data.CreateBinaryReaderUtf32())
            {
                for (int i = 25; i < count - 4; i++)
                {
                    reader.BaseStream.Position = i;
                    int id = reader.ReadInt32();

                    if (TestRange(id, 1000, 77000300))
                    {
                        if (await Found(id))
                        {
                            var item = this.dataContext.RenderAndOffsets.FirstOrDefault(x => x.Offset == i && x.RenderId == id);
                            if (item == null)
                            {
                                this.dataContext.RenderAndOffsets.Add(new RenderAndOffset
                                {
                                    Offset = i,
                                    RenderId = id,
                                    CacheIndexIdentity = entity.CacheIndex.identity
                                });

                                SetMessage(this.CObjectsLabel,
                                    string.Format("Found matching renderId at position {0}, id is {1}", i, id));
                            }
                        }
                    }
                }
            }
            await this.dataContext.CommitAsync();
        }

        private async Task<bool> Found(int id)
        {
            return await Task.Run(() =>
            {
                var renderId = this.renderFactory.Indexes.FirstOrDefault(x => x.identity == id);
                if (renderId.identity > 0)
                {
                    return true;
                }
                return false;
            });
        }

        private bool TestRange(int numberToCheck, int bottom, int top)
        {
            return (numberToCheck > bottom && numberToCheck < top);
        }

        private void SetMessage(Control control, string message)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => SetMessage(control, message)));
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
                SetMessage(this.CurrentCObjectLabel,
                    string.Format("Now finding RenderIds for CObject at Index {0}",
                    cacheIndex.identity));

                ICacheObject cacheObject = this.cacheObjectFactory.Create(cacheIndex);

                this.dataContext.CacheObjectEntities.Add(new CacheObjectEntity
                {
                    CacheIndexIdentity = cacheIndex.identity,
                    CompressedSize = (int)cacheIndex.compressedSize,
                    UncompressedSize = (int)cacheIndex.unCompressedSize,
                    FileOffset = cacheObject.InnerOffset,
                    Name = cacheObject.Name,
                    ObjectType = (int)cacheObject.Flag,
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
            await this.dataContext.CommitAsync();
        }

        private void EntityInitializerLoad(object sender, EventArgs e)
        {
        }

        private void UpdateMemoryLabel(object state)
        {
            long memoryUsed = currentProc.WorkingSet64;
            this.SetMessage(this.MemoryLabel, string.Format("Current memory use: {0} mb.", memoryUsed > 0 ? memoryUsed / 1048576 : 0));
        }
    }
}
