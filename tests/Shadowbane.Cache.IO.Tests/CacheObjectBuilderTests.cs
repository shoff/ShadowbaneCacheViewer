﻿namespace Shadowbane.Cache.IO.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Exporter.File;
    using Models;
    using Shadowbane.Exporter.Wavefront;
    using Xunit;

    public class CacheObjectBuilderTests
    {
        private readonly CacheObjectBuilder builder;

        public CacheObjectBuilderTests()
        {
            this.builder = new CacheObjectBuilder();
        }

        [Fact]
        public async Task Interactives_Parse_Correctly()
        {
            foreach (var index in ArchiveLoader.ObjectArchive.CacheIndices.Where(c => !BadRenderIds.IsInList(c)))
            {
                CacheAsset asset;
                try
                {
                    asset = ArchiveLoader.ObjectArchive[index.identity];

                    using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
                    var flag = (ObjectType)reader.ReadInt32();

                    if (flag == ObjectType.Interactive)
                    {
                        var interactive = this.builder.CreateAndParse(index.identity);
                        if (interactive != null)
                        {
                            var modelDirectoryName = string.IsNullOrWhiteSpace(interactive.Name) ? interactive.Identity.ToString()
                                : $"{interactive.Name}-{interactive.Identity}";
                            var modelDirectory = $"{CacheLocation.InteractiveFolder}{modelDirectoryName}";

                            foreach (var render in interactive.Renders.Where(r => r.HasMesh && r.MeshId > 0 && r.Mesh != null))
                            {
                                await MeshExporter.ExportAsync(render.Mesh, modelDirectory, $"{interactive.Name}-{render.Identity}");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    File.AppendAllText(CacheLocation.MobileFolder + "messages.txt", e.Message);
                }
            }
        }

        [Fact]
        public async Task Equipment_Parse_Correctly()
        {
            foreach (var index in ArchiveLoader.ObjectArchive.CacheIndices.Where(c => !BadRenderIds.IsInList(c)))
            {
                CacheAsset asset;
                try
                {
                    asset = ArchiveLoader.ObjectArchive[index.identity];

                    using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
                    var flag = (ObjectType)reader.ReadInt32();

                    if (flag == ObjectType.Equipment)
                    {
                        var equipment = this.builder.CreateAndParse(index.identity);
                        if (equipment != null)
                        {
                            var modelDirectoryName = string.IsNullOrWhiteSpace(equipment.Name) ? equipment.Identity.ToString()
                                : $"{equipment.Name}-{equipment.Identity}";
                            var modelDirectory = $"{CacheLocation.EquipmentFolder}{modelDirectoryName}";

                            foreach (var render in equipment.Renders.Where(r => r.HasMesh && r.MeshId > 0 && r.Mesh != null))
                            {
                                await MeshExporter.ExportAsync(render.Mesh, modelDirectory, $"{equipment.Name}-{render.Identity}");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    File.AppendAllText(CacheLocation.MobileFolder + "messages.txt", e.Message);
                }
            }
        }

        [Fact]
        public async Task Mobiles_Parse_Correctly()
        {
            foreach (var index in ArchiveLoader.ObjectArchive.CacheIndices.Where(c => !BadRenderIds.IsInList(c)))
            {
                CacheAsset asset;
                try
                {
                    asset = ArchiveLoader.ObjectArchive[index.identity];

                    using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
                    var flag = (ObjectType)reader.ReadInt32();

                    if (flag == ObjectType.Mobile)
                    {
                        var mobile = this.builder.CreateAndParse(index.identity);
                        if (mobile != null)
                        {
                            var modelDirectoryName = string.IsNullOrWhiteSpace(mobile.Name) ? mobile.Identity.ToString()
                                : $"{mobile.Name}-{mobile.Identity}";
                            var modelDirectory = $"{CacheLocation.MobileFolder}{modelDirectoryName}";

                            foreach (var render in mobile.Renders.Where(r => r.HasMesh && r.MeshId > 0 && r.Mesh != null))
                            {
                                    await MeshExporter.ExportAsync(render.Mesh, modelDirectory, $"{mobile.Name}-{render.Identity}");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    File.AppendAllText(CacheLocation.MobileFolder + "messages.txt", e.Message);
                }
            }
        }

        [Fact]
        public async Task Structure_Models_Parse_Correctly()
        {
            foreach (var index in ArchiveLoader.ObjectArchive.CacheIndices.Where(c => !BadRenderIds.IsInList(c)))
            {
                CacheAsset asset;
                try
                {
                    asset = ArchiveLoader.ObjectArchive[index.identity];

                    using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
                    var flag = (ObjectType)reader.ReadInt32();

                    if (flag == ObjectType.Structure)
                    {
                        var structure = this.builder.CreateAndParse(index.identity);
                        if (structure != null)
                        {
                            var modelDirectoryName = string.IsNullOrWhiteSpace(structure.Name) ? structure.Identity.ToString()
                                : $"{structure.Name}-{structure.Identity}";
                            var modelDirectory = $"{CacheLocation.StructureFolder}{modelDirectoryName}";

                            foreach (var render in structure.Renders.Where(r => r.HasMesh && r.MeshId > 0 && r.Mesh != null))
                            {
                                try
                                {
                                    await MeshExporter.ExportAsync(render.Mesh, modelDirectory, $"{structure.Name}-{render.Identity}");
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    File.AppendAllText(CacheLocation.StructureFolder + "messages.txt", e.Message);
                }
            }
        }


        [Fact]
        public async Task Simple_Models_Parse_Correctly()
        {
            foreach (var index in ArchiveLoader.ObjectArchive.CacheIndices.Where(c => !BadRenderIds.IsInList(c)))
            {
                var asset = ArchiveLoader.ObjectArchive[index.identity];
                using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
                var flag = (ObjectType)reader.ReadInt32();

                if (flag == ObjectType.Simple)
                {
                    var simple = this.builder.CreateAndParse(index.identity);
                    if (simple != null)
                    {
                        var modelDirectoryName = string.IsNullOrWhiteSpace(simple.Name) ? simple.Identity.ToString()
                            : $"{simple.Name}-{simple.Identity}";
                        var modelDirectory = $"{CacheLocation.SimpleFolder}{modelDirectoryName}";

                        foreach (var render in simple.Renders.Where(r => r.HasMesh && r.MeshId > 0 && r.Mesh != null))
                        {
                            try
                            {
                                await MeshExporter.ExportAsync(render.Mesh, modelDirectory, $"{simple.Name}-{render.Identity}");
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }
        }
    }
}