using System;

namespace CacheViewer.Domain.Extensions
{
    public static class MathExtensions
    {
        /// <summary>
        /// To the degrees.
        /// </summary>
        /// <param name="inRad">The in RAD.</param>
        /// <returns></returns>
        public static float ToDegrees(this float inRad)
        {
            return inRad * 180 / (float)Math.PI;
        }

        /// <summary>
        /// To the degrees.
        /// </summary>
        /// <param name="inRad">The in RAD.</param>
        /// <returns></returns>
        public static double ToDegrees(this double inRad)
        {
            return inRad * 180 / Math.PI;
        }

        /// <summary>
        /// To the radians.
        /// </summary>
        /// <param name="inDeg">The in deg.</param>
        /// <returns></returns>
        public static float ToRadians(this float inDeg)
        {
            return inDeg * (float)Math.PI / 180;
        }

        /// <summary>
        /// To the radians.
        /// </summary>
        /// <param name="inDeg">The in deg.</param>
        /// <returns></returns>
        public static double ToRadians(this double inDeg)
        {
            return inDeg * Math.PI / 180;
        }
    }
}