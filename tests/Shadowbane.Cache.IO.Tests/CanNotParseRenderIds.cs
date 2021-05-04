namespace Shadowbane.Cache.IO.Tests
{
    using Xunit;

    public class CanNotParseRenderIds
    {
        // the invalid filename seems to always have the value of 81792
        [Fact]
        public void RenderInformation_With_Identity_524035_Fails_To_Parse()
        {
            var asset = ArchiveLoader.RenderArchive[524035];
            Assert.Throws<ParseException>(() => _ = RenderableObjectBuilder.Create(asset.CacheIndex));
        }

        [Fact]
        public void RenderInformation_With_Identity_24038_Fails_To_Parse()
        {
            var asset = ArchiveLoader.RenderArchive[24038];
            Assert.Throws<ParseException>(()=>_ = RenderableObjectBuilder.Create(asset.CacheIndex));
        }
        
        [Fact]
        public void RenderInformation_With_Identity_460476_Fails_To_Parse()
        {
            var asset = ArchiveLoader.RenderArchive[460476];
            Assert.Throws<ParseException>(() => _ = RenderableObjectBuilder.Create(asset.CacheIndex));
        }

        [Fact]
        public void RenderInformation_With_Identity_565105_Fails_To_Parse()
        {
            var asset = ArchiveLoader.RenderArchive[565105];
            Assert.Throws<ParseException>(() => _ = RenderableObjectBuilder.Create(asset.CacheIndex));
        }

        [Fact]
        public void RenderInformation_With_Identity_1324312_Fails_To_Parse()
        {
            var asset = ArchiveLoader.RenderArchive[1324312];
            Assert.Throws<ParseException>(() => _ = RenderableObjectBuilder.Create(asset.CacheIndex));
        }
    }
}