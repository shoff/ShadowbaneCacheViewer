namespace Shadowbane.Models.Tests;

using Xunit;

public class RenderIdLookupTests
{

    [Theory]
    [InlineData(300)]
    public void IsValid_Returns_True_For_Identity(uint identity)
    {
        Assert.True(IdLookup.IsValidObjectId(identity));
    }

}