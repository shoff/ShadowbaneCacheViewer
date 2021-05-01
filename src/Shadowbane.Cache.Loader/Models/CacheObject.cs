namespace Shadowbane.Cache.Loader.Models
{
    using System;
    using System.Collections.Generic;

    public abstract class CacheObject : ICacheObject
    {
        protected CacheObject(CacheIndex cacheIndex, ObjectType flag, string name,
            uint offset, ReadOnlyMemory<byte> data, uint innerOffset)
        {
            this.CacheIndex = cacheIndex;
            this.Flag = flag;
            this.Name = name;
            this.CursorOffset = offset;
            this.Data = data;
            this.InnerOffset = innerOffset;
        }

        public uint Id { get; set; }
        public ICollection<uint> RenderIds { get; set; } = new HashSet<uint>();
        public ICollection<RenderInformation> Renders { get; set; } = new HashSet<RenderInformation>();
        public int UnParsedBytes { get; set; }
        public CacheIndex CacheIndex { get; set; }
        public uint RenderId { get; set; }
        public string Name { get; set; }
        public ObjectType Flag { get; set; }
        public uint CursorOffset { get; set; }
        public ReadOnlyMemory<byte> Data { get; set; }
        public uint InnerOffset { get; set; }
        public uint RenderCount { get; set; }
        public abstract void Parse();
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