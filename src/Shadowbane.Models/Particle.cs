namespace Shadowbane.Models;

using System;
using Cache;

public record Particle : ModelObject
{

    public Particle(
        uint identity,
        string? name,
        uint offset,
        ReadOnlyMemory<byte> data,
        uint innerOffset)
        : base(identity, ObjectType.Particle, name, offset, data, innerOffset)
    {
    }

    public override ICacheObject Parse()
    {
        using var reader = this.Data.CreateBinaryReaderUtf32(this.CursorOffset);
        // reader.BaseStream.Position = this.CursorOffset;
        this.RenderId = reader.ReadUInt32();
        return this;
    }
}