namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.Mesh)]
    internal sealed class MeshArchive : CacheArchive
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MeshArchive" /> class.
        /// </summary>
        public MeshArchive()
            : base("Mesh.cache")
        {
        }
    }
}