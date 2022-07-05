namespace Shadowbane.Models;

using System;
using Cache;

public record Simple : ModelObject
{

    public Simple(
        uint identity, 
        string name, 
        uint offset, 
        ReadOnlyMemory<byte> data,
        uint innerOffset)
        : base(identity, ObjectType.Simple, name, offset, data, innerOffset)
    {
    }

    public override ICacheObject Parse()
    {
        using var reader = this.Data.CreateBinaryReaderUtf32(this.CursorOffset);
        this.RenderId = reader.ReadUInt32();
        this.RenderIds.Add(this.RenderId);
        return this;
    }
}