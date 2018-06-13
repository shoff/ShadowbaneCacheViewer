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

        public static string ToHexString(this ArraySegment<byte> segment, string prefix)
        {
            char[] lookup = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            if (segment.Array == null)
            {
                return "";
            }

            int i = 0, p = prefix.Length, l = segment.Array.Length;
            char[] c = new char[l * 2 + p];
            byte d;
            for (; i < p; ++i)
            {
                c[i] = prefix[i];
            }

            i = -1;
            --l;
            --p;
            while (i < l)
            {
                d = segment.Array[++i];
                c[++p] = lookup[d >> 4];
                c[++p] = lookup[d & 0xF];
            }
            return new string(c, 0, c.Length);
        }
    }
}