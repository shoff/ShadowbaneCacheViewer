namespace Shadowbane.Cache;

using System;

public enum AssetOrder
{
    One,
    Two
}

public record CacheAsset(CacheIndex CacheIndex, ReadOnlyMemory<byte> Asset)
{
    // I'm beginning to wonder if Item1 and Item2 are actually Male/Female versions of this
    // hello old steve, this is a newer steve, we still don't know in 2021!
    // Hello old old steve and old steve, 2022 checking in... still nfc 
    public bool HasMultipleIdentityEntries { get; set; }
    public AssetOrder Order { get; set; }
    public bool IsValid => this.Asset.Length > 0
            && this.CacheIndex.IsValid()
            && Asset.Length == CacheIndex.unCompressedSize;
    public override string ToString()
    {
        return this.CacheIndex.identity.ToString();
    }
}