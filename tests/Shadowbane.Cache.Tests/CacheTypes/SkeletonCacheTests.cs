namespace Shadowbane.Cache.Tests.CacheTypes
{
    using System.Linq;
    using System.Threading.Tasks;
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

        [Fact]
        public void All_Indices_Compressed_Uncompressed_Values_Are_Valid_When_Loaded_With_Parallel_ForEach()
        {
            // this is probably going to be a long running test
            Parallel.ForEach(this.skeletonCache.CacheIndices, item =>
            {
                var cacheAsset = this.skeletonCache[item.identity];
                Assert.NotNull(cacheAsset);
            });
        }

        [Fact]
        public void Identity_Is_Unique()
        {
            Assert.True(this.skeletonCache.CacheIndices.GroupBy(x => x.identity).All(g => g.Count() == 1));
        }
    }
}