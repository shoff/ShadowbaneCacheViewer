namespace Shadowbane.Cache.Tests.CacheTypes
{
    using Cache.CacheTypes;
    using Xunit;

    public class TileCacheTests : CacheBaseTest
    {
        private readonly TileCache tileCache;

        public TileCacheTests()
        {
            this.tileCache = new TileCache();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.tileCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 9;
            var actual = this.tileCache.IndexCount;
            Assert.Equal(expected, actual);
        }

    }
}