// ReSharper disable ConvertToAutoProperty
#pragma warning disable IDE0032 // Use auto property

namespace Shadowbane.Cache.IO;

using CacheTypes;

public static class ArchiveLoader
{
    private static readonly MeshCache meshArchive = new();
    private static readonly TextureCache textureArchive = new();
    private static readonly ObjectCache objectArchive = new();
    private static readonly SoundCache soundArchive = new();
    private static readonly SkeletonCache skeletonArchive = new();
    private static readonly ZoneCache zoneArchive = new();
    private static readonly VisualCache visualArchive = new();
    private static readonly PaletteCache paletteArchive = new();
    private static readonly TileCache tileArchive = new();
    private static readonly RenderCache renderArchive = new();
    private static readonly MotionCache motionArchive = new();

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
}
#pragma warning restore IDE0032 // Use auto property
