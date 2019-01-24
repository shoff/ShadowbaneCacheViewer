namespace CacheViewer.Domain.Models
{
    using System;
    using System.IO;
    using Archive;
    using Data;
    using Exportable;
    using Extensions;
    using NLog;

    public class Simple : ModelObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Simple(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data,
            int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

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
                        logger?.Info($"Simple object model parsed render id {this.RenderId}");
                    }
                    catch (EndOfStreamException endOfStreamException)
                    {
                        logger?.Error(endOfStreamException, "Exception in Simple for CacheIndex {0}",
                            this.CacheIndex.Identity);
                        
                        this.Errors.Add(endOfStreamException);

                        throw;
                    }

                    this.UnParsedBytes = data.Count - (int) reader.BaseStream.Position;
                    logger?.Info(
                        $"{this.RenderId} of type {this.GetType().FullName} had {this.UnParsedBytes} unparsed bytes.");
                }
                catch (IOException ioException)
                {
                    logger?.Error(ioException, "Exception in Simple for CacheIndex {0}", this.CacheIndex.Identity);
                    this.Errors.Add(ioException);
                    throw;
                }

                logger?.Info("CacheIndex {0} in Simple contained {1} unparsed bytes.", this.CacheIndex.Identity,
                    this.UnParsedBytes);
            }
        }
    }
}