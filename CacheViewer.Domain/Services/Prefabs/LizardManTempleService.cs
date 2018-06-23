﻿
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

    public class LizardManTempleService
    {
        private readonly List<RenderEntity> renderEntities = new List<RenderEntity>();

        // private readonly List<RenderTexture> renderTextures = new List<RenderTexture>();
        private readonly List<MeshEntity> mesheEntities = new List<MeshEntity>();
        // private readonly List<TextureEntity> texures = new List<TextureEntity>();
        private readonly MeshOnlyObjExporter meshExporter;
        private readonly CacheObjectsCache cacheObjectsCache = CacheObjectsCache.Instance;
        // private readonly List<Mesh> meshes = new List<Mesh>();
        private readonly string folder = AppDomain.CurrentDomain.BaseDirectory + "Assembled\\LizardManTemple";
        //424005
        //424011
        //424020
        //424017
        //424022
        //424004
        //424018
        //424014
        //424006
        //424009
        //424010
        //424019
        public LizardManTempleService()
        {
            if (!Directory.Exists(this.folder))
            {
                Directory.CreateDirectory(this.folder);
            }

            this.meshExporter = new MeshOnlyObjExporter();
            StringBuilder sb = new StringBuilder();
            using (var context = new DataContext())
            {
                var indexes = (
                    from c in context.CacheObjectEntities.Include(r => r.RenderAndOffsets)
                    where c.Name == "Lizardman Temple" && c.ObjectType == ObjectType.Structure
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

                    //var rt = (from x in context.RenderTextures
                    //          where x.RenderId == r.CacheIndexIdentity
                    //          select x).ToList();

                    //this.renderTextures.AddRange(rt);

                    var m = (from x in context.MeshEntities.Include(rte => rte.Textures)
                             where x.CacheIndexIdentity == r.MeshId
                             select x).FirstOrDefault();

                    if (m != null)
                    {
                        this.mesheEntities.Add(m);
                    }

                    File.WriteAllText($"{this.folder}\\RenderEntities.txt", sb.ToString());
                }

                //foreach (var text in this.renderTextures)
                //{
                //    var texture = (from t in context.Textures
                //                   where t.TextureId == text.TextureId
                //                   select t).FirstOrDefault();

                //    this.texures.Add(texture);
                //}
            }
        }

        public async Task SaveAllAsync()
        {
            await this.AssociateTexturesAsync();
            // only ones currently working correctly
            //TextureEntityId TextureId   Width Height  Depth MeshEntity_MeshEntityId
            //5410    424004  128 256 3   6629
            //5409    424003  256 256 3   6634
            //5413    424007  512 256 3   6643

            this.meshExporter.ModelDirectory = this.folder;
            // var i = this.cacheObjectsCache.Indexes.FirstOrDefault(x => x.Identity == 424000);
            // var cobject = this.cacheObjectsCache.CreateAndParse(i);

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

                await this.meshExporter.ExportAsync(m);
            }
        }

        private async Task AssociateTexturesAsync()
        {
            using (var context = new DataContext())
            {
                foreach (var re in this.renderEntities)
                {
                    if (re.HasMesh && re.MeshId > 0 && re.HasTexture && re.TextureId > 0)
                    {
                        // var rt = this.renderTextures.FirstOrDefault(x => x.RenderId == re.CacheIndexIdentity);
                        // var rte = context.RenderTextures.FirstOrDefault(i => i.RenderTextureId == rt.RenderTextureId && i.TextureId == re.TextureId);
                        //this.renderTextures.FirstOrDefault(x => x.RenderId == re.CacheIndexIdentity);
                        //var mymesh =   this.mesheEntities.FirstOrDefault(x => x.CacheIndexIdentity == re.MeshId);

                        var mesh = context.MeshEntities.Include(t => t.Textures).FirstOrDefault(i => i.CacheIndexIdentity == re.MeshId);
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
}
