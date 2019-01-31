namespace CacheViewer.Domain.Factories
{
    using System;
    using System.IO;
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

            /*
                char crap[35];
                int hasMesh;
                int null1;      // 4
                int meshId;     // 4
                ushort null1;   // 2
                int jointNameSize;
                wchar_t name[jointNameSize];
                // unknown float
                float ux;
                float uy;
                float uz;
                // no clue int
                int unknown1; // a boolean?
             */

            // this really blows.
            // TODO allow for number 3?
            var arraySegment = order == 0 ? asset.Item1 : asset.Item2;

            using (var reader = arraySegment.CreateBinaryReaderUtf32())
            {

                if (Array.IndexOf(RenderProviders.type2RenderInfos, renderInfo.CacheIndex.Identity) > -1)
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
            //var nullByte = reader.ReadUInt32(); // skip over null byte
            //Debug.Assert(nullByte == 0);

            //// TODO validate all meshIds
            //renderInfo.MeshId = reader.ReadInt32();

            //while (renderInfo.MeshId == 0)
            //{
            //    renderInfo.Notes =
            //        $"{renderInfo.CacheIndex.Identity} claimed to have a mesh, however the meshId read was 0.";
            //    logger.Warn(renderInfo.Notes);

            //    // TODO this could blow through the index
            //    renderInfo.MeshId = reader.ReadInt32();
            //    if (renderInfo.MeshId == 1)
            //    {
            //        // experiment see if the next byte is the same
            //        var x = reader.ReadInt32();
            //        logger?.Fatal($"MeshId was 0 then 1 and on third read is {x} for {renderInfo.CacheIndex.Identity}");
            //    }
            //}

            try
            {
                if (this.AppendModel)
                {
                    if (this.IsValidMeshId(renderInfo.MeshId))
                    {
                        renderInfo.Mesh = MeshFactory.Instance.Create(renderInfo.MeshId);
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

            //// null short
            //var nullShort = reader.ReadUInt16();
            //Debug.Assert(nullShort == 0);

            //var size = reader.ReadUInt32();

            //if (size > 0)
            //{
            //    // since this size count is actually a Unicode character count, each number is actually 2 bytes.
            //    renderInfo.JointName = reader.ReadAsciiString(size);
            //    renderInfo.JointName = !string.IsNullOrEmpty(renderInfo.JointName)
            //        ? renderInfo.JointName.Replace(" ", string.Empty) : string.Empty;
            //}

            return true;
        }

        internal void HandleChildren(BinaryReader reader, RenderInformation renderInfo)
        {
            if (renderInfo.RenderCount > 1000)
            {
                // bail
                var message =
                    $"{renderInfo.CacheIndex.Identity} claimed to have a {renderInfo.RenderCount} child render nodes, bailing out.";
                logger.Error(message);
                return;
            }

            renderInfo.ChildRenderIds = new CacheIndex[renderInfo.RenderCount];

            for (var i = 0; i < renderInfo.RenderCount; i++)
            {
                reader.BaseStream.Position += 4;
                var childId = reader.ReadInt32();
                renderInfo.ChildRenderIdList.Add(childId);
            }
        }

        //internal void HandleTexture(BinaryReader reader, ref RenderInformation renderInfo)
        //{
        //    reader.BaseStream.Position += 12; // this seems to always be x:0 y:0 z:0 
        //    renderInfo.TextureId = reader.ReadInt32();

        //    if (renderInfo.TextureId == 0)
        //    {
        //        renderInfo.Notes = "RenderId claimed to have a texture but the archive reports the id as 0!";
        //        return;
        //    }

        //    renderInfo.Texture = TextureFactory.Instance.Build(renderInfo.TextureId);
        //}

        public CacheAsset GetById(int identity)
        {
            return this.RenderArchive[identity];
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

        public int TotalRenderItems => this.RenderArchive.CacheIndices.Length;

        public bool AppendModel { get; set; }

        public Tuple<int, int> IdentityRange =>
            new Tuple<int, int>(this.RenderArchive.LowestId, this.RenderArchive.HighestId);

        public int[] IdentityArray => this.RenderArchive.IdentityArray;
    }
}