namespace Shadowbane.Cache.Tests.CacheTypes
{
    using Cache.CacheTypes;
    using Xunit;

    public class SkeletonCacheTests : CacheBaseTest
    {
        private readonly SkeletonCache skeletonCache;

        public SkeletonCacheTests()
        {
            this.skeletonCache = new SkeletonCache();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.skeletonCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 103;
            var actual = this.skeletonCache.IndexCount;
            Assert.Equal(expected, actual);
        }
    }
}