namespace Shadowbane.Cache.IO.Tests
{
    using System;
    using System.IO;
    using Xunit;

    public class RenderableObjectBuilderTests : CacheLoaderBaseTest
    {
        [Fact]
        public void All_Render_Indices_Are_Exportable()
        {
            foreach (var index in ArchiveLoader.RenderArchive.CacheIndices)
            {
                if (!BadRenderIds.IsInList(index))
                {
                    try
                    {
                        var information = RenderableObjectBuilder.Create(index, true);
                        
                        if (information.IsValid)
                        {
                            // holy fucking shit a valid one.
                            File.AppendAllLines(AppDomain.CurrentDomain.BaseDirectory + "valid_render_ids.txt",
                                new[] { information.Identity.ToString() });
                        }
                    }
                    catch (ParseException)
                    {
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "bad_ids.txt", $"{index.identity},");
                    }
                }
            }
        }

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