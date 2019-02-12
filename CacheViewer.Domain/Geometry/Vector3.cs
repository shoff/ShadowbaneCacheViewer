// ReSharper disable InconsistentNaming
namespace CacheViewer.Domain.Geometry
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using Newtonsoft.Json;

    [Serializable]
    public struct Vector3 : IEquatable<Vector3>
    {
        /// <summary>
        /// Defines a unit-length Vector3 that points towards the X-axis.
        /// </summary>
        public static readonly Vector3 UnitX = new Vector3(1f, 0.0f, 0.0f);
        /// <summary>
        /// Defines a unit-length Vector3 that points towards the Y-axis.
        /// </summary>
        public static readonly Vector3 UnitY = new Vector3(0.0f, 1f, 0.0f);
        /// <summary>
        /// Defines a unit-length Vector3 that points towards the Z-axis.
        /// </summary>
        public static readonly Vector3 UnitZ = new Vector3(0.0f, 0.0f, 1f);
        /// <summary>Defines a zero-length Vector3.</summary>
        public static readonly Vector3 Zero = new Vector3(0.0f, 0.0f, 0.0f);
        /// <summary>Defines an instance with all components set to 1.</summary>
        public static readonly Vector3 One = new Vector3(1f, 1f, 1f);
        /// <summary>Defines the size of the Vector3 struct in bytes.</summary>
        public static readonly int SizeInBytes = Marshal.SizeOf((object)new Vector3());
        private static readonly string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
        /// <summary>The X component of the Vector3.</summary>
        public float X;
        /// <summary>The Y component of the Vector3.</summary>
        public float Y;
        /// <summary>The Z component of the Vector3.</summary>
        public float Z;

        /// <summary>Constructs a new instance.</summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public Vector3(float value)
        {
            this.X = value;
            this.Y = value;
            this.Z = value;
        }

        /// <summary>Constructs a new Vector3.</summary>
        /// <param name="x">The x component of the Vector3.</param>
        /// <param name="y">The y component of the Vector3.</param>
        /// <param name="z">The z component of the Vector3.</param>
        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>Constructs a new Vector3 from the given Vector2.</summary>
        /// <param name="v">The Vector2 to copy components from.</param>
        public Vector3(Vector2 v)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = 0.0f;
        }

        /// <summary>Constructs a new Vector3 from the given Vector3.</summary>
        /// <param name="v">The Vector3 to copy components from.</param>
        public Vector3(Vector3 v)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
        }

        /// <summary>Constructs a new Vector3 from the given Vector4.</summary>
        /// <param name="v">The Vector4 to copy components from.</param>
        public Vector3(Vector4 v)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
        }

        /// <summary>Gets or sets the value at the index of the Vector.</summary>
        public float this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return this.X;
                }

                if (index == 1)
                {
                    return this.Y;
                }

                if (index == 2)
                {
                    return this.Z;
                }

                throw new IndexOutOfRangeException("You tried to access this vector at index: " + (object)index);
            }
            set
            {
                if (index == 0)
                {
                    this.X = value;
                }
                else if (index == 1)
                {
                    this.Y = value;
                }
                else
                {
                    if (index != 2)
                    {
                        throw new IndexOutOfRangeException("You tried to set this vector at index: " + (object)index);
                    }

                    this.Z = value;
                }
            }
        }

        /// <summary>Gets the length (magnitude) of the vector.</summary>
        /// <see cref="P:OpenTK.Vector3.LengthFast" />
        /// <seealso cref="P:OpenTK.Vector3.LengthSquared" />
        public float Length => (float)Math.Sqrt((double)this.X * (double)this.X + (double)this.Y * (double)this.Y + (double)this.Z * (double)this.Z);

        /// <summary>Gets an approximation of the vector length (magnitude).</summary>
        /// <remarks>
        /// This property uses an approximation of the square root function to calculate vector magnitude, with
        /// an upper error bound of 0.001.
        /// </remarks>
        /// <see cref="P:OpenTK.Vector3.Length" />
        /// <seealso cref="P:OpenTK.Vector3.LengthSquared" />
        public float LengthFast => 1f / MathHelper.InverseSqrtFast((float)((double)this.X * (double)this.X + (double)this.Y * (double)this.Y + (double)this.Z * (double)this.Z));

        /// <summary>Gets the square of the vector length (magnitude).</summary>
        /// <remarks>
        /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
        /// for comparisons.
        /// </remarks>
        /// <see cref="P:OpenTK.Vector3.Length" />
        /// <seealso cref="P:OpenTK.Vector3.LengthFast" />
        public float LengthSquared => (float)((double)this.X * (double)this.X + (double)this.Y * (double)this.Y + (double)this.Z * (double)this.Z);

        /// <summary>Returns a copy of the Vector3 scaled to unit length.</summary>
        public Vector3 Normalized()
        {
            Vector3 vector3 = this;
            vector3.Normalize();
            return vector3;
        }

        /// <summary>Scales the Vector3 to unit length.</summary>
        public void Normalize()
        {
            float num = 1f / this.Length;
            this.X *= num;
            this.Y *= num;
            this.Z *= num;
        }

        /// <summary>Scales the Vector3 to approximately unit length.</summary>
        public void NormalizeFast()
        {
            float num = MathHelper.InverseSqrtFast((float)((double)this.X * (double)this.X + (double)this.Y * (double)this.Y + (double)this.Z * (double)this.Z));
            this.X *= num;
            this.Y *= num;
            this.Z *= num;
        }

        /// <summary>Adds two vectors.</summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Result of operation.</returns>
        public static Vector3 Add(Vector3 a, Vector3 b)
        {
            Vector3.Add(ref a, ref b, out a);
            return a;
        }

        /// <summary>Adds two vectors.</summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <param name="result">Result of operation.</param>
        public static void Add(ref Vector3 a, ref Vector3 b, out Vector3 result)
        {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Z = a.Z + b.Z;
        }

        /// <summary>Subtract one Vector from another</summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Result of subtraction</returns>
        public static Vector3 Subtract(Vector3 a, Vector3 b)
        {
            Vector3.Subtract(ref a, ref b, out a);
            return a;
        }

        /// <summary>Subtract one Vector from another</summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">Result of subtraction</param>
        public static void Subtract(ref Vector3 a, ref Vector3 b, out Vector3 result)
        {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
            result.Z = a.Z - b.Z;
        }

        /// <summary>Multiplies a vector by a scalar.</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static Vector3 Multiply(Vector3 vector, float scale)
        {
            Vector3.Multiply(ref vector, scale, out vector);
            return vector;
        }

        /// <summary>Multiplies a vector by a scalar.</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Multiply(ref Vector3 vector, float scale, out Vector3 result)
        {
            result.X = vector.X * scale;
            result.Y = vector.Y * scale;
            result.Z = vector.Z * scale;
        }

        /// <summary>Multiplies a vector by the components a vector (scale).</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static Vector3 Multiply(Vector3 vector, Vector3 scale)
        {
            Vector3.Multiply(ref vector, ref scale, out vector);
            return vector;
        }

        /// <summary>
        /// Multiplies a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Multiply(ref Vector3 vector, ref Vector3 scale, out Vector3 result)
        {
            result.X = vector.X * scale.X;
            result.Y = vector.Y * scale.Y;
            result.Z = vector.Z * scale.Z;
        }

        /// <summary>Divides a vector by a scalar.</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static Vector3 Divide(Vector3 vector, float scale)
        {
            Vector3.Divide(ref vector, scale, out vector);
            return vector;
        }

        /// <summary>Divides a vector by a scalar.</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Divide(ref Vector3 vector, float scale, out Vector3 result)
        {
            result.X = vector.X / scale;
            result.Y = vector.Y / scale;
            result.Z = vector.Z / scale;
        }

        /// <summary>Divides a vector by the components of a vector (scale).</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static Vector3 Divide(Vector3 vector, Vector3 scale)
        {
            Vector3.Divide(ref vector, ref scale, out vector);
            return vector;
        }

        /// <summary>Divide a vector by the components of a vector (scale).</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Divide(ref Vector3 vector, ref Vector3 scale, out Vector3 result)
        {
            result.X = vector.X / scale.X;
            result.Y = vector.Y / scale.Y;
            result.Z = vector.Z / scale.Z;
        }

        /// <summary>
        /// Returns a vector created from the smallest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>The component-wise minimum</returns>
        public static Vector3 ComponentMin(Vector3 a, Vector3 b)
        {
            a.X = (double)a.X < (double)b.X ? a.X : b.X;
            a.Y = (double)a.Y < (double)b.Y ? a.Y : b.Y;
            a.Z = (double)a.Z < (double)b.Z ? a.Z : b.Z;
            return a;
        }

        /// <summary>
        /// Returns a vector created from the smallest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">The component-wise minimum</param>
        public static void ComponentMin(ref Vector3 a, ref Vector3 b, out Vector3 result)
        {
            result.X = (double)a.X < (double)b.X ? a.X : b.X;
            result.Y = (double)a.Y < (double)b.Y ? a.Y : b.Y;
            result.Z = (double)a.Z < (double)b.Z ? a.Z : b.Z;
        }

        /// <summary>
        /// Returns a vector created from the largest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>The component-wise maximum</returns>
        public static Vector3 ComponentMax(Vector3 a, Vector3 b)
        {
            a.X = (double)a.X > (double)b.X ? a.X : b.X;
            a.Y = (double)a.Y > (double)b.Y ? a.Y : b.Y;
            a.Z = (double)a.Z > (double)b.Z ? a.Z : b.Z;
            return a;
        }

        /// <summary>
        /// Returns a vector created from the largest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">The component-wise maximum</param>
        public static void ComponentMax(ref Vector3 a, ref Vector3 b, out Vector3 result)
        {
            result.X = (double)a.X > (double)b.X ? a.X : b.X;
            result.Y = (double)a.Y > (double)b.Y ? a.Y : b.Y;
            result.Z = (double)a.Z > (double)b.Z ? a.Z : b.Z;
        }

        /// <summary>
        /// Returns the Vector3 with the minimum magnitude. If the magnitudes are equal, the second vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <returns>The minimum Vector3</returns>
        public static Vector3 MagnitudeMin(Vector3 left, Vector3 right)
        {
            if ((double)left.LengthSquared >= (double)right.LengthSquared)
            {
                return right;
            }

            return left;
        }

        /// <summary>
        /// Returns the Vector3 with the minimum magnitude. If the magnitudes are equal, the second vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <param name="result">The magnitude-wise minimum</param>
        /// <returns>The minimum Vector3</returns>
        public static void MagnitudeMin(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result = (double)left.LengthSquared < (double)right.LengthSquared ? left : right;
        }

        /// <summary>
        /// Returns the Vector3 with the maximum magnitude. If the magnitudes are equal, the first vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <returns>The maximum Vector3</returns>
        public static Vector3 MagnitudeMax(Vector3 left, Vector3 right)
        {
            if ((double)left.LengthSquared < (double)right.LengthSquared)
            {
                return right;
            }

            return left;
        }

        /// <summary>
        /// Returns the Vector3 with the maximum magnitude. If the magnitudes are equal, the first vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <param name="result">The magnitude-wise maximum</param>
        /// <returns>The maximum Vector3</returns>
        public static void MagnitudeMax(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result = (double)left.LengthSquared >= (double)right.LengthSquared ? left : right;
        }

        /// <summary>Returns the Vector3 with the minimum magnitude</summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <returns>The minimum Vector3</returns>
        [Obsolete("Use MagnitudeMin() instead.")]
        public static Vector3 Min(Vector3 left, Vector3 right)
        {
            if ((double)left.LengthSquared >= (double)right.LengthSquared)
            {
                return right;
            }

            return left;
        }

        /// <summary>Returns the Vector3 with the minimum magnitude</summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <returns>The minimum Vector3</returns>
        [Obsolete("Use MagnitudeMax() instead.")]
        public static Vector3 Max(Vector3 left, Vector3 right)
        {
            if ((double)left.LengthSquared < (double)right.LengthSquared)
            {
                return right;
            }

            return left;
        }

        /// <summary>Clamp a vector to the given minimum and maximum vectors</summary>
        /// <param name="vec">Input vector</param>
        /// <param name="min">Minimum vector</param>
        /// <param name="max">Maximum vector</param>
        /// <returns>The clamped vector</returns>
        public static Vector3 Clamp(Vector3 vec, Vector3 min, Vector3 max)
        {
            vec.X = (double)vec.X < (double)min.X ? min.X : ((double)vec.X > (double)max.X ? max.X : vec.X);
            vec.Y = (double)vec.Y < (double)min.Y ? min.Y : ((double)vec.Y > (double)max.Y ? max.Y : vec.Y);
            vec.Z = (double)vec.Z < (double)min.Z ? min.Z : ((double)vec.Z > (double)max.Z ? max.Z : vec.Z);
            return vec;
        }

        /// <summary>Clamp a vector to the given minimum and maximum vectors</summary>
        /// <param name="vec">Input vector</param>
        /// <param name="min">Minimum vector</param>
        /// <param name="max">Maximum vector</param>
        /// <param name="result">The clamped vector</param>
        public static void Clamp(ref Vector3 vec, ref Vector3 min, ref Vector3 max, out Vector3 result)
        {
            result.X = (double)vec.X < (double)min.X ? min.X : ((double)vec.X > (double)max.X ? max.X : vec.X);
            result.Y = (double)vec.Y < (double)min.Y ? min.Y : ((double)vec.Y > (double)max.Y ? max.Y : vec.Y);
            result.Z = (double)vec.Z < (double)min.Z ? min.Z : ((double)vec.Z > (double)max.Z ? max.Z : vec.Z);
        }

        /// <summary>Compute the euclidean distance between two vectors.</summary>
        /// <param name="vec1">The first vector</param>
        /// <param name="vec2">The second vector</param>
        /// <returns>The distance</returns>
        public static float Distance(Vector3 vec1, Vector3 vec2)
        {
            float result;
            Vector3.Distance(ref vec1, ref vec2, out result);
            return result;
        }

        /// <summary>Compute the euclidean distance between two vectors.</summary>
        /// <param name="vec1">The first vector</param>
        /// <param name="vec2">The second vector</param>
        /// <param name="result">The distance</param>
        public static void Distance(ref Vector3 vec1, ref Vector3 vec2, out float result)
        {
            result = (float)Math.Sqrt(((double)vec2.X - (double)vec1.X) * ((double)vec2.X - (double)vec1.X) + ((double)vec2.Y - (double)vec1.Y) * ((double)vec2.Y - (double)vec1.Y) + ((double)vec2.Z - (double)vec1.Z) * ((double)vec2.Z - (double)vec1.Z));
        }

        /// <summary>
        /// Compute the squared euclidean distance between two vectors.
        /// </summary>
        /// <param name="vec1">The first vector</param>
        /// <param name="vec2">The second vector</param>
        /// <returns>The squared distance</returns>
        public static float DistanceSquared(Vector3 vec1, Vector3 vec2)
        {
            float result;
            Vector3.DistanceSquared(ref vec1, ref vec2, out result);
            return result;
        }

        /// <summary>
        /// Compute the squared euclidean distance between two vectors.
        /// </summary>
        /// <param name="vec1">The first vector</param>
        /// <param name="vec2">The second vector</param>
        /// <param name="result">The squared distance</param>
        public static void DistanceSquared(ref Vector3 vec1, ref Vector3 vec2, out float result)
        {
            result = (float)(((double)vec2.X - (double)vec1.X) * ((double)vec2.X - (double)vec1.X) + ((double)vec2.Y - (double)vec1.Y) * ((double)vec2.Y - (double)vec1.Y) + ((double)vec2.Z - (double)vec1.Z) * ((double)vec2.Z - (double)vec1.Z));
        }

        /// <summary>Scale a vector to unit length</summary>
        /// <param name="vec">The input vector</param>
        /// <returns>The normalized vector</returns>
        public static Vector3 Normalize(Vector3 vec)
        {
            float num = 1f / vec.Length;
            vec.X *= num;
            vec.Y *= num;
            vec.Z *= num;
            return vec;
        }

        /// <summary>Scale a vector to unit length</summary>
        /// <param name="vec">The input vector</param>
        /// <param name="result">The normalized vector</param>
        public static void Normalize(ref Vector3 vec, out Vector3 result)
        {
            float num = 1f / vec.Length;
            result.X = vec.X * num;
            result.Y = vec.Y * num;
            result.Z = vec.Z * num;
        }

        /// <summary>Scale a vector to approximately unit length</summary>
        /// <param name="vec">The input vector</param>
        /// <returns>The normalized vector</returns>
        public static Vector3 NormalizeFast(Vector3 vec)
        {
            float num = MathHelper.InverseSqrtFast((float)((double)vec.X * (double)vec.X + (double)vec.Y * (double)vec.Y + (double)vec.Z * (double)vec.Z));
            vec.X *= num;
            vec.Y *= num;
            vec.Z *= num;
            return vec;
        }

        /// <summary>Scale a vector to approximately unit length</summary>
        /// <param name="vec">The input vector</param>
        /// <param name="result">The normalized vector</param>
        public static void NormalizeFast(ref Vector3 vec, out Vector3 result)
        {
            float num = MathHelper.InverseSqrtFast((float)((double)vec.X * (double)vec.X + (double)vec.Y * (double)vec.Y + (double)vec.Z * (double)vec.Z));
            result.X = vec.X * num;
            result.Y = vec.Y * num;
            result.Z = vec.Z * num;
        }

        /// <summary>Calculate the dot (scalar) product of two vectors</summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The dot product of the two inputs</returns>
        public static float Dot(Vector3 left, Vector3 right)
        {
            return (float)((double)left.X * (double)right.X + (double)left.Y * (double)right.Y + (double)left.Z * (double)right.Z);
        }

        /// <summary>Calculate the dot (scalar) product of two vectors</summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <param name="result">The dot product of the two inputs</param>
        public static void Dot(ref Vector3 left, ref Vector3 right, out float result)
        {
            result = (float)((double)left.X * (double)right.X + (double)left.Y * (double)right.Y + (double)left.Z * (double)right.Z);
        }

        /// <summary>Caclulate the cross (vector) product of two vectors</summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The cross product of the two inputs</returns>
        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            Vector3 result;
            Vector3.Cross(ref left, ref right, out result);
            return result;
        }

        /// <summary>Caclulate the cross (vector) product of two vectors</summary>
        /// <remarks>
        /// It is incorrect to call this method passing the same variable for
        /// <paramref name="result" /> as for <paramref name="left" /> or
        /// <paramref name="right" />.
        /// </remarks>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The cross product of the two inputs</returns>
        /// <param name="result">The cross product of the two inputs</param>
        public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result.X = (float)((double)left.Y * (double)right.Z - (double)left.Z * (double)right.Y);
            result.Y = (float)((double)left.Z * (double)right.X - (double)left.X * (double)right.Z);
            result.Z = (float)((double)left.X * (double)right.Y - (double)left.Y * (double)right.X);
        }

        /// <summary>
        /// Returns a new Vector that is the linear blend of the 2 given Vectors
        /// </summary>
        /// <param name="a">First input vector</param>
        /// <param name="b">Second input vector</param>
        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
        /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise</returns>
        public static Vector3 Lerp(Vector3 a, Vector3 b, float blend)
        {
            a.X = blend * (b.X - a.X) + a.X;
            a.Y = blend * (b.Y - a.Y) + a.Y;
            a.Z = blend * (b.Z - a.Z) + a.Z;
            return a;
        }

        /// <summary>
        /// Returns a new Vector that is the linear blend of the 2 given Vectors
        /// </summary>
        /// <param name="a">First input vector</param>
        /// <param name="b">Second input vector</param>
        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
        /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
        public static void Lerp(ref Vector3 a, ref Vector3 b, float blend, out Vector3 result)
        {
            result.X = blend * (b.X - a.X) + a.X;
            result.Y = blend * (b.Y - a.Y) + a.Y;
            result.Z = blend * (b.Z - a.Z) + a.Z;
        }

        /// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
        /// <param name="a">First input Vector</param>
        /// <param name="b">Second input Vector</param>
        /// <param name="c">Third input Vector</param>
        /// <param name="u">First Barycentric Coordinate</param>
        /// <param name="v">Second Barycentric Coordinate</param>
        /// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</returns>
        public static Vector3 BaryCentric(Vector3 a, Vector3 b, Vector3 c, float u, float v)
        {
            return a + u * (b - a) + v * (c - a);
        }

        /// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
        /// <param name="a">First input Vector.</param>
        /// <param name="b">Second input Vector.</param>
        /// <param name="c">Third input Vector.</param>
        /// <param name="u">First Barycentric Coordinate.</param>
        /// <param name="v">Second Barycentric Coordinate.</param>
        /// <param name="result">Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</param>
        public static void BaryCentric(ref Vector3 a, ref Vector3 b, ref Vector3 c, float u, float v, out Vector3 result)
        {
            result = a;
            Vector3 result1 = b;
            Vector3.Subtract(ref result1, ref a, out result1);
            Vector3.Multiply(ref result1, u, out result1);
            Vector3.Add(ref result, ref result1, out result);
            Vector3 result2 = c;
            Vector3.Subtract(ref result2, ref a, out result2);
            Vector3.Multiply(ref result2, v, out result2);
            Vector3.Add(ref result, ref result2, out result);
        }

        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3 TransformVector(Vector3 vec, Matrix4 mat)
        {
            Vector3 result;
            Vector3.TransformVector(ref vec, ref mat, out result);
            return result;
        }

        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <remarks>
        /// It is incorrect to call this method passing the same variable for
        /// <paramref name="result" /> as for <paramref name="vec" />.
        /// </remarks>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed vector</param>
        public static void TransformVector(ref Vector3 vec, ref Matrix4 mat, out Vector3 result)
        {
            result.X = (float)((double)vec.X * (double)mat.Row0.X + (double)vec.Y * (double)mat.Row1.X + (double)vec.Z * (double)mat.Row2.X);
            result.Y = (float)((double)vec.X * (double)mat.Row0.Y + (double)vec.Y * (double)mat.Row1.Y + (double)vec.Z * (double)mat.Row2.Y);
            result.Z = (float)((double)vec.X * (double)mat.Row0.Z + (double)vec.Y * (double)mat.Row1.Z + (double)vec.Z * (double)mat.Row2.Z);
        }

        /// <summary>Transform a Normal by the given Matrix</summary>
        /// <remarks>
        /// This calculates the inverse of the given matrix, use TransformNormalInverse if you
        /// already have the inverse to avoid this extra calculation
        /// </remarks>
        /// <param name="norm">The normal to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed normal</returns>
        public static Vector3 TransformNormal(Vector3 norm, Matrix4 mat)
        {
            Vector3 result;
            Vector3.TransformNormal(ref norm, ref mat, out result);
            return result;
        }

        /// <summary>Transform a Normal by the given Matrix</summary>
        /// <remarks>
        /// This calculates the inverse of the given matrix, use TransformNormalInverse if you
        /// already have the inverse to avoid this extra calculation
        /// </remarks>
        /// <param name="norm">The normal to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed normal</param>
        public static void TransformNormal(ref Vector3 norm, ref Matrix4 mat, out Vector3 result)
        {
            Matrix4 invMat = Matrix4.Invert(mat);
            Vector3.TransformNormalInverse(ref norm, ref invMat, out result);
        }

        /// <summary>Transform a Normal by the (transpose of the) given Matrix</summary>
        /// <remarks>
        /// This version doesn't calculate the inverse matrix.
        /// Use this version if you already have the inverse of the desired transform to hand
        /// </remarks>
        /// <param name="norm">The normal to transform</param>
        /// <param name="invMat">The inverse of the desired transformation</param>
        /// <returns>The transformed normal</returns>
        public static Vector3 TransformNormalInverse(Vector3 norm, Matrix4 invMat)
        {
            Vector3 result;
            Vector3.TransformNormalInverse(ref norm, ref invMat, out result);
            return result;
        }

        /// <summary>Transform a Normal by the (transpose of the) given Matrix</summary>
        /// <remarks>
        /// This version doesn't calculate the inverse matrix.
        /// Use this version if you already have the inverse of the desired transform to hand
        /// </remarks>
        /// <param name="norm">The normal to transform</param>
        /// <param name="invMat">The inverse of the desired transformation</param>
        /// <param name="result">The transformed normal</param>
        public static void TransformNormalInverse(ref Vector3 norm, ref Matrix4 invMat, out Vector3 result)
        {
            result.X = (float)((double)norm.X * (double)invMat.Row0.X + (double)norm.Y * (double)invMat.Row0.Y + (double)norm.Z * (double)invMat.Row0.Z);
            result.Y = (float)((double)norm.X * (double)invMat.Row1.X + (double)norm.Y * (double)invMat.Row1.Y + (double)norm.Z * (double)invMat.Row1.Z);
            result.Z = (float)((double)norm.X * (double)invMat.Row2.X + (double)norm.Y * (double)invMat.Row2.Y + (double)norm.Z * (double)invMat.Row2.Z);
        }

        /// <summary>Transform a Position by the given Matrix</summary>
        /// <param name="pos">The position to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed position</returns>
        public static Vector3 TransformPosition(Vector3 pos, Matrix4 mat)
        {
            Vector3 result;
            Vector3.TransformPosition(ref pos, ref mat, out result);
            return result;
        }

        /// <summary>Transform a Position by the given Matrix</summary>
        /// <param name="pos">The position to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed position</param>
        public static void TransformPosition(ref Vector3 pos, ref Matrix4 mat, out Vector3 result)
        {
            result.X = (float)((double)pos.X * (double)mat.Row0.X + (double)pos.Y * (double)mat.Row1.X + (double)pos.Z * (double)mat.Row2.X) + mat.Row3.X;
            result.Y = (float)((double)pos.X * (double)mat.Row0.Y + (double)pos.Y * (double)mat.Row1.Y + (double)pos.Z * (double)mat.Row2.Y) + mat.Row3.Y;
            result.Z = (float)((double)pos.X * (double)mat.Row0.Z + (double)pos.Y * (double)mat.Row1.Z + (double)pos.Z * (double)mat.Row2.Z) + mat.Row3.Z;
        }

        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3 Transform(Vector3 vec, Matrix3 mat)
        {
            Vector3 result;
            Vector3.Transform(ref vec, ref mat, out result);
            return result;
        }

        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed vector</param>
        public static void Transform(ref Vector3 vec, ref Matrix3 mat, out Vector3 result)
        {
            result.X = (float)((double)vec.X * (double)mat.Row0.X + (double)vec.Y * (double)mat.Row1.X + (double)vec.Z * (double)mat.Row2.X);
            result.Y = (float)((double)vec.X * (double)mat.Row0.Y + (double)vec.Y * (double)mat.Row1.Y + (double)vec.Z * (double)mat.Row2.Y);
            result.Z = (float)((double)vec.X * (double)mat.Row0.Z + (double)vec.Y * (double)mat.Row1.Z + (double)vec.Z * (double)mat.Row2.Z);
        }

        /// <summary>Transforms a vector by a quaternion rotation.</summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 Transform(Vector3 vec, Quaternion quat)
        {
            Vector3 result;
            Vector3.Transform(ref vec, ref quat, out result);
            return result;
        }

        /// <summary>Transforms a vector by a quaternion rotation.</summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <param name="result">The result of the operation.</param>
        public static void Transform(ref Vector3 vec, ref Quaternion quat, out Vector3 result)
        {
            Vector3 xyz = quat.Xyz;
            Vector3 result1;
            Vector3.Cross(ref xyz, ref vec, out result1);
            Vector3 result2;
            Vector3.Multiply(ref vec, quat.W, out result2);
            Vector3.Add(ref result1, ref result2, out result1);
            Vector3.Cross(ref xyz, ref result1, out result2);
            Vector3.Multiply(ref result2, 2f, out result2);
            Vector3.Add(ref vec, ref result2, out result);
        }

        /// <summary>Transform a Vector by the given Matrix using right-handed notation</summary>
        /// <param name="mat">The desired transformation</param>
        /// <param name="vec">The vector to transform</param>
        public static Vector3 Transform(Matrix3 mat, Vector3 vec)
        {
            Vector3 result;
            Vector3.Transform(ref vec, ref mat, out result);
            return result;
        }

        /// <summary>Transform a Vector by the given Matrix using right-handed notation</summary>
        /// <param name="mat">The desired transformation</param>
        /// <param name="vec">The vector to transform</param>
        /// <param name="result">The transformed vector</param>
        public static void Transform(ref Matrix3 mat, ref Vector3 vec, out Vector3 result)
        {
            result.X = (float)((double)mat.Row0.X * (double)vec.X + (double)mat.Row0.Y * (double)vec.Y + (double)mat.Row0.Z * (double)vec.Z);
            result.Y = (float)((double)mat.Row1.X * (double)vec.X + (double)mat.Row1.Y * (double)vec.Y + (double)mat.Row1.Z * (double)vec.Z);
            result.Z = (float)((double)mat.Row2.X * (double)vec.X + (double)mat.Row2.Y * (double)vec.Y + (double)mat.Row2.Z * (double)vec.Z);
        }

        /// <summary>Transform a Vector3 by the given Matrix, and project the resulting Vector4 back to a Vector3</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3 TransformPerspective(Vector3 vec, Matrix4 mat)
        {
            Vector3 result;
            Vector3.TransformPerspective(ref vec, ref mat, out result);
            return result;
        }

        /// <summary>Transform a Vector3 by the given Matrix, and project the resulting Vector4 back to a Vector3</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed vector</param>
        public static void TransformPerspective(ref Vector3 vec, ref Matrix4 mat, out Vector3 result)
        {
            Vector4 result1 = new Vector4(vec.X, vec.Y, vec.Z, 1f);
            Vector4.Transform(ref result1, ref mat, out result1);
            result.X = result1.X / result1.W;
            result.Y = result1.Y / result1.W;
            result.Z = result1.Z / result1.W;
        }

        /// <summary>Calculates the angle (in radians) between two vectors.</summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>Angle (in radians) between the vectors.</returns>
        /// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
        public static float CalculateAngle(Vector3 first, Vector3 second)
        {
            float result;
            Vector3.CalculateAngle(ref first, ref second, out result);
            return result;
        }

        /// <summary>Calculates the angle (in radians) between two vectors.</summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <param name="result">Angle (in radians) between the vectors.</param>
        /// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
        public static void CalculateAngle(ref Vector3 first, ref Vector3 second, out float result)
        {
            float result1;
            Vector3.Dot(ref first, ref second, out result1);
            result = (float)Math.Acos(MathHelper.Clamp((double)result1 / ((double)first.Length * (double)second.Length), -1.0, 1.0));
        }

        /// <summary>Projects a vector from object space into screen space.</summary>
        /// <param name="vector">The vector to project.</param>
        /// <param name="x">The X coordinate of the viewport.</param>
        /// <param name="y">The Y coordinate of the viewport.</param>
        /// <param name="width">The width of the viewport.</param>
        /// <param name="height">The height of the viewport.</param>
        /// <param name="minZ">The minimum depth of the viewport.</param>
        /// <param name="maxZ">The maximum depth of the viewport.</param>
        /// <param name="worldViewProjection">The world-view-projection matrix.</param>
        /// <returns>The vector in screen space.</returns>
        /// <remarks>
        /// To project to normalized device coordinates (NDC) use the following parameters:
        /// Project(vector, -1, -1, 2, 2, -1, 1, worldViewProjection).
        /// </remarks>
        public static Vector3 Project(Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix4 worldViewProjection)
        {
            Vector4 vector4_1;
            vector4_1.X = (float)((double)vector.X * (double)worldViewProjection.M11 + (double)vector.Y * (double)worldViewProjection.M21 + (double)vector.Z * (double)worldViewProjection.M31) + worldViewProjection.M41;
            vector4_1.Y = (float)((double)vector.X * (double)worldViewProjection.M12 + (double)vector.Y * (double)worldViewProjection.M22 + (double)vector.Z * (double)worldViewProjection.M32) + worldViewProjection.M42;
            vector4_1.Z = (float)((double)vector.X * (double)worldViewProjection.M13 + (double)vector.Y * (double)worldViewProjection.M23 + (double)vector.Z * (double)worldViewProjection.M33) + worldViewProjection.M43;
            vector4_1.W = (float)((double)vector.X * (double)worldViewProjection.M14 + (double)vector.Y * (double)worldViewProjection.M24 + (double)vector.Z * (double)worldViewProjection.M34) + worldViewProjection.M44;
            Vector4 vector4_2 = vector4_1 / vector4_1.W;
            vector4_2.X = x + width * (float)(((double)vector4_2.X + 1.0) / 2.0);
            vector4_2.Y = y + height * (float)(((double)vector4_2.Y + 1.0) / 2.0);
            vector4_2.Z = minZ + (float)(((double)maxZ - (double)minZ) * (((double)vector4_2.Z + 1.0) / 2.0));
            return new Vector3(vector4_2.X, vector4_2.Y, vector4_2.Z);
        }

        /// <summary>Projects a vector from screen space into object space.</summary>
        /// <param name="vector">The vector to project.</param>
        /// <param name="x">The X coordinate of the viewport.</param>
        /// <param name="y">The Y coordinate of the viewport.</param>
        /// <param name="width">The width of the viewport.</param>
        /// <param name="height">The height of the viewport.</param>
        /// <param name="minZ">The minimum depth of the viewport.</param>
        /// <param name="maxZ">The maximum depth of the viewport.</param>
        /// <param name="inverseWorldViewProjection">The inverse of the world-view-projection matrix.</param>
        /// <returns>The vector in object space.</returns>
        /// <remarks>
        /// To project from normalized device coordinates (NDC) use the following parameters:
        /// Project(vector, -1, -1, 2, 2, -1, 1, inverseWorldViewProjection).
        /// </remarks>
        public static Vector3 Unproject(Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix4 inverseWorldViewProjection)
        {
            Vector4 vector4_1;
            vector4_1.X = (float)(((double)vector.X - (double)x) / (double)width * 2.0 - 1.0);
            vector4_1.Y = (float)(((double)vector.Y - (double)y) / (double)height * 2.0 - 1.0);
            vector4_1.Z = (float)((double)vector.Z / ((double)maxZ - (double)minZ) * 2.0 - 1.0);
            vector4_1.X = (float)((double)vector4_1.X * (double)inverseWorldViewProjection.M11 + (double)vector4_1.Y * (double)inverseWorldViewProjection.M21 + (double)vector4_1.Z * (double)inverseWorldViewProjection.M31) + inverseWorldViewProjection.M41;
            vector4_1.Y = (float)((double)vector4_1.X * (double)inverseWorldViewProjection.M12 + (double)vector4_1.Y * (double)inverseWorldViewProjection.M22 + (double)vector4_1.Z * (double)inverseWorldViewProjection.M32) + inverseWorldViewProjection.M42;
            vector4_1.Z = (float)((double)vector4_1.X * (double)inverseWorldViewProjection.M13 + (double)vector4_1.Y * (double)inverseWorldViewProjection.M23 + (double)vector4_1.Z * (double)inverseWorldViewProjection.M33) + inverseWorldViewProjection.M43;
            vector4_1.W = (float)((double)vector4_1.X * (double)inverseWorldViewProjection.M14 + (double)vector4_1.Y * (double)inverseWorldViewProjection.M24 + (double)vector4_1.Z * (double)inverseWorldViewProjection.M34) + inverseWorldViewProjection.M44;
            Vector4 vector4_2 = vector4_1 / vector4_1.W;
            return new Vector3(vector4_2.X, vector4_2.Y, vector4_2.Z);
        }

        /// <summary>
        /// Gets or sets an OpenTK.Vector2 with the X and Y components of this instance.
        /// </summary>
        [JsonIgnore]
        public Vector2 Xy
        {
            get => new Vector2(this.X, this.Y);
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.Vector2 with the X and Z components of this instance.
        /// </summary>
        [JsonIgnore]
        public Vector2 Xz
        {
            get => new Vector2(this.X, this.Z);
            set
            {
                this.X = value.X;
                this.Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.Vector2 with the Y and X components of this instance.
        /// </summary>
        [JsonIgnore]
        public Vector2 Yx
        {
            get => new Vector2(this.Y, this.X);
            set
            {
                this.Y = value.X;
                this.X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.Vector2 with the Y and Z components of this instance.
        /// </summary>
        [JsonIgnore]
        public Vector2 Yz
        {
            get => new Vector2(this.Y, this.Z);
            set
            {
                this.Y = value.X;
                this.Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.Vector2 with the Z and X components of this instance.
        /// </summary>
        [JsonIgnore]
        public Vector2 Zx
        {
            get => new Vector2(this.Z, this.X);
            set
            {
                this.Z = value.X;
                this.X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.Vector2 with the Z and Y components of this instance.
        /// </summary>
        [JsonIgnore]
        public Vector2 Zy
        {
            get => new Vector2(this.Z, this.Y);
            set
            {
                this.Z = value.X;
                this.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.Vector3 with the X, Z, and Y components of this instance.
        /// </summary>
        [JsonIgnore]
        public Vector3 Xzy
        {
            get => new Vector3(this.X, this.Z, this.Y);
            set
            {
                this.X = value.X;
                this.Z = value.Y;
                this.Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.Vector3 with the Y, X, and Z components of this instance.
        /// </summary>
        [JsonIgnore]
        public Vector3 Yxz
        {
            get => new Vector3(this.Y, this.X, this.Z);
            set
            {
                this.Y = value.X;
                this.X = value.Y;
                this.Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.Vector3 with the Y, Z, and X components of this instance.
        /// </summary>
        [JsonIgnore]
        public Vector3 Yzx
        {
            get => new Vector3(this.Y, this.Z, this.X);
            set
            {
                this.Y = value.X;
                this.Z = value.Y;
                this.X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.Vector3 with the Z, X, and Y components of this instance.
        /// </summary>
        [JsonIgnore]
        public Vector3 Zxy
        {
            get => new Vector3(this.Z, this.X, this.Y);
            set
            {
                this.Z = value.X;
                this.X = value.Y;
                this.Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.Vector3 with the Z, Y, and X components of this instance.
        /// </summary>
        [JsonIgnore]
        public Vector3 Zyx
        {
            get => new Vector3(this.Z, this.Y, this.X);
            set
            {
                this.Z = value.X;
                this.Y = value.Y;
                this.X = value.Z;
            }
        }

        /// <summary>Adds two instances.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            return left;
        }

        /// <summary>Subtracts two instances.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }

        /// <summary>Negates an instance.</summary>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vector3 operator -(Vector3 vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            vec.Z = -vec.Z;
            return vec;
        }

        /// <summary>Multiplies an instance by a scalar.</summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vector3 operator *(Vector3 vec, float scale)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            return vec;
        }

        /// <summary>Multiplies an instance by a scalar.</summary>
        /// <param name="scale">The scalar.</param>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vector3 operator *(float scale, Vector3 vec)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            return vec;
        }

        /// <summary>
        /// Component-wise multiplication between the specified instance by a scale vector.
        /// </summary>
        /// <param name="scale">Left operand.</param>
        /// <param name="vec">Right operand.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector3 operator *(Vector3 vec, Vector3 scale)
        {
            vec.X *= scale.X;
            vec.Y *= scale.Y;
            vec.Z *= scale.Z;
            return vec;
        }

        /// <summary>Transform a Vector by the given Matrix.</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3 operator *(Vector3 vec, Matrix3 mat)
        {
            Vector3 result;
            Vector3.Transform(ref vec, ref mat, out result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation
        /// </summary>
        /// <param name="mat">The desired transformation</param>
        /// <param name="vec">The vector to transform</param>
        /// <returns>The transformed vector</returns>
        public static Vector3 operator *(Matrix3 mat, Vector3 vec)
        {
            Vector3 result;
            Vector3.Transform(ref mat, ref vec, out result);
            return result;
        }

        /// <summary>Transforms a vector by a quaternion rotation.</summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <returns></returns>
        public static Vector3 operator *(Quaternion quat, Vector3 vec)
        {
            Vector3 result;
            Vector3.Transform(ref vec, ref quat, out result);
            return result;
        }

        /// <summary>Divides an instance by a scalar.</summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vector3 operator /(Vector3 vec, float scale)
        {
            vec.X /= scale;
            vec.Y /= scale;
            vec.Z /= scale;
            return vec;
        }

        /// <summary>Compares two instances for equality.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return left.Equals(right);
        }

        /// <summary>Compares two instances for inequality.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equal right; false otherwise.</returns>
        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a System.String that represents the current Vector3.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}{3} {1}{3} {2})", this.X, this.Y, this.Z, listSeparator);
        }

        /// <summary>Returns the hashcode for this instance.</summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            return (this.X.GetHashCode() * 397 ^ this.Y.GetHashCode()) * 397 ^ this.Z.GetHashCode();
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector3))
            {
                return false;
            }

            return this.Equals((Vector3)obj);
        }

        /// <summary>Indicates whether the current vector is equal to another vector.</summary>
        /// <param name="other">A vector to compare with this vector.</param>
        /// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
        public bool Equals(Vector3 other)
        {
            if ((double)this.X == (double)other.X && (double)this.Y == (double)other.Y)
            {
                return (double)this.Z == (double)other.Z;
            }

            return false;
        }
    }
}