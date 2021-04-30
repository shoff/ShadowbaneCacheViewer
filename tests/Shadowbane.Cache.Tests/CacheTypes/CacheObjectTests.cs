namespace Shadowbane.Cache.Tests.CacheTypes
{
    using Cache.CacheTypes;
    using Xunit;

    public class CacheObjectTests : CacheBaseTest
    {
        private readonly CacheObject cacheObjects;

        public CacheObjectTests()
        {
            this.cacheObjects = new CacheObject();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.cacheObjects
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 10618;
            var actual = this.cacheObjects.IndexCount;
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Cache_Has_Correct_Index_Count_Loading_From_Mapped_File()
        {
            this.cacheObjects
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();

            var expected = 10618;
            var actual = this.cacheObjects.IndexCount;
            Assert.Equal(expected, actual);
        }
    }
}