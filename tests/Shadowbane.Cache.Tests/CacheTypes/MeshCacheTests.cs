namespace Shadowbane.Cache.Tests.CacheTypes;

using System;
using System.Linq;
using System.Threading.Tasks;
using Cache.CacheTypes;
using Xunit;

public class MeshCacheTests : CacheBaseTest
{
    private readonly MeshCache meshCache;

    public MeshCacheTests()
    {
        this.meshCache = new MeshCache();
    }


    [Fact]
    public void Cache_Validates()
    {
        this.meshCache.Validate();
    }

    [Fact]
    public void Identity_Is_Unique()
    {
        var query = this.meshCache.CacheIndices
            .GroupBy(x => x.identity)
            .Where(g => g.Count() > 1)
            .Select(y => new { Identity = y.Key, Counter = y.Count() })
            .ToList();

        var moreThan2 = query
            .Where(a => a.Counter > 1).Select(a => a).ToList();
        Assert.True(moreThan2.Count == 0);
    }

    [Fact(Skip = "Slow test we know it works.")]
    public void All_Indices_Compressed_Uncompressed_Values_Are_Valid()
    {
        // this is probably going to be a long running test
        foreach(var item in this.meshCache.CacheIndices)
        {
            var cacheAsset = this.meshCache[item.identity];
            Assert.NotNull(cacheAsset);
        }
    }

    [Fact]
    public void All_Indices_Compressed_Uncompressed_Values_Are_Valid_When_Loaded_With_Parallel_ForEach()
    {
        // this is probably going to be a long running test
        Parallel.ForEach(this.meshCache.CacheIndices, item =>
        {
            var cacheAsset = this.meshCache[item.identity];
            Assert.NotNull(cacheAsset);
        });
    }
}