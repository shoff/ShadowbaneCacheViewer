namespace CacheViewer.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using EntityFramework.BulkInsert.Extensions;
    using Extensions;
    using Factories;

    public class RenderInfoSaveEventArgs : EventArgs
    {
        public int Count { get; set; }
    }

    public class RenderInfoDatabaseService
    {
        private readonly RenderInformationFactory renderInformationFactory = RenderInformationFactory.Instance;
        public event EventHandler<RenderInfoSaveEventArgs> RendersSaved;


        public async Task SaveToDatabaseAsync()
        {
            List<RenderEntity> entities = new List<RenderEntity>();

            var save = 0;
            foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
            {
                save++;
                // await this.renderInformationFactory.RenderArchive.SaveToFile(index, folder);
                var render = this.renderInformationFactory.Create(index.Identity, index.Order, true);
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
                    MeshId = render.MeshId,
                    Order = render.Order,
                    Position = $"{render.Position.X}-{render.Position.Y}-{render.Position.Z}",
                    TextureId = render.TextureId,
                    UncompressedSize = (int)render.CacheIndex.UnCompressedSize
                };
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

            using (var context = new DataContext())
            {
                context.ExecuteCommand("delete from dbo.RenderEntities");
                await context.BulkInsertAsync(entities);
            }
        }
    }
}