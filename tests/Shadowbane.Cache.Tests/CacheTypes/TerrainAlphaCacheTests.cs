namespace Shadowbane.Cache.Tests.CacheTypes
{
    using Cache.CacheTypes;
    using Xunit;

    public class TerrainAlphaCacheTests : CacheBaseTest
    {
        private readonly TerrainAlphaCache terrainAlphaCache;

        public TerrainAlphaCacheTests()
        {
            this.terrainAlphaCache = new TerrainAlphaCache();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.terrainAlphaCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 20912;
            var actual = this.terrainAlphaCache.IndexCount;
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Cache_Has_Correct_Index_Count_Loading_From_Mapped_File()
        {
            this.terrainAlphaCache
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();

            var expected = 20912;
            var actual = this.terrainAlphaCache.IndexCount;
            Assert.Equal(expected, actual);
        }
    }
}