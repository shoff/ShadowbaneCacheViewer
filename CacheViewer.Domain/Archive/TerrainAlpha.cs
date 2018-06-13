namespace CacheViewer.Domain.Archive
{
    [CacheFile(CacheFile.TerrainAlpha)]
    internal sealed class TerrainAlpha : CacheArchive
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TerrainAlpha" /> class.
        /// </summary>
        public TerrainAlpha()
            : base("TerrainAlpha.cache")
        {
        }
    }
}