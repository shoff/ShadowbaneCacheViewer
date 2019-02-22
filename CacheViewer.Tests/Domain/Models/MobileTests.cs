namespace CacheViewer.Tests.Domain.Models
{
    using System.Linq;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Services.Prefabs;
    using Xunit;

    public class MobileTests
    {
        private readonly CacheObjectFactory cacheObjectFactory = CacheObjectFactory.Instance;
        private readonly StructureService structureService;

        public MobileTests()
        {
            this.structureService = new StructureService();
        }

        [Fact]
        public async Task Vampire_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 2029);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Mobiles\\{mob.Name}", mob);
        }

        [Fact]
        public async Task Ice_Troll_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 12513);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Mobiles\\{mob.Name}", mob, true);
        }

        [Fact]
        public async Task Dervish_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 12526);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Mobiles\\{mob.Name}", mob);
        }

        [Fact]
        public async Task NKoth_Mystic_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 12845);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Mobiles\\{mob.Name}", mob);
        }

        [Fact]
        public async Task Dire_Hydra_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 13848);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Mobiles\\{mob.Name}", mob);
        }

        [Fact]
        public async Task Werewolf_Patriarch_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 13655);
            var mob = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Mobiles\\{mob.Name}", mob);
        }

        [Fact]
        public async Task Centaur_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci => ci.Identity == 2005);
            var centaur = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Mobiles\\{centaur.Name}", centaur);
        }

        [Fact]
        public async Task Dwarf_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault(ci=>ci.Identity == 2006);
            var dwarf = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);
            await this.structureService.SaveAssembledModelAsync($"Mobiles\\{dwarf.Name}", dwarf);
        }
    }
}