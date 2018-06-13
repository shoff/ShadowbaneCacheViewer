namespace CacheViewer.Domain.Factories
{
    using System;

    /// <summary>
    /// </summary>
    public struct PixelData : IEquatable<PixelData>
    {
        /// <summary>
        ///     The alpha
        /// </summary>
        public byte alpha;

        /// <summary>
        ///     The blue
        /// </summary>
        public byte blue;

        /// <summary>
        ///     The green
        /// </summary>
        public byte green;

        /// <summary>
        ///     The red
        /// </summary>
        public byte red;

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(PixelData other)
        {
            if (this.alpha != other.alpha)
            {
                return false;
            }

            if (this.blue != other.blue)
            {
                return false;
            }

            if (this.green != other.green)
            {
                return false;
            }

            if (this.red != other.red)
            {
                return false;
            }

            return true;
        }
    }
}