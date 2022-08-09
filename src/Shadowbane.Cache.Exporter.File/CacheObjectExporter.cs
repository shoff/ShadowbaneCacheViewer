namespace Shadowbane.Cache.Exporter.File;

using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

public class CacheObjectExporter
{
    // 1. handle each render info 
    // a. save render mesh
    // b. save render texture

    // 2. Load each .obj from disk that we just wrote

    // 3. Create one combined .obj from collection.

    // 4. Combine textures? 

    // 5. Combine materials?

    // 6. Export as single obj


}

public static class CacheArchiveExtensions 
{

    public static async Task SaveToFileAsync(this CacheArchive archive, CacheIndex index, string name, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        // TODO move to it's own object, this doesn't belong here.
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var asset = archive[index.identity];

        if (asset?.IsValid != null && asset.IsValid)
        {
            await FileWriter.Writer.WriteAsync(asset.Asset, path,
                $"{name}{index.identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }
}