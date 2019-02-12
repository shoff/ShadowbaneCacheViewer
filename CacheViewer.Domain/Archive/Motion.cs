namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.Motion)]
    internal sealed class Motion : CacheArchive
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Motion" /> class.
        /// </summary>
        public Motion()
            : base("Motion.cache")
        {
        }
    }
}