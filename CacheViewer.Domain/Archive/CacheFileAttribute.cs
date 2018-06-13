namespace CacheViewer.Domain.Archive
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class CacheFileAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CacheFileAttribute" /> class.
        /// </summary>
        /// <param name="cacheFile">The cache file.</param>
        public CacheFileAttribute(CacheFile cacheFile)
        {
            this.TypeOfCacheFile = cacheFile;
        }

        /// <summary>
        ///     Gets the type of cache file.
        /// </summary>
        /// <value>
        ///     The type of cache file.
        /// </value>
        public CacheFile TypeOfCacheFile { get; }
    }
}