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
        public static bool TexturesLoaded { get; private set; }

        private static CObjects cobjects;
        public static bool CObjectsLoaded { get; private set; }

        private static Render render;
        public static bool RenderLoaded { get; private set; }

        private static MeshArchive meshArchive;
        public static bool MeshLoaded { get; private set; }

        private static SoundCache soundCache;
        public static bool SoundLoaded { get; private set; }

        private static CZone czone;
        public static bool CZoneLoaded { get; private set; }

        private static SkeletonCache skeletonCache;
        public static bool SkeletonLoaded { get; private set; }

        private static Motion motion;
        public static bool MotionLoaded { get; private set; }

        private static readonly object syncRoot = new object();
        
        internal static ArchiveFactory Instance { get; } = new ArchiveFactory();

        internal static void BuildAll()
        {

        }

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
                        CObjectsLoaded = true;
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
                        RenderLoaded = true;
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
                        CZoneLoaded = true;
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
                        MeshLoaded = true;
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
                        MotionLoaded = true;
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
                        SkeletonLoaded = true;
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
                        SoundLoaded = true;
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

                        TexturesLoaded = true;
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