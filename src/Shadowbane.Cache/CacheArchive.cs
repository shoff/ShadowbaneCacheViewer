// ReSharper disable ArrangeAccessorOwnerBody
#pragma warning disable IDE0032 // Use auto property
namespace Shadowbane.Cache;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

public abstract class CacheArchive
{
    protected ReadOnlyMemory<byte> bufferData;
    private readonly FileInfo fileInfo;
    private CacheHeader cacheHeader;
    private readonly string name;

    protected readonly Dictionary<uint, CacheIndex> indices = new();
    //protected readonly List<CacheIndex> cacheIndices = new();
    private long indexOffset;

    protected const uint DUPE_FLAG = 900000; // is this big enough?
    
    protected CacheArchive(string name)
    {
        this.InstanceId = Guid.NewGuid();
        this.name = name;
        this.fileInfo = new FileInfo(Path.Combine(CacheLocation.CacheFolder.FullName, this.name));
        this.bufferData = new ReadOnlyMemory<byte>(File.ReadAllBytes(this.fileInfo.FullName));
        this.Header();
        this.Indexes();
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

    private void Indexes()
    {
        var cacheIndexSize = Marshal.SizeOf<CacheIndex>();

        for (var i = 0; i < this.cacheHeader.indexCount; i++)
        {
            var indexData = this.bufferData.Span.Slice((int)(this.indexOffset + cacheIndexSize * i), cacheIndexSize);
            var index = indexData.ByteArrayToStructure<CacheIndex>();
            if (this.indices.ContainsKey(index.identity))
            {
                this.indices.Add(index.identity+DUPE_FLAG, index);
            }
            else
            {
                this.indices.Add(index.identity, index);
            }   
            // this.cacheIndices.Add(index);
        }

        if (this.indices.Any()) // dungeon has none
        {
            this.LowestId = this.indices.First().Key;
            this.HighestId = this.indices.Last().Key;
        }
    }

    private void Header()
    {
        // TODO convert this to just use the extension
        using var reader = this.bufferData.CreateBinaryReaderUtf32();
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

        this.indexOffset = reader.BaseStream.Position;
        reader.Close();
    }

    public virtual CacheAsset? this[uint id]
    {
        // TODO figure out if passing the ReadOnlyMemory<byte> here is really better than say
        // TODO having a shared ArrayPool<byte> and renting/returning a simply byte[]
        get
        {
            if (id == 0 || !this.indices.ContainsKey(id))
            {
                return null;
            }
            // these "identities" are in fact duped, but the underlying data is ALWAYS identical so not sure why they duped them
            var cacheIndex = this.indices[id];
            var buffer = this.bufferData.Span.Slice((int)cacheIndex.offset, (int)cacheIndex.compressedSize);
            var asset = new CacheAsset(cacheIndex, this.Decompress(cacheIndex.unCompressedSize, buffer).ToArray());
            return asset;
        }
    }

    public IReadOnlyCollection<CacheIndex> CacheIndices
    {
        get
        {
            // ReSharper disable once ArrangeAccessorOwnerBody
            return indices.Values.ToList().AsReadOnly();
        }
    }

    public uint HighestId { get; protected set; }

    public uint LowestId { get; protected set; }

    public string Name
    {
        get { return this.name; }
    }

    public int IndexCount
    {
        get { return this.indices.Count; }
    }

    public uint DataOffset
    {
        get { return this.cacheHeader.dataOffset; }
    }

    public Guid InstanceId { get; }

    public CacheHeader CacheHeader
    {
        get => this.cacheHeader;
        set => this.cacheHeader = value;
    }

    public abstract CacheArchive Validate();

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