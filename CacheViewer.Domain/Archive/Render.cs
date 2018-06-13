namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.Render)]
    internal sealed class Render : CacheArchive
    {
        public Render()
            : base("Render.cache")
        {
        }
    }
}