namespace Shadowbane.Cache.IO
{
    using Models;

    public class CacheObjectBuilder
    {
        public ICacheObject CreateAndParse(CacheIndex cacheIndex)
        {
            var asset = ArchiveLoader.ObjectArchive[cacheIndex.identity];

            return null;
        }
    }
}