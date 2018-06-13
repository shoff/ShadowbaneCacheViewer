namespace CacheViewer.Domain.Factories
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Exceptions;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Models;
    using NLog;
    
    public class RenderInformationFactory
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly MeshFactory meshFactory;

        private RenderInformationFactory()
        {
            this.RenderArchive = (Render) ArchiveFactory.Instance.Build(CacheFile.Render);
            this.meshFactory = MeshFactory.Instance;
            this.AppendModel = true;
        }

        public RenderInformation Create(CacheIndex cacheIndex, int order = 0)
        {
            try
            {
                return Create(cacheIndex.Identity, order);
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
                // see if this has a joint
                renderInfo.HasJoint = reader.ReadUInt32() == 1;

                // null bytes
                reader.ReadUInt16();
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                long seconds = reader.ReadInt32();
                if (seconds > 0)
                {
                    renderInfo.CreateDate = epoch + TimeSpan.FromSeconds(seconds);
                }

                if (!renderInfo.HasJoint)
                {
                    logger?.Warn($"This is not a 'join' type render info we don't know how to handle this type yet.");
                    return renderInfo;
                }

                Debug.Assert(reader.BaseStream.Position == 10);
                renderInfo.B11 = reader.ReadByte();

                // let's play with the first 35 bytes
                //  this.PlayWithFirst35(buffer, renderId);
                reader.BaseStream.Position = 34;
                renderInfo.B34 = reader.ReadByte();

                // offset 35
                renderInfo.HasMesh = reader.ReadUInt32() == 1;

                // offset 39
                renderInfo.Unknown[0] = reader.ReadUInt32();
                // Debug.Assert(nullInt == 0);

                renderInfo.MeshId = reader.ReadInt32();

                renderInfo.Unknown[1] = reader.ReadUInt16();
                //Debug.Assert(nullShort == 0);

                // testing
                var jointNameSize = reader.ReadUInt32();
                if (reader.BaseStream.Position + jointNameSize <= renderInfo.ByteCount)
                {
                    renderInfo.JointName = reader.ReadAsciiString(jointNameSize);
                }

                if (reader.BaseStream.Position + 12 <= renderInfo.ByteCount)
                {
                    // object scale ?
                    renderInfo.Scale = reader.ReadToVector3();
                }

                if (reader.BaseStream.Position + 4 <= renderInfo.ByteCount)
                {
                    // I think this is probably a bool or flag of some kind
                    renderInfo.Unknown[2] = reader.ReadUInt32();
                }

                if (reader.BaseStream.Position + 12 <= renderInfo.ByteCount)
                {
                    // TODO this is where it gets fucked. 
                    // object position ?
                    renderInfo.Position = reader.ReadToVector3();
                }
                // TODO
                // this is incorrect
                //// Render object count
                //renderInfo.RenderCount = reader.ReadInt32();
                //HandleChildren(reader, renderInfo);

                //var hasTexture = reader.ReadBoolean();

                //if (hasTexture)
                //{
                //    // skip over unknown data to the texture id another vector3 ?
                //    HandleTexture(reader, ref renderInfo);
                //}

                if (renderInfo.HasMesh)
                {
                    renderInfo.ValidMeshFound = HandleMesh(renderInfo);
                }
            }

            if (asset.CacheIndex2.Identity > 0 && renderInfo.Order == 0)
            {
                renderInfo.SharedId = Create(asset.CacheIndex2, 1);
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
                    if (IsValidId(renderInfo.MeshId))
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

        internal void HandleTexture(BinaryReader reader, ref RenderInformation renderInfo)
        {
            reader.BaseStream.Position += 12; // this seems to always be x:0 y:0 z:0 
            renderInfo.TextureId = reader.ReadInt32();

            if (renderInfo.TextureId == 0)
            {
                renderInfo.Notes = "RenderId claimed to have a texture but the archive reports the id as 0!";
                return;
            }

            renderInfo.Texture = TextureFactory.Instance.Build(renderInfo.TextureId);
        }

        public CacheAsset GetById(int identity)
        {
            return this.RenderArchive[identity];
        }

        internal bool IsValidId(int id)
        {
            // TODO this does not belong here. 
            if (id.TestRange(this.meshFactory.IdentityRange.Item1, this.meshFactory.IdentityRange.Item2))
            {
                return this.meshFactory.IdentityArray.Any(x => x == id);
            }

            return false;
        }

        public static RenderInformationFactory Instance { get; } = new RenderInformationFactory();

        public CacheIndex[] Indexes => this.RenderArchive.CacheIndices.ToArray();

        internal Render RenderArchive { get; }
        public bool AppendModel { get; set; }

        public Tuple<int, int> IdentityRange =>
            new Tuple<int, int>(this.RenderArchive.LowestId, this.RenderArchive.HighestId);

        public int[] IdentityArray => this.RenderArchive.IdentityArray;
    }
}