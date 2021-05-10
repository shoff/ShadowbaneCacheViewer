namespace Shadowbane.Cache.Tests.CacheTypes
{
    using System.Linq;
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

        [Fact]
        public void Identity_Is_Unique()
        {
            var query = this.motionCache.CacheIndices
                .GroupBy(x => x.identity)
                .Where(g => g.Count() > 1)
                .Select(y => new { Identity = y.Key, Counter = y.Count() })
                .ToList();

            var moreThan2 = query
                .Where(a => a.Counter > 1).Select(a => a).ToList();
            Assert.True(moreThan2.Count == 0);
        }

    }
}