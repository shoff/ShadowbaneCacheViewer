using System.Collections.Generic;

namespace Shadowbane.Cache.IO;

public interface ICacheWriter
{
    void WriteNewHeader(CacheArchive cacheArchive, CacheHeader cacheHeader);
    void WriteNewIndex(CacheArchive cacheArchive, IEnumerable<CacheIndex> cacheIndices);
}