namespace Shadowbane.Cache.CacheTypes
{
    using System;

    internal sealed class VisualCache : CacheArchive
    {

        public VisualCache()
            : base("Visual.cache".AsMemory())
        {
        }
    }
}