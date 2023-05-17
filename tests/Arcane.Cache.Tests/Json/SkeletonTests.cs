namespace Arcane.Cache.Tests.Json;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arcane.Cache.Json.Skeleton;

public class SkeletonTests : BaseTest
{

    private readonly List<string> assetFiles;

    public SkeletonTests()
    {
        // load json file names into dictionary
        this.assetFiles = Directory.GetFiles(Path.Combine(ARCANE_DUMP, "SKELETON")).ToList();
    }

    [Fact]
    public void All_Asset_Json_Files_Deserialize()
    {
        foreach (var asset in this.assetFiles)
        {
            var json = File.ReadAllText(asset);
            var assetObj = JsonSerializer.Deserialize<Skeleton>(json);
            Assert.NotNull(assetObj);
        }
    }
}