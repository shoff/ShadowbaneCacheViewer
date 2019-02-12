namespace CacheViewer.Tests.Domain.Archive
{
    using System;
    using System.IO;
    using System.Linq;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models;
    using TestHelpers;
    using Xunit;

    public class SkeletonTests
    {
        private readonly SkeletonCache skeletonCache = (SkeletonCache) ArchiveFactory.Instance.Build(CacheFile.Skeleton);
        private readonly Motion motionCache = (Motion) ArchiveFactory.Instance.Build(CacheFile.Motion);

        private void TestMotionIdCollection(Skeleton skeleton)
        {
            foreach (var motionId in skeleton.MotionIds)
            {
                Console.WriteLine(motionId);
                var motion = this.motionCache.CacheIndices.First(x => x.Identity == motionId);
                Assert.NotNull(motion);
            }
        }

        [Fact]
        public void All_Indexes_Have_Unique_Identity()
        {
            var actual = this.skeletonCache.CacheIndices.Distinct().Count();
            AssertX.Equal(actual, this.skeletonCache.CacheHeader.indexCount);
        }

        [Fact]
        public void CacheHeader_Should_Contain_Correct_Values()
        {
            AssertX.Equal(2076, this.skeletonCache.CacheHeader.dataOffset);
            AssertX.Equal(95628, this.skeletonCache.CacheHeader.fileSize);
            AssertX.Equal(103, this.skeletonCache.CacheHeader.indexCount);
        }

        [Fact]
        public void Data_Should_Match_MotionId_Count()
        {
            var id = this.skeletonCache.CacheIndices[0].Identity;
            var buffer = this.skeletonCache[id].Item1;

            try
            {
                var skeleton = new Skeleton(buffer, id);
                AssertX.Equal(455, skeleton.MotionCount);
            }
            catch (EndOfStreamException)
            {
            }
            catch (IOException)
            {
            }
        }

        [Fact]
        public void Id_1_Parses()
        {
            var id = this.skeletonCache.CacheIndices[0].Identity;
            var buffer = this.skeletonCache[id].Item1;
            var skeleton = new Skeleton(buffer, id);
            AssertX.Equal(455, skeleton.MotionCount);
            this.TestMotionIdCollection(skeleton);
        }

        [Fact]
        public void Id_2_Parses()
        {
            var id = this.skeletonCache.CacheIndices[1].Identity;
            var buffer = this.skeletonCache[id].Item1;
            var skeleton = new Skeleton(buffer, id);
            AssertX.Equal(240, skeleton.MotionCount);
            this.TestMotionIdCollection(skeleton);
        }

        [Fact]
        public void Id_3_Parses()
        {
            var id = this.skeletonCache.CacheIndices[2].Identity;
            var buffer = this.skeletonCache[id].Item1;
            var skeleton = new Skeleton(buffer, id);
            AssertX.Equal(240, skeleton.MotionCount);
            this.TestMotionIdCollection(skeleton);
        }

        [Fact]
        public void Id_32_Parses()
        {
            var id = this.skeletonCache.CacheIndices[32].Identity;
            var buffer = this.skeletonCache[id].Item1;
            var skeleton = new Skeleton(buffer, id);
            AssertX.Equal(203, skeleton.MotionCount);
            this.TestMotionIdCollection(skeleton);
        }

        [Fact]
        public void Id_6_Parses()
        {
            var id = this.skeletonCache.CacheIndices[5].Identity;
            var buffer = this.skeletonCache[id].Item1;
            var skeleton = new Skeleton(buffer, id);
            AssertX.Equal(455, skeleton.MotionCount);
            this.TestMotionIdCollection(skeleton);
        }

        [Fact]
        public void Id_999_Parses()
        {
            var buffer = this.skeletonCache[999].Item1;
            var skeleton = new Skeleton(buffer, 999);
            AssertX.Equal(455, skeleton.MotionCount);

            this.TestMotionIdCollection(skeleton);
        }

        [Fact(Skip = Skip.CREATES_FILES)]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            foreach (var item in this.skeletonCache.CacheIndices)
            {
                await this.skeletonCache.SaveToFileAsync(item, AppDomain.CurrentDomain.BaseDirectory + "\\Skeleton\\");
            }
        }
    }
}
// ReSharper restore InconsistentNaming
// ReSharper restore ExceptionNotDocumented