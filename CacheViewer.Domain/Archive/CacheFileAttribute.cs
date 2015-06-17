using System;

namespace CacheViewer.Domain.Archive
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CacheFileAttribute : Attribute
    {
        private readonly CacheFile cacheFile;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheFileAttribute"/> class.
        /// </summary>
        /// <param name="cacheFile">The cache file.</param>
        public CacheFileAttribute(CacheFile cacheFile)
        {
            this.cacheFile = cacheFile;
        }

        /// <summary>
        /// Gets the type of cache file.
        /// </summary>
        /// <value>
        /// The type of cache file.
        /// </value>
        public CacheFile TypeOfCacheFile
        {
            get { return this.cacheFile; }
        }
    }
}