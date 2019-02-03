namespace CacheViewer.Domain.Services.Database
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using EntityFramework.BulkInsert.Extensions;
    using Extensions;
    using Factories;
    using NLog;

    public class CacheObjectSaveEventArgs : EventArgs
    {
        public int CacheObjectsCount { get; set; }
        public int RenderOffsetsCount { get; set; }

    }

    public class CacheObjectsDatabaseService
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly CacheObjectFactory cacheObjectFactory = CacheObjectFactory.Instance;
        public event EventHandler<CacheObjectSaveEventArgs> CacheObjectsSaved;
        private readonly bool saveInvalidData;

        private static readonly Dictionary<ObjectType, string> objectTypeDictionary = new Dictionary<ObjectType, string>
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

        public CacheObjectsDatabaseService()
        {
            var configValue = ConfigurationManager.AppSettings["SaveInvalidData"];
            bool.TryParse(configValue, out this.saveInvalidData);
        }

        public async Task SaveToDatabaseAsync(int validRange)
        {
            var cacheObjectEntities = new List<CacheObjectEntity>();
            var renderAndOffsets = new List<RenderAndOffset>();
            var invalidValues = new List<InvalidValue>();
            var save = 0;

            foreach (var i in this.cacheObjectFactory.Indexes)
            {
                save++;
                var cobject = this.cacheObjectFactory.CreateAndParse(i);
                var centity = new CacheObjectEntity
                {
                    CacheIndexIdentity = cobject.CacheIndex.Identity,
                    CompressedSize = (int)cobject.CacheIndex.CompressedSize,
                    FileOffset = (int)cobject.CacheIndex.Offset,
                    Name = cobject.Name,
                    ObjectType = cobject.Flag,
                    ObjectTypeDescription = objectTypeDictionary[cobject.Flag],
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
                        if (renderId > 0 && renderId < 8000101) // 8000100 is Regal Painting
                        {
                            int range = renderId > centity.CacheIndexIdentity ?
                                // if this renderId is greater that the cacheObject id subtract the cacheObject id from the render id
                                Math.Abs(renderId - centity.CacheIndexIdentity) :
                                // otherwise subtract the render id from cacheObject id
                                Math.Abs(centity.CacheIndexIdentity - renderId);

                            // if this value is within an acceptable range and is an known render id.
                            if (range <= validRange && Array.IndexOf(RenderInformationFactory.Instance.RenderArchive.IdentityArray, renderId) > -1)
                            {
                                logger?.Debug($"Found render id {renderId} for {centity.CacheIndexIdentity}");

                                var rao = new RenderAndOffset
                                {
                                    RenderId = renderId,
                                    OffSet = reader.BaseStream.Position,
                                    CacheIndexId = centity.CacheIndexIdentity
                                };

                                renderAndOffsets.Add(rao);
                            }
                            else if (this.saveInvalidData)
                            {
                                var iv = new InvalidValue
                                {
                                    RenderId = renderId,
                                    OffSet = reader.BaseStream.Position,
                                    CacheIndexId = centity.CacheIndexIdentity
                                };
                                invalidValues.Add(iv);
                            }
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

            using (var context = new SbCacheViewerContext())
            {
                context.ExecuteCommand("delete from dbo.InvalidValues");
                context.ExecuteCommand("DBCC CHECKIDENT ('InvalidValues', RESEED, 1)");

                context.ExecuteCommand("delete from dbo.RenderAndOffsets");
                context.ExecuteCommand("DBCC CHECKIDENT ('RenderAndOffsets', RESEED, 1)");

                context.ExecuteCommand("delete from dbo.CacheObjectEntities");
                context.ExecuteCommand("DBCC CHECKIDENT ('CacheObjectEntities', RESEED, 1)");

                await context.BulkInsertAsync(invalidValues);
                await context.BulkInsertAsync(cacheObjectEntities);
                await context.BulkInsertAsync(renderAndOffsets);

                if (RenderInfoObjectsSaved)
                {
                    int count = 0;
                    int total = 0;
                    var grouped = renderAndOffsets.GroupBy(g => new { g.CacheIndexId }).ToList();

                    foreach (var child in grouped)
                    {
                        var cacheId = child.First().CacheIndexId;

                        var cacheEntity = (from c in context.CacheObjectEntities
                            where c.CacheIndexIdentity == cacheId
                            select c).First();

                        foreach (var ch in child)
                        {
                            var render = (from r in context.RenderEntities
                                where r.CacheIndexIdentity == ch.RenderId
                                select r).First();
                            render.CacheObjectEntities.Add(cacheEntity);
                        }

                        total++;
                        count++;
                        if (count == 20)
                        {
                            count = 0;
                            var eventArgs = new CacheObjectSaveEventArgs
                            {
                                CacheObjectsCount = total
                            };
                            this.CacheObjectsSaved.Raise(this, eventArgs);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        public static bool RenderInfoObjectsSaved { get; set; }
    }
}