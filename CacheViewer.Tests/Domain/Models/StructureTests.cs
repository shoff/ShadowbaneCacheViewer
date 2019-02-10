namespace CacheViewer.Tests.Domain.Models
{
    using System.Linq;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models;
    using NUnit.Framework;

    [TestFixture]
    public class StructureTests
    {
        // this breaks testing in isolation, but the alternative is to start passing this as an interface
        // which would impact performance HUGELY so ...
        private readonly CacheObjectFactory cacheObjectFactory = CacheObjectFactory.Instance;

        [Test]
        public void CacheId_973400_Should_Find_RenderId_907019()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.First(x => x.Identity == 973400);
            var asset = this.cacheObjectFactory.CacheObjects[973400];
            var structure = new Structure(cacheIndex, Data.ObjectType.Structure, "Ancient Ruins", 23, asset.Item1, 23);
            structure.Parse();
            Assert.AreEqual(structure.RenderCount, structure.RenderIds.Count);
        }
    }
}