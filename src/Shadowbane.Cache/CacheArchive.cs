namespace Shadowbane.Cache
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.MemoryMappedFiles;
    using System.Linq;
    using System.Runtime.InteropServices;

    public abstract class CacheArchive
    {
        protected ReadOnlyMemory<byte> bufferData;
        protected FileInfo fileInfo;
        protected CacheHeader cacheHeader;
        protected ReadOnlyMemory<char> name;
        protected string saveName;
        protected readonly List<CacheIndex> cacheIndices = new List<CacheIndex>();

        protected CacheArchive(ReadOnlyMemory<char> name)
        {
            this.name = name;
            this.fileInfo = new FileInfo(Path.Combine(CacheLocation.CacheFolder.FullName, this.name.Span.ToString()));
            this.bufferData = new ReadOnlyMemory<byte>(File.ReadAllBytes(this.fileInfo.FullName));
        }

        protected ReadOnlyMemory<byte> Decompress(uint uncompressedSize, ReadOnlyMemory<byte> memory)
        {
            if (memory.Length == uncompressedSize)
            {
                return memory;
            }
            var item = memory.Uncompress(uncompressedSize);
            if (item.Length != uncompressedSize)
            {
                throw new InvalidCompressionSizeException
                    ($"Index raw size should be {uncompressedSize} , but was {item.Length}");
            }
            return item;
        }

        public virtual CacheArchive LoadWithMemoryMappedFile()
        {
            using var mmf = MemoryMappedFile.CreateFromFile(this.fileInfo.FullName, FileMode.Open, null);
            //var length = this.cacheHeader.dataOffset - this.cacheHeader.indexOffset;
            int cacheIndexSize = Marshal.SizeOf(typeof(CacheIndex));
            var length = this.cacheHeader.indexCount * cacheIndexSize;
            using var accessor = mmf.CreateViewAccessor(this.cacheHeader.indexOffset, length);
            // we shouldn't need to trust the cacheHeader's word about the number of indices available but
            // on at least the tile cache the offset to data does not line up with the cache indices count.

            for (long i = 0; i < length; i += cacheIndexSize)
            {
                accessor.Read(i, out CacheIndex cacheIndex);
                this.cacheIndices.Add(cacheIndex);
            }

            if (this.cacheIndices.Any()) // dungeon has none
            {
                this.LowestId = (int) this.cacheIndices.First().Identity;
                this.HighestId = (int) this.cacheIndices.Last().Identity;
            }

            return this;
        }

        public virtual CacheArchive LoadIndexes()
        {
            using var reader = this.bufferData.CreateBinaryReaderUtf32(this.cacheHeader.indexOffset);
            for (var i = 0; i < this.cacheHeader.indexCount; i++)
            {
                var index = new CacheIndex
                {
                    Junk1 = reader.ReadUInt32(),
                    Identity = reader.ReadUInt32(), 
                    Offset = reader.ReadUInt32(),
                    UnCompressedSize = reader.ReadUInt32(),
                    CompressedSize = reader.ReadUInt32()
                };
                this.cacheIndices.Add(index);
            }
            if (this.cacheIndices.Any()) // dungeon has none
            {
                this.LowestId = (int)this.cacheIndices.First().Identity;
                this.HighestId = (int)this.cacheIndices.Last().Identity;
            }
            return this;
        }
        public virtual CacheArchive LoadCacheHeader()
        {
            using var reader = this.bufferData.CreateBinaryReaderUtf32(0);
            // fill in the CacheHeader struct for this file.
            // number of entries in this stream?
            this.cacheHeader.indexCount = reader.ReadUInt32();
            this.cacheHeader.dataOffset = reader.ReadUInt32();
            this.cacheHeader.fileSize = reader.ReadUInt32();
            this.cacheHeader.junk1 = reader.ReadUInt32();

            // check if this file size is correct
            if ((int)this.cacheHeader.fileSize != this.fileInfo.Length)
            {
                var length = "0";
                if (this.fileInfo.Exists)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    length = this.fileInfo.Length.ToString();
                }

                throw new HeaderFileSizeException(
                    $"{this.Name} Header states file should be {this.cacheHeader.fileSize} in size, but FileInfo object reported {length} as actual size.");
            }

            this.cacheHeader.indexOffset = reader.BaseStream.Position;
            // this.IdentityArray = new int[this.cacheHeader.indexCount];
            // this.CacheIndices = new CacheIndex[this.cacheHeader.indexCount];
            //logger?.Info($"Creating identityArray for {this.name} with {this.cacheHeader.indexCount} indeces.");
            //logger?.Info($"{this.name} had junk UInt32 in cache header with value {this.cacheHeader.junk1}");
            return this;
        }
        public virtual CacheAsset this[int id]
        {
            // TODO figure out if passing the ReadOnlyMemory<byte> here is really better than say
            // TODO having a shared ArrayPool<byte> and renting/returning a simply byte[]
            get
            {
                if (id == 0 || this.cacheIndices.All(i => i.Identity != id))
                {
                    throw new IndexNotFoundException("no name", id);
                }
                var cacheIndex = this.cacheIndices.First(x => x.Identity == id);
                using var reader = this.bufferData.CreateBinaryReaderUtf32(cacheIndex.Offset);
                var buffer = new ReadOnlyMemory<byte>(reader.ReadBytes((int)cacheIndex.CompressedSize));
                var asset = new CacheAsset(cacheIndex, this.Decompress(cacheIndex.UnCompressedSize, buffer));
                return asset;
            }
        }
        public IReadOnlyCollection<CacheIndex> CacheIndices => this.cacheIndices.AsReadOnly();
        public int HighestId { get; private set; }
        public int LowestId { get; private set; }
        public ReadOnlyMemory<char> Name { get; }

        public int IndexCount => this.cacheIndices.Count;
    }
}