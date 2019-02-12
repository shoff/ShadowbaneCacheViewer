

namespace CacheViewer.Tests.Domain.Archive
{
    using System;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Exceptions;
    using TestHelpers;
    using Xunit;


    public class RenderTests
    {
        
        private readonly Render render;

        public RenderTests()
        {
            this.render = new Render();
            this.render.LoadCacheHeader();
        }
        
        [Fact]
        public void CacheHeader_Should_Contain_Correct_Values()
        {
            AssertX.Equal(835576, this.render.CacheHeader.dataOffset);
            AssertX.Equal(3675552, this.render.CacheHeader.fileSize);
            AssertX.Equal(41778, this.render.CacheHeader.indexCount);
        }

        [Fact]
        public async void LoadIndexes_Should_Load_The_Correct_Number()
        {
            await this.render.LoadIndexesAsync();
            AssertX.Equal(this.render.CacheHeader.indexCount, this.render.CacheIndices.Length);
        }

        [Fact]
        public void All_Indexes_Have_Unique_Identity()
        {
            this.render.CacheOnIndexLoad = false;
            this.render.LoadIndexes();
            var asset = this.render[20038];
            Assert.NotNull(asset.Item2);
        }

        [Fact]
        public void Indexer_On_Identity_With_Multiple_Indexes_Returns_Multiple_Arrays()
        {
            this.render.CacheOnIndexLoad = true;
            this.render.LoadIndexes();
            var asset = this.render[202];
            Console.WriteLine(asset);
            Assert.NotNull(asset.Item2);
        }

        [Fact]
        public void Indexer_On_NonExistent_Identity_Should_Throw()
        {
            this.render.CacheOnIndexLoad = true;
            Assert.Throws<IndexNotFoundException>(() => { var x = this.render[999999999]; });
        }

        [Fact(Skip = Skip.CREATES_FILES)]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            this.render.CacheOnIndexLoad = true;
            this.render.LoadIndexes();

            foreach (var item in this.render.CacheIndices)
            {
               await this.render.SaveToFileAsync(item, AppDomain.CurrentDomain.BaseDirectory + "\\Render\\");
            }
        }
    }
}