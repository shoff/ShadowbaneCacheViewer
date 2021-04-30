namespace Shadowbane.Cache.CacheTypes
{
    using System;

    public sealed class TileCache : CacheArchive
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Tile" /> class.
        /// </summary>
        public TileCache()
            : base("Tile.cache".AsMemory())
        {
        }
    }
}