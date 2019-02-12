namespace CacheViewer.Tests.Domain.Archive
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Factories;
    using TestHelpers;
    using Xunit;
    
    public class CObjectsTests
    {
        private readonly CObjects cobjects = (CObjects)ArchiveFactory.Instance.Build(CacheFile.CObjects);
        
        [Fact]
        public void CacheHeader_Should_Contain_Correct_Values()
        {
            AssertX.Equal(212376, this.cobjects.CacheHeader.dataOffset);
            AssertX.Equal(5211072, this.cobjects.CacheHeader.fileSize);
            AssertX.Equal(10618, this.cobjects.CacheHeader.indexCount);
        }
        
        [Fact]
        public void LoadIndexes_Should_Load_The_Correct_Number()
        {

            var expected = this.cobjects.CacheHeader.indexCount;
            var actual = this.cobjects.CacheIndices.Length;
            Assert.Equal((int)expected, actual);
        }
        
        [Fact]
        public void All_Indexes_Have_Unique_Identity()
        {
            this.cobjects.CacheOnIndexLoad = true;
            var actual = this.cobjects.CacheIndices.Distinct().Count();
            Assert.Equal(actual, (int)this.cobjects.CacheHeader.indexCount);
        }

        
        [Fact(Skip = Skip.CREATES_FILES)]
        public async Task SaveToFileAsync_Should_Output_All_Assets()
        {
            this.cobjects.CacheOnIndexLoad = true;
            foreach (var item in this.cobjects.CacheIndices)
            {
                await this.cobjects.SaveToFileAsync(item, AppDomain.CurrentDomain.BaseDirectory + "\\CObjects\\");
            }
        }

        [Fact]
        public void Parse_Finds_The_Correct_Values()
        {
            var index = this.cobjects.CacheIndices[50]; // random number here.
            var cobject = this.cobjects[index.Identity];
            AssertX.Equal(121, cobject.CacheIndex1.CompressedSize);
            AssertX.Equal(481, cobject.CacheIndex1.UnCompressedSize);
            Assert.Equal(582, cobject.CacheIndex1.Identity);
            AssertX.Equal(260008, cobject.CacheIndex1.Offset);
        }
    }
}