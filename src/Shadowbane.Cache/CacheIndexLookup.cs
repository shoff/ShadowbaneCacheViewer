namespace Shadowbane.Cache
{
    using System.Collections.Generic;
    using CacheTypes;

    // contains a list of valid ids for each cacheFile type
    public class CacheIndexLookup : Dictionary<CacheFile, List<uint>>
    {
        public bool IsValidId(CacheFile cacheFile, uint identity)
        {
            if (!this.ContainsKey(cacheFile))
            {
                return false;
            }

            return this[cacheFile].Contains(identity);
        }
    }

 
}