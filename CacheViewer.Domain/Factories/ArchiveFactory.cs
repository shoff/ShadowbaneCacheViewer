namespace CacheViewer.Domain.Factories
{
    using Archive;

    /// <summary>
    ///     Just simply maintains a collection of types to enums and
    ///     returns a new instance for the given type.
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

        private static readonly object syncRoot = new object();
        
        internal static ArchiveFactory Instance { get; } = new ArchiveFactory();
        
        internal CacheArchive Build(CacheFile cacheFile, bool preCacheData = false, bool loadIndexes = true)
        {
            lock (syncRoot)
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
                        var palette = new Palette();
                        palette.LoadCacheHeader();
                        palette.CacheOnIndexLoad = preCacheData;
                        // palette has no indexes.
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
                        var terrainAlpha = new TerrainAlpha();
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
                        var tile = new Tile();
                        tile.LoadCacheHeader();
                        tile.CacheOnIndexLoad = preCacheData;
                        if (loadIndexes && tile.CacheHeader.indexCount > 0)
                        {
                            tile.LoadIndexes();
                        }

                        return tile;

                    case CacheFile.Visual:
                        var visual = new Visual();
                        visual.LoadCacheHeader();
                        visual.CacheOnIndexLoad = preCacheData;
                        if (loadIndexes)
                        {
                            visual.LoadIndexes();
                        }

                        return visual;

                    case CacheFile.Dungeon:
                        var dungeon = new Dungeon();
                        dungeon.LoadCacheHeader();
                        dungeon.CacheOnIndexLoad = preCacheData;
                        // dungeon has no indexes.
                        return dungeon;
                    default:
                        return new Unknown();
                }
            }
        }
    }
}