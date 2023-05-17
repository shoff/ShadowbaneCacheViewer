namespace Arcane.Cache.Tests.Json;

public class ArcaneDataTests
{
    private readonly ArcaneData arcaneData;

    public ArcaneDataTests()
    {
        this.arcaneData = new ArcaneData();
    }

    [Theory]
    [InlineData(1808)]
    [InlineData(2501)]
    [InlineData(3022)]
    [InlineData(4018)]
    [InlineData(5048)]
    [InlineData(22318)]
    [InlineData(22553)]
    [InlineData(22640)]
    [InlineData(445609)]
    [InlineData(452033)]
    public void GetById_Returns_Renderable_For_Id(uint id)
    {
        var renderable = this.arcaneData.GetById(id);
        Assert.NotNull(renderable);
    }

    [Theory]
    [InlineData(12134)]
    [InlineData(12193)]
    [InlineData(12272)]
    [InlineData(12322)]
    [InlineData(13297)]
    [InlineData(13346)]
    [InlineData(14060)]
    [InlineData(14119)]
    [InlineData(250112)]
    [InlineData(252128)]
    public void GetRune_Returns_Rune_For_Id(uint id)
    {
        var rune = this.arcaneData.GetRune(id);
        Assert.Equal(id, rune!.RuneId);
        Assert.NotNull(rune);
    }
}