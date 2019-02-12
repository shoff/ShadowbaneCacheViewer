namespace CacheViewer.Tests.Domain.Archive
{
    using System;
    using System.Linq;
    using CacheViewer.Domain.Archive;
    using TestHelpers;
    using Xunit;

    public class MeshTests
    {
        private readonly MeshArchive meshArchive;

        public MeshTests()
        {
            this.meshArchive = new MeshArchive();
            this.meshArchive.LoadCacheHeader();
        }
        
        [Fact]
        public async void All_Indexes_Have_Unique_Identity()
        {
            this.meshArchive.CacheOnIndexLoad = true;
            await this.meshArchive.LoadIndexesAsync();
            var actual = this.meshArchive.CacheIndices.Distinct().Count();
            AssertX.Equal(actual, this.meshArchive.CacheHeader.indexCount);
        }

        [Fact]
        public void CacheHeader_Should_Contain_Correct_Values()
        {
            AssertX.Equal(487756, this.meshArchive.CacheHeader.dataOffset);
            AssertX.Equal(37232460, this.meshArchive.CacheHeader.fileSize);
            AssertX.Equal(24387, this.meshArchive.CacheHeader.indexCount);
        }

        [Fact]
        public async void LoadIndexes_Should_Load_The_Correct_Number()
        {
            await this.meshArchive.LoadIndexesAsync();
            AssertX.Equal(this.meshArchive.CacheHeader.indexCount, this.meshArchive.CacheIndices.Length);
        }

        [Fact(Skip = Skip.CREATES_FILES)]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            this.meshArchive.CacheOnIndexLoad = true;
            this.meshArchive.LoadIndexes();

            foreach (var item in this.meshArchive.CacheIndices)
            {
                await this.meshArchive.SaveToFileAsync(item, AppDomain.CurrentDomain.BaseDirectory + "\\MeshArchive\\");
            }
        }
    }
}