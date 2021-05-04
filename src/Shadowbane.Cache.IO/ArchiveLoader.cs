// ReSharper disable ConvertToAutoProperty
#pragma warning disable IDE0032 // Use auto property

namespace Shadowbane.Cache.IO
{
    using CacheTypes;

    public static class ArchiveLoader
    {
        private static readonly MeshCache meshArchive =
            (MeshCache)new MeshCache()
                .LoadCacheHeader()
                .LoadIndexes();
        private static readonly TextureCache textureArchive =
            (TextureCache)new TextureCache()
                .LoadCacheHeader()
                .LoadIndexes();
        private static readonly ObjectCache objectArchive =
            (ObjectCache)new ObjectCache()
                .LoadCacheHeader()
                .LoadIndexes();
        private static readonly SoundCache soundArchive =
            (SoundCache)new SoundCache()
                .LoadCacheHeader()
                .LoadIndexes();
        private static readonly SkeletonCache skeletonArchive =
            (SkeletonCache)new SkeletonCache()
                .LoadCacheHeader()
                .LoadIndexes();
        private static readonly ZoneCache zoneArchive =
            (ZoneCache)new ZoneCache()
                .LoadCacheHeader()
                .LoadIndexes();
        private static readonly VisualCache visualArchive =
            (VisualCache)new VisualCache()
                .LoadCacheHeader()
                .LoadIndexes();
        private static readonly PaletteCache paletteArchive =
            (PaletteCache)new PaletteCache()
                .LoadCacheHeader()
                .LoadIndexes();
        private static readonly TileCache tileArchive =
            (TileCache)new TileCache()
                .LoadCacheHeader()
                .LoadIndexes();
        private static readonly RenderCache renderArchive =
            (RenderCache)new RenderCache()
                .LoadCacheHeader()
                .LoadIndexes();

        private static readonly MotionCache motionArchive =
            (MotionCache)new MotionCache()
                .LoadCacheHeader()
                .LoadIndexes();
        //private static readonly TestCache testArchive =
        //    (TestCache)new TestCache()
        //        .LoadCacheHeader()
        //        .LoadWithMemoryMappedFile();

        public static MeshCache MeshArchive => meshArchive;
        public static TextureCache TextureArchive => textureArchive;
        public static ObjectCache ObjectArchive => objectArchive;
        public static SoundCache SoundArchive => soundArchive;
        public static SkeletonCache SkeletonArchive => skeletonArchive;
        public static ZoneCache ZoneArchive => zoneArchive;
        public static VisualCache VisualArchive => visualArchive;
        public static PaletteCache PaletteArchive => paletteArchive;
        public static TileCache TileArchive => tileArchive;
        public static RenderCache RenderArchive => renderArchive;
        public static MotionCache MotionArchive => motionArchive;
        //internal static TestCache TestArchive => testArchive;
    }
}
#pragma warning restore IDE0032 // Use auto property
