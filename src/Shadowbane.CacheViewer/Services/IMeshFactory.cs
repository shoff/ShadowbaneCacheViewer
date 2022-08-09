namespace Shadowbane.CacheViewer.Services;

using Cache;
using Models;

public interface IMeshFactory
{
    Mesh Create(CacheIndex cacheIndex);
    bool HasMeshId(uint id);
}