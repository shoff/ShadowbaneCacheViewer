namespace CacheViewer.Tests.Domain.Archive
{
    using System;
    using System.Linq;
    using CacheViewer.Domain.Archive;
    using TestHelpers;
    using Xunit;

    public class TextureTests
    {
        private readonly Textures textures;

        public TextureTests()
        {
            this.textures = new Textures();
            this.textures.LoadCacheHeader();
        }
        
        [Fact]
        public async void All_Indexes_Have_Unique_Identity()
        {
            this.textures.CacheOnIndexLoad = true;
            await this.textures.LoadIndexesAsync();
            var actual = this.textures.CacheIndices.Distinct().Count();
            AssertX.Equal(actual, this.textures.CacheHeader.indexCount);
        }

        [Fact]
        public void CachHeader_Should_Contain_Correct_Values()
        {
            AssertX.Equal(193616, this.textures.CacheHeader.dataOffset);
            AssertX.Equal(489896368, this.textures.CacheHeader.fileSize);
            AssertX.Equal(9680, this.textures.CacheHeader.indexCount);
        }

        [Fact(Skip = Skip.CREATES_FILES)]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            this.textures.CacheOnIndexLoad = true;
            this.textures.LoadIndexes();

            foreach (var item in this.textures.CacheIndices)
            {
                await this.textures.SaveToFileAsync(item, AppDomain.CurrentDomain.BaseDirectory + "\\Texture\\");
            }
        }
    }
}