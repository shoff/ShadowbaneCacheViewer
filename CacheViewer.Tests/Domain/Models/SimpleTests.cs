namespace CacheViewer.Tests.Domain.Models
{
    using System.Linq;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models;
    using Xunit;

    public class SimpleTests
    {
        private readonly CacheObjectFactory cacheObjectFactory = CacheObjectFactory.Instance;

        [Fact]
        public async Task CacheId_142000_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.First(x => x.Identity == 973400);
            var simple = await this.cacheObjectFactory.CreateAndParseAsync(cacheIndex);

        }
    }
}