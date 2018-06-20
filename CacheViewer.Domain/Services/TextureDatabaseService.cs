namespace CacheViewer.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Extensions;
    using Data;
    using Data.Entities;
    using EntityFramework.BulkInsert.Extensions;
    using Factories;
    using Models;

    public class TextureSaveEventArgs : EventArgs
    {
        public int Count { get; set; }  
    }

    public class TextureDatabaseService
    {
        public event EventHandler<TextureSaveEventArgs> TexturesSaved;

        public async Task SaveToDatabase()
        {
            var textureFactory = TextureFactory.Instance;
            List<TextureEntity> entities = new List<TextureEntity>();

            int i = 0;
            foreach (var cacheIndex in textureFactory.Indexes)
            {
                i++;
                Texture texture = TextureFactory.Instance.Build(cacheIndex.Identity, false);
                //context.Textures.Add(texture);
                var entity = new TextureEntity
                {
                    Depth = texture.Depth,
                    Height = texture.Height,
                    TextureId = cacheIndex.Identity,
                    Width = texture.Width
                };
                entities.Add(entity);
                if (i == 100)
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