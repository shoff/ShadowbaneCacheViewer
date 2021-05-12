namespace Shadowbane.Cache.IO.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Models;
    using Xunit;

    public class RenderableObjectBuilderTests : CacheLoaderBaseTest
    {
        [Fact]
        public void Record_All_RIs_With_Multiple_Textures()
        {
            File.AppendAllLines(CacheLocation.RenderOutputFolder + "multiple_textures.csv", new string[]
                {"Identity,RenderType,ChildCount,LastOffset,MeshId,TextureCount,TextureIds,JointName"});
            var renders = new List<string>();
            foreach (var index in ArchiveLoader.RenderArchive.CacheIndices)
            {
                try
                {
                    var information = RenderableObjectBuilder.Build(index);
                    if (information == null || information.TextureCount < 2)
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
            var information = RenderableObjectBuilder.Build(426407);

        }
        [Fact]
        public void Save_All_Render_Indices_Texture_Bytes_With_Indexed_Textures()
        {
            foreach (var index in ArchiveLoader.RenderArchive.CacheIndices.Where(c=> c.identity == 426407))
            {
                try
                {
                    _ = RenderableObjectBuilder.Build(index.identity, saveIndexedTextures: true);
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
                    var information = RenderableObjectBuilder.Build(index);

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

        //[Fact]
        //public void All_Textures_Are_Correct()
        //{
        //    foreach (var index in ArchiveLoader.TextureArchive.CacheIndices)
        //    {
        //        var asset = ArchiveLoader.TextureArchive[index.identity];
        //        var texture = new Texture(asset.Asset, index.identity);

        //        if (texture.Image != null)
        //        {
        //            texture.Image.Save($"{CacheLocation.TextureFolder.FullName}\\{texture.TextureId}.jpg");
        //        }
        //        else
        //        {
        //            FileWriter.Writer.Write(asset.Asset.Span, CacheLocation.TextureFolder.FullName + "\\bad_image_data", $"{index.identity}-{asset.Order}");
        //        }
        //    }
        //}

        [Fact]
        public void All_Duplicate_RenderIds_Have_Duplicate_Identical_Data()
        {
            foreach (var identity in DupeIdStore.DuplicateIdentities)
            {
                var asset = ArchiveLoader.RenderArchive[identity];
                var asset2 = ArchiveLoader.RenderArchive.GetSecondIndex(identity);

                // allows us to save the two render objects to a binary file so we can 
                // open them up in 010 editor and verify the parsing in the code.
                _ = RenderableObjectBuilder.Build(asset.CacheIndex);

                Assert.Equal(asset.Asset.ToArray(), asset2.Asset.ToArray());
                Assert.True(asset.HasMultipleIdentityEntries);
            }
        }

        [Fact]
        public void All_Duplicate_RenderIds_In_Dupe2_Have_Duplicate_Identical_Data()
        {
            foreach (var identity in DupeIdStore.Dupes2)
            {
                var asset = ArchiveLoader.RenderArchive[identity];
                var asset2 = ArchiveLoader.RenderArchive.GetSecondIndex(identity);

                // allows us to save the two render objects to a binary file so we can 
                // open them up in 010 editor and verify the parsing in the code.
                _ = RenderableObjectBuilder.Build(asset.CacheIndex);

                Assert.Equal(asset.Asset.ToArray(), asset2.Asset.ToArray());
                Assert.True(asset.HasMultipleIdentityEntries);
            }
        }
    }
}