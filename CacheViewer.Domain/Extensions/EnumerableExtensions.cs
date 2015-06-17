using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace CacheViewer.Domain.Extensions
{
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
            Contract.Requires<ArgumentNullException>(enumerable != null, "enumerable");
            Contract.Requires<ArgumentNullException>(function != null, "function");

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
            Contract.Requires<ArgumentNullException>(enumerable != null);
            Contract.Requires<ArgumentNullException>(function != null);
            foreach (var t in enumerable)
            {
                yield return function(t);
            }
        }   
    }
}