﻿namespace CacheViewer.Domain.Factories
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
        private readonly MeshFactory meshFactory;
        private readonly Render renderArchive;

        /// <summary>
        /// </summary>
        private RenderFactory()
        {
            this.renderArchive = (Render)ArchiveFactory.Instance.Build(CacheFile.Render);
            this.meshFactory = MeshFactory.Instance;
            this.AppendModel = true;
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>
        ///     The instance.
        /// </value>
        public static RenderFactory Instance { get; } = new RenderFactory();

        /// <summary>
        ///     Gets the indexes.
        /// </summary>
        /// <value>
        ///     The indexes.
        /// </value>
        public CacheIndex[] Indexes => this.renderArchive.CacheIndices.ToArray();


        /// <summary>
        ///     Gets or sets a value indicating whether [append model].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [append model]; otherwise, <c>false</c>.
        /// </value>
        public bool AppendModel { get; set; }

        /// <summary>
        /// </summary>
        public Tuple<int, int> IdentityRange =>
            new Tuple<int, int>(this.renderArchive.LowestId, this.renderArchive.HighestId);

        /// <summary>
        /// </summary>
        public int[] IdentityArray => this.renderArchive.IdentityArray;

        /// <summary>
        ///     Creates the specified buffer.
        /// </summary>
        /// <param name="cacheIndex">
        ///     Index of the cache.
        /// </param>
        /// <param name="order">
        ///     The order.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception>
        ///     The end of the stream is reached.
        ///     <cref>EndOfStreamException</cref>
        /// </exception>
        /// <exception>
        ///     An I/O error occurs.
        ///     <cref>IOException</cref>
        /// </exception>
        /// <exception cref="Exception">
        ///     Condition.
        /// </exception>
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

        /// <summary>
        ///     Creates the specified identity.
        /// </summary>
        /// <param name="identity">
        ///     The identity.
        /// </param>
        /// <param name="order">
        ///     The order.
        /// </param>
        /// <param name="addByteData">
        ///     if set to <c>true</c> [add byte data].
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="IOException">
        ///     An I/O error occurs.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        ///     The end of the stream is reached.
        /// </exception>
        /// <exception cref="Exception">
        ///     Condition.
        /// </exception>
        public RenderInformation Create(int identity, int order = 0, bool addByteData = false)
        {
            var asset = this.renderArchive[identity];

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
            // TODO allow for number 3?
            var arraySegment = order == 0 ? asset.Item1 : asset.Item2;

            using (var reader = arraySegment.CreateBinaryReaderUtf32())
            {
                // let's play with the first 35 bytes
                // this.PlayWithFirst35(buffer, renderId);
                reader.BaseStream.Position = 35;
                renderInfo.HasMesh = reader.ReadUInt32() == 1;

                if (renderInfo.HasMesh)
                {
                    var offset = reader.BaseStream.Position;
                    renderInfo.ValidMeshFound = this.HandleMesh(reader, renderInfo);
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
                this.HandleChildren(reader, renderInfo);

                var hasTexture = reader.ReadBoolean();

                if (hasTexture)
                {
                    // skip over unknown data to the texture id another vector3 ?
                    this.HandleTexture(reader, ref renderInfo);
                }
            }

            if (asset.CacheIndex2.Identity > 0 && renderInfo.Order == 0)
            {
                renderInfo.SharedId = this.Create(asset.CacheIndex2, 1);
            }

            return renderInfo;
        }

        /// <summary>
        /// we re-use the binary reader we've already opened.
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

            while (renderInfo.MeshId == 0)
            {
                renderInfo.Notes = $"{renderInfo.CacheIndex.Identity} claimed to have a mesh, however the meshId read was 0.";
                logger.Warn(renderInfo.Notes);

                // TODO this could blow through the index
                renderInfo.MeshId = reader.ReadInt32();
                if (renderInfo.MeshId == 1)
                {
                    // experiment see if the next byte is the same
                    var x = reader.ReadInt32();
                    logger?.Fatal($"MeshId was 0 then 1 and on third read is {x} for {renderInfo.CacheIndex.Identity}");
                }
            }

            try
            {
                if (this.AppendModel)
                {
                    if (this.IsValidId(renderInfo.MeshId))
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
            }

            // null short
            var nullShort = reader.ReadUInt16();
            Debug.Assert(nullShort == 0);

            var size = reader.ReadUInt32();

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
        private void HandleChildren(BinaryReader reader, RenderInformation renderInfo)
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
        ///     Gets the cache asset from the
        ///     render archive by identity from cache index.
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
        internal bool IsValidId(int id)
        {
            // TODO this does not belong here. 
            if (id.TestRange(this.meshFactory.IdentityRange.Item1, this.meshFactory.IdentityRange.Item2))
            {
                return this.meshFactory.IdentityArray.Any(x => x == id);
            }

            return false;
        }
    }
}