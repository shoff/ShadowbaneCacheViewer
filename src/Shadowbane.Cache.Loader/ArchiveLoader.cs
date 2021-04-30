namespace Shadowbane.Cache.Loader
{
    using CacheTypes;

    internal static class ArchiveLoader
    {
        internal static MeshCache MeshArchive => 
            (MeshCache)new MeshCache()
            .LoadCacheHeader()
            .LoadWithMemoryMappedFile();

        internal static TextureCache TextureArchive =>
            (TextureCache) new TextureCache()
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();

        internal static ObjectCache ObjectArchive =>
            (ObjectCache) new ObjectCache()
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();

        internal static SoundCache SoundArchive =>
            (SoundCache)new SoundCache()
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();

        internal static SkeletonCache SkeletonArchive =>
            (SkeletonCache)new SkeletonCache()
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();

        internal static ZoneCache ZoneArchive =>
            (ZoneCache)new ZoneCache()
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();

        internal static VisualCache VisualArchive =>
            (VisualCache)new VisualCache()
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();

        internal static PaletteCache PaletteArchive =>
            (PaletteCache)new PaletteCache()
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();
        
        internal static TileCache TileArchive =>
            (TileCache)new TileCache()
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();
        
        internal static RenderCache RenderArchive =>
            (RenderCache)new RenderCache()
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();
        
        internal static MotionCache MotionArchive =>
            (MotionCache)new MotionCache()
                .LoadCacheHeader()
                .LoadWithMemoryMappedFile();
    }
}