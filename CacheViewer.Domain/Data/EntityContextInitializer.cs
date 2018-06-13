namespace CacheViewer.Domain.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using Entities;
    using Extensions;
    using Factories;
    using Models;
    using Models.Exportable;

    public class EntitiesContextInitializer : CreateDatabaseIfNotExists<DataContext>
    {
        private const string Vertice = "v {0} {1} {2}\r\n";
        private const string Normal = "vn {0} {1} {2}\r\n";
        private const string Texture = "vt {0} {1}\r\n";
        private readonly CacheObjectsCache cacheObjectsCache = CacheObjectsCache.Instance;
        private readonly MeshFactory meshFactory = MeshFactory.Instance;
        private readonly RenderFactory renderFactory = RenderFactory.Instance;
        private readonly TextureFactory textureFactory = TextureFactory.Instance;

        /// <summary>
        ///     Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(DataContext context)
        {
            context.Configuration.AutoDetectChangesEnabled = false;
            context.ValidateOnSave = false;
            this.renderFactory.AppendModel = false;
            this.AddRenderEntities(context);
            this.AddCacheObjectEntities(context);
            this.AddTextureEntities(context);
            this.AddMeshEntities(context);
            this.AddCZoneEntites(context);
        }

        private void AddCZoneEntites(DataContext context)
        {
            // TODO
        }

        private void AddTextureEntities(DataContext context)
        {
            var i = 0;
            foreach (var cacheIndex in this.textureFactory.Indexes)
            {
                i++;
                var texture = this.textureFactory.Build(cacheIndex.Identity, false);
                context.Textures.Add(texture);

                if (i > 1000)
                {
                    context.Commit();
                    i = 0;
                }
            }

            context.Commit();
        }

        private void AddMeshEntities(DataContext context)
        {
            var i = 0;

            foreach (var cacheIndex in this.meshFactory.Indexes)
            {
                var mesh = this.meshFactory.Create(cacheIndex);
                var meshEntity = new MeshEntity
                {
                    CacheIndexIdentity = cacheIndex.Identity,
                    CompressedSize = (int) cacheIndex.CompressedSize,
                    UncompressedSize = (int) cacheIndex.CompressedSize,
                    FileOffset = (int) cacheIndex.Offset,
                    NormalsCount = (int) mesh.NormalsCount,
                    TexturesCount = (int) mesh.TextureCoordinatesCount,
                    VertexCount = (int) mesh.VertexCount,
                    Id = mesh.Id
                };
                var sb = new StringBuilder();

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

        private void AddRenderEntities(DataContext context)
        {
            var i = 0;
            var lastIdentity = 0;

            foreach (var cacheIndex in this.renderFactory.Indexes)
            {
                i++;
                if (cacheIndex.Identity == lastIdentity)
                {
                    lastIdentity = 0;
                    continue;
                }

                var ri = this.renderFactory.Create(cacheIndex);
                var re = BuildRenderEntity(ri);

                context.RenderEntities.Add(re);

                if (ri.SharedId != null)
                {
                    lastIdentity = cacheIndex.Identity;
                    var re1 = BuildRenderEntity(ri.SharedId);
                    context.RenderEntities.Add(re1);
                }

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
            var re = new RenderEntity
            {
                ByteCount = ri.ByteCount,
                CacheIndexIdentity = ri.CacheIndex.Identity,
                CompressedSize = (int) ri.CacheIndex.CompressedSize,
                FileOffset = (int) ri.CacheIndex.Offset,
                HasMesh = ri.HasMesh,
                JointName = ri.JointName,
                MeshId = ri.MeshId,
                Notes = ri.Notes,
                Order = ri.Order,
                Position = ri.Position.ToString(),
                Scale = ri.Scale.ToString(),
                TextureId = ri.TextureId,

                UncompressedSize = (int) ri.CacheIndex.UnCompressedSize,
                Children = ri.ChildRenderIdList.Map(x => new RenderChild
                {
                    RenderId = x
                }).ToList()
            };
            return re;
        }

        private void AddCacheObjectEntities(DataContext context)
        {
            var i = 0;
            foreach (var cacheIndex in this.cacheObjectsCache.Indexes)
            {
                i++;
                var cacheObject = this.cacheObjectsCache.Create(cacheIndex);
                var entity = new CacheObjectEntity
                {
                    CacheIndexIdentity = cacheIndex.Identity,
                    CompressedSize = (int) cacheIndex.CompressedSize,
                    UncompressedSize = (int) cacheIndex.UnCompressedSize,
                    FileOffset = (int) cacheIndex.Offset,
                    RenderKey = (int) cacheObject.RenderId,
                    Name = cacheObject.Name,
                    // Data = cacheObject.Data,
                    ObjectType = (int) cacheObject.Flag,
                    ObjectTypeDescription = cacheObject.Flag.ToString()
                };

                //if ((entity.ObjectType > 3) && (entity.ObjectType != 17))
                //{
                //    Find_Where_The_RenderId_Is_Hiding(cacheObject, context);
                //}

                context.CacheObjectEntities.Add(entity);

                if (i > 1000)
                {
                    context.Commit();
                    i = 0;
                }
            }

            context.Commit();
        }

        public void Find_Where_The_RenderId_Is_Hiding(ICacheObject entity, DataContext context)
        {
            var count = entity.Data.Count;
            using (var reader = entity.Data.CreateBinaryReaderUtf32())
            {
                for (var i = 25; i < count - 4; i++)
                {
                    reader.BaseStream.Position = i;
                    var id = reader.ReadInt32();
                    if (this.TestRange(id, 1000, 77000300))
                    {
                        if (this.Found(id))
                        {
                            context.RenderAndOffsets.Add(new RenderAndOffset
                            {
                                Offset = i,
                                RenderId = id,
                                CacheIndexIdentity = entity.CacheIndex.Identity
                            });
                            Console.WriteLine("Found matching renderId at position {0}, id is {1}", i, id);
                        }
                    }
                }
            }
        }

        private bool Found(int id)
        {
            var renderId = this.renderFactory.Indexes.FirstOrDefault(x => x.Identity == id);
            if (renderId.Identity > 0)
            {
                return true;
            }

            return false;
        }

        private bool TestRange(int numberToCheck, int bottom, int top)
        {
            return numberToCheck > bottom && numberToCheck < top;
        }
    }
}