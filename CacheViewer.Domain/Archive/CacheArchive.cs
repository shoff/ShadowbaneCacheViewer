namespace CacheViewer.Domain.Archive
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Exceptions;
    using Extensions;
    using Nito.ArraySegments;
    using NLog;
    using Services;
    using Utility;

    public abstract class CacheArchive
    {
        protected CacheArchive(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.cacheHeader = new CacheHeader();
            this.fileLocations = FileLocations.Instance;
            this.UseCache = true;
            this.SetFileLocation();
#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif
            this.bufferData = File.ReadAllBytes(this.fileInfo.FullName).AsArraySegment();
#if DEBUG
            sw.Stop();
            this.ReadTime = sw.ElapsedTicks;
#endif
        }

        public long ReadTime { get; }

        public virtual void LoadCacheHeader()
        {
            using (var reader = this.bufferData.CreateBinaryReaderUtf32())
            {
                // fill in the CacheHeader struct for this file.
                // number of entries in this stream?
                reader.BaseStream.Position = 0;
                this.cacheHeader.indexCount = reader.ReadUInt32();
                this.cacheHeader.dataOffset = reader.ReadUInt32();
                this.cacheHeader.fileSize = reader.ReadUInt32();
                this.cacheHeader.junk1 = reader.ReadUInt32();

                // check if this file size is correct
                if ((int) this.CacheHeader.fileSize != this.fileInfo.Length)
                {
                    var length = "0";
                    if (this.fileInfo.Exists)
                    {
                        // ReSharper disable once ExceptionNotDocumented
                        length = this.fileInfo.Length.ToString();
                    }

                    throw new HeaderFileSizeException(
                        $"{this.Name} Header states file should be {this.CacheHeader.fileSize} in size, but FileInfo object reported {length} as actual size.");
                }

                this.cacheHeader.indexOffset = reader.BaseStream.Position;
                this.IdentityArray = new int[this.cacheHeader.indexCount];
                this.CacheIndices = new CacheIndex[this.cacheHeader.indexCount];
                logger?.Info($"Creating identityArray for {this.name} with {this.cacheHeader.indexCount} indeces.");
                logger?.Info($"{this.name} had junk UInt32 in cache header with value {this.cacheHeader.junk1}");
            }
        }

        public virtual async Task LoadIndexesAsync()
        {
            await Task.Run(() => this.LoadIndexes());
        }

        public virtual void LoadIndexes()
        {
            using (var reader = this.bufferData.CreateBinaryReaderUtf32())
            {
                // set the offset.
                reader.BaseStream.Position = this.cacheHeader.indexOffset;
                int previousIdentity = 0;
                int previousOrder = 0;
                for (var i = 0; i < this.cacheHeader.indexCount; i++)
                {
                    var index = new CacheIndex
                    {
                        Index = i,
                        Junk1 = reader.ReadUInt32(),
                        Identity = reader.ReadInt32(),
                        Offset = reader.ReadUInt32(),
                        UnCompressedSize = reader.ReadUInt32(),
                        CompressedSize = reader.ReadUInt32()
                    };
                    // so we can easily correctly determine the cache order
                    if (previousIdentity == index.Identity)
                    {
                        index.Order = ++previousOrder;
                    }
                    else
                    {
                        previousOrder = 0;
                    }

                    previousIdentity = index.Identity;
                    this.IdentityArray[i] = index.Identity;
                    this.CacheIndices[i] = index;
                }

                // this is off on the tile archive for some reason ...
                if (this is Tile == false)
                {
                    Debug.Assert(reader.BaseStream.Position == this.cacheHeader.dataOffset);
                }
            }

            //foreach (var ci in this.cacheIndex)
            //{
            //    int count = this.cacheIndex.Count(x => x.identity == ci.identity);
            //    Debug.Assert(count < 3);
            //}

            this.LowestId = this.CacheIndices[0].Identity;
            this.HighestId = this.CacheIndices.Last().Identity;
        } // ReSharper disable InconsistentNaming
        protected ArraySegment<byte> bufferData;
        protected FileInfo fileInfo;
        protected CacheHeader cacheHeader;
        protected readonly FileLocations fileLocations;
        protected string name;
        protected string saveName;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming

        public virtual CacheAsset this[int id]
        {
            get
            {
                if (id == 0 || this.CacheIndices.All(i => i.Identity != id))
                {
                    throw new IndexNotFoundException("no name", id);
                }

                var count = this.CacheIndices.Count(x => x.Identity == id);

                var asset = new CacheAsset
                {
                    // leave this as First, we want it to throw if the id is invalid
                    CacheIndex1 = this.CacheIndices.First(x => x.Identity == id)
                };

                // do we have two with the same id?
                if (count > 1)
                {
                    logger?.Info("{0} found {1} entries for identity {2}", this.Name, count, id);

                    // don't think there are any that have more than 3
                    // I really don't think this is necessary anymore 1/23/2019
                    Debug.Assert(count < 4);
                    for (int i = 1; i < count; i++)
                    {
                        var ci = this.CacheIndices.Where(x => x.Identity == id).Skip(i).Select(x => x).Single();
                        ci.Order = i+1;
                        if (i == 1)
                        {
                            asset.CacheIndex2 = ci;
                        }
                        else
                        {
                            asset.CacheIndex3 = ci;
                        }
                    }
                }

                using (var reader = this.bufferData.CreateBinaryReaderUtf32())
                {
                    // ReSharper disable ExceptionNotDocumented
                    reader.BaseStream.Position = asset.CacheIndex1.Offset;
                    var buffer = reader.ReadBytes((int) asset.CacheIndex1.CompressedSize);
                    asset.Item1 = this.Decompress(asset.CacheIndex1.UnCompressedSize, buffer);

                    // hate this hack, freaking Wolfpack decided that the identity in the 
                    // render.cache didn't need to be unique... brilliant...
                    if (count > 1)
                    {
                        logger?.Debug(
                            $"Setting reader to offset{asset.CacheIndex2.Offset} to read second item with id {id}");
                        reader.BaseStream.Position = asset.CacheIndex2.Offset;
                        asset.Item2 = this.Decompress(asset.CacheIndex2.UnCompressedSize, reader.ReadBytes((int) asset.CacheIndex2.CompressedSize));
                    }

                    if (count > 2)
                    {
                        logger?.Debug(
                            $"Setting reader to offset{asset.CacheIndex2.Offset} to read third item with id {id}");
                        reader.BaseStream.Position = asset.CacheIndex3.Offset;
                        asset.Item2 = this.Decompress(asset.CacheIndex3.UnCompressedSize, reader.ReadBytes((int)asset.CacheIndex3.CompressedSize));
                    }

                    if (count > 3)
                    {
                        throw new ApplicationException(
                            "Need to refactor some twat added more than three indexes with the same id.");
                    }
                    // if there are more than 3 fuck it
                }

                return asset;
            }
        }

        public virtual bool Contains(int id)
        {
            return this.CacheIndices.Any(x => x.Identity == id);
        }

        public async Task SaveToFileAsync(CacheIndex index, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            // TODO move to it's own object, this doesn't belong here.
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var asset = this[index.Identity];

            if (asset.Item1.Count > 0)
            {
                await FileWriter.Writer.WriteAsync(asset.Item1.ToArray(), path,
                        $"{this.saveName}{asset.CacheIndex1.Identity.ToString(CultureInfo.InvariantCulture)}.cache");

                if (asset.Item2.Count > 0)
                {
                    await FileWriter.Writer.WriteAsync(asset.Item2.ToArray(), path,
                        $"{this.saveName}{asset.CacheIndex2.Identity.ToString(CultureInfo.InvariantCulture)}_1.cache");
                }
            }
        }

        protected void SetFileLocation()
        {
            var folderName = this.fileLocations.GetCacheFolder();
            this.fileInfo = new FileInfo(Path.Combine(folderName, this.Name));
        }

        protected ArraySegment<byte> Decompress(uint uncompressedSize, byte[] file)
        {
            if (file.Length == uncompressedSize)
            {
                return new ArraySegment<byte>(file);
            }

            //var item = file.UnZip();
            var item = file.Uncompress();

            if (item.Length != uncompressedSize)
            {
                throw new InvalidCompressionSizeException
                    ("Index raw size should be " + uncompressedSize + " , but was " + item.Length);
            }

            // gets the name
            // cacheIndex.name = this.GetName(item);
            // this.items.Add(cacheIndex.id, item);
            // also update the index in the list
            // int indexPosition = this.itemsById[cacheIndex.id];
            // this.index[indexPosition] = cacheIndex;
            return new ArraySegment<byte>(item);
        }

        public string Name
        {
            get => this.name;
            protected set
            {
                this.name = value;
                this.saveName = this.name.Replace(".cache", "_").ToLowerInvariant().Trim();
            }
        }

        public CacheHeader CacheHeader => this.cacheHeader;

        public CacheIndex[] CacheIndices { get; private set; }

        public bool CacheOnIndexLoad { get; set; }

        public bool UseCache { get; set; }

        public int LowestId { get; set; }

        public int HighestId { get; set; }

        public int[] IdentityArray { get; private set; }
    }
}