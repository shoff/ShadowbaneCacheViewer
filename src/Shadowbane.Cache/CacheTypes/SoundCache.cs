namespace Shadowbane.Cache.CacheTypes
{
    using System;

    internal sealed class SoundCache : CacheArchive
    {

        public SoundCache()
            : base("Sound.cache".AsMemory())
        {
        }
    }
}