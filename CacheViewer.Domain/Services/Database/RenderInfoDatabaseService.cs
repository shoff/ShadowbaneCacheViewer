namespace CacheViewer.Domain.Services.Database
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using Extensions;
    using Factories;
    using NLog;

    public class RenderInfoSaveEventArgs : EventArgs
    {
        public int Count { get; set; }
    }

    public class RenderInfoDatabaseService
    {
        private readonly RenderInformationFactory renderInformationFactory = RenderInformationFactory.Instance;
        public event EventHandler<RenderInfoSaveEventArgs> RendersSaved;

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private static readonly int[] ignores =  { 20 };

        public async Task SaveToDatabaseAsync()
        {
            using (var context = new SbCacheViewerContext())
            {
                context.ExecuteCommand("delete from dbo.RenderChildren");
                context.ExecuteCommand("DBCC CHECKIDENT ('RenderChildren', RESEED, 1)");

                context.ExecuteCommand("delete from dbo.RenderEntities");
                context.ExecuteCommand("DBCC CHECKIDENT ('RenderEntities', RESEED, 1)");

                var save = 0;
                foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
                {
                    if (Array.IndexOf(ignores, index.Identity) > -1)
                    {
                        // crap like sun, moon rain puddle etc that we really don't give a shit about.
                        continue;
                    }

                    logger?.Debug($"Now creating render information for {index.Identity}");

                    try
                    {
                        save++;
                        var entity = BuildEntity(index.Identity, index.Order);
                        if (entity.HasMesh && entity.MeshId == 0)
                        {
                            entity.InvalidData = true;
                        }

                        if (entity.HasTexture && entity.TextureId == 0)
                        {
                            entity.InvalidData = true;
                        }

                        context.RenderEntities.Add(entity);

                        if (save == 20)
                        {
                            await context.SaveChangesAsync();
                            save = 0;
                            var eventArgs = new RenderInfoSaveEventArgs
                            {
                                Count = context.RenderEntities.Count()
                            };
                            this.RendersSaved.Raise(this, eventArgs);
                        }
                    }
                    catch (Exception e)
                    {
                        logger?.Error(e, e.Message);
                        throw;
                    }
                }

                // await context.BulkInsertAsync(entities);
            }

            //using (var context = new SbCacheViewerContext())
            //{
            //    var grouped = children.GroupBy(g => new { g.ParentId }).ToList();

            //    int count = 0;
            //    int total = 0;

            //    foreach (var child in grouped)
            //    {
            //        total++;
            //        var renderId = child.First().ParentId;

            //        var render = (from r in context.RenderEntities
            //            where r.CacheIndexIdentity == renderId
            //                      select r).First();

            //        foreach(var ch in child)
            //        {
            //            render.Children.Add(new RenderChild
            //            {
            //                ParentId = ch.ParentId,
            //                ChildRenderId = ch.ChildRenderId
            //            });

            //            var childRender = (from c in context.RenderEntities
            //                where c.CacheIndexIdentity == ch.ChildRenderId
            //                select c).First();

            //            render.ChildRenderEntities.Add(childRender);
            //        }

            //        count++;
            //        if (count == 20)
            //        {
            //            count = 0;
            //            var eventArgs = new RenderInfoSaveEventArgs
            //            {
            //                Count = total
            //            };
            //            this.RendersSaved.Raise(this, eventArgs);
            //            await context.SaveChangesAsync();
            //        }
            //    }

            //    await context.SaveChangesAsync();
            //}

            CacheObjectsDatabaseService.RenderInfoObjectsSaved = true;
        }

        private RenderEntity BuildEntity(int renderId, int order)
        {
            try
            {
                var render = this.renderInformationFactory.Create(renderId, order, true);
                var entity = new RenderEntity
                {
                    ByteCount = render.ByteCount,
                    CacheIndexIdentity = render.CacheIndex.Identity,
                    CompressedSize = (int)render.CacheIndex.CompressedSize,
                    FileOffset = (int)render.CacheIndex.Offset,
                    HasMesh = render.HasMesh,
                    HasTexture = render.HasTexture,
                    RenderCount = render.ChildCount,
                    JointName = render.JointName,
                    MeshId = render.MeshId > 0 ? (int?)render.MeshId : null,
                    Order = render.Order,
                    Position = $"{render.Position.X:0.###}-{render.Position.Y:0.###}-{render.Position.Z:0.###}",
                    UncompressedSize = (int)render.CacheIndex.UnCompressedSize,
                };

                foreach (var id in render.ChildRenderIdList)
                {
                    // possibly dangerous here. we'll see
                    entity.ChildRenderEntities.Add(this.BuildEntity(id, 0));

                    var renderChild = new RenderChild
                    {
                        ChildRenderId = id,
                        ParentId = render.CacheIndex.Identity
                    };

                    if (renderChild.IsValidId())
                    {
                        entity.Children.Add(renderChild);
                    }
                    else
                    {
                        logger?.Warn(
                            $"Invalid range or id detected for child of reder object {renderChild.ParentId} invalid child is {renderChild.ChildRenderId}");
                    }
                }

                return entity;
            }
            catch (Exception e)
            {
                logger?.Error(e, e.Message);
                throw;
            }
        }
    }
}