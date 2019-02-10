namespace CacheViewer.Domain.Models
{
    using System;
    using Archive;
    using Data;
    using Exportable;

    public class Sun : CacheObject
    {
        public Sun(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data,
            int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }
        
        public override void Parse()
        {
        }
    }
}