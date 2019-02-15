namespace CacheViewer.Domain.Services.Prefabs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Exporters;
    using Models;
    using Models.Exportable;
    using NLog;

    public class StructureService
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IPrefabObjExporter meshExporter;
        private readonly string folder = AppDomain.CurrentDomain.BaseDirectory + "\\Assembled\\";

        public StructureService(IPrefabObjExporter prefabObjExporter = null)
        {
            this.meshExporter = prefabObjExporter ?? new PrefabObjExporter();
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
            var meshModels = new List<Mesh>();

            foreach (var r in cacheObject.Renders)
            {
                sb.AppendLine(
                    $"CacheIndexIdentity: {r.CacheIndex.Identity}, HasMesh: {r.HasMesh}, MeshId: {r.MeshId}, " +
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
}