namespace Shadowbane.Cache.Tests.CacheTypes
{
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
        public void Cache_Has_Correct_Index_Count_Loading_From_Mapped_File()
        {
            this.soundCache
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();

            var expected = 1086;
            var actual = this.soundCache.IndexCount;
            Assert.Equal(expected, actual);
        }
    }
}