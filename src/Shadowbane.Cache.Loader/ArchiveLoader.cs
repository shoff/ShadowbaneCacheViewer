// ReSharper disable ConvertToAutoProperty
#pragma warning disable IDE0032 // Use auto property

namespace Shadowbane.Cache.IO
{
    using CacheTypes;

    internal static class ArchiveLoader
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

        internal static MeshCache MeshArchive => meshArchive;
        internal static TextureCache TextureArchive => textureArchive;
        internal static ObjectCache ObjectArchive => objectArchive;
        internal static SoundCache SoundArchive => soundArchive;
        internal static SkeletonCache SkeletonArchive => skeletonArchive;
        internal static ZoneCache ZoneArchive => zoneArchive;
        internal static VisualCache VisualArchive => visualArchive;
        internal static PaletteCache PaletteArchive => paletteArchive;
        internal static TileCache TileArchive => tileArchive;
        internal static RenderCache RenderArchive => renderArchive;
        internal static MotionCache MotionArchive => motionArchive;
        //internal static TestCache TestArchive => testArchive;
    }
}
#pragma warning restore IDE0032 // Use auto property
