namespace Shadowbane.Cache.Tests.CacheTypes
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Cache.CacheTypes;
    using Xunit;

    public class MeshCacheTests : CacheBaseTest
    {
        private readonly MeshCache meshCache;

        public MeshCacheTests()
        {
            this.meshCache = new MeshCache();
            this.meshCache.LoadCacheHeader()
            .LoadIndexes();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            var expected = 24387;
            var actual = this.meshCache.IndexCount;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Identity_Is_Unique()
        {
            var query = this.meshCache.CacheIndices.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => new { Element = y.Key, Counter = y.Count() })
                .ToList();

            Assert.True(query.Count == 0);
        }

        [Fact(Skip = "Slow test we know it works.")]
        public void All_Indices_Compressed_Uncompressed_Values_Are_Valid()
        {
            // this is probably going to be a long running test
            foreach(var item in this.meshCache.CacheIndices)
            {
                var cacheAsset = this.meshCache[item.identity];
                Assert.NotNull(cacheAsset);
            }
        }

        [Fact]
        public void All_Indices_Compressed_Uncompressed_Values_Are_Valid_When_Loaded_With_Parallel_ForEach()
        {
            // this is probably going to be a long running test
            Parallel.ForEach(this.meshCache.CacheIndices, item =>
            {
                var cacheAsset = this.meshCache[item.identity];
                Assert.NotNull(cacheAsset);
            });
        }
    }
}