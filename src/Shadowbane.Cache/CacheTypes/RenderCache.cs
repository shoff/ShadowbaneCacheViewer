namespace Shadowbane.Cache.CacheTypes
{
    using System;

    internal sealed class RenderCache : CacheArchive
    {
        public RenderCache()
            : base("Render.cache".AsMemory())
        {
        }

    }
}