namespace Shadowbane.Cache
{
    using System.IO;

    public static class CacheLocation
    {
        public static DirectoryInfo CacheFolder => new DirectoryInfo(@"C:\dev\ShadowbaneCacheViewer\cache");
        public static DirectoryInfo OutputFolder => new DirectoryInfo(@"C:\dev\ShadowbaneCacheViewer\output");
    }
}