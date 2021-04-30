namespace Shadowbane.Cache.CacheTypes
{
    using System;

    internal sealed class SkeletonCache : CacheArchive
    {

        public SkeletonCache()
            : base("Skeleton.cache".AsMemory())
        {
        }
    }
}