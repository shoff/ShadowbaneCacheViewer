namespace CacheViewer.Domain.Services.Database
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Extensions;
    using NLog;
    public class TextureMeshEventArgs : EventArgs
    {
        public int Count { get; set; }
    }
    public class AssociateTexturesDatabaseService
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public event EventHandler<TextureMeshEventArgs> TextureMeshSaved;

        public async Task AssociateTextures()
        {
            using (var context = new SbCacheViewerContext())
            {
                var renderEntities = (from r in context.RenderEntities select r).ToList();
                var cacheObjects = (from c in context.CacheObjectEntities.Include(r => r.RenderAndOffsets) select c)
                    .ToList();
                int count = 0;
                int total = 0;
                foreach (var cacheObject in cacheObjects)
                {
                    foreach (var renderAndOffset in cacheObject.RenderAndOffsets)
                    {
                        var re =
                            (from r in renderEntities where r.CacheIndexIdentity == renderAndOffset.RenderId select r)
                            .FirstOrDefault();

                        if (re == null)
                        {
                            // wtf?
                            logger?.Error($"Could not find a renderEntity for id {renderAndOffset.RenderId}");
                            continue;
                        }

                        if (re.HasMesh && re.MeshId > 0 && re.HasTexture && re.TextureId > 0)
                        {
                            // var rt = this.renderTextures.FirstOrDefault(x => x.RenderId == re.CacheIndexIdentity);
                            // var rte = context.RenderTextures.FirstOrDefault(i => i.RenderTextureId == rt.RenderTextureId && i.TextureId == re.TextureId);
                            //this.renderTextures.FirstOrDefault(x => x.RenderId == re.CacheIndexIdentity);
                            //var mymesh =   this.mesheEntities.FirstOrDefault(x => x.CacheIndexIdentity == re.MeshId);

                            var mesh = context.MeshEntities.Include(t => t.Textures)
                                .FirstOrDefault(i => i.CacheIndexIdentity == re.MeshId);

                            var texture = context.Textures.FirstOrDefault(i => i.TextureId == re.TextureId);

                            if (mesh != null && texture != null && !mesh.Textures.Contains(texture))
                            {
                                mesh.Textures.Add(texture);
                                mesh.TexturesCount = mesh.RenderTextures.Count;
                                count++;
                                total++;
                            }
                        }

                        if (count == 20)
                        {
                            count = 0;
                            var eventArgs1 = new TextureMeshEventArgs
                            {
                                Count = total
                            };

                            this.TextureMeshSaved.Raise(this, eventArgs1);
                            await context.SaveChangesAsync();
                        }
                    }
                }

                var eventArgs2 = new TextureMeshEventArgs
                {
                    Count = total
                };

                this.TextureMeshSaved.Raise(this, eventArgs2);
                await context.SaveChangesAsync();
            }
        }

    }
}