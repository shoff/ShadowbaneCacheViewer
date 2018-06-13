﻿namespace CacheViewer.Domain.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Archive;
    using Exceptions;
    using Extensions;
    using Models;
    using Models.Exportable;
    using NLog;

    // TODO this is poorly named, it should just be the CObjects Cache or something
    public class CacheObjectsCache
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly CObjects cobjects;

        /// <summary>
        ///     Prevents a default instance of the <see cref="CacheObjectsCache" /> class from being created.
        /// </summary>
        private CacheObjectsCache()
        {
#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif
            this.cobjects = (CObjects) ArchiveFactory.Instance.Build(CacheFile.CObjects, true);

#if DEBUG
            sw.Stop();
            this.LoadTime = sw.ElapsedTicks;
#else
            this.loadTime = 0;
#endif
        }

        /// <summary>Gets the indexes.</summary>
        public ICollection<CacheIndex> Indexes => this.cobjects.CacheIndices;

        /// <summary>Gets the instance.</summary>
        public static CacheObjectsCache Instance { get; } = new CacheObjectsCache();

        /// <summary>Gets the load time.</summary>
        public long LoadTime { get; }

        /// <summary>Returns a basic cache object based on the flag type</summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Condition. </exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        /// <exception cref="IndexNotFoundException">Condition. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public async Task<ICacheObject> CreateAsync(CacheIndex cacheIndex)
        {
            return await Task.FromResult(this.Create(cacheIndex));
        }

        /// <summary>
        ///     Creates the specified cache index.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Condition. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="IndexNotFoundException">Condition. </exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        public ICacheObject Create(CacheIndex cacheIndex)
        {
            var asset = this.cobjects[cacheIndex.Identity];

            using (var reader = asset.Item1.CreateBinaryReaderUtf32())
            {
                // reader.skip(4); // ignore "TNLC" tag
                // ReSharper disable once UnusedVariable
                var tnlc = reader.ReadInt32();

                // 4
                var flag = (ObjectType) reader.ReadInt32();
                var nameLength = reader.ReadUInt32();
                var name = reader.ReadAsciiString(nameLength);

                // why are we using this inner offset?
                var innerOffset = (int) reader.BaseStream.Position;

                // what are we doing with the offset here??
                // so I think this must be the bug? 
                var offset = (int) reader.BaseStream.Position + 25;

                try
                {
                    // TODO this can be optimized by passing the reader to the parse method.
                    switch (flag)
                    {
                        case ObjectType.Simple:
                            var simple = new Simple(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                            simple.Parse(asset.Item1);
                            return simple;

                        case ObjectType.Structure:
                            var structure = new Structure(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                            structure.Parse(asset.Item1);
                            return structure;

                        case ObjectType.Interactive:
                            var interactive = new Interactive(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                            interactive.Parse(asset.Item1);
                            return interactive;

                        case ObjectType.Equipment:
                            var equipment = new Equipment(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                            equipment.Parse(asset.Item1);
                            return equipment;

                        case ObjectType.Mobile:

                            var mobile = new Mobile(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                            // mobile.Parse(asset.Item1);
                            return mobile;

                        case ObjectType.Deed:
                            var deed = new DeedObject(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                            deed.Parse(asset.Item1);
                            return deed;

                        case ObjectType.Sun:
                            return new Sun(cacheIndex, flag, name, offset, asset.Item1, innerOffset);

                        case ObjectType.Warrant:
                            var warrent = new Warrant(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
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
    }
}