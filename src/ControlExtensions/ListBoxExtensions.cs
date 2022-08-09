namespace ControlExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public static class ListBoxExtensions
    {
        /// <summary>
        /// Loads the list items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="lb">The lb.</param>
        /// <param name="items">The items.</param>
        /// <param name="func">The function.</param>
        public static void LoadListItems<T, TK>(this ListBox lb, ICollection<T> items, Func<T, TK> func)
        {
            if (lb.InvokeRequired)
            {
                lb.BeginInvoke(new MethodInvoker(() => lb.LoadListItems(items, func)));
            }
            else
            {
                lb.Items.Clear();
                var newItems = items.Map(func);

                foreach (var item in newItems)
                {
                    lb.Items.Add(item);
                }
            }
        }

        /// <summary>
        ///     Apply a given function to each element of a collection, returning a new collection with the items altered by
        ///     function.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <typeparam name="TR"> The type of the new enumerable. </typeparam>
        /// <param name="enumerable"> The enumerable. </param>
        /// <param name="function"> The function. </param>
        /// <returns> </returns>
        internal static IEnumerable<TR> Map<T, TR>(this IEnumerable<T> enumerable, Func<T, TR> function)
        {
            foreach (var t in enumerable)
            {
                yield return function(t);
            }
        }
    }
}