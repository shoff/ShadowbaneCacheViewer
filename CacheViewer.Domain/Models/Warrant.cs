namespace CacheViewer.Domain.Models
{
    using System;
    using Archive;
    using CacheViewer.Data;
    using Exportable;
    using Extensions;

    public class Warrant : CacheObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Warrant" /> class.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="name">The name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="data">The data.</param>
        /// <param name="innerOffset">The inner offset.</param>
        public Warrant(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data,
            int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        /// <summary>
        ///     Gets the inventory texture.
        /// </summary>
        /// <value>
        ///     The inventory texture.
        /// </value>
        public uint InventoryTexture { get; private set; }

        /// <summary>
        ///     Gets the map texture.
        /// </summary>
        /// <value>
        ///     The map texture.
        /// </value>
        public uint MapTexture { get; private set; }

        /// <summary>
        ///     Parses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public override void Parse()
        {
            using (var reader = this.Data.CreateBinaryReaderUtf32())
            {
                this.RenderId = reader.ReadUInt32();
                //memcpy(&renderID, data + ptr, 4); // world texture id
                //ptr += 4;
                //wxLogMessage(_T("Render ID: %i"), renderID);
                this.InventoryTexture = reader.ReadUInt32();
                //memcpy(&invTex, data + ptr, 4); // inventory texture id
                //ptr += 4;
                //wxLogMessage(_T("Inventory Texture ID: %i"), invTex);
                this.MapTexture = reader.ReadUInt32();
                //memcpy(&mapTex, data + ptr, 4); // Get the minimap texture id
                //ptr += 4;
                //wxLogMessage(_T("Minimap Texture ID: %i"), mapTex);

                //memcpy(&iUnk, data + ptr, 4);
                //ptr += 4;
                //wxLogMessage(_T("Unknown ID: %i"), iUnk);

                //memcpy(&iUnk, data + ptr, 4);
                //ptr += 4;
                //wxLogMessage(_T("Unknown ID: %i"), iUnk);
            }
        }
    }
}