
namespace CacheViewer.Domain.Archive
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using ArraySegments;
    using CacheViewer.Domain.Exceptions;
    using Extensions;
    using Services;
    using Utility;
    using NLog;

    public abstract class CacheArchive
    {
        // ReSharper disable InconsistentNaming
        protected ArraySegment<byte> bufferData;
        protected FileInfo fileInfo;
        private CacheHeader cacheHeader;
        private readonly List<CacheIndex> cacheIndex;
        private readonly FileLocations fileLocations;
        private string name;
        private string saveName;
        private int[] identityArray;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming

        /// <summary>Initializes a new instance of the <see cref="CacheArchive" /> class.</summary>
        /// <param name="name">The name.</param>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file. </exception>
        /// <exception cref="UnauthorizedAccessException">This operation is not supported on the current platform.-or-  specified a directory.-or- The caller does not have the required permission. </exception>
        /// <exception cref="ArgumentException">
        ///   <name>path</name>
        ///   is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see>
        ///     <cref>F:System.IO.Path.InvalidPathChars</cref>
        ///   </see>
        ///   . </exception>
        /// <exception cref="ArgumentNullException">
        ///   <name>path</name>
        ///   is null. </exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example,
        ///  on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
        /// <exception cref="NotSupportedException">
        ///   <name>path</name>
        ///   is in an invalid format. </exception>
        /// <exception cref="FileNotFoundException">The file specified in  was not found. </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        protected CacheArchive(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            this.Name = name;
            this.cacheIndex = new List<CacheIndex>();
            this.cacheHeader = new CacheHeader();
            this.fileLocations = FileLocations.Instance;
            this.UseCache = true;
            this.SetFileLocation();
            this.bufferData = File.ReadAllBytes(this.fileInfo.FullName).AsArraySegment();
        }

        /// <summary>Loads the cache header.</summary>
        /// <exception cref="System.ApplicationException"></exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        public virtual void LoadCacheHeader()
        {
            using (var reader = bufferData.CreateBinaryReaderUtf32())
            {
                // fill in the CachecacheHeader struct for this file.
                // number of entries in this stream?
                reader.BaseStream.Position = 0;
                this.cacheHeader.indexCount = reader.ReadUInt32();
                this.cacheHeader.dataOffset = reader.ReadUInt32();
                this.cacheHeader.fileSize = reader.ReadUInt32();
                this.cacheHeader.junk1 = reader.ReadUInt32();

                // check if this file size is correct
                if ((int)this.CacheHeader.fileSize != this.fileInfo.Length)
                {
                    string length = "0";
                    if (this.fileInfo.Exists)
                    {
                        // ReSharper disable once ExceptionNotDocumented
                        length = this.fileInfo.Length.ToString();
                    }

                    throw new ApplicationException(
                        string.Format(
                            "{0} Header states file should be {1} in size, but FileInfo object reported {2} as actual size.",
                            this.Name, (int)this.CacheHeader.fileSize, length));
                }

                this.cacheHeader.indexOffset = reader.BaseStream.Position;
                this.identityArray = new int[this.cacheHeader.indexCount];
            }
        }

        /// <summary>Loads the indexes.</summary>
        /// <returns></returns>
        public async virtual Task LoadIndexesAsync()
        {
            await Task.Run(() => this.LoadIndexes());
        }

        /// <summary>Loads the indexes.</summary>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        public virtual void LoadIndexes()
        {
            using (var reader = bufferData.CreateBinaryReaderUtf32())
            {
                // set the offset.
                reader.BaseStream.Position = this.cacheHeader.indexOffset;

                for (int i = 0; i < this.cacheHeader.indexCount; i++)
                {
                    CacheIndex index = new CacheIndex
                    {
                        Junk1 = reader.ReadUInt32(),
                        Identity = reader.ReadInt32(),
                        Offset = reader.ReadUInt32(),
                        UnCompressedSize = reader.ReadUInt32(),
                        CompressedSize = reader.ReadUInt32()
                    };
                    this.identityArray[i] = index.Identity;
                    this.cacheIndex.Add(index);
                }

                // this is off on the tile archive for some reason ...
                if (this is Tile == false)
                {
                    Debug.Assert(reader.BaseStream.Position == this.cacheHeader.dataOffset);
                    Debug.Assert(this.cacheIndex.Count == this.cacheHeader.indexCount);
                }
            }

            //foreach (var ci in this.cacheIndex)
            //{
            //    int count = this.cacheIndex.Count(x => x.identity == ci.identity);
            //    Debug.Assert(count < 3);
            //}

            this.LowestId = this.cacheIndex[0].Identity;
            this.HighestId = this.cacheIndex.Last().Identity;
        }

        /// <summary>
        /// Gets the <see cref="CacheViewer.Domain.Archive.CacheAsset" /> with the specified cache index.
        /// </summary>
        /// <value>
        /// The <see cref="CacheViewer.Domain.Archive.CacheAsset" />.
        /// </value>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception accessor="get">An I/O error occurs.
        ///   <cref>IOException</cref>
        /// </exception>
        /// <exception accessor="get">
        ///   <cref>ArgumentNullException</cref>
        ///   <paramref>
        ///     <name>source</name>
        ///   </paramref>
        ///   or <paramref>
        ///     <name>predicate</name>
        ///   </paramref>
        ///   is null.</exception>
        /// <exception accessor="get">The number of elements in
        ///   <cref>OverflowException</cref>
        ///   <paramref>
        ///     <name>source</name>
        ///   </paramref>
        ///   is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
        public virtual CacheAsset this[int id]
        {
            get
            {
                int count = this.cacheIndex.Count(x => x.Identity == id);

                CacheAsset asset = new CacheAsset
                {
                    CacheIndex1 = this.cacheIndex.FirstOrDefault(x => x.Identity == id)
                };

                if (asset.CacheIndex1.Identity == 0)
                {
                    throw new IndexNotFoundException("no name", id);
                }

                // do we have two with the same id?
                if (count > 1)
                {
                    logger.Info("{0} found {1} entries for identity {2}", this.Name, count, id);
                    CacheIndex ci = this.cacheIndex.Where(x => x.Identity == id).Skip(1).Select(x => x).Single();
                    ci.Order = 2;
                    asset.CacheIndex2 = ci;
                }

                using (var reader = this.bufferData.CreateBinaryReaderUtf32())
                {
                    // ReSharper disable ExceptionNotDocumented
                    reader.BaseStream.Position = asset.CacheIndex1.Offset;
                    var buffer = reader.ReadBytes((int)asset.CacheIndex1.CompressedSize);
                    asset.Item1 = this.Decompress(asset.CacheIndex1.UnCompressedSize, buffer);

                    // hate this hack, freaking Wolfpack decided that the identity in the 
                    // render.cache didn't need to be unique... brilliant...
                    if (count > 1)
                    {
                        reader.BaseStream.Position = asset.CacheIndex2.Offset;
                        asset.Item2 = this.Decompress(asset.CacheIndex2.UnCompressedSize,
                            reader.ReadBytes((int)asset.CacheIndex2.CompressedSize));
                    }
                }
                return asset;
            }
        }

        /// <summary>Saves to file.</summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public async Task SaveToFile(CacheIndex cacheIndex, string path)
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
            var asset = this[cacheIndex.Identity];

            if (asset.Item1.Count > 0)
            {
                await FileWriter.Writer.WriteAsync(asset.Item1,
                    Path.Combine(path, this.saveName + asset.CacheIndex1.Identity.ToString(CultureInfo.InvariantCulture) + ".cache"));

                if (asset.Item2.Count > 0)
                {
                    await FileWriter.Writer.WriteAsync(asset.Item2, Path.Combine(path,
                        this.saveName + asset.CacheIndex2.Identity.ToString(CultureInfo.InvariantCulture) + "_1.cache"));
                }
            }
        }

        /// <summary>Gets the name.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <returns></returns>
        protected virtual string GetName(byte[] buffer, CacheIndex cacheIndex)
        {
            return string.Empty;
        }

        /// <summary>Sets the file location.</summary>
        protected void SetFileLocation()
        {
            string folderName = this.fileLocations.GetCacheFolder();
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
                throw new InvalidCastException("Index raw size should be " + uncompressedSize + " , but was " + item.Length);
            }

            // gets the name
            // cacheIndex.name = this.GetName(item);
            // this.items.Add(cacheIndex.id, item);
            // also update the index in the list
            // int indexPosition = this.itemsById[cacheIndex.id];
            // this.index[indexPosition] = cacheIndex;
            return new ArraySegment<byte>(item);
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }
            protected set
            {
                this.name = value;
                this.saveName = name.Replace(".cache", "_").ToLowerInvariant().Trim();
            }
        }

        /// <summary>Gets the cache header.</summary>
        public CacheHeader CacheHeader
        {
            get { return this.cacheHeader; }
        }

        /// <summary>Gets the indexes.</summary>
        public List<CacheIndex> CacheIndices
        {
            get { return this.cacheIndex; }
        }

        /// <summary>Gets or sets a value indicating whether [cache on index load].</summary>
        public bool CacheOnIndexLoad { get; set; }

        /// <summary>Gets or sets a value indicating whether [use cache].</summary>
        public bool UseCache { get; set; }

        /// <summary>Gets or sets the lowest identifier.</summary>
        public int LowestId { get; set; }

        /// <summary>Gets or sets the highest identifier.</summary>
        public int HighestId { get; set; }

        /// <summary>Gets the identity array.</summary>
        public int[] IdentityArray
        {
            get { return this.identityArray; }
        }

    }
}