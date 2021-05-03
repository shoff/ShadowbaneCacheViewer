namespace Shadowbane.Cache.CacheTypes
{
    using System;

    public sealed class TestCache : CacheArchive
    {

        public TestCache()
            : base("Test.cache")
        {
        }
    }
}