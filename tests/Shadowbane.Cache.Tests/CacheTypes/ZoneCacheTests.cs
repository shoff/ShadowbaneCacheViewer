namespace Shadowbane.Cache.Tests.CacheTypes;

using System.Linq;
using System.Threading.Tasks;
using Cache.CacheTypes;
using Xunit;

public class ZoneCacheTests : CacheBaseTest
{
    private readonly ZoneCache zoneCache;

    public ZoneCacheTests()
    {
        this.zoneCache = new ZoneCache();
    }

    [Fact]
    public void Cache_Has_Correct_Index_Count()
    {
        var expected = 861;
        var actual = this.zoneCache.IndexCount;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void All_Indices_Compressed_Uncompressed_Values_Are_Valid_When_Loaded_With_Parallel_ForEach()
    {
        // this is probably going to be a long running test
        Parallel.ForEach(this.zoneCache.CacheIndices, item =>
        {
            var cacheAsset = this.zoneCache[item.identity];
            Assert.NotNull(cacheAsset);
        });
    }

    [Fact]
    public void Identity_Is_Unique()
    {
        var query = this.zoneCache.CacheIndices
            .GroupBy(x => x.identity)
            .Where(g => g.Count() > 1)
            .Select(y => new { Identity = y.Key, Counter = y.Count() })
            .ToList();

        var moreThan2 = query
            .Where(a => a.Counter > 1).Select(a => a).ToList();
        Assert.True(moreThan2.Count == 0);
    }
    
    [Fact]
    public void Cache_Validates()
    {
        this.zoneCache.Validate();
    }
}