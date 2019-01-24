using System;
using System.Linq;
using CacheViewer.Domain.Archive;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Archive
{
    public class MeshTests
    {
        private MeshArchive meshArchive;

        [SetUp]
        public void SetUp()
        {
            this.meshArchive = new MeshArchive();
            this.meshArchive.LoadCacheHeader();
        }

        [Test]
        public void CachHeader_Should_Contain_Correct_Values()
        {
            Assert.AreEqual(487756, this.meshArchive.CacheHeader.dataOffset);
            Assert.AreEqual(37232460, this.meshArchive.CacheHeader.fileSize);
            Assert.AreEqual(24387, this.meshArchive.CacheHeader.indexCount);
        }

        [Test]
        public async void LoadIndexes_Should_Load_The_Corrent_Number()
        {
            await this.meshArchive.LoadIndexesAsync();
            Assert.AreEqual(this.meshArchive.CacheHeader.indexCount, this.meshArchive.CacheIndices.Length);
        }

        [Test]
        public async void All_Indexes_Have_Unique_Identity()
        {
            this.meshArchive.CacheOnIndexLoad = true;
            await this.meshArchive.LoadIndexesAsync();
            var actual = this.meshArchive.CacheIndices.Distinct().Count();
            Assert.AreEqual(actual, this.meshArchive.CacheHeader.indexCount);
        }

        [Test, Explicit]
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