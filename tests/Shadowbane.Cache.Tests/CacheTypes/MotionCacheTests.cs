namespace Shadowbane.Cache.Tests.CacheTypes
{
    using Cache.CacheTypes;
    using Xunit;

    public class MotionCacheTests : CacheBaseTest
    {
        private readonly MotionCache motionCache;

        public MotionCacheTests()
        {
            this.motionCache = new MotionCache();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.motionCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 1503;
            var actual = this.motionCache.IndexCount;
            Assert.Equal(expected, actual);
        }

    }
}