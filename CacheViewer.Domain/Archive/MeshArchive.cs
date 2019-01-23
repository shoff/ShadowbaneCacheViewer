namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.Mesh)]
    public class MeshArchive : CacheArchive
    {
        public MeshArchive()
            : base("Mesh.cache")
        {
        }
    }
}