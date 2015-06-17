namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.Tile)]
    internal sealed class Tile : CacheArchive
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        public Tile()
            : base("Tile.cache")
        {
        }
    }
}