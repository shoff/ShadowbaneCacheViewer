namespace Shadowbane.Cache.CacheTypes;

using System.Linq;
using FluentValidation;

public sealed class RenderCache : CacheArchive
{
    public RenderCache()
        : base("Render.cache")
    {
    }

    public override CacheAsset this[uint id]
    {
        // TODO figure out if passing the ReadOnlyMemory<byte> here is really better than say
        // TODO having a shared ArrayPool<byte> and renting/returning a simply byte[]
        get
        {
            if (id == 0 || this.cacheIndices.All(i => i.identity != id))
            {
                throw new IndexNotFoundException("no name", id);
            }
            // these "identities" are in fact duped, it could be either a male/female thing or a "versioning" strategy..

            var cacheIndex = this.cacheIndices.First(x => x.identity == id);
            var buffer = this.bufferData.Span.Slice((int)cacheIndex.offset, (int)cacheIndex.compressedSize);
            var asset = new CacheAsset(cacheIndex, this.Decompress(cacheIndex.unCompressedSize, buffer).ToArray())
            {
                HasMultipleIdentityEntries = this.cacheIndices.Count(c => c.identity == id) > 1
            };

            return asset;
        }
    }

    public CacheAsset GetSecondIndex(uint id)
    {
        if (id == 0 || this.cacheIndices.All(i => i.identity != id) || this.cacheIndices.Count(c => c.identity == id) == 1)
        {
            return null;
        }
            
        // these "identities" are in fact duped, they are always byte for byte identical.
        var cacheIndices = this.cacheIndices.Where(x => x.identity == id).Select(x => x).ToArray();

        var cacheIndex = cacheIndices[0];
        var cacheIndex1 = cacheIndices[1];

        var buffer = this.bufferData.Span.Slice((int)cacheIndex1.offset, (int)cacheIndex1.compressedSize);
        var asset = new CacheAsset(cacheIndex1, this.Decompress(cacheIndex1.unCompressedSize, buffer).ToArray())
        {
            HasMultipleIdentityEntries = true,
            Order = AssetOrder.Two // male / female?
        };
        return asset;
    }
    public override CacheArchive Validate()
    {
        new Validator().ValidateAndThrow(this);
        return this;
    }

    private class Validator : AbstractValidator<CacheArchive>
    {
        public Validator()
        {
            this.RuleFor(c => c)
                .Must(ch => ch.CacheHeader.indexCount == ch.CacheIndices.Count)
                .WithMessage("cache header index count does not match index count");
        }
    }
}