namespace Arcane.Cache.Tests.Json.CObjects;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arcane.Cache.Json.CObjects;
using Arcane.Cache.Tests.Json;

public class DeedTests : BaseTest
{

    private readonly List<string> assetFiles;

    public DeedTests()
    {
        // load json file names into dictionary
        this.assetFiles = Directory.GetFiles(Path.Combine(ARCANE_DUMP, "COBJECTS", "DEED")).ToList();
    }

    [Fact]
    public void All_Asset_Json_Files_Deserialize()
    {
        foreach (var asset in this.assetFiles)
        {
            var json = File.ReadAllText(asset);
            var assetObj = JsonSerializer.Deserialize<Deed>(json);
            Assert.NotNull(assetObj);
        }
    }
}