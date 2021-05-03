namespace Shadowbane.Cache.Tests.CacheTypes
{
    using System.Linq;
    using System.Threading.Tasks;
    using Cache.CacheTypes;
    using Xunit;

    public class RenderCacheTests : CacheBaseTest
    {
        private readonly RenderCache renderCache;

        public RenderCacheTests()
        {
            this.renderCache = new RenderCache();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.renderCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 41778;
            var actual = this.renderCache.IndexCount;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void All_Indices_Compressed_Uncompressed_Values_Are_Valid_When_Loaded_With_Parallel_ForEach()
        {
            // this is probably going to be a long running test
            Parallel.ForEach(this.renderCache.CacheIndices, item =>
            {
                var cacheAsset = this.renderCache[item.identity];
                Assert.NotNull(cacheAsset);
            });
        }

        [Fact]
        public void Identity_Is_Unique()
        {
            Assert.True(this.renderCache.CacheIndices.GroupBy(x => x.identity).All(g => g.Count() == 1));
        }
    }
}