namespace Shadowbane.Cache.IO.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Exporter.File;
    using Models;
    using Shadowbane.Exporter.Wavefront;
    using Xunit;

    public class CacheObjectBuilderTests
    {
        private readonly CacheObjectBuilder builder;

        public CacheObjectBuilderTests()
        {
            this.builder = new CacheObjectBuilder();
        }

        [Fact]
        public async Task Simple_Models_Parse_Correctly()
        {
            foreach (var index in ArchiveLoader.ObjectArchive.CacheIndices)
            {
                var asset = ArchiveLoader.ObjectArchive[index.identity];
                using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
                var flag = (ObjectType)reader.ReadInt32();

                if (flag == ObjectType.Simple)
                {
                    var simple = this.builder.CreateAndParse(index.identity);
                    if (simple != null)
                    {
                        var modelDirectoryName = simple.Name ?? simple.Identity.ToString();
                        var modelDirectory = $"{CacheLocation.SimpleFolder}{modelDirectoryName}";
                        foreach (var render in simple.Renders.Where(r => r.HasMesh && r.MeshId > 0 && r.Mesh != null))
                        {
                            try
                            {
                                await MeshExporter.ExportAsync(render.Mesh, modelDirectory, simple.Name);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }
            }
        }
    }
}