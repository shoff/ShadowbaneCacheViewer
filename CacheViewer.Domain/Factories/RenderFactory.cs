
namespace CacheViewer.Domain.Factories
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Archive;
    using Exceptions;
    using Extensions;
    using Models;
    using NLog;

    /// <summary>
    /// </summary>
    public class RenderFactory
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly RenderFactory instance = new RenderFactory();
        private readonly Render renderArchive;
        private readonly MeshFactory meshFactory;

        /// <summary>
        /// </summary>
        private RenderFactory()
        {
            this.renderArchive = (Render)ArchiveFactory.Instance.Build(CacheFile.Render);
            this.meshFactory = MeshFactory.Instance;
            this.AppendModel = true;
        }

        /// <summary>
        /// Creates the specified buffer.
        /// </summary>
        /// <param name="cacheIndex">
        /// Index of the cache.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception>
        /// The end of the stream is reached.
        ///   <cref>EndOfStreamException</cref>
        /// </exception>
        /// <exception>
        /// An I/O error occurs.
        ///   <cref>IOException</cref>
        /// </exception>
        /// <exception cref="Exception">
        /// Condition. 
        /// </exception>
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

        /// <summary>
        /// Creates the specified identity.
        /// </summary>
        /// <param name="identity">
        /// The identity.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <param name="addByteData">
        /// if set to <c>true</c> [add byte data].
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="IOException">
        /// An I/O error occurs. 
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached. 
        /// </exception>
        /// <exception cref="Exception">
        /// Condition. 
        /// </exception>
        public RenderInformation Create(int identity, int order = 0, bool addByteData = false)
        {
            CacheAsset asset = this.renderArchive[identity];

            RenderInformation renderInfo = new RenderInformation
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
                int null1;
                int meshId;
                ushort null1;
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
            var arraySegment = order == 0 ? asset.Item1 : asset.Item2;

            using (BinaryReader reader = arraySegment.CreateBinaryReaderUtf32())
            {
                // let's play with the first 35 bytes
                // this.PlayWithFirst35(buffer, renderId);
                reader.BaseStream.Position = 35;
                renderInfo.HasMesh = reader.ReadUInt32() == 1;

                if (renderInfo.HasMesh)
                {
                    var offset = reader.BaseStream.Position;
                    renderInfo.ValidMeshFound = HandleMesh(reader, renderInfo);
                    reader.BaseStream.Position = offset;
                }

                // object scale ?
                renderInfo.Scale = reader.ReadToVector3();
                renderInfo.Unknown[2] = reader.ReadUInt32();

                // TODO this is where it gets fucked. 
                // object position ?
                renderInfo.Position = reader.ReadToVector3();

                // Render object count
                renderInfo.RenderCount = reader.ReadInt32();
                HandleChildren(reader, renderInfo);

                bool hasTexture = reader.ReadBoolean();

                if (hasTexture)
                {
                    // skip over unknown data to the texture id another vector3 ?
                    HandleTexture(reader, ref renderInfo);
                }
            }

            if ((asset.CacheIndex2.Identity > 0) && (renderInfo.Order == 0))
            {
                renderInfo.SharedId = this.Create(asset.CacheIndex2, 1);
            }

            return renderInfo;
        }

        /// <summary>
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <param name="renderInfo">
        /// </param>
        /// <returns>
        /// </returns>
        private bool HandleMesh(BinaryReader reader, RenderInformation renderInfo)
        {
            var nullByte = reader.ReadUInt32(); // skip over null byte
            Debug.Assert(nullByte == 0);

            // TODO validate all meshIds
            renderInfo.MeshId = reader.ReadInt32();

            if (renderInfo.MeshId == 0)
            {
                renderInfo.Notes = string.Format("{0} claimed to have a mesh, however the meshId read was 0.",
                    renderInfo.CacheIndex.Identity);
                logger.Warn(renderInfo.Notes);
                return false;
            }

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
                            string.Format("{0} claimed to have a mesh with MeshId {1}, however the MeshCache does not contain an item with that id.", renderInfo.CacheIndex.Identity, renderInfo.MeshId);
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
            }

            // null short
            var nullShort = reader.ReadUInt16();
            Debug.Assert(nullShort == 0);

            uint size = reader.ReadUInt32();

            if (size > 0)
            {
                // since this size count is actually a Unicode character count, each number is actually 2 bytes.

                renderInfo.JointName = reader.ReadAsciiString(size);
                renderInfo.JointName = !string.IsNullOrEmpty(renderInfo.JointName)
                    ? renderInfo.JointName.Replace(" ", string.Empty) : string.Empty;
                
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <param name="renderInfo">
        /// </param>
        private void HandleChildren(BinaryReader reader,  RenderInformation renderInfo)
        {
            if (renderInfo.RenderCount > 1000)
            {
                // bail
                var message =
                    string.Format(
                        "{0} claimed to have a {1} child render nodes, bailing out.",renderInfo.CacheIndex.Identity,renderInfo.RenderCount);
                logger.Error(message);
                return;
            }

            renderInfo.ChildRenderIds = new CacheIndex[renderInfo.RenderCount];

            for (int i = 0; i < renderInfo.RenderCount; i++)
            {
                reader.BaseStream.Position += 4;
                int childId = reader.ReadInt32();
                renderInfo.ChildRenderIdList.Add(childId);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <param name="renderInfo">
        /// </param>
        private void HandleTexture(BinaryReader reader, ref RenderInformation renderInfo)
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

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static RenderFactory Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        /// <value>
        /// The indexes.
        /// </value>
        public CacheIndex[] Indexes 
        {
            get { return this.renderArchive.CacheIndices.ToArray(); }
        }
            

        /// <summary>
        /// Gets or sets a value indicating whether [append model].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [append model]; otherwise, <c>false</c>.
        /// </value>
        public bool AppendModel { get; set; }

        /// <summary>
        /// </summary>
        public Tuple<int, int> IdentityRange
        {
            get { return new Tuple<int, int>(this.renderArchive.LowestId, this.renderArchive.HighestId); }
        }

        /// <summary>
        /// </summary>
        public int[] IdentityArray
        {
            get { return this.renderArchive.IdentityArray; }
        }

        /// <summary>
        /// Gets the cache asset from the
        /// render archive by identity from cache index.
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        public CacheAsset GetById(int identity)
        {
            return this.renderArchive[identity];
        }

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsValidId(int id)
        {
            if (id.TestRange(this.meshFactory.IdentityRange.Item1, this.meshFactory.IdentityRange.Item2))
            {
                return this.meshFactory.IdentityArray.Where(x => x == id).Any();
            }

            return false;
        }
    }
}