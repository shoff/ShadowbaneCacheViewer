namespace Shadowbane.Cache.CacheTypes
{
    using System;

    public sealed class TestCache : CacheArchive
    {

        public TestCache()
            : base("Unknown.cache".AsMemory())
        {
        }
    }
}