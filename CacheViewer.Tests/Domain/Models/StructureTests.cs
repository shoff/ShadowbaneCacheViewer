namespace CacheViewer.Tests.Domain.Models
{
    using System.Linq;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models;
    using CacheViewer.Domain.Services.Prefabs;
    using Xunit;

    public class StructureTests
    {
        private readonly CacheObjectFactory cacheObjectFactory = CacheObjectFactory.Instance;
        private readonly StructureService structureService;

        public StructureTests()
        {
            this.structureService = new StructureService();
        }

        [Fact]
        public async Task Elven_Tent_Positioned_Creates_Valid_Model()
        {
            var cacheObject = this.cacheObjectFactory.CreateAndParse(423400, true);
            await this.structureService.SaveAssembledModelAsync($"Combined\\{cacheObject.Name}", cacheObject, true);
        }

        [Fact]
        public void CacheId_973400_Should_Find_RenderId_907019()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.First(x => x.Identity == 973400);
            var asset = this.cacheObjectFactory.CacheObjects[973400];
            var structure = new Structure(cacheIndex, Data.ObjectType.Structure, "Ancient Ruins", 23, asset.Item1, 23);
            structure.Parse();
            Assert.Equal(structure.RenderCount, structure.RenderIds.Count);
        }
    }
}