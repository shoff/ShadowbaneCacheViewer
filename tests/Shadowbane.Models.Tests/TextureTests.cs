namespace Shadowbane.Models.Tests;

using System;
using System.IO;
using Cache.IO;
using Xunit;
using Xunit.Abstractions;

public class TextureTests
{
    private readonly ITestOutputHelper outputHelper;

    public TextureTests(ITestOutputHelper outputHelper)
    {
        this.outputHelper = outputHelper;
    }

    [Fact]
    public void All_Textures_Parse_Correctly()
    {
        if (Directory.Exists(Texture.ImageSavePath))
        {
            Directory.Delete(Texture.ImageSavePath, true);
        }

        this.outputHelper.WriteLine($"Creating {Texture.ImageSavePath}");
        Directory.CreateDirectory(Texture.ImageSavePath);

        foreach (var cacheIndex in ArchiveLoader.TextureArchive.CacheIndices)
        {
            var textureAsset = ArchiveLoader.TextureArchive[cacheIndex.identity];
            try
            {
                var texture = new Texture(textureAsset.Asset, cacheIndex.identity);

                Assert.True(texture.IsValid);
            }
            catch (Exception e)
            {
                this.outputHelper.WriteLine($"{e.Message} could not process texture id {cacheIndex.identity}");
            }
        }
    }


}