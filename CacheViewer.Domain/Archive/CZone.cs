namespace CacheViewer.Domain.Archive
{
    using System;
    using System.IO;
    using System.Security;

    [CacheFile(CacheFile.CZone)]
    internal sealed class CZone : CacheArchive
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CZone" /> class.
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     This operation is not supported on the current platform.-or-  specified a
        ///     directory.-or- The caller does not have the required permission.
        /// </exception>
        /// <exception cref="FileNotFoundException">The file specified in  was not found. </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        public CZone()
            : base("CZone.cache")
        {
        }
    }
}