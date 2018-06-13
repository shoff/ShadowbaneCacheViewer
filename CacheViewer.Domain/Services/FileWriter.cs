namespace CacheViewer.Domain.Services
{
    using System;
    using System.IO;
    using System.Security;
    using System.Threading.Tasks;

    public class FileWriter
    {
        /// <summary>
        ///     Gets the writer.
        /// </summary>
        public static FileWriter Writer { get; } = new FileWriter();

        /// <summary>
        ///     Writes the asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public async Task WriteAsync(ArraySegment<byte> data, string path)
        {
            await Task.Run(() =>
            {
                FileStream fs = null;

                try
                {
                    fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                    using (var writer = new BinaryWriter(fs))
                    {
                        writer.Write(data.Array);
                    }
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                    }
                }
            });
        }

        /// <summary>
        ///     Writes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="path">The path.</param>
        /// <exception cref="FileNotFoundException">
        ///     The file cannot be found, such as when <paramref name="mode" /> is
        ///     FileMode.Truncate or FileMode.Open, and the file specified by <paramref name="path" /> does not exist. The file
        ///     must already exist in these modes.
        /// </exception>
        /// <exception cref="IOException">
        ///     An I/O error, such as specifying FileMode.CreateNew when the file specified by
        ///     <paramref name="path" /> already exists, occurred. -or-The stream has been closed.
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The
        ///     <paramref>
        ///         <name>access</name>
        ///     </paramref>
        ///     requested is not permitted by the operating system for the specified <paramref name="path" />, such as when
        ///     <paramref name="access" /> is Write or ReadWrite and the file or directory is set for read-only access.
        /// </exception>
        public void Write(ArraySegment<byte> data, string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new BinaryWriter(fs))
                {
                    writer.Write(data.Array);
                }
            }
        }
    }
}