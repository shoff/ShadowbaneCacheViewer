using System;
using System.Linq;
using CacheViewer.Domain.Archive;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Archive
{
    public class SoundCacheTests
    {
        private SoundCache soundCache;

        [SetUp]
        public void SetUp()
        {
            this.soundCache = new SoundCache();
            this.soundCache.LoadCacheHeader();
        }

        [Test]
        public void CachHeader_Should_Contain_Correct_Values()
        {
            Assert.AreEqual(21736, this.soundCache.CacheHeader.dataOffset);
            Assert.AreEqual(166165512, this.soundCache.CacheHeader.fileSize);
            Assert.AreEqual(1086, this.soundCache.CacheHeader.indexCount);
        }

        [Test]
        public async void All_Indexes_Have_Unique_Identity()
        {
            this.soundCache.CacheOnIndexLoad = true;
            await this.soundCache.LoadIndexesAsync();
            var actual = this.soundCache.CacheIndices.Distinct().Count();
            Assert.AreEqual(actual, this.soundCache.CacheHeader.indexCount);
        }

        [Test, Explicit]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            this.soundCache.CacheOnIndexLoad = true;
            this.soundCache.LoadIndexes();

            foreach (var item in this.soundCache.CacheIndices)
            {
                await this.soundCache.SaveToFile(item, AppDomain.CurrentDomain.BaseDirectory + "\\Sound\\");
            }
        }
    }
}