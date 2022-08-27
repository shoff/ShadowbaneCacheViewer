namespace Shadowbane.Cache.IO.Tests;

using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

public class RenderableObjectBuilderTests : CacheLoaderBaseTest
{
    [Fact]
    public void Record_All_RIs_With_Multiple_Textures()
    {
        File.AppendAllLines(CacheLocation.RenderOutputFolder + "multiple_textures.csv", new[]
            {"Identity,RenderType,ChildCount,LastOffset,MeshId,TextureCount,TextureIds,JointName"});
        var renders = new List<string>();
        foreach (var index in ArchiveLoader.RenderArchive.CacheIndices)
        {
            try
            {
                var information = this.renderableBuilder.Build(index);
                if (information.TextureCount < 2)
                {
                    continue;
                }

                renders.Add(information.ToString());

                if (renders.Count > 100)
                {
                    File.AppendAllLines(CacheLocation.RenderOutputFolder + "multiple_textures.csv", renders);
                    renders.Clear();
                }
            }
            catch (ParseException)
            {
                File.AppendAllText(CacheLocation.RenderOutputFolder + "bad_ids.csv", $"{index.identity},");
            }
            catch (ArgumentException)
            {
                File.AppendAllText(CacheLocation.RenderOutputFolder + "argument_exception.txt",
                    $"{index.identity},");
            }
        }
        File.AppendAllLines(CacheLocation.RenderOutputFolder + "multiple_textures.csv", renders);
    }

    [Fact]
    public void RenderId_426407_Exports_Textured_Mesh()
    {
        var asset = ArchiveLoader.RenderArchive[426407];
        var information = this.renderableBuilder.Build(426407);
    }

    [Fact]
    public void Save_All_Render_Indices_Texture_Bytes_With_Indexed_Textures()
    {
        foreach (var index in ArchiveLoader.RenderArchive.CacheIndices)
        {
            try
            {
                _ = this.renderableBuilder.Build(index.identity, saveIndexedTextures: true);
            }
            catch (ParseException)
            {
            }
        }
    }


    [Fact]
    public void All_Render_Indices_Are_Exportable()
    {
        foreach (var index in ArchiveLoader.RenderArchive.CacheIndices)
        {
            try
            {
                var information = this.renderableBuilder.Build(index);

                if (information.IsValid)
                {
                    // holy fucking shit a valid one.
                    File.AppendAllLines(CacheLocation.RenderOutputFolder + "valid_render_ids.csv",
                        new[] { information.ToString() });
                }
            }
            catch (ParseException)
            {
                File.AppendAllText(CacheLocation.RenderOutputFolder + "bad_ids.txt", $"{index.identity},");
            }
            catch (ArgumentException)
            {
                File.AppendAllText(CacheLocation.RenderOutputFolder + "argument_exception.txt", $"{index.identity},");
            }
        }
    }
}