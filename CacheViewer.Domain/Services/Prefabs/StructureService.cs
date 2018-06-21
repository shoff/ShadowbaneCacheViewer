﻿namespace CacheViewer.Domain.Services.Prefabs
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

    public class StructureService
    {
        private readonly List<RenderEntity> renderEntities = new List<RenderEntity>();

        // private readonly List<RenderTexture> renderTextures = new List<RenderTexture>();
        private readonly List<MeshEntity> mesheEntities = new List<MeshEntity>();
        // private readonly List<TextureEntity> texures = new List<TextureEntity>();
        private readonly MeshOnlyObjExporter meshExporter;
        private readonly CacheObjectsCache cacheObjectsCache = CacheObjectsCache.Instance;
        // private readonly List<Mesh> meshes = new List<Mesh>();
        private string folder = AppDomain.CurrentDomain.BaseDirectory + "Assembled\\{0}";

        public StructureService()
        {
            this.meshExporter = new MeshOnlyObjExporter();
        }

        public async Task SaveAll(string saveFolder, string name, ObjectType objectType)
        {
            this.folder = string.Format(this.folder, saveFolder);
            if (!Directory.Exists(this.folder))
            {
                Directory.CreateDirectory(this.folder);
            }

            StringBuilder sb = new StringBuilder();
            using (var context = new DataContext())
            {
                var indexes = (
                    from c in context.CacheObjectEntities.Include(r => r.RenderAndOffsets)
                    where c.Name == name && c.ObjectType == objectType
                    select c).ToList();

                foreach (var re in indexes)
                {
                    foreach (var ro in re.RenderAndOffsets)
                    {
                        var reo = (from x in context.RenderEntities
                                   where x.CacheIndexIdentity == ro.RenderId
                                   select x).ToList();

                        this.renderEntities.AddRange(reo);
                    }
                }

                foreach (var r in this.renderEntities)
                {
                    sb.AppendLine(
                        $"RenderEntityId: {r.RenderEntityId}, CacheIndexIdentity: {r.CacheIndexIdentity}, HasMesh: {r.HasMesh}, MeshId: {r.MeshId}, HasTexture: {r.HasTexture}, TextureId: {r.TextureId}");

                    var m = (from x in context.MeshEntities.Include(rte => rte.Textures)
                             where x.CacheIndexIdentity == r.MeshId
                             select x).FirstOrDefault();

                    if (m != null)
                    {
                        this.mesheEntities.Add(m);
                    }

                    File.WriteAllText($"{this.folder}\\RenderEntities.txt", sb.ToString());
                }

                await this.AssociateTexturesAsync(context);
            }

            this.meshExporter.ModelDirectory = this.folder;
            // var i = this.cacheObjectsCache.Indexes.FirstOrDefault(x => x.Identity == 424000);
            // var cobject = this.cacheObjectsCache.CreateAndParse(i);

            // let's try combining them :)
            var meshModels = new List<Mesh>();
            foreach (var mesh in this.mesheEntities)
            {
                if (mesh == null)
                {
                    continue;
                }
                var cindex =
                    MeshFactory.Instance.Indexes.FirstOrDefault(c => c.Identity == mesh.CacheIndexIdentity);

                var m = MeshFactory.Instance.Create(cindex);

                foreach (var rt in mesh.Textures)
                {
                    var tex = TextureFactory.Instance.Build(rt.TextureId);
                    m.Textures.Add(tex);
                }

                meshModels.Add(m);
                // await this.meshExporter.ExportAsync(m);
            }

            await this.meshExporter.CreateCombinedObjectAsync(meshModels, name.Replace(" ", ""));
        }

        private async Task AssociateTexturesAsync(DataContext context)
        {

            foreach (var re in this.renderEntities)
            {
                if (re.HasMesh && re.MeshId > 0 && re.HasTexture && re.TextureId > 0)
                {
                    // var rt = this.renderTextures.FirstOrDefault(x => x.RenderId == re.CacheIndexIdentity);
                    // var rte = context.RenderTextures.FirstOrDefault(i => i.RenderTextureId == rt.RenderTextureId && i.TextureId == re.TextureId);
                    //this.renderTextures.FirstOrDefault(x => x.RenderId == re.CacheIndexIdentity);
                    //var mymesh =   this.mesheEntities.FirstOrDefault(x => x.CacheIndexIdentity == re.MeshId);

                    var mesh = this.mesheEntities.FirstOrDefault(x => x.CacheIndexIdentity == re.MeshId);
                    // context.MeshEntities.Include(t => t.Textures).FirstOrDefault(i => i.CacheIndexIdentity == re.MeshId);
                    var texture = context.Textures.FirstOrDefault(i => i.TextureId == re.TextureId);

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