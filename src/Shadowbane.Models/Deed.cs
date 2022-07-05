namespace Shadowbane.Models;

using System;
using Cache;

public record Deed : CacheObject
{
    public Deed(uint identity, string name, uint offset, ReadOnlyMemory<byte> data,
        uint innerOffset)
        : base(identity, ObjectType.Deed, name, offset, data, innerOffset)
    {
    }

    public uint InventoryTexture { get; private set; }

    public uint MapTexture { get; private set; }

    public override ICacheObject Parse()
    {
        using var reader = this.Data.CreateBinaryReaderUtf32(this.CursorOffset);
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
        return this;
    }
}