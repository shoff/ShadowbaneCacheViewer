using CacheViewer.Domain.Factories;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.Linq;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Models;
    using CacheViewer.Domain.Models.Exportable;

    [TestFixture]
    public class CacheObjectFactoryTests
    {
        // this breaks testing in isolation, but the alternative is to start passing this as an interface
        // which would impact performance HUGELY so ...
        private readonly CacheObjectFactory cacheObjectFactory = CacheObjectFactory.Instance;

        [Test]
        public void The_First_CacheIndex_Is_The_Sun()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault();
            var sun = this.cacheObjectFactory.Create(cacheIndex);
            Assert.AreEqual(ObjectType.Sun, sun.Flag);
        }

        [Test]
        public void Create_Simpler_Returns_A_Simple_CacheObject()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.First(x => x.identity == 103);
            var cacheObject = this.cacheObjectFactory.Create(cacheIndex);
            Assert.AreEqual(ObjectType.Simple, cacheObject.Flag);
        }

    }
}