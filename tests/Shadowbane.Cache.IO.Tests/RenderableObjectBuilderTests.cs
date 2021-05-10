namespace Shadowbane.Cache.IO.Tests
{
    using Xunit;

    public class RenderableObjectBuilderTests : CacheLoaderBaseTest
    {
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