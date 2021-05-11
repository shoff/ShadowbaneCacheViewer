namespace Shadowbane.Cache
{
    using System;
    using System.Buffers;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using Geometry;
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
                var ptr = handle.AddrOfPinnedObject();
                return (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            finally
            {
                handle.Free();
            }
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
        public static Vector3 ReadToVector3(this BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
        public static Vector2 ReadToVector2(this BinaryReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }
        public static string ReadAsciiString(this BinaryReader reader, uint counter)
        {
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

    }
}