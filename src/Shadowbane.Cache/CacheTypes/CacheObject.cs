namespace Shadowbane.Cache.CacheTypes
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class CacheObject : CacheArchive
    {

        public CacheObject()
            : base("CObjects.cache".AsMemory())
        {
        }

        public ReadOnlyMemory<byte> Data => this.bufferData;
        public override CacheAsset this[int id]
        {
            get
            {
                if (id == 0)
                {
                    throw new IndexNotFoundException("no name", id);
                }

                //var count = this.CacheIndices.Count(x => x.Identity == id);
                var cacheIndex = this.CacheIndices.First(x => x.Identity == id);

                //// do we have two with the same id?
                //if (count > 1)
                //{
                //    // logger?.Info("{0} found {1} entries for identity {2}", this.Name, count, id);

                //    // don't think there are any that have more than 3
                //    // I really don't think this is necessary anymore 1/23/2019
                //    Debug.Assert(count < 4);
                //    for (int i = 1; i < count; i++)
                //    {
                //        var ci = this.cacheIndices.Where(x => x.Identity == id).Skip(i).Select(x => x).Single();
                //        //ci.Order = i + 1;
                //        if (i == 1)
                //        {
                //            //  asset.CacheIndex2 = ci;
                //        }
                //        else
                //        {
                //            //asset.CacheIndex3 = ci;
                //        }
                //    }
                //}

                using var reader = this.bufferData.CreateBinaryReaderUtf32(cacheIndex.Offset);
                var buffer = new ReadOnlyMemory<byte>(reader.ReadBytes((int)cacheIndex.CompressedSize));
                var asset = new CacheAsset(cacheIndex, this.Decompress(cacheIndex.UnCompressedSize, buffer));

                // hate this hack, freaking Wolfpack decided that the identity in the 
                // render.cache didn't need to be unique... brilliant...
                //if (count > 1)
                //    {
                //        logger?.Debug(
                //            $"Setting reader to offset{asset.CacheIndex2.Offset} to read second item with id {id}");
                //        reader.BaseStream.Position = asset.CacheIndex2.Offset;
                //        asset.Item2 = this.Decompress(asset.CacheIndex2.UnCompressedSize, reader.ReadBytes((int)asset.CacheIndex2.CompressedSize));
                //    }

                //    if (count > 2)
                //    {
                //        logger?.Debug(
                //            $"Setting reader to offset{asset.CacheIndex2.Offset} to read third item with id {id}");
                //        reader.BaseStream.Position = asset.CacheIndex3.Offset;
                //        asset.Item2 = this.Decompress(asset.CacheIndex3.UnCompressedSize, reader.ReadBytes((int)asset.CacheIndex3.CompressedSize));
                //    }

                //    if (count > 3)
                //    {
                //        throw new ApplicationException(
                //            "Need to refactor some twat added more than three indexes with the same id.");
                //    }
                // if there are more than 3 fuck it


                return asset;
            }
        }
    }
}