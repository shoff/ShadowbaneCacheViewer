using System;
using System.Diagnostics.Contracts;
using System.Linq;
using SlimDX;

namespace CacheViewer.Domain.Extensions
{
    public static class VectorExtensions
    {
        /// <summary>To the vector3.</summary>
        /// <param name="vectorString">The vector string.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The value of 'vectorString' cannot be null. </exception>
        /// <exception cref="OverflowException">The array is multidimensional and contains more than 
        /// <see cref="F:System.Int32.MaxValue" /> elements.</exception>
        public static Vector3 ToVector3(this string vectorString)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(vectorString));
            Contract.Requires<ArgumentException>(vectorString.Split(' ').Length == 3);
            Contract.Ensures(Contract.Result<Vector3>() != null);

            // X:1.33823 Y:2.3356 Z:-4.008
            var array = vectorString.Split(' ');

            float[] v = array.Map(x => float.Parse(x.Split(':')[1])).ToArray();

            if (v.Length != 3)
            {
                return Vector3.Zero;
            }

            return new Vector3(v[0], v[1], v[2]);
        }

        /// <summary>To the vector2.</summary>
        /// <param name="vectorString">The vector string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">vectorString</exception>
        /// <exception cref="OverflowException">The array is multidimensional and contains more than 
        /// <see cref="F:System.Int32.MaxValue" /> elements.</exception>
        public static Vector2 ToVector2(this string vectorString)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(vectorString));
            Contract.Requires<ArgumentException>(vectorString.Split(' ').Length >= 2);
            Contract.Ensures(Contract.Result<Vector2>() != null);

            // X:1.33823 Y:2.3356
            var array = vectorString.Split(' ');
            float[] v = array.Map(x => float.Parse(x.Split(':')[1])).ToArray();
            if (v.Length != 2)
            {
                return Vector2.Zero;
            }
            return new Vector2(v[0], v[1]);
        }
    }
}