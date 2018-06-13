namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.Skeleton)]
    internal sealed class SkeletonCache : CacheArchive
    {

        public SkeletonCache()
            : base("Skeleton.cache")
        {
        }
    }
}