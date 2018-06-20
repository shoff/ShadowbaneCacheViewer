namespace CacheViewer.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using EntityFramework.BulkInsert.Extensions;
    using Extensions;
    using Factories;

    public class CacheObjectSaveEventArgs : EventArgs
    {
        public int CacheObjectsCount { get; set; }
        public int RenderOffsetsCount { get; set; }

    }

    public class CacheObjectsDatabaseService
    {

        private readonly CacheObjectsCache cacheObjectsCache = CacheObjectsCache.Instance;
        public event EventHandler<CacheObjectSaveEventArgs> CacheObjectsSaved;

        private static readonly Dictionary<ObjectType, string> objectTypeDicitonary = new Dictionary<ObjectType, string>
        {
            {ObjectType.Sun, "Sun"},
            {ObjectType.Simple, "Simple"},
            {ObjectType.Structure, "Structure"},
            {ObjectType.Interactive, "Interactive"},
            {ObjectType.Equipment, "Equipment"},
            {ObjectType.Mobile, "Mobile"},
            {ObjectType.Deed, "Deed"},
            {ObjectType.Warrant, "Warrant"},
            {ObjectType.Unknown, "Unknown"},
            {ObjectType.Particle, "Particle"}
        };


        public async Task SaveToDatabase()
        {
            List<CacheObjectEntity> cacheObjectEntities = new List<CacheObjectEntity>();
            List<RenderAndOffset> renderAndOffsets = new List<RenderAndOffset>();


            var save = 0;
            foreach (var i in this.cacheObjectsCache.Indexes)
            {
                save++;

                var cobject = this.cacheObjectsCache.CreateAndParse(i);
                var centity = new CacheObjectEntity
                {
                    CacheIndexIdentity = cobject.CacheIndex.Identity,
                    CompressedSize = (int)cobject.CacheIndex.CompressedSize,
                    FileOffset = (int)cobject.CacheIndex.Offset,
                    Name = cobject.Name,
                    ObjectType = cobject.Flag,
                    ObjectTypeDescription = objectTypeDicitonary[cobject.Flag],
                    UncompressedSize = (int)cobject.CacheIndex.UnCompressedSize

                };
                cacheObjectEntities.Add(centity);

                var structure = cobject;
                using (var reader = structure.Data.CreateBinaryReaderUtf32())
                {
                    reader.BaseStream.Position = 57; // this is common to all cache files and doesn't contain any render ids

                    while (reader.BaseStream.Position + 4 <= structure.Data.Count)
                    {
                        int renderId = reader.ReadInt32();

                        int range = renderId > centity.CacheIndexIdentity ?
                            Math.Abs(renderId - centity.CacheIndexIdentity) :
                            Math.Abs(centity.CacheIndexIdentity - renderId);

                        if (range < 5000 && Array.IndexOf(RenderInformationFactory.Instance.RenderArchive.IdentityArray, renderId) > -1)
                        {
                            var rao = new RenderAndOffset
                            {
                                RenderId = renderId,
                                OffSet = reader.BaseStream.Position,
                                CacheIndexId = centity.CacheIndexIdentity
                            };
                            renderAndOffsets.Add(rao);
                        }
                        reader.BaseStream.Position -= 3;
                    }
                }

                if (save == 20)
                {
                    save = 0;
                    var eventArgs = new CacheObjectSaveEventArgs
                    {
                        CacheObjectsCount = cacheObjectEntities.Count,
                        RenderOffsetsCount = renderAndOffsets.Count
                    };
                    this.CacheObjectsSaved.Raise(this, eventArgs);
                }
            }

            using (var context = new DataContext())
            {
                context.ExecuteCommand("delete from dbo.RenderAndOffsets");
                context.ExecuteCommand("delete from dbo.CacheObjectEntities");
                await context.BulkInsertAsync(cacheObjectEntities);
                await context.BulkInsertAsync(renderAndOffsets);
            }

        }

    }
}