namespace Shadowbane.Cache.Tests.CacheTypes
{
    using Cache.CacheTypes;
    using Xunit;

    public class PaletteCacheTests : CacheBaseTest
    {
        private readonly PaletteCache paletteCache;

        public PaletteCacheTests()
        {
            this.paletteCache = new PaletteCache();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.paletteCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 0;
            var actual = this.paletteCache.IndexCount;
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Cache_Has_Correct_Index_Count_Loading_From_Mapped_File()
        {
            this.paletteCache
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();

            var expected = 0;
            var actual = this.paletteCache.IndexCount;
            Assert.Equal(expected, actual);
        }
    }
}