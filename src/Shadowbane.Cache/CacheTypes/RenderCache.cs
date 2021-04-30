namespace Shadowbane.Cache.CacheTypes
{
    using System;

    public sealed class RenderCache : CacheArchive
    {
        public RenderCache()
            : base("Render.cache".AsMemory())
        {
        }

    }
}