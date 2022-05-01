namespace Shadowbane.Cache;

using System.IO;

public static class CacheLocation
{
    static CacheLocation()
    {
        CheckDirectory(RenderOutputFolder.FullName);
        CheckDirectory(CacheFolder.FullName);
        CheckDirectory(TextureFolder.FullName);
        CheckDirectory(OutputFolder.FullName);
        CheckDirectory(ModelFolder.FullName);
        CheckDirectory(SimpleFolder.FullName);
        CheckDirectory(StructureFolder.FullName);
        CheckDirectory(MeshOutputFolder.FullName);
        CheckDirectory(MobileFolder.FullName);
        CheckDirectory(EquipmentFolder.FullName);
        CheckDirectory(InteractiveFolder.FullName);
    }

    private static void CheckDirectory(string dir)
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
    public static DirectoryInfo CacheFolder => new(@"C:\dev\ShadowbaneCacheViewer\cache\");
    public static DirectoryInfo TextureFolder => new(@"C:\dev\ShadowbaneCacheViewer\output\textures");
    public static DirectoryInfo OutputFolder => new(@"C:\dev\ShadowbaneCacheViewer\output\");
    public static DirectoryInfo ModelFolder => new(@"C:\dev\ShadowbaneCacheViewer\output\models\");
    public static DirectoryInfo SimpleFolder => new(@"C:\dev\ShadowbaneCacheViewer\output\models\simple\");
    public static DirectoryInfo StructureFolder => new(@"C:\dev\ShadowbaneCacheViewer\output\models\structures\");
    public static DirectoryInfo MobileFolder => new(@"C:\dev\ShadowbaneCacheViewer\output\models\mobiles\");
    public static DirectoryInfo EquipmentFolder => new(@"C:\dev\ShadowbaneCacheViewer\output\models\equipment\");
    public static DirectoryInfo InteractiveFolder => new(@"C:\dev\ShadowbaneCacheViewer\output\models\interactive\");
    public static DirectoryInfo MeshOutputFolder => new(@"C:\dev\ShadowbaneCacheViewer\output\mesh\");
    public static DirectoryInfo RenderOutputFolder => new(@"C:\dev\ShadowbaneCacheViewer\output\render\");

}