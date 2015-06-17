namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.Visual)]
    internal sealed class Visual : CacheArchive
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Visual"/> class.
        /// </summary>
        public Visual()
            :base("Visual.cache"){}
    }
}