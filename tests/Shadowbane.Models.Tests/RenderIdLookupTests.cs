namespace Shadowbane.Models.Tests;

using System;
using System.Threading.Tasks;
using Xunit;

public class RenderIdLookupTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ExportToCSVAsync_Throws_When_Path_Is_Null_Empty_Or_Whitespace(string path)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => IdLookup.ExportObjectIdsToCSVAsync(path));
    }

    [Theory]
    [InlineData(300)]
    public void IsValid_Returns_True_For_Identity(uint identity)
    {
        Assert.True(IdLookup.IsValidObjectId(identity));
    }

}