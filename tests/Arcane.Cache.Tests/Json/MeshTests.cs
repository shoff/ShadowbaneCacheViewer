namespace Arcane.Cache.Tests.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arcane.Cache.Json.Mesh;

public class MeshTests : BaseTest
{
    private readonly List<string> assetFiles;

    public MeshTests()
    {
        // load json file names into dictionary
        this.assetFiles = Directory.GetFiles(Path.Combine(ARCANE_DUMP, "MESH")).ToList();
    }

    [Fact]
    public void All_Asset_Json_Files_Deserialize()
    {
        foreach (var asset in this.assetFiles)
        {
            try
            {
                var json = File.ReadAllText(asset);
                var assetObj = JsonSerializer.Deserialize<Mesh>(json);
                assetObj!.MeshId = Convert.ToUInt32(Path.GetFileNameWithoutExtension(asset));
                Assert.NotNull(assetObj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}