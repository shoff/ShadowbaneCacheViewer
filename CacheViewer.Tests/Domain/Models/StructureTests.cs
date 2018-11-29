namespace CacheViewer.Tests.Domain.Models
{
    using CacheViewer.Domain.Factories;
    using NUnit.Framework;

    [TestFixture]
    public class StructureTests
    {
        // this breaks testing in isolation, but the alternative is to start passing this as an interface
        // which would impact performance HUGELY so ...
        private readonly CacheObjectsCache cacheObjectsCache = CacheObjectsCache.Instance;

        [Test]
        public void Parse_Ranger_Blind_Should_Be_Correct()
        {
            // Get the cache file
            //var cacheFile = this.cacheObjectsCache.CreateAndParse(this.c)
        }

    }
}