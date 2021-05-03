namespace Shadowbane.Cache.Tests.CacheTypes
{
    using Cache.CacheTypes;
    using Xunit;

    public class DungeonCacheTests : CacheBaseTest
    {
        private readonly DungeonCache dungeonCache;

        public DungeonCacheTests()
        {
            this.dungeonCache = new DungeonCache();
        }

        [Fact]
        public void Cache_Has_Correct_Index_Count()
        {
            this.dungeonCache
                .LoadCacheHeader()
                .LoadIndexes();

            var expected = 0;
            var actual = this.dungeonCache.IndexCount;
            Assert.Equal(expected, actual);
        }
    }
}