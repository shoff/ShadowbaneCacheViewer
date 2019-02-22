namespace CacheViewer.Tests.Domain.Models
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CacheViewer.Domain;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Services.Prefabs;
    using Xunit;

    public class InteractiveTests
    {
        private readonly CacheObjectFactory cacheObjectFactory;
        private readonly CObjects cobjects;
        private readonly Render renderArchive;
        private readonly StructureService structureService;

        public InteractiveTests()
        {
            this.renderArchive = (Render) ArchiveFactory.Instance.Build(CacheFile.Render);
            this.cobjects = (CObjects) ArchiveFactory.Instance.Build(CacheFile.CObjects);
            this.cacheObjectFactory = CacheObjectFactory.Instance;
            this.structureService = new StructureService();

        }

        [Fact]
        public async Task Gate_House_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 450000);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}5000", mob);
        }

        [Fact]
        public async Task Outer_Gate_House_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 450550);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}550", mob);
        }

        [Fact]
        public async Task Convex_Tower_400_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 445400);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}400", mob);
        }

        [Fact]
        public async Task Convex_Tower_500_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 445500);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}500", mob);
        }

        [Fact]
        public async Task Convex_Tower_600_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 445600);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}600", mob);
        }

        [Fact]
        public async Task Concave_Tower_100_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 445100);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}100", mob);
        }

        [Fact]
        public async Task Concave_Tower_200_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 445200);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}200", mob);
        }

        [Fact]
        public async Task Concave_Tower_300_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 445300);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}300", mob);
        }

        [Fact]
        public async Task Inner_Wall_Cap_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 450150);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}150", mob);
        }

        [Fact]
        public async Task Outer_Wall_Gate_200_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 444200);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}200", mob);
        }

        [Fact]
        public async Task Outer_Wall_Gate_300_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 444300);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}300", mob);
        }

        [Fact]
        public async Task Outer_Wall_Gate_400_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 444400);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}400", mob);
        }

        [Fact]
        public async Task Outer_Wall_Stairs_500_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 444500);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}500", mob);
        }

        [Fact]
        public async Task Outer_Wall_Stairs_600_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 444600);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}600", mob);
        }

        [Fact]
        public async Task Outer_Wall_Stairs_700_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 444700);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}700", mob);
        }

        [Fact]
        public async Task Outer_Straight_Wall_5000_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 445000);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}5000", mob);
        }

        [Fact]
        public async Task Outer_Straight_Wall_900_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 444900);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}900", mob);
        }


        [Fact]
        public async Task Outer_Straight_Wall_800_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 444800);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}800", mob);
        }

        [Fact]
        public async Task Outer_Wall_With_Tower_100_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 451000);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}451000", mob);
        }

        [Fact]
        public async Task Straight_Outer_Wall_With_Tower_454700_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 454700);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name}454700", mob);
        }
        
        [Fact]
        public async Task Straight_Outer_Wall_With_Tower_452700_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 452700);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name} 452700", mob);
        }

        [Fact]
        public async Task Straight_Outer_Wall_452650_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 452650);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name} 452650", mob);
        }


        [Fact]
        public async Task Inner_Straight_Wall_452300_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 452300);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name} 452300", mob);
        }

        [Theory]
        [InlineData(1344000)]
        [InlineData(1344200)]
        [InlineData(1344800)]
        public async Task Invorri_Magic_Shop_Parses_Correctly(int id)
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == id);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Walls\\{mob.Name} {id}", mob);
        }
        
        [Theory]
        [InlineData(594800)]
        [InlineData(595200)]
        [InlineData(595600)]
        public async Task Feudal_Magic_Shop_Parses_Correctly(int id)
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == id);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Feudal\\{mob.Name} {id}", mob);
        }

        [Theory]
        [InlineData(591200)]
        [InlineData(591600)]
        [InlineData(592000)]
        public async Task Feudal_Church_Parses_Correctly(int id)
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == id);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Feudal\\{mob.Name} {id}", mob);
        }

        [Theory]
        [InlineData(592400)]
        [InlineData(592800)]
        [InlineData(593200)]
        public async Task Feudal_Forge_Parses_Correctly(int id)
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == id);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Feudal\\{mob.Name} {id}", mob);
        }

        [Theory]
        [InlineData(24000)]
        [InlineData(24100)]
        [InlineData(24200)]
        [InlineData(24300)]
        public async Task Tree_Of_Life_Parses_Correctly(int id)
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == id);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"TreeOfLife\\{mob.Name} {id}", mob);
        }

        [Theory]
        [InlineData(200511)]
        public async Task Unknown_Parses_Correctly(int id)
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == id);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Feudal\\{mob.Name} {id}", mob);
        }
    }
}