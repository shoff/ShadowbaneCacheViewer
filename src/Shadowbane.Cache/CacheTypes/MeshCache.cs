namespace Shadowbane.Cache.CacheTypes;

using FluentValidation;

public class MeshCache : CacheArchive
{
    public const string CACHE_NAME = "Mesh.cache";
    public MeshCache()
        : base(CACHE_NAME)
    {
    }

    internal uint IdentityAt(int index)
    {
        if (index > this.IndexCount)
        {
            return default;
        }

        return this.cacheIndices[index].identity;
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