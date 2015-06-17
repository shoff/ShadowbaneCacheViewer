using System;
using System.Linq;
using CacheViewer.Domain.Archive;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Archive
{
    [TestFixture]
    public class MotionTests
    {
        private Motion motion;

        [SetUp]
        public void SetUp()
        {
            this.motion = new Motion();
        }

        [Test]
        public void CachHeader_Should_Contain_Correct_Values()
        {
            this.motion.LoadCacheHeader();
            Assert.AreEqual(30076, this.motion.CacheHeader.dataOffset);
            Assert.AreEqual(26580492, this.motion.CacheHeader.fileSize);
            Assert.AreEqual(1503, this.motion.CacheHeader.indexCount);
        }

        [Test]
        public void All_Indexes_Have_Unique_Identity()
        {
            this.motion.CacheOnIndexLoad = true;
            var actual = this.motion.CacheIndices.Distinct().Count();
            Assert.AreEqual(actual, this.motion.CacheHeader.indexCount);
        }

        [Test, Explicit]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            this.motion.CacheOnIndexLoad = true;
            this.motion.LoadIndexes();

            foreach (var item in this.motion.CacheIndices)
            {
                await this.motion.SaveToFile(item, AppDomain.CurrentDomain.BaseDirectory + "\\Motion\\");
            }
        }
    }
}