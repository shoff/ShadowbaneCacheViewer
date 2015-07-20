

namespace CacheViewer.Domain.Extensions
{
    using System.IO;
    using System.Text;
    using SlimDX;
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// Determines whether [has enough bytes left] [the specified bytes to read].
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="bytesToRead">The bytes to read.</param>
        /// <returns></returns>
        public static bool HasEnoughBytesLeft(this BinaryReader reader, uint bytesToRead)
        {
            return reader.BaseStream.Position + bytesToRead < reader.BaseStream.Length;
        }

        /// <summary>
        /// Reads to vector3.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static Vector3 ReadToVector3(this BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        /// <summary>
        /// Reads to vector2.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static Vector2 ReadToVector2(this BinaryReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }

        /// <summary>
        /// Reads the ASCII string.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="counter">The counter.</param>
        /// <returns></returns>
        public static string ReadAsciiString(this BinaryReader reader, uint counter)
        {
            byte[] byteArray = reader.ReadBytes((int)counter * 2);

            ASCIIEncoding enc = new ASCIIEncoding();
            
            string tvTemp = enc.GetString(byteArray);
            
            //remove all the \0 and trim the string
            return tvTemp.Replace("\0", "").Trim();
        }

    }
}