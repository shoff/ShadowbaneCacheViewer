namespace Shadowbane.Cache.CacheTypes;

using FluentValidation;

public sealed class TileCache : CacheArchive
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Tile" /> class.
    /// </summary>
    public TileCache()
        : base("Tile.cache")
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