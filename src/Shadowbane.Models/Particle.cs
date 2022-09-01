namespace Shadowbane.Models;

using System;
using Cache;

public record Particle(uint Identity, string Name, uint CursorOffset, ReadOnlyMemory<byte> Data)
    : ModelObject(Identity, ObjectType.Particle, Name, CursorOffset, Data)
{
    public override void Parse()
    {
        using var reader = this.Data.CreateBinaryReaderUtf32(this.CursorOffset);
        // reader.BaseStream.Position = this.CursorOffset;
        this.RenderId = reader.ReadUInt32();
    }
}