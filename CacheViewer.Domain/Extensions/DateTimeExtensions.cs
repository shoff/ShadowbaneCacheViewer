namespace CacheViewer.Domain.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Converts a Unix style timestamp to a .Net <see cref="DateTime" />
        /// </summary>
        /// <param name="unixTimeStamp"> The Unix timestamp. </param>
        /// <returns> </returns>
        public static DateTime FromUnixTimeStamp(this double unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(unixTimeStamp);
        }
    }
}