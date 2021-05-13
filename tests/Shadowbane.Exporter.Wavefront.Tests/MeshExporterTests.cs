namespace Shadowbane.Exporter.Wavefront.Tests
{
    using System.Threading.Tasks;
    using Cache;
    using Cache.IO;
    using Xunit;

    public class MeshExporterTests : ExporterBaseTest
    {
        [Fact]
        public async Task Charred_Corpse3_Exports_Correctly()
        {
            // 5001031
            var modelDirectory = $"{CacheLocation.SimpleFolder}{5001031}";
            //var information = RenderableObjectBuilder.Build(5001030);
            // information.Mesh?.ApplyPosition();
            //await MeshExporter.ExportAsync(information.Mesh, modelDirectory);

            var asset = ArchiveLoader.MeshArchive[5001031];
            var meshBuilder = new MeshBuilder();
            var mesh = meshBuilder.SaveRawMeshData(asset.Asset, 5001031);
            await MeshExporter.ExportAsync(mesh, modelDirectory);
        }

        [Fact]
        public async Task Hair_Meshes_Export_Correctly()
        {
            // 6500
            var modelDirectory = $"{CacheLocation.SimpleFolder}{6500}";
            var information = RenderableObjectBuilder.Build(6500);
            information.Mesh?.ApplyPosition();
            await MeshExporter.ExportAsync(information.Mesh, modelDirectory);
        }

        [Fact]
        public async Task Meshes_With_Multiple_Textures_Export_Correctly()
        {
            // 5000
            var modelDirectory = $"{CacheLocation.SimpleFolder}{5000}";
            var information = RenderableObjectBuilder.Build(5000);
            await MeshExporter.ExportAsync(information.Mesh, modelDirectory);
        }

        [Fact]
        public async Task RenderId_426407_Exports_Textured_Mesh()
        {
            var modelDirectory = $"{CacheLocation.SimpleFolder}{426407}";
            var information = RenderableObjectBuilder.Build(426407);
            await MeshExporter.ExportAsync(information.Mesh, modelDirectory);
        }
    }
}