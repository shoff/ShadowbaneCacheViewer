using System;
using System.Linq;
using CacheViewer.Domain.Archive;
using NUnit.Framework;
// ReSharper disable InconsistentNaming
// ReSharper disable ExceptionNotDocumented

namespace CacheViewer.Tests.Domain.Archive
{
    using System.IO;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models;

    public class SkeletonTests
    {
        private readonly SkeletonCache skeletonCache = (SkeletonCache)ArchiveFactory.Instance.Build(CacheFile.Skeleton);
        private readonly Motion motionCache = (Motion)ArchiveFactory.Instance.Build(CacheFile.Motion);

        [Test]
        public void CachHeader_Should_Contain_Correct_Values()
        {
            Assert.AreEqual(2076, this.skeletonCache.CacheHeader.dataOffset);
            Assert.AreEqual(95628, this.skeletonCache.CacheHeader.fileSize);
            Assert.AreEqual(103, this.skeletonCache.CacheHeader.indexCount);
        }

        [Test]
        public void All_Indexes_Have_Unique_Identity()
        {
            var actual = this.skeletonCache.CacheIndices.Distinct().Count();
            Assert.AreEqual(actual, this.skeletonCache.CacheHeader.indexCount);
        }

        [Test]
        public void Data_Should_Match_MotionId_Count()
        {
            var id = skeletonCache.CacheIndices[0].identity;
            ArraySegment<byte> buffer = skeletonCache[id].Item1;

            try
            {
                Skeleton skeleton = new Skeleton(buffer, id);
                Assert.AreEqual(455, skeleton.MotionCount);
            }
            catch (EndOfStreamException)
            {
            }
            catch (IOException)
            {
            }
        }

        [Test]
        public void Id_1_Parses()
        {
            var id = skeletonCache.CacheIndices[0].identity;
            ArraySegment<byte> buffer = skeletonCache[id].Item1;
            Skeleton skeleton = new Skeleton(buffer, id);
            Assert.AreEqual(455, skeleton.MotionCount);
            TestMotionIdCollection(skeleton);
        }

        [Test]
        public void Id_2_Parses()
        {
            var id = skeletonCache.CacheIndices[1].identity;
            ArraySegment<byte> buffer = skeletonCache[id].Item1;
            Skeleton skeleton = new Skeleton(buffer, id);
            Assert.AreEqual(240, skeleton.MotionCount);
            TestMotionIdCollection(skeleton);
        }

        [Test]
        public void Id_3_Parses()
        {
            var id = skeletonCache.CacheIndices[2].identity;
            ArraySegment<byte> buffer = skeletonCache[id].Item1;
            Skeleton skeleton = new Skeleton(buffer, id);
            Assert.AreEqual(240, skeleton.MotionCount);
            TestMotionIdCollection(skeleton);
        }

        [Test]
        public void Id_6_Parses()
        {
            var id = skeletonCache.CacheIndices[5].identity;
            ArraySegment<byte> buffer = skeletonCache[id].Item1;
            Skeleton skeleton = new Skeleton(buffer, id);
            Assert.AreEqual(455, skeleton.MotionCount);
            TestMotionIdCollection(skeleton);
        }

        [Test]
        public void Id_32_Parses()
        {
            var id = skeletonCache.CacheIndices[32].identity;
            ArraySegment<byte> buffer = skeletonCache[id].Item1;
            Skeleton skeleton = new Skeleton(buffer, id);
            Assert.AreEqual(203, skeleton.MotionCount);
            TestMotionIdCollection(skeleton);
        }

        [Test]
        public void Id_999_Parses()
        {
            ArraySegment<byte> buffer = skeletonCache[999].Item1;
            Skeleton skeleton = new Skeleton(buffer, 999);
            Assert.AreEqual(455, skeleton.MotionCount);

            TestMotionIdCollection(skeleton);
        }

        private void TestMotionIdCollection(Skeleton skeleton)
        {
            foreach (var motionId in skeleton.MotionIds)
            {
                Console.WriteLine(motionId);
                var motion = this.motionCache.CacheIndices.Find(x => x.identity == motionId);
                Assert.IsNotNull(motion);
            }
        }

        [Test, Explicit]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            foreach (var item in this.skeletonCache.CacheIndices)
            {
                await this.skeletonCache.SaveToFile(item, AppDomain.CurrentDomain.BaseDirectory + "\\Skeleton\\");
            }
        }
    }
}
// ReSharper restore InconsistentNaming
// ReSharper restore ExceptionNotDocumented

