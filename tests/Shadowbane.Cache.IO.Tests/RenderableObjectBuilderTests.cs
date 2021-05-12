namespace Shadowbane.Cache.IO.Tests
{
    using System.IO;
    using Xunit;

    public class RenderableObjectBuilderTests : CacheLoaderBaseTest
    {
        [Fact]
        public void All_Render_Indices_Are_Exportable()
        {
            foreach (var index in ArchiveLoader.RenderArchive.CacheIndices)
            {
                try
                {
                    var information = RenderableObjectBuilder.Create(index, true);

                    if (information.IsValid)
                    {
                        // holy fucking shit a valid one.
                        File.AppendAllLines(CacheLocation.RenderOutputFolder + "valid_render_ids.txt",
                            new[] { information.Identity.ToString() });
                    }
                }
                catch (ParseException)
                {
                    File.AppendAllText(CacheLocation.RenderOutputFolder + "bad_ids.txt", $"{index.identity},");
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
                _ = RenderableObjectBuilder.Create(asset.CacheIndex);

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
                _ = RenderableObjectBuilder.Create(asset.CacheIndex);

                Assert.Equal(asset.Asset.ToArray(), asset2.Asset.ToArray());
                Assert.True(asset.HasMultipleIdentityEntries);
            }
        }
    }
}