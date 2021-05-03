namespace Shadowbane.Cache.Tests.CacheTypes
{
    using Cache.CacheTypes;
    using Xunit;

    public class ZoneCacheTests : CacheBaseTest
    {
        private readonly ZoneCache zoneCache;

        public ZoneCacheTests()
        {
            this.zoneCache = new ZoneCache();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.zoneCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 861;
            var actual = this.zoneCache.IndexCount;
            Assert.Equal(expected, actual);
        }
    }
}