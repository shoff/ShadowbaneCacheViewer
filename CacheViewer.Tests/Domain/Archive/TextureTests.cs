using System;
using System.Linq;
using CacheViewer.Domain.Archive;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Archive
{
    [TestFixture]
    public class TextureTests
    {
        private Textures textures;
        [SetUp]
        public void SetUp()
        {
            this.textures = new Textures();
            this.textures.LoadCacheHeader();
        }

        [Test]
        public void CachHeader_Should_Contain_Correct_Values()
        {
            Assert.AreEqual(193616, this.textures.CacheHeader.dataOffset);
            Assert.AreEqual(489896368, this.textures.CacheHeader.fileSize);
            Assert.AreEqual(9680, this.textures.CacheHeader.indexCount);
        }

        [Test]
        public async void All_Indexes_Have_Unique_Identity()
        {
            this.textures.CacheOnIndexLoad = true;
            await this.textures.LoadIndexesAsync();
            var actual = this.textures.CacheIndices.Distinct().Count();
            Assert.AreEqual(actual, this.textures.CacheHeader.indexCount);
        }
        
        [Test, Explicit]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            this.textures.CacheOnIndexLoad = true;
            this.textures.LoadIndexes();

            foreach (var item in this.textures.CacheIndices)
            {
                await this.textures.SaveToFileAsync(item, AppDomain.CurrentDomain.BaseDirectory + "\\Texture\\");
            }
        }
    }
}