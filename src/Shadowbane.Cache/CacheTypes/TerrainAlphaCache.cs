namespace Shadowbane.Cache.CacheTypes
{
    using System;

    internal sealed class TerrainAlphaCache : CacheArchive
    {
        public TerrainAlphaCache()
            : base("TerrainAlpha.cache".AsMemory())
        {
        }
    }
}