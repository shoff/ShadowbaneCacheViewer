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

    public class MeshSaveEventArgs : EventArgs
    {
        public int Count { get; set; }
    }

    public class MeshDatabaseService
    {
        public event EventHandler<MeshSaveEventArgs> MeshesSaved;

        public async Task SaveToDatabaseAsync()
        {
            int count = 0;
            List<MeshEntity> entities = new List<MeshEntity>();
            foreach (var index in MeshFactory.Instance.Indexes)
            {
                count++;
                var mesh = MeshFactory.Instance.Create(index);

                var ent = new MeshEntity
                {
                    CacheIndexIdentity = mesh.CacheIndex.Identity,
                    CompressedSize = (int)mesh.CacheIndex.CompressedSize,
                    NormalsCount = (int)mesh.NormalsCount,
                    FileOffset = (int)mesh.CacheIndex.Offset,
                    Id = mesh.Id,
                    Normals = string.Join(";", mesh.Normals.Map(v => $"{v.X}:{v.Y}:{v.Z}").ToArray()),
                    TexturesCount = mesh.Textures?.Count() ?? 0,
                    TextureVectors = string.Join(";", mesh.TextureVectors.Map(v => $"{v.X}:{v.Y}").ToArray()),
                    UncompressedSize = (int)mesh.CacheIndex.UnCompressedSize,
                    VertexCount = mesh.Vertices?.Count ?? 0,
                    Vertices = string.Join(";", mesh.Vertices.Map(v => $"{v.X}:{v.Y}:{v.Z}").ToArray())
                };

                entities.Add(ent);
                mesh = null;

                //var texture = (from r in context.RenderEntities
                //               join t in context.Textures on r.TextureId equals t.TextureId
                //               where r.MeshId == ent.Id && r.HasTexture && r.TextureId > 0
                //               select t).FirstOrDefault();

                //if (texture != null)
                //{
                //    ent.Textures.Add(texture);
                //}

                //context.MeshEntities.Add(ent);
                if (count == 20)
                {
                    count = 0;
                    var eventArgs = new MeshSaveEventArgs
                    {
                        Count = entities.Count
                    };
                    this.MeshesSaved.Raise(this, eventArgs);
                }
            }

            using (var context = new DataContext())
            {
                context.ExecuteCommand("delete from dbo.MeshEntities");
                await context.BulkInsertAsync(entities);
            }
        }
    }
}