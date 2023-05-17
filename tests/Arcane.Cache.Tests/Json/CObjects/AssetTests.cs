namespace Arcane.Cache.Tests.Json.CObjects;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arcane.Cache.Json.CObjects;
using Arcane.Cache.Tests.Json;

public class AssetTests : BaseTest
{

    private readonly List<string> assetFiles;

    public AssetTests()
    {
        // load json file names into dictionary
        assetFiles = Directory.GetFiles(Path.Combine(ARCANE_DUMP, "COBJECTS", "asset")).ToList();
    }

    [Fact]
    public void All_Asset_Json_Files_Deserialize()
    {
        foreach (var asset in assetFiles)
        {
            var json = File.ReadAllText(asset);
            var assetObj = JsonSerializer.Deserialize<Asset>(json);
            Assert.NotNull(assetObj);
        }
    }

}