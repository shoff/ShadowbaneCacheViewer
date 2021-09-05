namespace Shadowbane.Exporter.Wavefront.Tests
{
    using System.IO;
    using System.Threading.Tasks;
    using Cache;
    using Cache.IO;
    using Xunit;

    public class MeshExporterTests : ExporterBaseTest
    {
        [Theory]
        [InlineData(5001031)]
        [InlineData(6500)]
        [InlineData(6505)]
        [InlineData(426407)]
        public async Task Any_Mesh_Exports_Correctly(int meshId)
        {
            var modelDirectory = SetupModelDirectory(meshId);
            var asset = ArchiveLoader.MeshArchive[(uint)meshId];
            var meshBuilder = new MeshBuilder();
            var mesh = meshBuilder.SaveRawMeshData(asset.Asset, (uint)meshId);
            await MeshExporter.ExportAsync(mesh, modelDirectory);
        }

        private static string SetupModelDirectory(int id)
        {
            var modelDirectory = $"{CacheLocation.SimpleFolder}{id}";

            if (Directory.Exists(modelDirectory))
            {
                Directory.Delete(modelDirectory, true);
            }

            return modelDirectory;
        }



        [Fact] 
        public async Task Hair_Meshes_Export_Correctly()
        {
            // 6500
            var modelDirectory = SetupModelDirectory(6500);
            var information = RenderableObjectBuilder.Build(6500);
            information.Mesh?.ApplyPosition();
            await MeshExporter.ExportAsync(information.Mesh, modelDirectory);
        }
        public async Task Meshes_With_Multiple_Textures_Export_Correctly()
        {
            // 5000
            var modelDirectory = SetupModelDirectory(5000);
            var information = RenderableObjectBuilder.Build(5000);
            await MeshExporter.ExportAsync(information.Mesh, modelDirectory);
        }

        [Fact]
        public async Task RenderId_426407_Exports_Textured_Mesh()
        {
            var modelDirectory = SetupModelDirectory(426407);
            var information = RenderableObjectBuilder.Build(426407);
            await MeshExporter.ExportAsync(information.Mesh, modelDirectory);
        }
    }
}