
namespace CacheViewer.Domain.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class EnumerableExtensions
    {
        /// <summary>
        ///   Iterates through the specified enumerable object.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="enumerable"> The enumerable. </param>
        /// <param name="function"> The function. </param>
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> function)
        {
            foreach (var t in enumerable)
            {
                function(t);
            }
        }

        /// <summary>
        ///   Apply a given function to each element of a collection, returning a new collection with the items altered by function.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <typeparam name="TR"> The type of the new enumerable. </typeparam>
        /// <param name="enumerable"> The enumerable. </param>
        /// <param name="function"> The function. </param>
        /// <returns> </returns>
        public static IEnumerable<TR> Map<T, TR>(this IEnumerable<T> enumerable, Func<T, TR> function)
        {

            foreach (var t in enumerable)
            {
                yield return function(t);
            }
        }   
    }
}