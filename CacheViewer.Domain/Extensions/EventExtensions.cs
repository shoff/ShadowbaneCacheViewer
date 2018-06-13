namespace CacheViewer.Domain.Extensions
{
    using System;

    public static class EventExtensions
    {
        /// <summary>
        ///     Raises the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     sender
        ///     or
        ///     args
        /// </exception>
        public static void Raise(this EventHandler handler, object sender, EventArgs args)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }

            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            if (handler != null)
            {
                handler(sender, args);
            }
        }

        /// <summary>
        ///     Raises the specified handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     sender
        ///     or
        ///     args
        /// </exception>
        public static void Raise<T>(this EventHandler<T> handler, object sender, T args)
            where T : EventArgs
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }

            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            if (handler != null)
            {
                handler(sender, args);
            }
        }

        /// <summary>
        ///     Determines whether the specified handler is null.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns></returns>
        public static bool IsNull(this EventHandler handler)
        {
            return handler == null;
        }

        /// <summary>
        ///     Determines whether the specified handler is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The handler.</param>
        /// <returns></returns>
        public static bool IsNull<T>(this EventHandler<T> handler)
            where T : EventArgs
        {
            return handler == null;
        }
    }
}