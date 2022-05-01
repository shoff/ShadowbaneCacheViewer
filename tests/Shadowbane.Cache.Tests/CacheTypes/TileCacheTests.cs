namespace Shadowbane.Cache.Tests.CacheTypes;

using System.Linq;
using System.Threading.Tasks;
using Cache.CacheTypes;
using Xunit;

public class TileCacheTests : CacheBaseTest
{
    private readonly TileCache tileCache;

    public TileCacheTests()
    {
        this.tileCache = new TileCache();
    }

    [Fact]
    public void Cache_Has_Correct_Index_Count()
    {
        var expected = 9;
        var actual = this.tileCache.IndexCount;
        Assert.Equal(expected, actual);
    }
    [Fact]
    public void All_Indices_Compressed_Uncompressed_Values_Are_Valid_When_Loaded_With_Parallel_ForEach()
    {
        // this is probably going to be a long running test
        Parallel.ForEach(this.tileCache.CacheIndices, item =>
        {
            var cacheAsset = this.tileCache[item.identity];
            Assert.NotNull(cacheAsset);
        });
    }

    [Fact]
    public void Identity_Is_Unique()
    {
        var query = this.tileCache.CacheIndices.GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(y => new { Element = y.Key, Counter = y.Count() })
            .ToList();

        Assert.True(query.Count == 0);
    }

    [Fact]
    public void Cache_Validates()
    {
        this.tileCache.Validate();
    }
}