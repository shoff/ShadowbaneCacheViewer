namespace Shadowbane.Cache.Tests.CacheTypes
{
    using Cache.CacheTypes;
    using Xunit;

    public class TextureCacheTests : CacheBaseTest
    {
        private readonly TextureCache textureCache;

        public TextureCacheTests()
        {
            this.textureCache = new TextureCache();
        }
        
        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.textureCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 9680;
            var actual = this.textureCache.IndexCount;
            Assert.Equal(expected, actual);
        }
    }
}