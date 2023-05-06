namespace Shadowbane.Cache.AllTests.CacheTypes;

using Shadowbane.Cache.CacheTypes;
using Xunit;

public class PaletteCacheTests : CacheBaseTest
{
    private readonly PaletteCache paletteCache;

    public PaletteCacheTests()
    {
        this.paletteCache = new PaletteCache();
    }


    [Fact]
    public void Cache_Validates()
    {
        this.paletteCache.Validate();
    }

}