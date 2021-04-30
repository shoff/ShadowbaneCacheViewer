namespace Shadowbane.Cache.CacheTypes
{
    using System;

    public sealed class TerrainAlphaCache : CacheArchive
    {
        public TerrainAlphaCache()
            : base("TerrainAlpha.cache".AsMemory())
        {
        }
    }
}