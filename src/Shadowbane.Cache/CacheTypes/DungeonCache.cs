namespace Shadowbane.Cache.CacheTypes
{
    using System;

    internal sealed class DungeonCache : CacheArchive
    {
        public DungeonCache()
            : base("Dungeon.cache".AsMemory())
        {
        }
    }
}