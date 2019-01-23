namespace CacheViewer.Domain.Archive
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Extensions;

    [CacheFile(CacheFile.CObjects)]
    internal sealed class CObjects : CacheArchive
    {
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

            //foreach (var ci in this.cacheIndex)
            //{
            //    int count = this.cacheIndex.Count(x => x.identity == ci.identity);
            //    Debug.Assert(count < 3);
            //}

            this.LowestId = this.CacheIndices[0].Identity;
            this.HighestId = this.CacheIndices.Last().Identity;
        } // ReSharper disable InconsistentNaming
    }
}