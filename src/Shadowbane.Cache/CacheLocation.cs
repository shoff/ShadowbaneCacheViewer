namespace Shadowbane.Cache
{
    using System.IO;

    public static class CacheLocation
    {
        public static DirectoryInfo CacheFolder => new DirectoryInfo(@"C:\dev\ShadowbaneCacheViewer\cache\");
        public static DirectoryInfo TextureFolder => new DirectoryInfo(@"C:\dev\ShadowbaneCacheViewer\output\textures");
        public static DirectoryInfo OutputFolder => new DirectoryInfo(@"C:\dev\ShadowbaneCacheViewer\output\");
        public static DirectoryInfo ModelFolder => new DirectoryInfo(@"C:\dev\ShadowbaneCacheViewer\output\models\");
        public static DirectoryInfo SimpleFolder => new DirectoryInfo(@"C:\dev\ShadowbaneCacheViewer\output\models\simple\");
        public static DirectoryInfo StructureFolder => new DirectoryInfo(@"C:\dev\ShadowbaneCacheViewer\output\models\structures\");
        public static DirectoryInfo MeshOutputFolder => new DirectoryInfo(@"C:\dev\ShadowbaneCacheViewer\output\mesh\");
        public static DirectoryInfo RenderOutputFolder => new DirectoryInfo(@"C:\dev\ShadowbaneCacheViewer\output\render\");

    }
}