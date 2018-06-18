namespace CacheViewer.Tests.Domain.Assembled
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Data;
    using CacheViewer.Domain.Data.Entities;
    using CacheViewer.Domain.Exporters;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models;
    using NUnit.Framework;

    [TestFixture]
    public class ElvenChurchTests
    {
        private readonly List<RenderEntity> renderEntities = new List<RenderEntity>();
        private readonly List<CacheObjectEntity> indexes;
        private readonly List<RenderTexture> renderTextures = new List<RenderTexture>();
        private readonly List<MeshEntity> mesheEntities = new List<MeshEntity>();
        private readonly List<TextureEntity> texures = new List<TextureEntity>();
        private readonly MeshOnlyObjExporter meshExporter;
        private readonly CacheObjectsCache cacheObjectsCache = CacheObjectsCache.Instance;
        private readonly List<Mesh> meshes = new List<Mesh>();
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
        public ElvenChurchTests()
        {
            this.meshExporter = new MeshOnlyObjExporter();

            using (var context = new DataContext())
            {
                this.indexes = (
                    from c in context.CacheObjectEntities.Include(r => r.RenderAndOffsets)
                    where c.Name == "Elven Church" && c.ObjectType == CacheViewer.Domain.Models.ObjectType.Structure
                    select c).ToList();

                foreach (var re in this.indexes)
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
                    var rt = (from x in context.RenderTextures
                              where x.RenderId == r.CacheIndexIdentity
                              select x).ToList();
                    this.renderTextures.AddRange(rt);

                    var m = (from x in context.MeshEntities.Include(rte => rte.RenderTextures)
                             where x.CacheIndexIdentity == r.MeshId
                             select x).FirstOrDefault();
                    if (m != null)
                    {
                        this.mesheEntities.Add(m);
                    }
                }

                foreach (var text in this.renderTextures)
                {
                    var texture = (from t in context.Textures
                                   where t.TextureId == text.TextureId
                                   select t).FirstOrDefault();

                    this.texures.Add(texture);
                }
            }
        }

        [Test, Explicit]
        public void Find_Elven_Church_Structure()
        {
            using (var context = new DataContext())
            {
                foreach (var re in this.renderEntities)
                {
                    if (re.HasMesh)
                    {
                        var rt = this.renderTextures.FirstOrDefault(x => x.RenderId == re.CacheIndexIdentity);
                        var rte = context.RenderTextures.FirstOrDefault(i => i.RenderTextureId == rt.RenderTextureId);
                        //this.renderTextures.FirstOrDefault(x => x.RenderId == re.CacheIndexIdentity);
                        //var mymesh =   this.mesheEntities.FirstOrDefault(x => x.CacheIndexIdentity == re.MeshId);
                        var mesh = context.MeshEntities.FirstOrDefault(i => i.CacheIndexIdentity == re.MeshId);

                        if (mesh != null)
                        {
                            mesh.RenderTextures.Add(rte);
                            mesh.TexturesCount = mesh.RenderTextures.Count;

                            context.SaveChanges();
                        }
                    }
                }
            }
        }


        [Test, Explicit]
        public async Task SaveAll()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory + "Assembled\\ElvenChurch";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            this.meshExporter.ModelDirectory = folder;
            using (var context = new DataContext())
            {
                var i = this.cacheObjectsCache.Indexes.FirstOrDefault(x => x.Identity == 424000);
                var cobject = this.cacheObjectsCache.CreateAndParse(i);

                //if (cobject.Flag == ObjectType.Structure)
                //{
                var centity = (from c in context.CacheObjectEntities
                               where c.CacheIndexIdentity == cobject.CacheIndex.Identity
                               select c).AsNoTracking().FirstOrDefault();

                // foreach (var re in centity.RenderAndOffsets)
            }

            foreach (var mesh in this.mesheEntities)
            {
                if (mesh == null)
                {
                    continue;
                }
                var cindex =
                    MeshFactory.Instance.Indexes.FirstOrDefault(c => c.Identity == mesh.CacheIndexIdentity);

                var m = MeshFactory.Instance.Create(cindex);

                foreach (var rt in mesh.RenderTextures)
                {
                    var tex = TextureFactory.Instance.Build(rt.TextureId, true);
                    m.Textures.Add(tex);
                }


                await this.meshExporter.ExportAsync(m);
            }
        }
    }
}