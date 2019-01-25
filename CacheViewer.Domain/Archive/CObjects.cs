namespace CacheViewer.Domain.Archive
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Exceptions;
    using Extensions;
    using NLog;

    [CacheFile(CacheFile.CObjects)]
    internal sealed class CObjects : CacheArchive
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public CObjects()
            : base("CObjects.cache")
        {
        }

        public ArraySegment<byte> Data => this.bufferData;

        public ArraySegment<byte> Unzip(uint uncompressedSize, byte[] file)
        {
            return this.Decompress(uncompressedSize, file);
        }

        public override void LoadIndexes()
        {
            using (var reader = this.bufferData.CreateBinaryReaderUtf32())
            {
                // set the offset.
                reader.BaseStream.Position = this.cacheHeader.indexOffset;
                int previousIdentity = 0;
                int previousOrder = 0;
                for (var i = 0; i < this.cacheHeader.indexCount; i++)
                {
                    var index = new CacheIndex
                    {
                        Index = i,
                        Junk1 = reader.ReadUInt32(),
                        Identity = reader.ReadInt32(),
                        Offset = reader.ReadUInt32(),
                        UnCompressedSize = reader.ReadUInt32(),
                        CompressedSize = reader.ReadUInt32()
                    };

                    // so we can easily correctly determine the cache order
                    if (previousIdentity == index.Identity)
                    {
                        index.Order = ++previousOrder;
                    }
                    else
                    {
                        previousOrder = 0;
                    }

                    previousIdentity = index.Identity;
                    this.IdentityArray[i] = index.Identity;
                    this.CacheIndices[i] = index;
                }
            }
            this.LowestId = this.CacheIndices[0].Identity;
            this.HighestId = this.CacheIndices.Last().Identity;
        } // ReSharper disable InconsistentNaming

        public override CacheAsset this[int id]
        {
            get
            {
                if (id == 0)
                {
                    throw new IndexNotFoundException("no name", id);
                }

                var count = this.CacheIndices.Count(x => x.Identity == id);

                var asset = new CacheAsset
                {
                    // leave this as First, we want it to throw if the id is invalid
                    CacheIndex1 = this.CacheIndices.First(x => x.Identity == id)
                };

                // do we have two with the same id?
                if (count > 1)
                {
                    logger?.Info("{0} found {1} entries for identity {2}", this.Name, count, id);

                    // don't think there are any that have more than 3
                    // I really don't think this is necessary anymore 1/23/2019
                    Debug.Assert(count < 4);
                    for (int i = 1; i < count; i++)
                    {
                        var ci = this.CacheIndices.Where(x => x.Identity == id).Skip(i).Select(x => x).Single();
                        ci.Order = i + 1;
                        if (i == 1)
                        {
                            asset.CacheIndex2 = ci;
                        }
                        else
                        {
                            asset.CacheIndex3 = ci;
                        }
                    }
                }

                using (var reader = this.bufferData.CreateBinaryReaderUtf32())
                {
                    // ReSharper disable ExceptionNotDocumented
                    reader.BaseStream.Position = asset.CacheIndex1.Offset;
                    var buffer = reader.ReadBytes((int)asset.CacheIndex1.CompressedSize);
                    asset.Item1 = this.Decompress(asset.CacheIndex1.UnCompressedSize, buffer);

                    // hate this hack, freaking Wolfpack decided that the identity in the 
                    // render.cache didn't need to be unique... brilliant...
                    if (count > 1)
                    {
                        logger?.Debug(
                            $"Setting reader to offset{asset.CacheIndex2.Offset} to read second item with id {id}");
                        reader.BaseStream.Position = asset.CacheIndex2.Offset;
                        asset.Item2 = this.Decompress(asset.CacheIndex2.UnCompressedSize, reader.ReadBytes((int)asset.CacheIndex2.CompressedSize));
                    }

                    if (count > 2)
                    {
                        logger?.Debug(
                            $"Setting reader to offset{asset.CacheIndex2.Offset} to read third item with id {id}");
                        reader.BaseStream.Position = asset.CacheIndex3.Offset;
                        asset.Item2 = this.Decompress(asset.CacheIndex3.UnCompressedSize, reader.ReadBytes((int)asset.CacheIndex3.CompressedSize));
                    }

                    if (count > 3)
                    {
                        throw new ApplicationException(
                            "Need to refactor some twat added more than three indexes with the same id.");
                    }
                    // if there are more than 3 fuck it
                }

                return asset;
            }
        }
    }
}