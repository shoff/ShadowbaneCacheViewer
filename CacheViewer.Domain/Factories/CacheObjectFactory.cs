namespace CacheViewer.Domain.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Exceptions;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Models;
    using CacheViewer.Domain.Models.Exportable;
    using NLog;

    // TODO this is poorly named, it should just be the CObjects Cache or something
    public class CacheObjectFactory
    {
        private CObjects cobjects;
        private static readonly CacheObjectFactory instance = new CacheObjectFactory();
        private readonly long loadTime;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Prevents a default instance of the <see cref="CacheObjectFactory"/> class from being created.
        /// </summary>
        private CacheObjectFactory()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            this.cobjects = (CObjects)ArchiveFactory.Instance.Build(CacheFile.CObjects, true);

            sw.Stop();
            this.loadTime = sw.ElapsedTicks;
        }

        /// <summary>
        /// Returns a basic cache object based on the flag type
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <returns></returns>
        public async Task<ICacheObject> CreateAsync(CacheIndex cacheIndex)
        {
            return await Task.Run(() => Create(cacheIndex));
        }

        /// <summary>
        /// Creates the specified cache index.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Condition. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="IndexNotFoundException">Condition. </exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        public ICacheObject Create(CacheIndex cacheIndex)
        {
            CacheAsset asset = this.cobjects[cacheIndex.identity];

            using (BinaryReader reader = asset.Item1.CreateBinaryReaderUtf32())
            {
                // reader.skip(4); // ignore "TNLC" tag
                // ReSharper disable once UnusedVariable
                var tnlc = reader.ReadInt32();

                // 4
                ObjectType flag = (ObjectType)reader.ReadInt32();
                uint nameLength = reader.ReadUInt32();
                string name = reader.ReadAsciiString(nameLength);

                // why are we using this inner offset?
                int innerOffset = (int)reader.BaseStream.Position;

                // what are we doing with the offset here??
                // so I think this must be the bug? 
                int offset = (int)reader.BaseStream.Position + 25;

                try
                {
                    // TODO this can be optimized by passing the reader to the parse method.
                    switch (flag)
                    {

                        case ObjectType.Simple:
                            Simple simple = new Simple(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                            simple.Parse(asset.Item1);
                            return simple;

                        case ObjectType.Structure:
                            Structure structure = new Structure(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                            structure.Parse(asset.Item1);
                            return structure;

                        case ObjectType.Interactive:
                            Interactive interactive = new Interactive(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                            interactive.Parse(asset.Item1);
                            return interactive;

                        case ObjectType.Equipment:
                            Equipment equipment = new Equipment(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                            equipment.Parse(asset.Item1);
                            return equipment;

                        case ObjectType.Mobile:

                            Mobile mobile = new Mobile(cacheIndex, flag, name, offset, asset.Item1, innerOffset);

                            // mobile.Parse(asset.Item1);
                            return mobile;

                        case ObjectType.Deed:
                            DeedObject deed = new DeedObject(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                            deed.Parse(asset.Item1);
                            return deed;

                        case ObjectType.Sun:
                            return new Sun(cacheIndex, flag, name, offset, asset.Item1, innerOffset);

                        case ObjectType.Warrant:
                            Warrant warrent = new Warrant(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                            warrent.Parse(asset.Item1);
                            return warrent;

                        case ObjectType.Unknown:
                            return new UnknownObject(cacheIndex, flag, name, offset, asset.Item1, innerOffset);

                        case ObjectType.Particle:
                            return new Particle(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                    }
                }
                catch (Exception e)
                {
                    logger.ErrorException("CacheObjectFactory Exception.", e);
                    throw;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        /// <value>
        /// The indexes.
        /// </value>
        public IList<CacheIndex> Indexes
        {
            get
            {
                lock (this)
                {
                    if (this.cobjects == null)
                    {
                        // ReSharper disable once ExceptionNotDocumented
                        this.cobjects = (CObjects)ArchiveFactory.Instance.Build(CacheFile.CObjects);
                    }
                }
                return this.cobjects.CacheIndices;
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static CacheObjectFactory Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Gets the load time.
        /// </summary>
        /// <value>
        /// The load time.
        /// </value>
        public long LoadTime
        {
            get
            {
                return this.loadTime;
            }
        }
    }
}