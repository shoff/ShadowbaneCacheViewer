namespace CacheViewer.Domain.Models.Exportable
{
    using System;
    using System.Collections.Generic;
    using Archive;
    using CacheViewer.Data;

    /// <summary>
    /// </summary>
    public abstract class CacheObject : ICacheObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CacheObject" /> class.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="name">The name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="data">The data.</param>
        /// <param name="innerOffset">The inner offset.</param>
        protected CacheObject(CacheIndex cacheIndex, ObjectType flag, string name,
            int offset, ArraySegment<byte> data, int innerOffset)
        {
            this.CacheIndex = cacheIndex;
            this.Flag = flag;
            this.Name = name;
            this.CursorOffset = offset;
            this.Data = data;
            this.InnerOffset = innerOffset;
        }

        public int Id { get; set; }
        public ICollection<int> RenderIds { get; set; } = new HashSet<int>();
        public int UnParsedBytes { get; set; }
        public CacheIndex CacheIndex { get; set; }
        public uint RenderId { get; set; }
        public string Name { get; set; }
        public ObjectType Flag { get; set; }
        public int CursorOffset { get; set; }
        public ArraySegment<byte> Data { get; set; }
        public int InnerOffset { get; set; }
        public int RenderCount { get; set; }
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