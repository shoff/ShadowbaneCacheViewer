namespace Shadowbane.Models
{
    using System;
    using Cache;

    public class Simple : ModelObject
    {

        public Simple(CacheIndex cacheIndex, ObjectType flag, string name, uint offset, ReadOnlyMemory<byte> data,
            uint innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        public override void Parse()
        {
            using var reader = this.Data.CreateBinaryReaderUtf32(0);
            reader.BaseStream.Position = this.CursorOffset;
            this.RenderId = reader.ReadUInt32();
        }
    }
}