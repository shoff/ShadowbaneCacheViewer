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


        public async Task SaveToDatabaseAsync(int validRange)
        {
            List<CacheObjectEntity> cacheObjectEntities = new List<CacheObjectEntity>();
            List<RenderAndOffset> renderAndOffsets = new List<RenderAndOffset>();
            List<InvalidValue> invalidValues = new List<InvalidValue>();
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

                        if (renderId > 0)
                        {
                            int range = renderId > centity.CacheIndexIdentity ?
                                Math.Abs(renderId - centity.CacheIndexIdentity) :
                                Math.Abs(centity.CacheIndexIdentity - renderId);

                            if (range < validRange && Array.IndexOf(RenderInformationFactory.Instance.RenderArchive.IdentityArray, renderId) >
                                -1)
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
                            else
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
                context.ExecuteCommand("delete from dbo.RenderAndOffsets");
                context.ExecuteCommand("delete from dbo.CacheObjectEntities");
                await context.BulkInsertAsync(invalidValues);
                await context.BulkInsertAsync(cacheObjectEntities);
                await context.BulkInsertAsync(renderAndOffsets);
            }

        }

    }
}