using System.Threading.Tasks;

namespace Shadowbane.Cache.Exporter.File;

public static class IdListExporter
{
    public static async Task SaveCacheIndexesAsync(this CacheArchive archive, string path, string fileName)
    {
        await using var f = System.IO.File.AppendText($"{path}\\{fileName}");
        foreach (var index in archive.CacheIndices)
        {
            await f.WriteLineAsync($"{index.identity},{index.compressedSize}, {index.unCompressedSize}");
        }
    }
}