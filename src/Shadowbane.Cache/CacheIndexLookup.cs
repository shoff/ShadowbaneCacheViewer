namespace Shadowbane.Cache
{
    using System.Collections.Generic;
    using CacheTypes;

    // contains a list of valid ids for each cacheFile type
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public sealed class CacheIndexLookup : Dictionary<CacheFile, List<uint>>
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
#pragma warning disable IDE0032 // Use auto property
        private static readonly CacheIndexLookup instance = new CacheIndexLookup();
        // ReSharper disable once ConvertToAutoProperty
        public static CacheIndexLookup Instance => instance;
#pragma warning restore IDE0032 // Use auto property

        private CacheIndexLookup()
        {
        }

        public bool IsValidId(CacheFile cacheFile, uint identity)
        {
            if (!this.ContainsKey(cacheFile))
            {
                return false;
            }

            return this[cacheFile].Contains(identity);
        }

        public bool IsValidId(uint identity)
        {
            // This is a dangerously incorrect method to have
            foreach (var kvp in this)
            {
                if (kvp.Value.Contains(identity))
                {
                    return true;
                }
            }

            return false;
        }
    }

 
}