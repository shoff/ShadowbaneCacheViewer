namespace Shadowbane.Cache.Tests.CacheTypes
{
    using Cache.CacheTypes;
    using Xunit;

    public class MeshCacheTests : CacheBaseTest
    {
        private readonly MeshCache meshCache;

        public MeshCacheTests()
        {
            this.meshCache = new MeshCache();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.meshCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 24387;
            var actual = this.meshCache.IndexCount;
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Cache_Has_Correct_Index_Count_Loading_From_MappedFile()
        {
            this.meshCache
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();

            var expected = 24387;
            var actual = this.meshCache.IndexCount;
            Assert.Equal(expected, actual);
        }
    }
}