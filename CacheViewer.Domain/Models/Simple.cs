namespace CacheViewer.Domain.Models
{
    using System;
    using System.IO;
    using Archive;
    using CacheViewer.Data;
    using Exportable;
    using Extensions;
    using NLog;

    public class Simple : ModelObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Simple" /> class.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="name">The name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="data">The data.</param>
        /// <param name="innerOffset">The inner offset.</param>
        public Simple(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data,
            int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        /// <summary>
        ///     Parses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="System.ArgumentNullException">data</exception>
        /// <exception cref="IOException">Condition. </exception>
        public override void Parse(ArraySegment<byte> data)
        {
            using (var reader = data.CreateBinaryReaderUtf32())
            {
                try
                {
                    reader.BaseStream.Position = this.CursorOffset;

                    try
                    {
                        this.RenderId = reader.ReadUInt32();
                        logger.Info($"Simple object model parsed render id {this.RenderId}");
                    }
                    catch (EndOfStreamException endOfStreamException)
                    {
                        logger.Error(endOfStreamException, "Exception in Simple for CacheIndex {0}",
                            this.CacheIndex.Identity);
                        throw;
                    }

                    this.UnParsedBytes = data.Count - (int) reader.BaseStream.Position;
                    logger.Info(
                        $"{this.RenderId} of type {this.GetType().FullName} had {this.UnParsedBytes} unparsed bytes.");
                }
                catch (IOException ioException)
                {
                    logger?.Error(ioException, "Exception in Simple for CacheIndex {0}", this.CacheIndex.Identity);
                    throw;
                }

                logger?.Info("CacheIndex {0} in Simple contained {1} unparsed bytes.", this.CacheIndex.Identity,
                    this.UnParsedBytes);
            }
        }
    }
}