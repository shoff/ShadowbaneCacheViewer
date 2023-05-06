namespace Shadowbane.CacheViewer.Services;

using Cache;

public interface IRealTimeModelService
{
    Task<List<IMesh?>> GenerateModelAsync(uint cacheObjectIdentity);
}