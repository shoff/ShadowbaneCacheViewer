namespace CacheViewer.Tests.Domain.Assembled
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CacheViewer.Data;
    using CacheViewer.Data.Entities;
    using CacheViewer.Domain.Exporters;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models;
    using NUnit.Framework;

    [TestFixture]
    public class ConfessorShrineTests
    {
        private readonly List<RenderEntity> renderEntities = new List<RenderEntity>();
        private readonly List<CacheObjectEntity> indexes;
        private readonly List<RenderTexture> renderTextures = new List<RenderTexture>();
        private readonly List<MeshEntity> mesheEntities = new List<MeshEntity>();
        private readonly List<TextureEntity> texures = new List<TextureEntity>();
        private readonly MeshOnlyObjExporter meshExporter;
        private readonly CacheObjectFactory cacheObjectFactory = CacheObjectFactory.Instance;
        private readonly List<Mesh> meshes = new List<Mesh>();

        public ConfessorShrineTests()
        {
            this.meshExporter = new MeshOnlyObjExporter();

            using (var context = new DataContext())
            {
                this.indexes = (
                    from c in context.CacheObjectEntities.Include(r => r.RenderAndOffsets)
                    where c.Name == "Confessor Shrine" && c.ObjectType == ObjectType.Interactive
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
        public async Task SaveAll()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory + "Assembled\\ConfessorShrine";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            this.meshExporter.ModelDirectory = folder;
            //using (var context = new DataContext())
            //{
            //    var i = this.cacheObjectFactory.Indexes.FirstOrDefault(x => x.Identity == 424000);
            //    var cobject = this.cacheObjectFactory.CreateAndParse(i);

            //    //if (cobject.Flag == ObjectType.Structure)
            //    //{
            //    var centity = (from c in context.CacheObjectEntities
            //        where c.CacheIndexIdentity == cobject.CacheIndex.Identity
            //        select c).AsNoTracking().FirstOrDefault();

            //    // foreach (var re in centity.RenderAndOffsets)
            //}

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