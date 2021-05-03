namespace Shadowbane.Cache.Tests.CacheTypes
{
    using Cache.CacheTypes;
    using Xunit;

    public class VisualCacheTests : CacheBaseTest
    {
        private readonly VisualCache visualCache;

        public VisualCacheTests()
        {
            this.visualCache = new VisualCache();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.visualCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 480;
            var actual = this.visualCache.IndexCount;
            Assert.Equal(expected, actual);
        }

    }
}