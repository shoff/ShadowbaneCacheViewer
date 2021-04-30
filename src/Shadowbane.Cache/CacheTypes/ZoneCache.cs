namespace Shadowbane.Cache.CacheTypes
{
    using System;

    internal sealed class ZoneCache : CacheArchive
    {
        public ZoneCache()
            : base("CZone.cache".AsMemory())
        {
        }
    }
}