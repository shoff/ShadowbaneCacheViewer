namespace CacheViewer.Domain.Extensions
{
    using System;
    using System.IO;
    using System.Text;
    using Nito.ArraySegments;

    public static class ArraySegmentExtensions
    {
        /// <summary>
        ///     Creates the binary reader ut F32.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <returns></returns>
        public static BinaryReader CreateBinaryReaderUtf32(this ArraySegment<byte> segment)
        {
            return new BinaryReader(segment.CreateStream(false), Encoding.UTF32);
        }
    }
}