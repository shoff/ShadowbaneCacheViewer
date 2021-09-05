namespace Shadowbane.Models
{
    using System;
    using System.Collections.Generic;
    using Cache;

    public abstract class CacheObject : ICacheObject
    {
        protected CacheObject(uint identity, ObjectType flag, string name, uint offset, ReadOnlyMemory<byte> data, uint innerOffset) =>
            (this.Identity, this.Flag, this.Name, this.CursorOffset, this.Data, this.InnerOffset) =
            (identity, flag, name, offset, data, innerOffset);

        public uint Identity { get; }
        public ICollection<uint> RenderIds { get; } = new HashSet<uint>();
        public ICollection<IRenderable> Renders { get;} = new HashSet<IRenderable>();
        public int UnParsedBytes { get; set; }
        public uint RenderId { get; set; }
        public string Name { get; protected set; }
        public ObjectType Flag { get; }
        public uint CursorOffset { get; }
        public ReadOnlyMemory<byte> Data { get; }
        public uint InnerOffset { get; }
        public uint RenderCount { get; set; }
        public abstract ICacheObject Parse();
        public int CompareTo(ICacheObject other)
        {
            if (other == null || this.Flag > other.Flag)
            {
                return 1;
            }

            if (other.Flag == this.Flag)
            {
                return string.Compare(this.Name, other.Name, StringComparison.Ordinal);
            }

            return -1;
        }
    }
}