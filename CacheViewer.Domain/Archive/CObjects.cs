namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.CObjects)]
    internal sealed class CObjects : CacheArchive
    {
        public CObjects()
            : base("CObjects.cache")
        {
        }
    }
}