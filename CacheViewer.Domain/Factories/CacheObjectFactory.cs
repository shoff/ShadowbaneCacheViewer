namespace CacheViewer.Domain.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Archive;
    using Data;
    using Extensions;
    using Models;
    using Models.Exportable;
    using NLog;

    public class CacheObjectFactory
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private CacheObjectFactory()
        {
#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif
            this.CacheObjects = (CObjects) ArchiveFactory.Instance.Build(CacheFile.CObjects, true);

#if DEBUG
            sw.Stop();
            this.LoadTime = sw.ElapsedTicks;
#endif
        }

        public ICollection<CacheIndex> Indexes => this.CacheObjects.CacheIndices;

        public CacheIndex FindById(int id)
        {
            return (from c in Indexes where c.Identity == id select c).FirstOrDefault();
        }

        public static CacheObjectFactory Instance { get; } = new CacheObjectFactory();

        public async Task SaveToFileAsync(CacheIndex index, string path)
        {
            await CacheObjects.SaveToFileAsync(index, path);
        }

        public long LoadTime { get; }

        internal CObjects CacheObjects { get; }

        public async Task<ICacheObject> CreateAndParseAsync(CacheIndex cacheIndex)
        {
            return await Task.FromResult(this.CreateAndParse(cacheIndex));
        }
        
        internal ICacheObject Create(CacheIndex cacheIndex)
        {
            var asset = this.CacheObjects[cacheIndex.Identity];
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
                return new UnknownObject(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
            }
        }

        public ICacheObject CreateAndParse(int identity)
        {
            // TODO there is still the shit with multiple cache objects having the same Identity
            var asset = this.CacheObjects[identity];
            return this.CreateAndParse(asset.CacheIndex1);
        }

        public ICacheObject CreateAndParse(CacheIndex cacheIndex)
        {
            var asset = this.CacheObjects[cacheIndex.Identity];
            int innerOffset;
            int offset;
            ObjectType flag;
            string name;
            using (var reader = asset.Item1.CreateBinaryReaderUtf32())
            {
                // reader.skip(4); // ignore "TNLC" tag
                // ReSharper disable once UnusedVariable
                var tnlc = reader.ReadInt32();
                // 4
                flag = (ObjectType) reader.ReadInt32();

                var nameLength = reader.ReadUInt32();
                name = reader.ReadAsciiString(nameLength);

                // why are we using this inner offset?
                innerOffset = (int) reader.BaseStream.Position;

                logger.Debug($"Creating cacheObject flag {flag}, name {name}, inner offset {innerOffset}");

                // what are we doing with the offset here??
                // so I think this must be the bug? 
                offset = (int) reader.BaseStream.Position + 25;
            }

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
                        var warrant = new Warrant(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                        warrant.Parse(asset.Item1);
                        return warrant;

                    case ObjectType.Unknown:
                        return new UnknownObject(cacheIndex, flag, name, offset, asset.Item1, innerOffset);

                    case ObjectType.Particle:
                        return new Particle(cacheIndex, flag, name, offset, asset.Item1, innerOffset);
                }
            }
            catch (Exception e)
            {
                logger?.Error(e, e.Message);
                throw;
            }


            return null;
        }
    }
}