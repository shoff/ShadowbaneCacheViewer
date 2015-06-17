using System;
using System.IO;
using CacheViewer.Domain.Archive;
using CacheViewer.Domain.Extensions;
using CacheViewer.Domain.Models.Exportable;

namespace CacheViewer.Domain.Models
{
    public class DeedObject : CacheObject
    {
        private uint inventoryTexture;
        private uint mapTexture;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeedObject"/> class.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="name">The name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="data">The data.</param>
        /// <param name="innerOffset">The inner offset.</param>
        public DeedObject(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data, int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        /// <summary>
        /// Gets the inventory texture.
        /// </summary>
        /// <value>
        /// The inventory texture.
        /// </value>
        public uint InventoryTexture
        {
            get { return this.inventoryTexture; }
        }

        /// <summary>
        /// Gets the map texture.
        /// </summary>
        /// <value>
        /// The map texture.
        /// </value>
        public uint MapTexture
        {
            get { return this.mapTexture; }
        }

        /// <summary>
        /// Parses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public override void Parse(ArraySegment<byte> data)
        {
            using (BinaryReader reader = data.CreateBinaryReaderUtf32())
            {
                this.RenderId = reader.ReadUInt32();
                //memcpy(&renderID, data + ptr, 4); // world texture id
                //ptr += 4;
                //wxLogMessage(_T("Render ID: %i"), renderID);
                this.inventoryTexture = reader.ReadUInt32();
                //memcpy(&invTex, data + ptr, 4); // inventory texture id
                //ptr += 4;
                //wxLogMessage(_T("Inventory Texture ID: %i"), invTex);
                this.mapTexture = reader.ReadUInt32();
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