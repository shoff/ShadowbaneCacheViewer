namespace CacheViewer.Tests.Domain.Archive
{
    using System;
    using System.Linq;
    using CacheViewer.Domain.Archive;
    using TestHelpers;
    using Xunit;

    public class MotionTests
    {

        private readonly Motion motion;

        public MotionTests()
        {
            this.motion = new Motion();
            this.motion.LoadCacheHeader();
            this.motion.LoadIndexes();
        }
        
        [Fact]
        public void All_Indexes_Have_Unique_Identity()
        {
            this.motion.CacheOnIndexLoad = true;
            var actual = this.motion.CacheIndices.Distinct().Count();
            AssertX.Equal(this.motion.CacheHeader.indexCount, actual);
        }


        [Fact]
        public void CacheHeader_Should_Contain_Correct_Values()
        {
            this.motion.LoadCacheHeader();
            AssertX.Equal(30076, this.motion.CacheHeader.dataOffset);
            AssertX.Equal(26580492, this.motion.CacheHeader.fileSize);
            AssertX.Equal(1503, this.motion.CacheHeader.indexCount);
        }

        [Fact(Skip = Skip.CREATES_FILES)]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            this.motion.CacheOnIndexLoad = true;
            this.motion.LoadIndexes();

            foreach (var item in this.motion.CacheIndices)
            {
                await this.motion.SaveToFileAsync(item, AppDomain.CurrentDomain.BaseDirectory + "\\Motion\\");
            }
        }
    }
}