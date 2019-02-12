namespace CacheViewer.Domain.Factories
{
    using System;
    using System.Linq;
    using Archive;
    using Exceptions;
    using Extensions;
    using Models;
    using NLog;
    using Providers;

    public class RenderInformationFactory
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly MeshFactory meshFactory;
        private static readonly RenderInformationFactory instance = new RenderInformationFactory();
        private RenderInformationFactory()
        {
            this.RenderArchive = (Render)ArchiveFactory.Instance.Build(CacheFile.Render);
            this.meshFactory = MeshFactory.Instance;
            this.AppendModel = true;
        }

        public RenderInformation Create(CacheIndex cacheIndex, int order = 0)
        {
            try
            {
                return this.Create(cacheIndex.Identity, order);
            }
            catch (Exception e)
            {
                logger.Error(e, "Failed to created RenderInformation for cacheIndex {0}", cacheIndex.Identity);
                throw;
            }
        }

        public RenderInformation Create(int identity, int order = 0, bool addByteData = false)
        {
            // TODO why am I getting the asset here, then when I create the renderInfo below,
            // TODO I'm doing a linq query for the initial cache index?
            var asset = this.RenderArchive[identity];

            // should duplicate identity render indexes be allowed?
            var indexesCount = this.Indexes.Count(x => x.Identity == identity);
            if (indexesCount > 1)
            {
                logger?.Warn($"Multiple ({indexesCount}) Render Cache Index items have the identity {identity}");
            }

            var renderInfo = new RenderInformation
            {
                CacheIndex = this.Indexes.First(x => x.Identity == identity),
                ByteCount = order == 0 ? asset.Item1.Count : asset.Item2.Count,
                Unknown = new object[6],
                Order = order
            };

            if (addByteData)
            {
                renderInfo.BinaryAsset = asset;
            }

            var arraySegment = order == 0 ? asset.Item1 : asset.Item2;

            using (var reader = arraySegment.CreateBinaryReaderUtf32())
            {
                if (Array.IndexOf(RenderProviders.type4RenderInfos, renderInfo.CacheIndex.Identity) > -1)
                {
                    reader.ParseTypeFour(renderInfo);
                }
                else if (Array.IndexOf(RenderProviders.type2RenderInfos, renderInfo.CacheIndex.Identity) > -1)
                {
                    reader.ParseTypeTwo(renderInfo);
                }
                else if (Array.IndexOf(RenderProviders.type3RenderInfos, renderInfo.CacheIndex.Identity) > -1)
                {
                    reader.ParseTypeThree(renderInfo);
                }
                else
                {
                    reader.ParseTypeOne(renderInfo);
                }

                renderInfo.UnreadByteCount = reader.BaseStream.Length - reader.BaseStream.Position;

                if (renderInfo.HasMesh)
                {
                    renderInfo.ValidMeshFound = this.HandleMesh(renderInfo);
                }
            }

            if (asset.CacheIndex2.Identity > 0 && renderInfo.Order == 0)
            {
                renderInfo.SharedId = this.Create(asset.CacheIndex2, 1);
            }

            return renderInfo;
        }

        internal bool HandleMesh(RenderInformation renderInfo)
        {
            // render objects only ever have 1 mesh 
            try
            {
                if (this.AppendModel)
                {
                    if (this.IsValidMeshId(renderInfo.MeshId))
                    {
                        renderInfo.Mesh = MeshFactory.Instance.Create(renderInfo.MeshId);
                        renderInfo.Mesh.Scale = renderInfo.Scale;
                        renderInfo.Mesh.Position = renderInfo.Position;

                        if (renderInfo.HasTexture && renderInfo.TextureCount > 0 && renderInfo.Textures.Count > 0)
                        {
                            foreach (var textureId in renderInfo.Textures)
                            {
                                renderInfo.Mesh.Textures.Add(TextureFactory.Instance.Build(textureId));
                            }
                        }
                    }
                    else
                    {
                        renderInfo.Notes =
                            $"{renderInfo.CacheIndex.Identity} claimed to have a mesh with MeshId {renderInfo.MeshId}, however the MeshCache does not contain an item with that id.";
                        logger.Warn(renderInfo.Notes);
                        return false;
                    }
                }
            }
            catch (IndexNotFoundException infe)
            {
                renderInfo.Notes = string.Join("\r\n", new
                {
                    renderInfo.Notes,
                    infe.Message
                });
                logger.Error(infe);
                return false;
            }
            return true;
        }

        public bool IsValidRenderId(int id)
        {
            return (from c in this.Indexes where c.Identity == id select c).Any();
        }

        internal bool IsValidMeshId(int id)
        {
            // TODO this does not belong here. 
            if (id.TestRange(this.meshFactory.IdentityRange.Item1, this.meshFactory.IdentityRange.Item2))
            {
                return this.meshFactory.IdentityArray.Any(x => x == id);
            }

            return false;
        }

        public static RenderInformationFactory Instance { get; } = instance;

        public CacheIndex[] Indexes => this.RenderArchive.CacheIndices.ToArray();

        internal Render RenderArchive { get; }

        public bool AppendModel { get; set; }

    }
}