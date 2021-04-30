namespace Shadowbane.Cache.CacheTypes
{
    using System;

    public sealed class SoundCache : CacheArchive
    {
        public SoundCache()
            : base("Sound.cache".AsMemory())
        {
        }
    }
}