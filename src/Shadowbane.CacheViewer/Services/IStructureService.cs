namespace Shadowbane.CacheViewer.Services;

using Cache;

public interface IStructureService
{
    Task SaveAssembledModelAsync(string saveFolder, ICacheObject cacheObject, bool singleFile = false);
}