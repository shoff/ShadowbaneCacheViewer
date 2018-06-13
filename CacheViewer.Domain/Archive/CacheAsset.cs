namespace CacheViewer.Domain.Archive
{
    using System;

    public class CacheAsset
    {
        /// <summary>
        ///     Gets the cache index1.
        /// </summary>
        /// <value>
        ///     The cache index1.
        /// </value>
        public CacheIndex CacheIndex1 { get; set; }

        /// <summary>
        ///     Gets the cache index2.
        /// </summary>
        /// <value>
        ///     The cache index2.
        /// </value>
        public CacheIndex CacheIndex2 { get; set; }

        /// <summary>
        ///     Gets the item1.
        /// </summary>
        /// <value>
        ///     The item1.
        /// </value>
        public ArraySegment<byte> Item1 { get; set; }

        /// <summary>
        ///     Gets the item2.
        /// </summary>
        /// <value>
        ///     The item2.
        /// </value>
        public ArraySegment<byte> Item2 { get; set; }

        /// <summary>
        ///     Gets or sets the build time.
        /// </summary>
        /// <value>
        ///     The build time.
        /// </value>
        public long BuildTime { get; set; }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (this.CacheIndex2.Identity == 0)
            {
                return this.CacheIndex1.ToString();
            }

            return this.CacheIndex1 + "\r\n" + this.CacheIndex2;
        }
    }
}