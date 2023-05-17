namespace Arcane.Cache.Tests.Json.CObjects;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arcane.Cache.Json.CObjects;
using Arcane.Cache.Tests.Json;

public class StructureTests : BaseTest
{

    private readonly List<string> assetFiles;

    public StructureTests()
    {
        // load json file names into dictionary
        this.assetFiles = Directory.GetFiles(Path.Combine(ARCANE_DUMP, "COBJECTS", "STATIC")).ToList();
    }

    [Fact]
    public void All_Asset_Json_Files_Deserialize()
    {
        foreach (var asset in this.assetFiles)
        {
            var json = File.ReadAllText(asset);
            var assetObj = JsonSerializer.Deserialize<Structure>(json);
            Assert.NotNull(assetObj);
        }
    }
}