﻿namespace Shadowbane.Cache.IO.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

public class CacheObjectBuilderTests
{
    private const int TAKE = 100;
    private static readonly Random counter = new((int)DateTime.UtcNow.Ticks);
    private readonly CacheObjectBuilder builder;
    private readonly Mock<IRenderableBuilder> renderableBuilder;
    private static List<object[]> identities = 
        ArchiveLoader.ObjectArchive.CacheIndices
            .Select(x => new object[] { x.identity })
            .ToList();

    public CacheObjectBuilderTests()
    {
        this.renderableBuilder = new Mock<IRenderableBuilder>();
        this.builder = new CacheObjectBuilder(this.renderableBuilder.Object);
    }

    public static IEnumerable<object[]> Data_1000 => identities.Skip(0).Take(1000);
    public static IEnumerable<object[]> Data_2000 => identities.Skip(1000).Take(1000);
    public static IEnumerable<object[]> Data_3000 => identities.Skip(2000).Take(1000);
    public static IEnumerable<object[]> Data_4000 => identities.Skip(3000).Take(1000);
    public static IEnumerable<object[]> Data_5000 => identities.Skip(4000).Take(1000);
    public static IEnumerable<object[]> Data_6000 => identities.Skip(5000).Take(1000);
    public static IEnumerable<object[]> Data_7000 => identities.Skip(6000).Take(1000);
    public static IEnumerable<object[]> Data_8000 => identities.Skip(7000).Take(1000);
    public static IEnumerable<object[]> Data_9000 => identities.Skip(8000).Take(1000);
    public static IEnumerable<object[]> Data_10000 => identities.Skip(9000);
    
    [Theory]
    [MemberData(nameof(Data_1000))]
    public void Cache_Id_1_1000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
    }
    
    [Theory]
    [MemberData(nameof(Data_2000))]
    public void Cache_Id_1001_2000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
    }
    
    [Theory]
    [MemberData(nameof(Data_3000))]
    public void Cache_Id_2001_3000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
    }
    
    [Theory]
    [MemberData(nameof(Data_4000))]
    public void Cache_Id_3001_4000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
    }
    
    [Theory]
    [MemberData(nameof(Data_5000))]
    public void Cache_Id_4001_5000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
    }
    
    [Theory]
    [MemberData(nameof(Data_6000))]
    public void Cache_Id_5001_6000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
    }
    
    [Theory]
    [MemberData(nameof(Data_7000))]
    public void Cache_Id_6001_7000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
    }
    
    [Theory]
    [MemberData(nameof(Data_8000))]
    public void Cache_Id_7001_8000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
    }
    
    [Theory]
    [MemberData(nameof(Data_9000))]
    public void Cache_Id_8001_9000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
    }
    
    [Theory]
    [MemberData(nameof(Data_10000))]
    public void Cache_Id_9001_Last_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
    }


    //[Fact]
    //public async Task Interactives_Parse_Correctly()
    //{
    //    foreach (var index in ArchiveLoader.ObjectArchive.CacheIndices.Where(c => !BadRenderIds.IsInList(c)))
    //    {
    //        try
    //        {
    //            var asset = ArchiveLoader.ObjectArchive[index.identity];
    //            using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
    //            var flag = (ObjectType)reader.ReadInt32();

    //            if (flag == ObjectType.Interactive)
    //            {
    //                var interactive = this.builder.CreateAndParse(index.identity);
    //                if (interactive != null)
    //                {
    //                    var modelDirectoryName = string.IsNullOrWhiteSpace(interactive.Name) ? interactive.Identity.ToString()
    //                        : $"{interactive.Name}-{interactive.Identity}";
    //                    var modelDirectory = $"{CacheLocation.InteractiveFolder}{modelDirectoryName}";

    //                    foreach (var render in interactive.Renders.Where(r => r.HasMesh && r.MeshId > 0 && r.Mesh != null))
    //                    {
    //                        await MeshExporter.ExportAsync(render.Mesh, modelDirectory, $"{interactive.Name}-{render.Identity}");
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            await File.AppendAllTextAsync(CacheLocation.MobileFolder + "messages.txt", e.Message);
    //        }
    //    }
    //}

    //[Fact]
    //public async Task Equipment_Parse_Correctly()
    //{
    //    foreach (var index in ArchiveLoader.ObjectArchive.CacheIndices.Where(c => !BadRenderIds.IsInList(c)))
    //    {
    //        try
    //        {
    //            var asset = ArchiveLoader.ObjectArchive[index.identity];

    //            using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
    //            var flag = (ObjectType)reader.ReadInt32();

    //            if (flag == ObjectType.Equipment)
    //            {
    //                var equipment = this.builder.CreateAndParse(index.identity);
    //                if (equipment != null)
    //                {
    //                    var modelDirectoryName = string.IsNullOrWhiteSpace(equipment.Name) ? equipment.Identity.ToString()
    //                        : $"{equipment.Name}-{equipment.Identity}";
    //                    var modelDirectory = $"{CacheLocation.EquipmentFolder}{modelDirectoryName}";

    //                    foreach (var render in equipment.Renders.Where(r => r.HasMesh && r.MeshId > 0 && r.Mesh != null))
    //                    {
    //                        await MeshExporter.ExportAsync(render.Mesh, modelDirectory, $"{equipment.Name}-{render.Identity}");
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            await File.AppendAllTextAsync(CacheLocation.MobileFolder + "messages.txt", e.Message);
    //        }
    //    }
    //}

    //[Fact]
    //public async Task Mobiles_Parse_Correctly()
    //{
    //    int mobilesParsed = 0;

    //    do
    //    {

    //        var mobileTestSubjects = ArchiveLoader.ObjectArchive.CacheIndices.Where(c => !BadRenderIds.IsInList(c))
    //            .Skip(counter.Next(0, ArchiveLoader.ObjectArchive.IndexCount - TAKE)).Take(TAKE);

    //        foreach (var index in mobileTestSubjects)
    //        {
    //            try
    //            {
    //                var asset = ArchiveLoader.ObjectArchive[index.identity];

    //                using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
    //                var flag = (ObjectType)reader.ReadInt32();

    //                if (flag != ObjectType.Mobile)
    //                {
    //                    continue;
    //                }

    //                mobilesParsed++;

    //                var mobile = this.builder.CreateAndParse(index.identity);

    //                if (mobile != null)
    //                {
    //                    var modelDirectoryName = string.IsNullOrWhiteSpace(mobile.Name)
    //                        ? mobile.Identity.ToString() : $"{mobile.Name}-{mobile.Identity}";
    //                    var modelDirectory = $"{CacheLocation.MobileFolder}{modelDirectoryName}";

    //                    foreach (var render in mobile.Renders.Where(r =>
    //                                 r.HasMesh && r.MeshId > 0 && r.Mesh != null))
    //                    {
    //                        await MeshExporter.ExportAsync(render.Mesh, modelDirectory,
    //                            $"{mobile.Name}-{render.Identity}");
    //                    }
    //                }
    //            }
    //            catch (Exception e)
    //            {
    //                await File.AppendAllTextAsync(CacheLocation.MobileFolder + "messages.txt", e.Message);
    //            }
    //        }
    //    }
    //    while (mobilesParsed < TAKE);
    //}

    //[Fact]
    //public async Task Structure_Models_Parse_Correctly()
    //{
    //    foreach (var index in ArchiveLoader.ObjectArchive.CacheIndices.Where(c => !BadRenderIds.IsInList(c)))
    //    {
    //        try
    //        {
    //            var asset = ArchiveLoader.ObjectArchive[index.identity];

    //            using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
    //            var flag = (ObjectType)reader.ReadInt32();

    //            if (flag == ObjectType.Structure)
    //            {
    //                var structure = this.builder.CreateAndParse(index.identity);
    //                if (structure != null)
    //                {
    //                    var modelDirectoryName = string.IsNullOrWhiteSpace(structure.Name) ? structure.Identity.ToString()
    //                        : $"{structure.Name}-{structure.Identity}";
    //                    var modelDirectory = $"{CacheLocation.StructureFolder}{modelDirectoryName}";

    //                    List<Task<bool>> renderTasks = new List<Task<bool>>();

    //                    foreach (var render in structure.Renders.Where(r => r.HasMesh && r.MeshId > 0 && r.Mesh != null))
    //                    {
    //                        renderTasks.Add(MeshExporter.ExportAsync(render.Mesh, modelDirectory,
    //                            $"{structure.Name}-{render.Identity}"));
    //                    }

    //                    await Task.WhenAll(renderTasks);
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            await File.AppendAllTextAsync(CacheLocation.StructureFolder + "messages.txt", e.Message);
    //        }
    //    }
    //}

    //[Fact]
    //public async Task Simple_Models_Parse_Correctly()
    //{
    //    foreach (var index in ArchiveLoader.ObjectArchive.CacheIndices.Where(c => !BadRenderIds.IsInList(c)))
    //    {
    //        var asset = ArchiveLoader.ObjectArchive[index.identity];
    //        using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
    //        var flag = (ObjectType)reader.ReadInt32();

    //        if (flag == ObjectType.Simple)
    //        {
    //            var simple = this.builder.CreateAndParse(index.identity);
    //            if (simple != null)
    //            {
    //                var modelDirectoryName = string.IsNullOrWhiteSpace(simple.Name) ? simple.Identity.ToString()
    //                    : $"{simple.Name}-{simple.Identity}";
    //                var modelDirectory = $"{CacheLocation.SimpleFolder}{modelDirectoryName}";
    //                List<Task<bool>> renderTasks = new List<Task<bool>>();

    //                foreach (var render in simple.Renders.Where(r => r.HasMesh && r.MeshId > 0 && r.Mesh != null))
    //                {
    //                    renderTasks.Add(MeshExporter.ExportAsync(render.Mesh, modelDirectory, $"{simple.Name}-{render.Identity}"));
    //                }
    //                await Task.WhenAll(renderTasks);
    //            }
    //        }
    //    }
    //}
}