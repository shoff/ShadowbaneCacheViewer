namespace Arcane.Cache.Tests.Json;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arcane.Cache.Json.Motion;

public class MotionTests : BaseTest
{

    private readonly List<string> assetFiles;

    public MotionTests()
    {
        // load json file names into dictionary
        this.assetFiles = Directory.GetFiles(Path.Combine(ARCANE_DUMP, "MOTION")).ToList();
    }

    [Fact]
    public void All_Asset_Json_Files_Deserialize()
    {
        foreach (var asset in this.assetFiles)
        {
            var json = File.ReadAllText(asset);
            var assetObj = JsonSerializer.Deserialize<Motion>(json);
            Assert.NotNull(assetObj);
        }
    }
}