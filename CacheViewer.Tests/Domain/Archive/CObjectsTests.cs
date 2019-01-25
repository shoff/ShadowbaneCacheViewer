

namespace CacheViewer.Tests.Domain.Archive
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Factories;
    using NUnit.Framework;

    
    [TestFixture]
    public class CObjectsTests
    {
        
        private readonly CObjects cobjects = (CObjects)ArchiveFactory.Instance.Build(CacheFile.CObjects);

        
        [Test]
        public void CachHeader_Should_Contain_Correct_Values()
        {
            Assert.AreEqual(212376, this.cobjects.CacheHeader.dataOffset);
            Assert.AreEqual(5211072, this.cobjects.CacheHeader.fileSize);
            Assert.AreEqual(10618, this.cobjects.CacheHeader.indexCount);
        }

        
        [Test]
        public void LoadIndexes_Should_Load_The_Correct_Number()
        {

            var expected = this.cobjects.CacheHeader.indexCount;
            var actual = this.cobjects.CacheIndices.Length;
            Assert.AreEqual(expected, actual);
        }

        
        [Test]
        public void All_Indexes_Have_Unique_Identity()
        {
            this.cobjects.CacheOnIndexLoad = true;
            var actual = this.cobjects.CacheIndices.Distinct().Count();
            Assert.AreEqual(actual, this.cobjects.CacheHeader.indexCount);
        }

        
        [Test, Explicit]
        public async Task SaveToFileAsync_Should_Output_All_Assets()
        {
            this.cobjects.CacheOnIndexLoad = true;
            foreach (var item in this.cobjects.CacheIndices)
            {
                await this.cobjects.SaveToFileAsync(item, AppDomain.CurrentDomain.BaseDirectory + "\\CObjects\\");
            }
        }

        [Test]
        public void Parse_Finds_The_Correct_Values()
        {
            var index = this.cobjects.CacheIndices[50]; // random number here.
            var cobject = this.cobjects[index.Identity];
            Assert.AreEqual(121, cobject.CacheIndex1.CompressedSize);
            Assert.AreEqual(481, cobject.CacheIndex1.UnCompressedSize);
            Assert.AreEqual(582, cobject.CacheIndex1.Identity);
            Assert.AreEqual(260008, cobject.CacheIndex1.Offset);
        }
        
        private bool TestRange(int numberToCheck, int bottom, int top)
        {
            return numberToCheck > bottom && numberToCheck < top;
        }

    }
}