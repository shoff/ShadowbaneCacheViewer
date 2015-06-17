namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.CZone)]
    internal sealed class CZone : CacheArchive
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CZone"/> class.
        /// </summary>
        public CZone()
            : base ("CZone.cache"){}
    }
}