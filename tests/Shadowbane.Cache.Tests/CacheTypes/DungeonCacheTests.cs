namespace Shadowbane.Cache.Tests.CacheTypes;

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
    public void Cache_Validates()
    {
        this.dungeonCache.Validate();
    }

    [Fact]
    public void Cache_Has_Correct_Index_Count()
    {
        var expected = 0;
        var actual = this.dungeonCache.IndexCount;
        Assert.Equal(expected, actual);
    }
}