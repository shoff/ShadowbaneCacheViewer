namespace Arcane.Cache.Tests.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arcane.Cache.Json.Render;

public class RenderableTests : BaseTest
{

    private readonly List<string> assetFiles;

    public RenderableTests()
    {
        // load json file names into dictionary
        this.assetFiles = Directory.GetFiles(Path.Combine(ARCANE_DUMP, "RENDER")).ToList();
    }

    [Fact]
    public void All_Asset_Json_Files_Deserialize()
    {
        foreach (var asset in this.assetFiles)
        {
            var json = File.ReadAllText(asset);
            var assetObj = JsonSerializer.Deserialize<Renderable>(json);
            assetObj!.RederableId = Convert.ToUInt32(Path.GetFileNameWithoutExtension(asset));
            Assert.NotNull(assetObj);
        }
    }
}