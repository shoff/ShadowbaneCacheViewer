namespace CacheViewer.Domain.Extensions
{
    using System;
    using System.IO;
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using NLog;
    using Utility;

    // http://stackoverflow.com/questions/14352480/high-memory-issues-in-net-framework-4-but-not-in-framework-4-5
    public static class StreamExtensions
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Uncompresses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static byte[] Uncompress(this byte[] input)
        {
            var decompressor = new Inflater();
            decompressor.SetInput(input);

            // Create an expandable byte array to hold the decompressed data  
            using (var bos = new MemoryStream(input.Length))
            {
                // Decompress the data  
                var buf = new byte[1024];
                while (!decompressor.IsFinished)
                {
                    var count = decompressor.Inflate(buf);
                    bos.Write(buf, 0, count);
                }

                // Get the decompressed data  
                return bos.ToArray();
            }
        }


        /// <summary>
        ///     Decompresses a zip file or stream
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The value of 'buffer' cannot be null. </exception>
        public static byte[] UnZip(this byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (buffer.Length == 0)
            {
                return null;
            }

            byte[] uncompressedArray;

            using (var mem = new MemoryStream(buffer))
            {
                // to hold the output
                // to send to inflator
                using (var memBuffer = new MemoryStream())
                {
                    // inflate the memory stream
                    using (var inf = new InflaterInputStream(mem))
                    {
                        try
                        {
                            // not sure why I'm getting a null from the buffer pool
                            var result = BufferPool.Instance.CheckOut(1024) ?? new byte[1024];

                            int resLen;
                            while ((resLen = inf.Read(result, 0, result.Length)) > 0)
                            {
                                memBuffer.Write(result, 0, resLen);
                            }

                            inf.Close();
                            uncompressedArray = memBuffer.ToArray();
                            BufferPool.Instance.CheckIn(result);
                        }
                        catch (Exception e)
                        {
                            logger.Error(e, e.Message);
                            throw;
                        }
                    }
                }
            }

            return uncompressedArray;
        }
    }
}