namespace Shadowbane.Cache.Tests.CacheTypes;

using System.Linq;
using System.Threading.Tasks;
using Cache.CacheTypes;
using Xunit;

public class SkeletonCacheTests : CacheBaseTest
{
    private readonly SkeletonCache skeletonCache;

    public SkeletonCacheTests()
    {
        this.skeletonCache = new SkeletonCache();
    }

    [Fact]
    public void Cache_Has_Correct_Index_Count()
    {
        var expected = 103;
        var actual = this.skeletonCache.IndexCount;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void All_Indices_Compressed_Uncompressed_Values_Are_Valid_When_Loaded_With_Parallel_ForEach()
    {
        // this is probably going to be a long running test
        Parallel.ForEach(this.skeletonCache.CacheIndices, item =>
        {
            var cacheAsset = this.skeletonCache[item.identity];
            Assert.NotNull(cacheAsset);
        });
    }

    [Fact]
    public void Identity_Is_Unique()
    {
        var query = this.skeletonCache.CacheIndices
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
        this.skeletonCache.Validate();
    }
}