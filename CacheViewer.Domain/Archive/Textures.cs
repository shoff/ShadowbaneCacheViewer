namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.Textures)]
    public sealed class Textures : CacheArchive
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Textures"/> class.
        /// </summary>
        public Textures()
            : base("Textures.cache"){}
    }
}