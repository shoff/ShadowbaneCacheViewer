namespace CacheViewer.Domain.Models
{
    using System;
    using System.IO;
    using Archive;
    using Data;
    using Exportable;
    using Extensions;
    using Factories;
    using NLog;

    public class Equipment : ModelObject
    {
        // ReSharper disable NotAccessedField.Local
        private uint inventoryTextureId;
        private uint mapTex;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Equipment" /> class.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="name">The name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="data">The data.</param>
        /// <param name="innerOffset">The inner offset.</param>
        public Equipment(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data,
            int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        /// <summary>
        ///     Parses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="IOException">Condition.</exception>
        public override void Parse(ArraySegment<byte> data)
        {
            using (var reader = data.CreateBinaryReaderUtf32())
            {
                // ReSharper disable once NotAccessedVariable
                uint iUnk = 0;
                try
                {
                    reader.BaseStream.Position = this.CursorOffset;
                }
                catch (IOException ioException)
                {
                    logger?.Error($"Exception thrown in Equipment parsing CacheIndex {this.CacheIndex.Identity}", ioException);
                    throw;
                }

                // Error handler
                try
                {
                    this.RenderId = reader.ReadUInt32();
#if DEBUG
                    long tempOffset = reader.BaseStream.Position;
                    this.ValidateRenderId(reader);
                    reader.BaseStream.Position = tempOffset;
#endif
                    // skip null bytes.
                    reader.BaseStream.Position += 24;

                    this.inventoryTextureId = reader.ReadUInt32();

                    //memcpy(&invTex, data + ptr, 4); // inventory texture id
                    //ptr += 4;
                    //wxLogMessage(_T("Inventory Texture ID: %i"), invTex);
                    this.mapTex = reader.ReadUInt32();

                    //memcpy(&mapTex, data + ptr, 4); // Get the minimap texture id
                    //ptr += 4;
                    //wxLogMessage(_T("Minimap Texture ID: %i"), mapTex);
                    // ReSharper disable once RedundantAssignment
                    iUnk = reader.ReadUInt32();

                    //memcpy(&iUnk, data + ptr, 4);
                    //ptr += 4;
                    //wxLogMessage(_T("Unknown ID: %i"), iUnk);
                    // ReSharper disable once RedundantAssignment
                    iUnk = reader.ReadUInt32();

                    //memcpy(&iUnk, data + ptr, 4); 
                    //ptr += 4;
                    //wxLogMessage(_T("Unknown ID: %i"), iUnk);

                    if (this.RenderId == 0)
                    {
                        logger.Error("No RenderId found for Equipment CacheIndex {0}", this.CacheIndex.Identity);
                    }

                    //var cii = new CacheIndexIdentity
                    //{
                    //    FileId = 7,
                    //    Id = renderId
                    //};

                    //var ri = this.renderRepository.BuildRenderInfo(cii);
                    //this.renderInfo.Add(ri);
                }
                catch (Exception e)
                {
                    logger.Error(e, "Error parsing Equipment for CacheIndex {0}", this.CacheIndex.Identity);
                }
            }
        }

        private void ValidateRenderId(BinaryReader reader)
        {
            if (this.RenderId == 0 || this.RenderId > 1000000)
            {
                logger?.Debug($"Invalid RenderId generated, experimenting to try to find a valid one.");

                // how many variations do we want to try?
                // probably should be sticking these in the DB so we
                // can figure out what the pattern is here on invalid offsets
                // let's try 10
                int i = 0;
                reader.BaseStream.Position = this.CursorOffset - 4; // back up 4? 
                while (i <= 10)
                {
                    var tempRenderId = reader.ReadUInt32();
                    if (ArchiveFactory.Instance.Build(CacheFile.Render).Contains((int) this.RenderId))
                    {
                        // TODO add this to the db
                        logger?.Debug($"Found a valid RenderId of {tempRenderId}");
                    }

                    i++;
                }
            }
        }
    }

    // ReSharper restore NotAccessedField.Local
}