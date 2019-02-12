namespace CacheViewer.Domain.Services.Prefabs
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using Exporters;
    using Factories;
    using Models;
    using Models.Exportable;
    using NLog;

    public class StructureService
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly List<RenderEntity> renderEntities = new List<RenderEntity>();
        private readonly List<MeshEntity> meshEntities = new List<MeshEntity>();
        private readonly IPrefabObjExporter meshExporter;
        private string folder = AppDomain.CurrentDomain.BaseDirectory + "Assembled\\{0}";

        public StructureService(IPrefabObjExporter prefabObjExporter = null)
        {
            this.meshExporter = prefabObjExporter ?? new PrefabObjExporter();
        }

        public async Task SaveAssembledModelAsync(string saveFolder, ICacheObject cacheObject)
        {
            this.folder = string.Format(this.folder, saveFolder);
            if (!Directory.Exists(this.folder))
            {
                Directory.CreateDirectory(this.folder);
            }

            var sb = new StringBuilder();

            foreach (var r in cacheObject.Renders)
            {
                sb.AppendLine(
                    $"CacheIndexIdentity: {r.CacheIndex.Identity}, HasMesh: {r.HasMesh}, MeshId: {r.MeshId}, " +
                    $"HasTexture: {r.HasTexture}, TextureIds: {string.Concat(r.Textures, ",")}");

                File.WriteAllText($"{this.folder}\\RenderEntities.txt", sb.ToString());
            }
            // the mesh should already have the texture associated to it before it ever gets here.
            this.meshExporter.ModelDirectory = this.folder;

            // let's try combining them :)
            var meshModels = new List<Mesh>();
            foreach (var mesh in this.meshEntities)
            {
                if (mesh == null)
                {
                    continue;
                }
                var cindex = MeshFactory.Instance.Indexes.FirstOrDefault(c => c.Identity == mesh.CacheIndexIdentity);
                var m = MeshFactory.Instance.Create(cindex);

                // experimenting here!
                m.ApplyPosition();

                foreach (var rt in mesh.Textures)
                {
                    var tex = TextureFactory.Instance.Build(rt.TextureId);
                    m.Textures.Add(tex);
                }
                meshModels.Add(m);
            }
            await this.meshExporter.CreatePrefabAsync(meshModels, cacheObject.Name.Replace(" ", ""));
        }

        public async Task SaveAllAsync(string saveFolder, string name, ObjectType objectType, bool saveAsOneFile)
        {
            this.folder = string.Format(this.folder, saveFolder);
            if (!Directory.Exists(this.folder))
            {
                Directory.CreateDirectory(this.folder);
            }

            var sb = new StringBuilder();

            using (var context = new SbCacheViewerContext())
            {
                var indexes = (
                    from c in context.CacheObjectEntities
                        .Include(r => r.RenderEntities)
                    where c.Name == name && c.ObjectType == objectType
                    select c).FirstOrDefault();

                if (indexes != null)
                {

                    this.renderEntities.AddRange(indexes?.RenderEntities);

                    foreach (var r in this.renderEntities)
                    {
                        sb.AppendLine(
                            $"RenderEntityId: {r.RenderEntityId}, CacheIndexIdentity: " +
                            $"{r.CacheIndexIdentity}, HasMesh: {r.HasMesh}, MeshId: {r.MeshId}, " +
                            $"HasTexture: {r.HasTexture}, TextureId: {r.TextureId}");

                        var m = (from x in context.MeshEntities.Include(rte => rte.Textures)
                                 where x.CacheIndexIdentity == r.MeshId
                                 select x).FirstOrDefault();

                        if (m != null)
                        {
                            this.meshEntities.Add(m);
                        }

                        File.WriteAllText($"{this.folder}\\RenderEntities.txt", sb.ToString());
                    }
                    await this.AssociateTexturesAsync(context);
                }
            }

            this.meshExporter.ModelDirectory = this.folder;

            // let's try combining them :)
            var meshModels = new List<Mesh>();
            foreach (var mesh in this.meshEntities)
            {
                if (mesh == null)
                {
                    continue;
                }
                var cindex = MeshFactory.Instance.Indexes.FirstOrDefault(c => c.Identity == mesh.CacheIndexIdentity);
                var m = MeshFactory.Instance.Create(cindex);

                foreach (var rt in mesh.Textures)
                {
                    var tex = TextureFactory.Instance.Build(rt.TextureId);
                    m.Textures.Add(tex);
                }
                meshModels.Add(m);
            }

            if (saveAsOneFile)
            {

                await this.meshExporter.CreatePrefabAsync(meshModels, name.Replace(" ", ""));
            }
            else
            {
                await this.meshExporter.CreatePrefabIndividualFiles(meshModels, name.Replace(" ", ""));
            }
        }

        private async Task AssociateTexturesAsync(SbCacheViewerContext context)
        {
            foreach (var re in this.renderEntities)
            {
                if (re.HasMesh && re.MeshId > 0 && re.HasTexture && re.TextureId > 0)
                {
                    // var rt = this.renderTextures.FirstOrDefault(x => x.RenderId == re.CacheIndexIdentity);
                    // var rte = context.RenderTextures.FirstOrDefault(i => i.RenderTextureId == rt.RenderTextureId && i.TextureId == re.TextureId);
                    //this.renderTextures.FirstOrDefault(x => x.RenderId == re.CacheIndexIdentity);
                    //var mymesh =   this.mesheEntities.FirstOrDefault(x => x.CacheIndexIdentity == re.MeshId);

                    var mesh = this.meshEntities.FirstOrDefault(x => x.CacheIndexIdentity == re.MeshId);
                    if (mesh == null)
                    {
                        logger?.Warn(
                            $"Unable to find a mesh for the listed meshId {re.MeshId} for RenderId {re.CacheIndexIdentity}");
                    }
                    // context.MeshEntities.Include(t => t.Textures).FirstOrDefault(i => i.CacheIndexIdentity == re.MeshId);
                    var texture = context.Textures.FirstOrDefault(i => i.TextureId == re.TextureId);

                    if (texture == null)
                    {
                        logger?.Warn(
                            $"Unable to find a texture for the listed textureId {re.TextureId} for RenderId {re.CacheIndexIdentity}");
                    }

                    if (mesh != null && texture != null && !mesh.Textures.Contains(texture))
                    {
                        mesh.Textures.Add(texture);
                        mesh.TexturesCount = mesh.RenderTextures.Count;
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}