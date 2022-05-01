namespace Shadowbane.Cache.CacheTypes;

using FluentValidation;

public sealed class ZoneCache : CacheArchive
{
    public ZoneCache() : base("CZone.cache")
    {
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