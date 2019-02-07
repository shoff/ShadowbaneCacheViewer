namespace CacheViewer.Domain.Services.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using EntityFramework.BulkInsert.Extensions;
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

        public async Task SaveToDatabaseAsync()
        {
            var children = new List<RenderChild>();

            using (var context = new SbCacheViewerContext())
            {
                context.ExecuteCommand("delete from dbo.RenderChildren");
                context.ExecuteCommand("DBCC CHECKIDENT ('RenderChildren', RESEED, 1)");

                context.ExecuteCommand("delete from dbo.RenderEntities");
                context.ExecuteCommand("DBCC CHECKIDENT ('RenderEntities', RESEED, 1)");

                var entities = new List<RenderEntity>();

                var save = 0;
                foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
                {
                    logger?.Debug($"Now creating render information for {index.Identity}");

                    try
                    {
                        save++;
                        // read the data from the render cache file.
                        var render = this.renderInformationFactory.Create(index.Identity, index.Order, true);
                        var entity = new RenderEntity
                        {
                            ByteCount = render.ByteCount,
                            CacheIndexIdentity = render.CacheIndex.Identity,
                            CompressedSize = (int) render.CacheIndex.CompressedSize,
                            FileOffset = (int) render.CacheIndex.Offset,
                            HasMesh = render.HasMesh,
                            HasTexture = render.HasTexture,
                            RenderCount = render.ChildCount,
                            JointName = render.JointName,
                            MeshId = render.MeshId > 0 ? (int?) render.MeshId : null,
                            Order = render.Order,
                            Position = $"{render.Position.X:0.###}-{render.Position.Y:0.###}-{render.Position.Z:0.###}",
                            UncompressedSize = (int) render.CacheIndex.UnCompressedSize
                        };

                        foreach (var id in render.ChildRenderIdList)
                        {
                            var renderChild = new RenderChild
                            {
                                ChildRenderId = id,
                                ParentId = render.CacheIndex.Identity
                            };

                            if (renderChild.IsValidId())
                            {
                                children.Add(renderChild);
                            }
                            else
                            {
                                logger?.Warn(
                                    $"Invalid range or id detected for child of reder object {renderChild.ParentId} invalid child is {renderChild.ChildRenderId}");
                            }
                        }

                        if (entity.HasMesh && entity.MeshId == 0)
                        {
                            entity.InvalidData = true;
                        }

                        if (entity.HasTexture && entity.TextureId == 0)
                        {
                            entity.InvalidData = true;
                        }

                        entities.Add(entity);

                        if (save == 20)
                        {
                            save = 0;
                            var eventArgs = new RenderInfoSaveEventArgs
                            {
                                Count = entities.Count()
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

                await context.BulkInsertAsync(entities);
            }

            using (var context = new SbCacheViewerContext())
            {
                var grouped = children.GroupBy(g => new { g.ParentId }).ToList();

                int count = 0;
                int total = 0;

                foreach (var child in grouped)
                {
                    total++;
                    var renderId = child.First().ParentId;

                    var render = (from r in context.RenderEntities
                        where r.CacheIndexIdentity == renderId
                                  select r).First();

                    foreach(var ch in child)
                    {
                        render.Children.Add(new RenderChild
                        {
                            ParentId = ch.ParentId,
                            ChildRenderId = ch.ChildRenderId
                        });

                        var childRender = (from c in context.RenderEntities
                            where c.CacheIndexIdentity == ch.ChildRenderId
                            select c).First();

                        render.ChildRenderEntities.Add(childRender);
                    }

                    count++;
                    if (count == 20)
                    {
                        count = 0;
                        var eventArgs = new RenderInfoSaveEventArgs
                        {
                            Count = total
                        };
                        this.RendersSaved.Raise(this, eventArgs);
                        await context.SaveChangesAsync();
                    }
                }

                await context.SaveChangesAsync();
            }

            CacheObjectsDatabaseService.RenderInfoObjectsSaved = true;
        }

        // todo make range configurable
    }
}