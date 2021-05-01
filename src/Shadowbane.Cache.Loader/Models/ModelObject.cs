namespace Shadowbane.Cache.Loader.Models
{
    using System;
    using System.Collections.Generic;

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