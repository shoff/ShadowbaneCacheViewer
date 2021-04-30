namespace Shadowbane.Cache.CacheTypes
{
    using System;

    internal sealed class MotionCache : CacheArchive
    {
        public MotionCache()
            : base("Motion.cache".AsMemory())
        {
        }
    }
}