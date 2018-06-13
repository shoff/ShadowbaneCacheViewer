namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.Palette)]
    internal sealed class Palette : CacheArchive
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Palette" /> class.
        /// </summary>
        public Palette()
            : base("Palette.cache")
        {
        }
    }
}