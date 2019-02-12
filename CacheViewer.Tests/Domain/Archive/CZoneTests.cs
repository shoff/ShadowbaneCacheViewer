namespace CacheViewer.Tests.Domain.Archive
{
    using System;
    using System.Linq;
    using CacheViewer.Domain.Archive;
    using TestHelpers;
    using Xunit;

    public class CZoneTests
    {
        private readonly CZone czone;

        public CZoneTests()
        {
            this.czone = new CZone();
            this.czone.LoadCacheHeader();
        }
        
        [Fact]
        public async void All_Indexes_Have_Unique_Identity()
        {
            this.czone.CacheOnIndexLoad = true;
            await this.czone.LoadIndexesAsync();
            var actual = this.czone.CacheIndices.Distinct().Count();
            AssertX.Equal(actual, this.czone.CacheHeader.indexCount);
        }

        [Fact]
        public void CacheHeader_Should_Contain_Correct_Values()
        {
            AssertX.Equal(17236, this.czone.CacheHeader.dataOffset);
            AssertX.Equal(1230484, this.czone.CacheHeader.fileSize);
            AssertX.Equal(861, this.czone.CacheHeader.indexCount);
        }


        [Fact]
        public async void LoadIndexes_Should_Load_The_Correct_Number()
        {
            await this.czone.LoadIndexesAsync();
            AssertX.Equal(this.czone.CacheHeader.indexCount, this.czone.CacheIndices.Length);
        }

        [Fact(Skip = Skip.CREATES_FILES)]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            this.czone.CacheOnIndexLoad = true;
            this.czone.LoadIndexes();

            foreach (var item in this.czone.CacheIndices)
            {
                await this.czone.SaveToFileAsync(item, AppDomain.CurrentDomain.BaseDirectory + "\\CZone\\");
            }
        }
    }
}