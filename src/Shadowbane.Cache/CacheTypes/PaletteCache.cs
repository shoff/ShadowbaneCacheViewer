namespace Shadowbane.Cache.CacheTypes
{
    using System;

    internal sealed class PaletteCache : CacheArchive
    {
        public PaletteCache()
            : base("Palette.cache".AsMemory())
        {
        }
    }
}