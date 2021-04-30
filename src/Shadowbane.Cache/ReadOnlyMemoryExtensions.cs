namespace Shadowbane.Cache
{
    using System;
    using System.Buffers;
    using System.IO;
    using System.Text;
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using Microsoft.Toolkit.HighPerformance;

    public static class ReadOnlyMemoryExtensions
    {
        public static BinaryReader CreateBinaryReaderUtf32(this ReadOnlyMemory<byte> segment, long cacheIndexOffset)
        {
            var reader = new BinaryReader(segment.AsStream(), Encoding.UTF32);
            reader.BaseStream.Position = cacheIndexOffset;
            return reader;
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

        public static ReadOnlyMemory<byte> Uncompress(this ReadOnlyMemory<byte> memory, uint uncompressedSize)
        {
            var decompressor = new Inflater();
            decompressor.SetInput(memory.ToArray()); 
            var shared = ArrayPool<byte>.Shared;

            // CreateAndParse an expandable byte array to hold the decompressed data  
            using var memoryStream = new MemoryStream(memory.Length);

            // Decompress the data  
            var buffer = shared.Rent((int) uncompressedSize);
            while (!decompressor.IsFinished)
            {
                var count = decompressor.Inflate(buffer);
                memoryStream.Write(buffer, 0, count);
            }

            shared.Return(buffer);
            // Get the decompressed data  
            return memoryStream.ToArray();
        }
    }
}