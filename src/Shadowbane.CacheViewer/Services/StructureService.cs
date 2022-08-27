namespace Shadowbane.CacheViewer.Services;

using System.Text;
using Cache;
using ChaosMonkey.Guards;
using Serilog;

public class StructureService : IStructureService
{
    private readonly ILogger logger;
    private readonly IPrefabObjExporter meshExporter;
    private readonly string folder = AppDomain.CurrentDomain.BaseDirectory + "\\Assembled\\";

    public StructureService(
        IPrefabObjExporter? prefabObjExporter = null)
    {
        this.meshExporter = prefabObjExporter ?? new PrefabObjExporter();
    }

    public async Task SaveAssembledModelAsync(string saveFolder, ICacheObject cacheObject, bool singleFile = false)
    {
        Guard.IsNotNull(saveFolder, nameof(saveFolder));
        Guard.IsNotNull(cacheObject, nameof(cacheObject));
        
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