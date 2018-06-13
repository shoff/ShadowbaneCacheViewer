

namespace CacheViewer.Tests.Domain.Archive
{
    using System;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Exceptions;
    using NUnit.Framework;

    
    public class RenderTests
    {
        
        private Render render;

        
        [SetUp]
        public void SetUp()
        {
            this.render = new Render();
            this.render.LoadCacheHeader();
        }
        
        [Test]
        public void CachHeader_Should_Contain_Correct_Values()
        {
            Assert.AreEqual(835576, this.render.CacheHeader.dataOffset);
            Assert.AreEqual(3675552, this.render.CacheHeader.fileSize);
            Assert.AreEqual(41778, this.render.CacheHeader.indexCount);
        }

        [Test]
        public async void LoadIndexes_Should_Load_The_Corrent_Number()
        {
            await this.render.LoadIndexesAsync();
            Assert.AreEqual(this.render.CacheHeader.indexCount, this.render.CacheIndices.Length);
        }

        [Test]
        public void All_Indexes_Have_Unique_Identity()
        {
            this.render.CacheOnIndexLoad = false;
            this.render.LoadIndexes();
            var asset = this.render[20038];
            Assert.IsNotNull(asset.Item2);
        }

        [Test]
        public void Indexer_On_Identity_With_Multiple_Indexes_Returns_Multiple_Arrays()
        {
            this.render.CacheOnIndexLoad = true;
            this.render.LoadIndexes();
            var asset = this.render[202];
            Console.WriteLine(asset);
            Assert.IsNotNull(asset.Item2);
        }

        [Test]
        public void Indexer_On_NonExistent_Identity_Should_Throw()
        {
            this.render.CacheOnIndexLoad = true;
            Assert.Throws<IndexNotFoundException>(() => { var x = this.render[999999999]; });
        }

        [Test, Explicit]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            this.render.CacheOnIndexLoad = true;
            this.render.LoadIndexes();

            foreach (var item in this.render.CacheIndices)
            {
               await this.render.SaveToFile(item, AppDomain.CurrentDomain.BaseDirectory + "\\Render\\");
            }
        }
    }
}