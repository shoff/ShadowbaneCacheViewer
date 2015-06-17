
namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.Dungeon)]
    internal sealed class Dungeon : CacheArchive
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dungeon"/> class.
        /// </summary>
        public Dungeon()
            : base("Dungeon.cache") { }
    }
}