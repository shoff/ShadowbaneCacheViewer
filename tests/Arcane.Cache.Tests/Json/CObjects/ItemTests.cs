namespace Arcane.Cache.Tests.Json.CObjects;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arcane.Cache.Json.CObjects;
using Arcane.Cache.Tests.Json;

public class ItemTests : BaseTest
{

    private readonly List<string> assetFiles;

    public ItemTests()
    {
        // load json file names into dictionary
        this.assetFiles = Directory.GetFiles(Path.Combine(ARCANE_DUMP, "COBJECTS", "ITEM")).ToList();
    }

    [Fact]
    public void All_Asset_Json_Files_Deserialize()
    {
        foreach (var asset in this.assetFiles)
        {
            var json = File.ReadAllText(asset);
            var assetObj = JsonSerializer.Deserialize<Item>(json);
            assetObj!.ItemId = Convert.ToUInt32(Path.GetFileNameWithoutExtension(asset));
            Assert.NotNull(assetObj);
        }
    }

}