namespace Shadowbane.CacheViewer.Services;

using Cache;

public interface IStructureService
{
    Task SaveAssembledModelAsync(string saveFolder, ICacheRecord cacheRecord, bool singleFile = false);
}