namespace CacheViewer.Domain.Models
{
    using System;
    using Archive;
    using CacheViewer.Data;
    using Exportable;

    public class Particle : CacheObject
    {
        public Particle(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data,
            int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        public override void Parse()
        {
            // ignore for now.
        }
    }
}