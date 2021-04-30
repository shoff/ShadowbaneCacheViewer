namespace Shadowbane.Cache
{
    using System;

    public class CacheAsset
    {
        // I'm beginning to wonder if Item1 and Item2 are actually Male/Female versions of this
        public CacheAsset(CacheIndex cacheIndex, ReadOnlyMemory<byte> asset) => 
            (this.CacheIndex, this.Asset) = (cacheIndex, asset);

        public CacheIndex CacheIndex { get; }
        public ReadOnlyMemory<byte> Asset { get; set; }

        public override string ToString()
        {
            return this.CacheIndex.Identity.ToString();
        }
    }
}