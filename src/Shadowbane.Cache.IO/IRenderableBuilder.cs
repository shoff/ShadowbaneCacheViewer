namespace Shadowbane.Cache.IO;

using Models;

public interface IRenderableBuilder
{
    Renderable? Build(CacheIndex cacheIndex, bool saveToFile = false);

    Renderable? Build(uint identity, 
        bool saveToFile = false, 
        bool saveIndexedTextures = false,
        bool parseChildren = false,
        bool buildTexture = true);

    Renderable? RecurseBuildAndExport(CacheIndex cacheIndex);
    Renderable? RecurseBuild(CacheIndex cacheIndex);
}