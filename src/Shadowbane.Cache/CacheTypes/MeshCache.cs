namespace Shadowbane.Cache.CacheTypes
{
    using System;

    public class MeshCache : CacheArchive
    {
        public MeshCache()
            : base("Mesh.cache")
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
    }
}