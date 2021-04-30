namespace Shadowbane.Cache.Tests.CacheTypes
{
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
        public void Cache_Has_Correct_Index_Count_Loading_From_MappedFile()
        {
            this.renderCache
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();

            var expected = 41778;
            var actual = this.renderCache.IndexCount;
            Assert.Equal(expected, actual);
        }
    }
}