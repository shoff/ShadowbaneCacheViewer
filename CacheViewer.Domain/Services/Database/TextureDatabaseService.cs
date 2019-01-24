namespace CacheViewer.Domain.Services.Database
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using EntityFramework.BulkInsert.Extensions;
    using Extensions;
    using Factories;
    using Models;
    using NLog;

    public class TextureSaveEventArgs : EventArgs
    {
        public int Count { get; set; }  
    }

    public class TextureDatabaseService
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public event EventHandler<TextureSaveEventArgs> TexturesSaved;

        public async Task SaveToDatabaseAsync()
        {
            var textureFactory = TextureFactory.Instance;
            List<TextureEntity> entities = new List<TextureEntity>();

            int i = 0;
            foreach (var cacheIndex in textureFactory.Indexes)
            {
                i++;
                Texture texture = TextureFactory.Instance.Build(cacheIndex.Identity, false);
                if (texture == null)
                {
                    logger.Error(
                        $"Texture factory returned null texture for cacheIndex.Identity {cacheIndex.Identity}");
                    continue;
                }
                var entity = new TextureEntity
                {
                    Depth = texture.Depth,
                    Height = texture.Height,
                    TextureId = cacheIndex.Identity,
                    Width = texture.Width
                };

                entities.Add(entity);

                if (i == 20)
                {
                    i = 0;
                    var eventArgs = new TextureSaveEventArgs
                    {
                        Count = entities.Count
                    };
                    this.TexturesSaved.Raise(this, eventArgs);
                }
            }

            using (var context = new DataContext())
            {
                await context.BulkInsertAsync(entities);
            }
        }
    }
}