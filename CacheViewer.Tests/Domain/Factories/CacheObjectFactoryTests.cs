using CacheViewer.Domain.Factories;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Models;

    [TestFixture]
    public class CacheObjectFactoryTests
    {
        // this breaks testing in isolation, but the alternative is to start passing this as an interface
        // which would impact performance HUGELY so ...
        private readonly CacheObjectsCache cacheObjectsCache = CacheObjectsCache.Instance;

        [Test, Explicit]
        public async Task Temp_OutPut_All_To_Files()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory + "CacheObjectIndexes";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            foreach (var index in this.cacheObjectsCache.CacheObjects.CacheIndices)
            {
                await this.cacheObjectsCache.CacheObjects.SaveToFile(index, folder);
            }
        }

        [Test]
        public void The_First_CacheIndex_Is_The_Sun()
        {
            var cacheIndex = this.cacheObjectsCache.Indexes.FirstOrDefault();
            var sun = this.cacheObjectsCache.Create(cacheIndex);
            Assert.AreEqual(ObjectType.Sun, sun.Flag);
        }

        [Test]
        public void Create_Simpler_Returns_A_Simple_CacheObject()
        {
            var cacheIndex = this.cacheObjectsCache.Indexes.First(x => x.Identity == 103);
            var cacheObject = this.cacheObjectsCache.Create(cacheIndex);
            Assert.AreEqual(ObjectType.Simple, cacheObject.Flag);
        }

    }
}