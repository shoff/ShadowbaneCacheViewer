using System;
using System.Linq;
using CacheViewer.Domain.Archive;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Archive
{
    [TestFixture]
    public class CZoneTests
    {
        private CZone czone;

        [SetUp]
        public void SetUp()
        {
            this.czone = new CZone();
            this.czone.LoadCacheHeader();
        }

        [Test]
        public void CachHeader_Should_Contain_Correct_Values()
        {
            Assert.AreEqual(17236, this.czone.CacheHeader.dataOffset);
            Assert.AreEqual(1230484, this.czone.CacheHeader.fileSize);
            Assert.AreEqual(861, this.czone.CacheHeader.indexCount);
        }


        [Test]
        public async void LoadIndexes_Should_Load_The_Corrent_Number()
        {
            await this.czone.LoadIndexesAsync();
            Assert.AreEqual(this.czone.CacheHeader.indexCount, this.czone.CacheIndices.Count);
        }

        [Test]
        public async void All_Indexes_Have_Unique_Identity()
        {
            this.czone.CacheOnIndexLoad = true;
            await this.czone.LoadIndexesAsync();
            var actual = this.czone.CacheIndices.Distinct().Count();
            Assert.AreEqual(actual, this.czone.CacheHeader.indexCount);
        }

        [Test, Explicit]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            this.czone.CacheOnIndexLoad = true;
            this.czone.LoadIndexes();

            foreach (var item in this.czone.CacheIndices)
            {
                await this.czone.SaveToFile(item, AppDomain.CurrentDomain.BaseDirectory + "\\CZone\\");
            }
        }
    }
}