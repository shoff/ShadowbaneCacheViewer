namespace Shadowbane.Cache.Tests.CacheTypes
{
    using Cache.CacheTypes;
    using Xunit;

    public class ObjectCacheTests : CacheBaseTest
    {
        private readonly ObjectCache objectCacheObjectsObjectCache;

        public ObjectCacheTests()
        {
            this.objectCacheObjectsObjectCache = new ObjectCache();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.objectCacheObjectsObjectCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 10618;
            var actual = this.objectCacheObjectsObjectCache.IndexCount;
            Assert.Equal(expected, actual);
        }
    }
}