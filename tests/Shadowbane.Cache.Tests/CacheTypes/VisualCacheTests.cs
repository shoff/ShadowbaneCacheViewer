namespace Shadowbane.Cache.Tests.CacheTypes
{
    using System.Linq;
    using System.Threading.Tasks;
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

        [Fact]
        public void All_Indices_Compressed_Uncompressed_Values_Are_Valid_When_Loaded_With_Parallel_ForEach()
        {
            // this is probably going to be a long running test
            Parallel.ForEach(this.visualCache.CacheIndices, item =>
            {
                var cacheAsset = this.visualCache[item.identity];
                Assert.NotNull(cacheAsset);
            });
        }

        [Fact]
        public void Identity_Is_Unique()
        {
            var query = this.visualCache.CacheIndices.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => new { Element = y.Key, Counter = y.Count() })
                .ToList();

            Assert.True(query.Count == 0);
        }
    }
}