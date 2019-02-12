namespace CacheViewer.Tests.Domain.Archive
{
    using System;
    using System.Linq;
    using CacheViewer.Domain.Archive;
    using TestHelpers;
    using Xunit;

    public class SoundCacheTests
    {
        private readonly SoundCache soundCache;
        public SoundCacheTests()
        {
            this.soundCache = new SoundCache();
            this.soundCache.LoadCacheHeader();
        }

        [Fact]
        public async void All_Indexes_Have_Unique_Identity()
        {
            this.soundCache.CacheOnIndexLoad = true;
            await this.soundCache.LoadIndexesAsync();
            var actual = this.soundCache.CacheIndices.Distinct().Count();
            AssertX.Equal(actual, this.soundCache.CacheHeader.indexCount);
        }

        [Fact]
        public void CacheHeader_Should_Contain_Correct_Values()
        {
            AssertX.Equal(21736, this.soundCache.CacheHeader.dataOffset);
            AssertX.Equal(166165512, this.soundCache.CacheHeader.fileSize);
            AssertX.Equal(1086, this.soundCache.CacheHeader.indexCount);
        }

        [Fact(Skip = Skip.CREATES_FILES)]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            this.soundCache.CacheOnIndexLoad = true;
            this.soundCache.LoadIndexes();

            foreach (var item in this.soundCache.CacheIndices)
            {
                await this.soundCache.SaveToFileAsync(item, AppDomain.CurrentDomain.BaseDirectory + "\\Sound\\");
            }
        }
    }
}