namespace Shadowbane.Cache.CacheTypes
{
    using System;

    internal sealed class TestCache : CacheArchive
    {

        public TestCache()
            : base("Unknown.cache".AsMemory())
        {
        }
    }
}