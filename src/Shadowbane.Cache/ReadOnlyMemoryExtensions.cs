namespace Shadowbane.Cache;

using System;
using System.Buffers;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression;
using Microsoft.Toolkit.HighPerformance;

public static class ReadOnlyMemoryExtensions
{
    public static T ByteArrayToStructure<T>(this ReadOnlySpan<byte> bytes)
        where T : struct
    {
        var handle = GCHandle.Alloc(bytes.ToArray(), GCHandleType.Pinned);
        try
        {
            IntPtr ptr = handle.AddrOfPinnedObject();
            if(ptr != IntPtr.Zero)
            {
                 return (T)(Marshal.PtrToStructure(ptr, typeof(T)) 
                            ?? throw new InvalidOperationException());   
            }
        }
        finally
        {
            handle.Free();
        }
        return default(T);
    }
    public static BinaryReader CreateBinaryReaderUtf32(this ReadOnlyMemory<byte> segment, long cacheIndexOffset = 0)
    {
        var reader = new BinaryReader(segment.AsStream(), Encoding.UTF32);
        reader.BaseStream.Position = cacheIndexOffset;
        return reader;
    }

    public static BinaryReader CreateBinaryReaderUtf8(this ReadOnlyMemory<byte> segment)
    {
        var reader = new BinaryReader(segment.AsStream(), Encoding.UTF8);
        return reader;
    }
    
    public static Vector3 ToVector3(this BinaryReader reader)
    {
        return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }
   
    public static Vector2 ToVector2(this BinaryReader reader)
    {
        return new Vector2(reader.ReadSingle(), reader.ReadSingle());
    }
    
    public static string? AsciiString(this BinaryReader reader, uint counter)
    {
        if (counter < 2) // utf so must be at least 2 bytes
        {
            return null;
        }
        var byteArray = reader.ReadBytes((int)counter * 2);
        var enc = new ASCIIEncoding();
        var tvTemp = enc.GetString(byteArray);
        return tvTemp.Replace("\0", "").Trim();
    }
    public static bool CanRead(this BinaryReader reader, uint bytesToRead)
    {
        return reader.BaseStream.Position + bytesToRead <= reader.BaseStream.Length;
    }
    public static ReadOnlyMemory<byte> Compress(this ReadOnlyMemory<byte> memory)
    {
        var deflator = new Deflater();
        deflator.SetInput(memory.ToArray());
        deflator.Finish();
        var shared = ArrayPool<byte>.Shared;
        using var memoryStream = new MemoryStream(memory.Length);
        var buffer = shared.Rent(memory.Length);

        while (!deflator.IsFinished)
        {
            var count = deflator.Deflate(buffer);
            memoryStream.Write(buffer, 0, count);
        }

        shared.Return(buffer);
        return memoryStream.ToArray();
    }
    public static Span<byte> Compress(this ReadOnlySpan<byte> byteBuffer)
    {
        var deflator = new Deflater();
        deflator.SetInput(byteBuffer.ToArray());
        deflator.Finish();
        var shared = ArrayPool<byte>.Shared;
        using var memoryStream = new MemoryStream(byteBuffer.Length);
        var buffer = shared.Rent(byteBuffer.Length);

        while (!deflator.IsFinished)
        {
            var count = deflator.Deflate(buffer);
            memoryStream.Write(buffer, 0, count);
        }

        shared.Return(buffer);
        return memoryStream.ToArray();
    }
    public static Span<byte> Uncompress(this ReadOnlySpan<byte> memory, uint uncompressedSize)
    {
        var decompressor = new Inflater();
        decompressor.SetInput(memory.ToArray());
        var shared = ArrayPool<byte>.Shared;

        // CreateAndParse an expandable byte array to hold the decompressed data  
        using var memoryStream = new MemoryStream(memory.Length);

        // Decompress the data  
        var buffer = shared.Rent((int)uncompressedSize);
        while (!decompressor.IsFinished)
        {
            var count = decompressor.Inflate(buffer);
            memoryStream.Write(buffer, 0, count);
        }

        shared.Return(buffer);
        // Get the decompressed data  
        return memoryStream.ToArray();
    }
    public static ReadOnlyMemory<byte> Uncompress(this ReadOnlyMemory<byte> memory, uint uncompressedSize)
    {
        var decompressor = new Inflater();
        decompressor.SetInput(memory.ToArray());
        var shared = ArrayPool<byte>.Shared;

        // CreateAndParse an expandable byte array to hold the decompressed data  
        using var memoryStream = new MemoryStream(memory.Length);

        // Decompress the data  
        var buffer = shared.Rent((int)uncompressedSize);
        while (!decompressor.IsFinished)
        {
            var count = decompressor.Inflate(buffer);
            memoryStream.Write(buffer, 0, count);
        }

        shared.Return(buffer);
        // Get the decompressed data  
        return memoryStream.ToArray();
    }
    public static int SafeReadInt32(this BinaryReader reader)
    {
        return !reader.CanRead(4) ? 0 : reader.ReadInt32();
    }
    public static uint SafeReadUInt32(this BinaryReader reader)
    {
        return !reader.CanRead(4) ? 0 : reader.ReadUInt32();
    }
    public static Vector3 SafeReadToVector3(this BinaryReader reader)
    {
        return !reader.CanRead(12) ? new Vector3() :  new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }
    public static Vector2 SafeReadToVector2(this BinaryReader reader)
    {
        return !reader.CanRead(8) ? new Vector2() : new Vector2(reader.ReadSingle(), reader.ReadSingle());
    }

}