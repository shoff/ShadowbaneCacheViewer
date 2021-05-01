﻿namespace Shadowbane.Cache.Loader.Models
{
    using System;

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