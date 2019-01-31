namespace CacheViewer.Domain.Services.Database
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using EntityFramework.BulkInsert.Extensions;
    using Extensions;
    using Factories;
    using Factories.Providers;
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
            using (var context = new SbCacheViewerContext())
            {
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
                            MeshId = render.MeshId > 0 ? (int?)render.MeshId : null,
                            Order = render.Order,
                            Position = $"{render.Position.X:0.###}-{render.Position.Y:0.###}-{render.Position.Z:0.###}",
                            UncompressedSize = (int) render.CacheIndex.UnCompressedSize
                        };

                        foreach (var texture in render.Textures)
                        {
                            var textureEntity = context.Textures.FirstOrDefault(t => t.TextureId == texture);
                            if (textureEntity != null)
                            {
                                entity.TextureEntities.Add(textureEntity);
                            }
                            else
                            {
                                throw new ApplicationException($"RenderEntity {render.CacheIndex.Identity} " +
                                    $"parsed a texture id of {texture} but that is not a known textureId. Perhaps these should be render id children?");
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
                                Count = entities.Count
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
                
                context.ExecuteCommand("delete from dbo.RenderEntities");
                context.ExecuteCommand("DBCC CHECKIDENT ('RenderEntities', RESEED, 1)");

                await context.BulkInsertAsync(entities);
            }
        }
    }
}