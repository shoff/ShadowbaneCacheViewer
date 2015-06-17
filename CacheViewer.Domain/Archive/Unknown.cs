namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.Unknown)]
    internal sealed class Unknown : CacheArchive
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Unknown"/> class.
        /// </summary>
        public Unknown()
            : base("Unknown.cache") { }
    }
}