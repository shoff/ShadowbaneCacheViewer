namespace Shadowbane.CacheViewer.Services;

using System.Text;
using Cache;
using Serilog;
public interface IPrefabObjExporter
{
    Task CreateIndividualObjFiles(ICollection<IMesh>? meshModels, string modelName);
    Task CreateSingleObjFile(ICollection<IMesh>? meshModels, string modelName);
    string ModelDirectory { get; set; }
}
public class StructureService
{
    private readonly ILogger logger;
    private readonly IPrefabObjExporter meshExporter;
    private readonly string folder = AppDomain.CurrentDomain.BaseDirectory + "\\Assembled\\";

    public StructureService(
        ILogger logger,
        IPrefabObjExporter prefabObjExporter = null)
    {
        this.logger = logger;
        this.meshExporter = prefabObjExporter ?? new PrefabObjExporter(this.logger);
    }

    public async Task SaveAssembledModelAsync(string saveFolder, ICacheObject cacheObject, bool singleFile = false)
    {
        var projectFolder = $"{this.folder}{saveFolder}";
        if (!Directory.Exists(projectFolder))
        {
            Directory.CreateDirectory(projectFolder);
        }

        var sb = new StringBuilder();
        // the mesh should already have the texture associated to it before it ever gets here.
        this.meshExporter.ModelDirectory = projectFolder;

        // let's try combining them :)
        var meshModels = new List<IMesh>();

        foreach (var r in cacheObject.Renders)
        {
            sb.AppendLine(
                $"CacheIndexIdentity: {r.CacheIndex.identity}, HasMesh: {r.HasMesh}, MeshId: {r.MeshId}, " +
                $"HasTexture: {r.HasTexture}, TextureIds: {string.Concat(r.Textures, ",")}");

            File.WriteAllText($"{projectFolder}\\RenderEntities.txt", sb.ToString());

            if (r.Mesh == null || !r.HasMesh || r.MeshId == 0)
            {
                continue;
            }

            // experimenting here!
            r.Mesh.ApplyPosition();
            meshModels.Add(r.Mesh);
        }

        if (singleFile)
        {
            await this.meshExporter.CreateSingleObjFile(meshModels, cacheObject.Name.Replace(" ", ""));
        }
        else
        {
            await this.meshExporter.CreateIndividualObjFiles(meshModels, cacheObject.Name.Replace(" ", ""));
        }
    }
}