namespace Shadowbane.Models
{
    using System;
    using System.Collections.Generic;
    using Cache;

    public abstract class ModelObject : CacheObject
    {
        protected ModelObject(CacheIndex cacheIndex, ObjectType flag, string name, uint offset, ReadOnlyMemory<byte> data,
            uint innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        public ICollection<Exception> Errors { get; } = new List<Exception>();
    }
}