namespace Shadowbane.Cache.AllTests.CacheTypes;

using System.Linq;
using System.Threading.Tasks;
using Shadowbane.Cache.CacheTypes;
using Xunit;

public class TerrainAlphaCacheTests : CacheBaseTest
{
    private readonly TerrainAlphaCache terrainAlphaCache;

    public TerrainAlphaCacheTests()
    {
        this.terrainAlphaCache = new TerrainAlphaCache();
    }

    [Fact]
    public void Cache_Has_Correct_Index_Count()
    {
        var expected = 20912;
        var actual = this.terrainAlphaCache.IndexCount;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void All_Indices_Compressed_Uncompressed_Values_Are_Valid_When_Loaded_With_Parallel_ForEach()
    {
        // this is probably going to be a long running test
        Parallel.ForEach(this.terrainAlphaCache.CacheIndices, item =>
        {
            var cacheAsset = this.terrainAlphaCache[item.identity];
            Assert.NotNull(cacheAsset);
        });
    }

    [Fact]
    public void Identity_Is_Unique()
    {
        Assert.True(this.terrainAlphaCache.CacheIndices.GroupBy(x => x.identity).All(g => g.Count() == 1));
    }

    [Fact]
    public void Cache_Validates()
    {
        this.terrainAlphaCache.Validate();
    }
}