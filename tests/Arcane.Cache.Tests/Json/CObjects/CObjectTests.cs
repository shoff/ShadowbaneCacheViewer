namespace Arcane.Cache.Tests.Json.CObjects;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arcane.Cache.Json.CObjects;
using Arcane.Cache.Tests.Json;

public class CObjectTests : BaseTest
{

    private readonly List<string> assetFiles;

    public CObjectTests()
    {
        // load json file names into dictionary
        this.assetFiles = Directory.GetFiles(Path.Combine(ARCANE_DUMP, "COBJECTS", "OBJECT")).ToList();
    }

    [Fact]
    public void All_Asset_Json_Files_Deserialize()
    {
        foreach (var asset in this.assetFiles)
        {
            var json = File.ReadAllText(asset);
            var assetObj = JsonSerializer.Deserialize<CObject>(json);
            Assert.NotNull(assetObj);
        }
    }

}