namespace Shadowbane.Exporter.Wavefront.Tests
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Cache;
    using Cache.IO;
    using Models;
    using Xunit;
    using Xunit.Abstractions;

    public class ObjExporterTests : ExporterBaseTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ObjExporterTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task _Find_And_Parse()
        {
            int noChildren = 0;
            int haveChildren = 0;
            var cache = ArchiveLoader.ObjectArchive;
            foreach (var ci in cache.CacheIndices)
            {
                try
                {
                    var modelDirectory = SetupModelDirectory(ci.identity);
                    var renderableObject = RenderableObjectBuilder.Build(ci.identity);
                    if (renderableObject.ChildCount > 0 &&
                        renderableObject.Children.Count == renderableObject.ChildCount)
                    {
                        haveChildren++;
                    }
                    else
                    {
                        noChildren++;
                    }

                    await ObjExporter.ExportAsync(renderableObject, modelDirectory, default);
                }
                catch (Exception e)
                {
                   //testOutputHelper.WriteLine(e.Message);
                }
            }

            testOutputHelper.WriteLine(noChildren.ToString());
            testOutputHelper.WriteLine(haveChildren.ToString());
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