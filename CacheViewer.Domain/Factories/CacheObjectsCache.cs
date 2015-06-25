namespace CacheViewer.Domain.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Exceptions;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Models;
    using CacheViewer.Domain.Models.Exportable;
    using NLog;

    // TODO this is poorly named, it should just be the CObjects Cache or something
    public class CacheObjectsCache
    {
        private CObjects cobjects;
        private static readonly CacheObjectsCache instance = new CacheObjectsCache();
        private readonly long loadTime;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Prevents a default instance of the <see cref="CacheObjectsCache"/> class from being created.
        /// </summary>
        private CacheObjectsCache()
        {
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            this.cobjects = (CObjects)ArchiveFactory.Instance.Build(CacheFile.CObjects, true);

#if DEBUG
            sw.Stop();
            this.loadTime = sw.ElapsedTicks;
#else
            this.loadTime = 0;
#endif
        }

        /// <summary>Returns a basic cache object based on the flag type</summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Condition. </exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        /// <exception cref="IndexNotFoundException">Condition. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public async Task<ICacheObject> CreateAsync(CacheIndex cacheIndex)
        {
            Contract.Ensures(Contract.Result<Task<ICacheObject>>() != null);
            return await Task.FromResult(Create(cacheIndex));
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
            CacheAsset asset = this.cobjects[cacheIndex.Identity];

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
                    logger.Error(e);
                    throw;
                }
            }

            return null;
        }

        /// <summary>Gets the indexes.</summary>
        public IList<CacheIndex> Indexes
        {
            get { return this.cobjects.CacheIndices; }
        }

        /// <summary>Gets the instance.</summary>
        public static CacheObjectsCache Instance
        {
            get
            {
                Contract.Ensures(Contract.Result<CacheObjectsCache>() != null);
                Contract.Assert(instance != null, "instance != null");
                return instance;
            }
        }

        /// <summary>Gets the load time.</summary>
        public long LoadTime
        {
            get { return this.loadTime; }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(this.cobjects != null);
        }
    }
}