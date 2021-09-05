namespace Shadowbane.Exporter.Wavefront.Tests
{
    using System.IO;
    using System.Threading.Tasks;
    using Cache;
    using Cache.IO;
    using Models;
    using Xunit;

    public class ObjExporterTests : ExporterBaseTest
    {
        [Fact]
        public async Task _Find_And_Parse()
        {
            var cache = ArchiveLoader.ObjectArchive;
            foreach (var ci in cache.CacheIndices)
            {
                var cacheIndex = cache[ci.identity];
                var modelDirectory = SetupModelDirectory(ci.identity);
                var renderableObject = RenderableObjectBuilder.Build(ci.identity);
                await ObjExporter.ExportAsync(renderableObject, modelDirectory, default);
            }
        }

        [Theory]
        [InlineData(5000)]
        [InlineData(5001)]
        [InlineData(5002)]
        [InlineData(5004)]
        public async Task _DoesIt(uint id)
        {
            var modelDirectory = SetupModelDirectory(id);
            var information = RenderableObjectBuilder.Build((uint)id);
            await ObjExporter.ExportAsync(information, modelDirectory, default);
        }


        private static string SetupModelDirectory(uint id)
        {
            var modelDirectory = $"{CacheLocation.SimpleFolder}{id}";

            if (Directory.Exists(modelDirectory))
            {
                Directory.Delete(modelDirectory, true);
            }

            return modelDirectory;
        }
    }
}