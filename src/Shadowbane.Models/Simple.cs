namespace Shadowbane.Models;

using System;
using Cache;

public record Simple(uint Identity, string Name, uint CursorOffset, ReadOnlyMemory<byte> Data) 
    : ModelRecord(Identity, ObjectType.Simple, Name, CursorOffset, Data)
{
    public override void Parse()
    {
        using var reader = this.Data.CreateBinaryReaderUtf32(this.CursorOffset);
        this.RenderId = reader.ReadUInt32();
        // only child render ids go into the renderIds collection
        // this.RenderIds.Add(this.RenderId);
    }
}