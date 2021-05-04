#pragma warning disable IDE0032 // Use auto property
namespace Shadowbane.Cache
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;

    public abstract class CacheArchive : IDisposable
    {
        protected ReadOnlyMemory<byte> bufferData;
        protected FileInfo fileInfo;
        protected CacheHeader cacheHeader;
        protected string name;
        protected string saveName;
        protected readonly List<CacheIndex> cacheIndices = new List<CacheIndex>();
        private readonly IMemoryOwner<byte> memoryOwner;
        private long indexOffset;

        protected CacheArchive(string name)
        {
            this.InstanceId = Guid.NewGuid();
            this.memoryOwner = MemoryPool<byte>.Shared.Rent();
            this.name = name;
            this.fileInfo = new FileInfo(Path.Combine(CacheLocation.CacheFolder.FullName, this.name));
            this.bufferData = new ReadOnlyMemory<byte>(File.ReadAllBytes(this.fileInfo.FullName));
        }

        ~CacheArchive()
        {
            this.Dispose(false);
        }

        internal ReadOnlySpan<byte> Decompress(uint uncompressedSize, ReadOnlySpan<byte> memory)
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

        public virtual CacheArchive LoadIndexes()
        {
            // using var reader = this.bufferData.CreateBinaryReaderUtf32(this.indexOffset);
            var cacheIndexSize = Marshal.SizeOf<CacheIndex>();
            for (var i = 0; i < this.cacheHeader.indexCount; i++)
            {
                var indexData = this.bufferData.Span.Slice((int) (this.indexOffset + cacheIndexSize * i), cacheIndexSize);
                var index = indexData.ByteArrayToStructure<CacheIndex>();
                this.cacheIndices.Add(index);
            }

            if (this.cacheIndices.Any()) // dungeon has none
            {
                this.LowestId = (int) this.cacheIndices.First().identity;
                this.HighestId = (int) this.cacheIndices.Last().identity;
            }

            // reader.Close(); // shouldn't need to do this
            return this;
        }
        public virtual CacheArchive LoadCacheHeader()
        {
            // TODO convert this to just use the extension
            using var reader = this.bufferData.CreateBinaryReaderUtf32(0);
            // fill in the CacheHeader struct for this file.
            // number of entries in this stream?
            this.cacheHeader.indexCount = reader.ReadUInt32();
            this.cacheHeader.dataOffset = reader.ReadUInt32();
            this.cacheHeader.fileSize = reader.ReadUInt32();
            this.cacheHeader.junk1 = reader.ReadUInt32();

            // check if this file size is correct
            if ((int) this.cacheHeader.fileSize != this.fileInfo.Length)
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

            this.indexOffset = reader.BaseStream.Position;
            reader.Close();
            return this;
        }
        public virtual CacheAsset this[uint id]
        {
            // TODO figure out if passing the ReadOnlyMemory<byte> here is really better than say
            // TODO having a shared ArrayPool<byte> and renting/returning a simply byte[]
            get
            {
                if (id == 0 || this.cacheIndices.All(i => i.identity != id))
                {
                    throw new IndexNotFoundException("no name", id);
                }
                // these "identities" are in fact duped, it could be either a male/female thing or a "versioning" strategy..
                var cacheIndex = this.cacheIndices.First(x => x.identity == id);
                var buffer = this.bufferData.Span.Slice((int)cacheIndex.offset, (int) cacheIndex.compressedSize);
                var asset = new CacheAsset(cacheIndex, this.Decompress(cacheIndex.unCompressedSize, buffer).ToArray());
                return asset;
            }
        }
        internal IReadOnlyCollection<CacheIndex> CacheIndices => this.cacheIndices.AsReadOnly();
        public int HighestId { get; private set; }
        public int LowestId { get; private set; }
        public string Name => this.name;
        public long IndexOffset => this.indexOffset;
        public int IndexCount => this.cacheIndices.Count;
        public uint DataOffset => this.cacheHeader.dataOffset;
        public DateTime CreationDate => this.fileInfo.CreationTime;
        public DateTime LastWriteTime => this.fileInfo.LastWriteTime;
        public Guid InstanceId { get; }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {

        }
    }
}
#pragma warning restore IDE0032 // Use auto property
/*
https://docs.microsoft.com/en-us/dotnet/standard/memory-and-spans/memory-t-usage-guidelines
Owners, consumers, and lifetime management
Since buffers can be passed around between APIs, and since buffers can sometimes be accessed from 
multiple threads, it's important to consider lifetime management. There are three core concepts:

Ownership. The owner of a buffer instance is responsible for lifetime management, 
including destroying the buffer when it's no longer in use. All buffers have a single owner. 
Generally the owner is the component that created the buffer or that received the buffer from a 
factory. 

Ownership can also be transferred; Component-A can relinquish control of the buffer to 
Component-B, at which point Component-A may no longer use the buffer, and Component-B becomes
responsible for destroying the buffer when it's no longer in use.

Consumption. The consumer of a buffer instance is allowed to use the buffer instance by reading 
from it and possibly writing to it. Buffers can have one consumer at a time unless some external 
synchronization mechanism is provided. The active consumer of a buffer isn't necessarily the buffer's owner.

Lease. The lease is the length of time that a particular component is allowed to be the consumer of the buffer.
*/