

namespace CacheViewer.Domain.Factories
{
    using System;
    using System.IO;
    using System.Security;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Archive;

    /// <summary>
    /// Just simply maintains a collection of types to enums and
    /// returns a new instance for the given type.
    /// </summary>
    internal class ArchiveFactory
    {
        private static Textures textures;
        private static CObjects cobjects;
        private static Render render;
        private static MeshArchive meshArchive;
        private static SoundCache soundCache;
        private static CZone czone;
        private static SkeletonCache skeletonCache;
        private static Motion motion;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        internal static ArchiveFactory Instance
        {
            get { return new ArchiveFactory(); }
        }

        /// <summary>
        /// Builds the asynchronous.
        /// </summary>
        /// <param name="cacheFile">The cache file.</param>
        /// <param name="preCacheData">if set to <c>true</c> [pre cache data].</param>
        /// <returns></returns>
        internal async Task<CacheArchive> BuildAsync(CacheFile cacheFile, bool preCacheData = false)
        {
            return await Task.Run(() => this.Build(cacheFile, preCacheData));
        }

        /// <summary>
        /// Builds the specified cache file.
        /// </summary>
        /// <param name="cacheFile">The cache file.</param>
        /// <param name="preCacheData">if set to <c>true</c> [pre cache data].</param>
        /// <param name="loadIndexes">if set to <c>true</c> [load indexes].</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Condition. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
        /// <exception cref="UnauthorizedAccessException">This operation is not supported on the current platform.-or-  specified a directory.-or- The caller does not have the required permission. </exception>
        /// <exception cref="FileNotFoundException">The file specified in  was not found. </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        internal CacheArchive Build(CacheFile cacheFile, bool preCacheData = false, bool loadIndexes = true)
        {
            switch (cacheFile)
            {
                case CacheFile.CObjects:
                    if (cobjects != null)
                    {
                        return cobjects;
                    }
                    cobjects = new CObjects();
                    cobjects.LoadCacheHeader();
                    cobjects.CacheOnIndexLoad = preCacheData;
                    if (loadIndexes)
                    {
                        cobjects.LoadIndexes();
                    }
                    return cobjects;

                case CacheFile.Render:
                    if (render != null)
                    {
                        return render;
                    }
                    render = new Render();
                    render.LoadCacheHeader();
                    render.CacheOnIndexLoad = preCacheData;
                    if (loadIndexes)
                    {
                        render.LoadIndexes();
                    }
                    return render;

                case CacheFile.CZone:
                    if (czone == null)
                    {
                        czone = new CZone();
                    }
                    czone.LoadCacheHeader();
                    czone.CacheOnIndexLoad = preCacheData;

                    if (loadIndexes)
                    {
                        czone.LoadIndexes();
                    }
                    return czone;

                case CacheFile.Mesh:
                    if (meshArchive != null)
                    {
                        return meshArchive;
                    }
                    meshArchive = new MeshArchive();
                    meshArchive.LoadCacheHeader();
                    meshArchive.CacheOnIndexLoad = preCacheData;
                    if (loadIndexes)
                    {
                        meshArchive.LoadIndexes();
                    }
                    return meshArchive;

                case CacheFile.Motion:
                    if (motion == null)
                    {
                        motion = new Motion();
                    }
                    motion.LoadCacheHeader();
                    motion.CacheOnIndexLoad = preCacheData;
                    if (loadIndexes)
                    {
                        motion.LoadIndexes();
                    }
                    return motion;

                case CacheFile.Palette:
                    Palette palette = new Palette();
                    palette.LoadCacheHeader();
                    palette.CacheOnIndexLoad = preCacheData;
                    if (loadIndexes)
                    {
                        palette.LoadIndexes();
                    }
                    return palette;

                case CacheFile.Skeleton:
                    if (skeletonCache == null)
                    {
                        skeletonCache = new SkeletonCache();
                    }
                    skeletonCache.LoadCacheHeader();
                    skeletonCache.CacheOnIndexLoad = preCacheData;
                    if (loadIndexes)
                    {
                        skeletonCache.LoadIndexes();
                    }
                    return skeletonCache;

                case CacheFile.Sound:
                    if (soundCache != null)
                    {
                        return soundCache;
                    }
                    soundCache = new SoundCache();
                    soundCache.LoadCacheHeader();
                    soundCache.CacheOnIndexLoad = preCacheData;
                    if (loadIndexes)
                    {
                        soundCache.LoadIndexes();
                    }
                    return soundCache;

                case CacheFile.TerrainAlpha:
                    TerrainAlpha terrainAlpha = new TerrainAlpha();
                    terrainAlpha.LoadCacheHeader();
                    terrainAlpha.CacheOnIndexLoad = preCacheData;
                    if (loadIndexes)
                    {
                        terrainAlpha.LoadIndexes();
                    }
                    return terrainAlpha;

                case CacheFile.Textures:
                    if (textures != null)
                    {
                        return textures;
                    }
                    textures = new Textures();
                    textures.LoadCacheHeader();
                    textures.CacheOnIndexLoad = preCacheData;
                    if (loadIndexes)
                    {
                        textures.LoadIndexes();
                    }
                    return textures;

                case CacheFile.Tile:
                    Tile tile = new Tile();
                    tile.LoadCacheHeader();
                    tile.CacheOnIndexLoad = preCacheData;
                    if (loadIndexes)
                    {
                        tile.LoadIndexes();
                    }
                    return tile;

                case CacheFile.Visual:
                    Visual visual = new Visual();
                    visual.LoadCacheHeader();
                    visual.CacheOnIndexLoad = preCacheData;
                    if (loadIndexes)
                    {
                        visual.LoadIndexes();
                    }
                    return visual;

                case CacheFile.Dungeon:
                    Dungeon dungeon = new Dungeon();
                    dungeon.LoadCacheHeader();
                    dungeon.CacheOnIndexLoad = preCacheData;
                    if (loadIndexes)
                    {
                        dungeon.LoadIndexes();
                    }
                    return dungeon;
                default:
                    return new Unknown();
            }
        }
    }
}