namespace Shadowbane.Cache.CacheTypes;

using FluentValidation;

public sealed class RenderCache : CacheArchive
{
    private const string CACHE_NAME = "Render.cache";

    public RenderCache()
        : base(CACHE_NAME)
    {
    }

    public override CacheAsset? this[uint id]
    {
        // TODO figure out if passing the ReadOnlyMemory<byte> here is really better than say
        // TODO having a shared ArrayPool<byte> and renting/returning a simply byte[]
        get
        {
            if (id == 0 || !this.indices.ContainsKey(id))
            {
                return null;
            }
            
            // these "identities" are in fact duped, it could be either a male/female thing or a "versioning" strategy..
            var cacheIndex = this.indices[id];
            var buffer = this.bufferData.Span.Slice((int)cacheIndex.offset, (int)cacheIndex.compressedSize);
            var asset = new CacheAsset(cacheIndex, this.Decompress(cacheIndex.unCompressedSize, buffer).ToArray());
            return asset;
        }
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
                .Must(ch => ch.CacheHeader.indexCount == ch.CacheIndices.Count + ch.DuplicateCount)
                .WithMessage("cache header index count does not match index count");
        }
    }
}