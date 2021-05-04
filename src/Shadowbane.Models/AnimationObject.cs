namespace Shadowbane.Models
{
    using System;
    using Cache;

    public abstract class AnimationObject : ModelObject
    {
        protected AnimationObject(CacheIndex cacheIndex, ObjectType flag, string name, uint offset,
            ReadOnlyMemory<byte> data, uint innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        public ICacheObject Skeleton { get; set; }
    }
}