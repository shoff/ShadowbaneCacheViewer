namespace Shadowbane.Cache.Tests.CacheTypes;

using System.Linq;
using System.Threading.Tasks;
using Cache.CacheTypes;
using Xunit;

public class ObjectCacheTests : CacheBaseTest
{
    private readonly ObjectCache objectsCache;

    public ObjectCacheTests()
    {
        this.objectsCache = new ObjectCache();
    }


    [Fact]
    public void Cache_Validates()
    {
        this.objectsCache.Validate();
    }

    [Fact]
    public void All_Indices_Compressed_Uncompressed_Values_Are_Valid_When_Loaded_With_Parallel_ForEach()
    {
        // this is probably going to be a long running test
        Parallel.ForEach(this.objectsCache.CacheIndices, item =>
        {
            var cacheAsset = this.objectsCache[item.identity];
            Assert.NotNull(cacheAsset);
        });
    }

    [Fact]
    public void Identity_Is_Unique()
    {
        var query = this.objectsCache.CacheIndices.GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(y => new { Element = y.Key, Counter = y.Count() })
            .ToList();

        Assert.True(query.Count == 0);
    }
}