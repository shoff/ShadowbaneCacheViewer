namespace CacheViewer.Domain.Services
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using Factories;
    using Models;
    using NLog;

    public class RealTimeModelService
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public async Task<List<Mesh>> GenerateModelAsync(int cacheObjectIdentity)
        {
            var renderEntities = new List<RenderEntity>();
            var meshEntities = new List<MeshEntity>();

            using (var context = new SbCacheViewerContext())
            {
                var indexes = await (
                    from c in context.CacheObjectEntities.Include(r => r.RenderAndOffsets)
                    where c.CacheIndexIdentity == cacheObjectIdentity
                    select c).ToListAsync();

                foreach (var re in indexes)
                {
                    foreach (var ro in re.RenderAndOffsets)
                    {
                        var reo = await (from x in context.RenderEntities
                                   where x.CacheIndexIdentity == ro.RenderId
                                   select x).ToListAsync();

                        renderEntities.AddRange(reo);
                    }
                }

                foreach (var r in renderEntities)
                {

                    var m = await (from x in context.MeshEntities.Include(rte => rte.Textures)
                             where x.CacheIndexIdentity == r.MeshId
                             select x).FirstOrDefaultAsync();

                    if (m != null)
                    {
                        meshEntities.Add(m);
                    }
                }
            }

            var meshModels = new List<Mesh>();
            foreach (var mesh in meshEntities)
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

            logger?.Info(
                $"GenerateModelAsync for {cacheObjectIdentity} returned mesh collection of {meshModels.Count}.");
            return meshModels;
        }
    }
}