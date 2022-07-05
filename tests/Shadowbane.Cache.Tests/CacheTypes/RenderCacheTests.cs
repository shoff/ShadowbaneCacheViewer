using Shadowbane.Cache.IO;

namespace Shadowbane.Cache.Tests.CacheTypes;

using System.Linq;
using System.Threading.Tasks;
using Cache.CacheTypes;
using Xunit;

public class RenderCacheTests : CacheBaseTest
{
    private readonly RenderCache renderCache;

    public RenderCacheTests()
    {
        this.renderCache = ArchiveLoader.RenderArchive;
    }

    [Fact]
    public void Cache_Has_Correct_Index_Count()
    {
        var expected = 41778;
        var actual = this.renderCache.IndexCount;
        Assert.Equal(expected, actual);
    }

    // [Fact(Skip = "Slow test we know it works.")]
    [Fact]
    public void All_Indices_Compressed_Uncompressed_Values_Are_Valid()
    {
        // this is probably going to be a long running test
        foreach (var item in this.renderCache.CacheIndices)
        {
            var cacheAsset = this.renderCache[item.identity];
            Assert.NotNull(cacheAsset);
        }
    }

    [Fact]
    public void All_Indices_Compressed_Uncompressed_Values_Are_Valid_When_Loaded_With_Parallel_ForEach()
    {
        // this is probably going to be a long running test
        Parallel.ForEach(this.renderCache.CacheIndices, item =>
        {
            var cacheAsset = this.renderCache[item.identity];
            Assert.NotNull(cacheAsset);
        });
    }

    [Fact]
    public void Identity_Is_Not_Unique_On_Renderable_Items()
    {
        var query = this.renderCache.CacheIndices
            .GroupBy(x => x.identity)
            .Where(g => g.Count() > 1)
            .Select(y => new { Identity = y.Key, Counter = y.Count() })
            .ToList();

        var moreThan2 = query
            .Where(a => a.Counter > 1).Select(a => a).ToList();
        Assert.True(moreThan2.Count > 0);
    }

    [Fact]
    public void Cache_Validates()
    {
        this.renderCache.Validate();
    }
}