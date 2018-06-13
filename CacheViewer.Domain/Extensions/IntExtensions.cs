namespace CacheViewer.Domain.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        ///     Tests the range.
        /// </summary>
        /// <param name="numberToCheck">The number to check.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public static bool TestRange(this int numberToCheck, int bottom, int top)
        {
            return numberToCheck > bottom && numberToCheck < top;
        }
    }
}