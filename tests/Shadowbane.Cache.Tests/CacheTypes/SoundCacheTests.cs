namespace Shadowbane.Cache.Tests.CacheTypes
{
    using System.Linq;
    using System.Threading.Tasks;
    using Cache.CacheTypes;
    using Xunit;

    public class SoundCacheTests : CacheBaseTest
    {
        private readonly SoundCache soundCache;

        public SoundCacheTests()
        {
            this.soundCache = new SoundCache();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.soundCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 1086;
            var actual = this.soundCache.IndexCount;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void All_Indices_Compressed_Uncompressed_Values_Are_Valid_When_Loaded_With_Parallel_ForEach()
        {
            // this is probably going to be a long running test
            Parallel.ForEach(this.soundCache.CacheIndices, item =>
            {
                var cacheAsset = this.soundCache[item.identity];
                Assert.NotNull(cacheAsset);
            });
        }

        [Fact]
        public void Identity_Is_Unique()
        {
            Assert.True(this.soundCache.CacheIndices.GroupBy(x => x.identity).All(g => g.Count() == 1));
        }
    }
}