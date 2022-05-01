namespace Shadowbane.Cache;

using System;

public enum AssetOrder
{
    One,
    Two
}

public class CacheAsset
{
    // I'm beginning to wonder if Item1 and Item2 are actually Male/Female versions of this
    // hello old steve, this is a newer steve, we still don't know in 2021!
    // Hello old old steve and old steve, 2022 checking in... still nfc 
    public CacheAsset(CacheIndex cacheIndex, ReadOnlyMemory<byte> asset) => 
        (this.CacheIndex, this.Asset) = (cacheIndex, asset);

    public CacheIndex CacheIndex { get; }
    public ReadOnlyMemory<byte> Asset { get; }
    public bool HasMultipleIdentityEntries { get; set; }
    public AssetOrder Order { get; set; }
    public bool IsValid => this.Asset.Length > 0 && this.CacheIndex.IsValid();
    public override string ToString()
    {
        return this.CacheIndex.identity.ToString();
    }
}