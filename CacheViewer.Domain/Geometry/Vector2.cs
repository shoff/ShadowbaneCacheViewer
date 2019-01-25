﻿namespace CacheViewer.Domain.Geometry
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Xml.Serialization;

    /// <summary>Represents a 2D vector using two single-precision floating-point numbers.</summary>
    /// <remarks>
    /// The Vector2 structure is suitable for interoperation with unmanaged code requiring two consecutive floats.
    /// </remarks>
    [Serializable]
    public struct Vector2 : IEquatable<Vector2>
    {
        /// <summary>
        /// Defines a unit-length Vector2 that points towards the X-axis.
        /// </summary>
        public static readonly Vector2 UnitX = new Vector2(1f, 0.0f);
        /// <summary>
        /// Defines a unit-length Vector2 that points towards the Y-axis.
        /// </summary>
        public static readonly Vector2 UnitY = new Vector2(0.0f, 1f);
        /// <summary>Defines a zero-length Vector2.</summary>
        public static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);
        /// <summary>Defines an instance with all components set to 1.</summary>
        public static readonly Vector2 One = new Vector2(1f, 1f);
        /// <summary>Defines the size of the Vector2 struct in bytes.</summary>
        public static readonly int SizeInBytes = Marshal.SizeOf((object)new Vector2());
        private static string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
        /// <summary>The X component of the Vector2.</summary>
        public float X;
        /// <summary>The Y component of the Vector2.</summary>
        public float Y;

        /// <summary>Constructs a new instance.</summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public Vector2(float value)
        {
            this.X = value;
            this.Y = value;
        }

        /// <summary>Constructs a new Vector2.</summary>
        /// <param name="x">The x coordinate of the net Vector2.</param>
        /// <param name="y">The y coordinate of the net Vector2.</param>
        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
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

                throw new IndexOutOfRangeException("You tried to access this vector at index: " + (object)index);
            }
            set
            {
                if (index == 0)
                {
                    this.X = value;
                }
                else
                {
                    if (index != 1)
                    {
                        throw new IndexOutOfRangeException("You tried to set this vector at index: " + (object)index);
                    }

                    this.Y = value;
                }
            }
        }

        /// <summary>Gets the length (magnitude) of the vector.</summary>
        /// <see cref="P:Vector2.LengthFast" />
        /// <seealso cref="P:Vector2.LengthSquared" />
        public float Length
        {
            get
            {
                return (float)Math.Sqrt((double)this.X * (double)this.X + (double)this.Y * (double)this.Y);
            }
        }

        /// <summary>Gets an approximation of the vector length (magnitude).</summary>
        /// <remarks>
        /// This property uses an approximation of the square root function to calculate vector magnitude, with
        /// an upper error bound of 0.001.
        /// </remarks>
        /// <see cref="P:Vector2.Length" />
        /// <seealso cref="P:Vector2.LengthSquared" />
        public float LengthFast
        {
            get
            {
                return 1f / MathHelper.InverseSqrtFast((float)((double)this.X * (double)this.X + (double)this.Y * (double)this.Y));
            }
        }

        /// <summary>Gets the square of the vector length (magnitude).</summary>
        /// <remarks>
        /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
        /// for comparisons.
        /// </remarks>
        /// <see cref="P:Vector2.Length" />
        /// <seealso cref="P:Vector2.LengthFast" />
        public float LengthSquared
        {
            get
            {
                return (float)((double)this.X * (double)this.X + (double)this.Y * (double)this.Y);
            }
        }

        /// <summary>
        /// Gets the perpendicular vector on the right side of this vector.
        /// </summary>
        public Vector2 PerpendicularRight
        {
            get
            {
                return new Vector2(this.Y, -this.X);
            }
        }

        /// <summary>
        /// Gets the perpendicular vector on the left side of this vector.
        /// </summary>
        public Vector2 PerpendicularLeft
        {
            get
            {
                return new Vector2(-this.Y, this.X);
            }
        }

        /// <summary>Returns a copy of the Vector2 scaled to unit length.</summary>
        /// <returns></returns>
        public Vector2 Normalized()
        {
            Vector2 vector2 = this;
            vector2.Normalize();
            return vector2;
        }

        /// <summary>Scales the Vector2 to unit length.</summary>
        public void Normalize()
        {
            float num = 1f / this.Length;
            this.X *= num;
            this.Y *= num;
        }

        /// <summary>Scales the Vector2 to approximately unit length.</summary>
        public void NormalizeFast()
        {
            float num = MathHelper.InverseSqrtFast((float)((double)this.X * (double)this.X + (double)this.Y * (double)this.Y));
            this.X *= num;
            this.Y *= num;
        }

        /// <summary>Adds two vectors.</summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Result of operation.</returns>
        public static Vector2 Add(Vector2 a, Vector2 b)
        {
            Vector2.Add(ref a, ref b, out a);
            return a;
        }

        /// <summary>Adds two vectors.</summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <param name="result">Result of operation.</param>
        public static void Add(ref Vector2 a, ref Vector2 b, out Vector2 result)
        {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
        }

        /// <summary>Subtract one Vector from another</summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Result of subtraction</returns>
        public static Vector2 Subtract(Vector2 a, Vector2 b)
        {
            Vector2.Subtract(ref a, ref b, out a);
            return a;
        }

        /// <summary>Subtract one Vector from another</summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">Result of subtraction</param>
        public static void Subtract(ref Vector2 a, ref Vector2 b, out Vector2 result)
        {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
        }

        /// <summary>Multiplies a vector by a scalar.</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static Vector2 Multiply(Vector2 vector, float scale)
        {
            Multiply(ref vector, scale, out vector);
            return vector;
        }

        /// <summary>Multiplies a vector by a scalar.</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Multiply(ref Vector2 vector, float scale, out Vector2 result)
        {
            result.X = vector.X * scale;
            result.Y = vector.Y * scale;
        }

        /// <summary>Multiplies a vector by the components a vector (scale).</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static Vector2 Multiply(Vector2 vector, Vector2 scale)
        {
            Vector2.Multiply(ref vector, ref scale, out vector);
            return vector;
        }

        /// <summary>
        /// Multiplies a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Multiply(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
        {
            result.X = vector.X * scale.X;
            result.Y = vector.Y * scale.Y;
        }

        /// <summary>Divides a vector by a scalar.</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static Vector2 Divide(Vector2 vector, float scale)
        {
            Vector2.Divide(ref vector, scale, out vector);
            return vector;
        }

        /// <summary>Divides a vector by a scalar.</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Divide(ref Vector2 vector, float scale, out Vector2 result)
        {
            result.X = vector.X / scale;
            result.Y = vector.Y / scale;
        }

        /// <summary>Divides a vector by the components of a vector (scale).</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static Vector2 Divide(Vector2 vector, Vector2 scale)
        {
            Vector2.Divide(ref vector, ref scale, out vector);
            return vector;
        }

        /// <summary>Divide a vector by the components of a vector (scale).</summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Divide(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
        {
            result.X = vector.X / scale.X;
            result.Y = vector.Y / scale.Y;
        }

        /// <summary>
        /// Returns a vector created from the smallest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>The component-wise minimum</returns>
        public static Vector2 ComponentMin(Vector2 a, Vector2 b)
        {
            a.X = (double)a.X < (double)b.X ? a.X : b.X;
            a.Y = (double)a.Y < (double)b.Y ? a.Y : b.Y;
            return a;
        }

        /// <summary>
        /// Returns a vector created from the smallest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">The component-wise minimum</param>
        public static void ComponentMin(ref Vector2 a, ref Vector2 b, out Vector2 result)
        {
            result.X = (double)a.X < (double)b.X ? a.X : b.X;
            result.Y = (double)a.Y < (double)b.Y ? a.Y : b.Y;
        }

        /// <summary>
        /// Returns a vector created from the largest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>The component-wise maximum</returns>
        public static Vector2 ComponentMax(Vector2 a, Vector2 b)
        {
            a.X = (double)a.X > (double)b.X ? a.X : b.X;
            a.Y = (double)a.Y > (double)b.Y ? a.Y : b.Y;
            return a;
        }

        /// <summary>
        /// Returns a vector created from the largest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">The component-wise maximum</param>
        public static void ComponentMax(ref Vector2 a, ref Vector2 b, out Vector2 result)
        {
            result.X = (double)a.X > (double)b.X ? a.X : b.X;
            result.Y = (double)a.Y > (double)b.Y ? a.Y : b.Y;
        }

        /// <summary>
        /// Returns the Vector2 with the minimum magnitude. If the magnitudes are equal, the second vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <returns>The minimum Vector2</returns>
        public static Vector2 MagnitudeMin(Vector2 left, Vector2 right)
        {
            if ((double)left.LengthSquared >= (double)right.LengthSquared)
            {
                return right;
            }

            return left;
        }

        /// <summary>
        /// Returns the Vector2 with the minimum magnitude. If the magnitudes are equal, the second vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <param name="result">The magnitude-wise minimum</param>
        /// <returns>The minimum Vector2</returns>
        public static void MagnitudeMin(ref Vector2 left, ref Vector2 right, out Vector2 result)
        {
            result = (double)left.LengthSquared < (double)right.LengthSquared ? left : right;
        }

        /// <summary>
        /// Returns the Vector2 with the maximum magnitude. If the magnitudes are equal, the first vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <returns>The maximum Vector2</returns>
        public static Vector2 MagnitudeMax(Vector2 left, Vector2 right)
        {
            if ((double)left.LengthSquared < (double)right.LengthSquared)
            {
                return right;
            }

            return left;
        }

        /// <summary>
        /// Returns the Vector2 with the maximum magnitude. If the magnitudes are equal, the first vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <param name="result">The magnitude-wise maximum</param>
        /// <returns>The maximum Vector2</returns>
        public static void MagnitudeMax(ref Vector2 left, ref Vector2 right, out Vector2 result)
        {
            result = (double)left.LengthSquared >= (double)right.LengthSquared ? left : right;
        }

        /// <summary>Returns the Vector3 with the minimum magnitude</summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <returns>The minimum Vector3</returns>
        [Obsolete("Use MagnitudeMin() instead.")]
        public static Vector2 Min(Vector2 left, Vector2 right)
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
        public static Vector2 Max(Vector2 left, Vector2 right)
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
        public static Vector2 Clamp(Vector2 vec, Vector2 min, Vector2 max)
        {
            vec.X = (double)vec.X < (double)min.X ? min.X : ((double)vec.X > (double)max.X ? max.X : vec.X);
            vec.Y = (double)vec.Y < (double)min.Y ? min.Y : ((double)vec.Y > (double)max.Y ? max.Y : vec.Y);
            return vec;
        }

        /// <summary>Clamp a vector to the given minimum and maximum vectors</summary>
        /// <param name="vec">Input vector</param>
        /// <param name="min">Minimum vector</param>
        /// <param name="max">Maximum vector</param>
        /// <param name="result">The clamped vector</param>
        public static void Clamp(ref Vector2 vec, ref Vector2 min, ref Vector2 max, out Vector2 result)
        {
            result.X = (double)vec.X < (double)min.X ? min.X : ((double)vec.X > (double)max.X ? max.X : vec.X);
            result.Y = (double)vec.Y < (double)min.Y ? min.Y : ((double)vec.Y > (double)max.Y ? max.Y : vec.Y);
        }

        /// <summary>Compute the euclidean distance between two vectors.</summary>
        /// <param name="vec1">The first vector</param>
        /// <param name="vec2">The second vector</param>
        /// <returns>The distance</returns>
        public static float Distance(Vector2 vec1, Vector2 vec2)
        {
            float result;
            Vector2.Distance(ref vec1, ref vec2, out result);
            return result;
        }

        /// <summary>Compute the euclidean distance between two vectors.</summary>
        /// <param name="vec1">The first vector</param>
        /// <param name="vec2">The second vector</param>
        /// <param name="result">The distance</param>
        public static void Distance(ref Vector2 vec1, ref Vector2 vec2, out float result)
        {
            result = (float)Math.Sqrt(((double)vec2.X - (double)vec1.X) * ((double)vec2.X - (double)vec1.X) + ((double)vec2.Y - (double)vec1.Y) * ((double)vec2.Y - (double)vec1.Y));
        }

        /// <summary>
        /// Compute the squared euclidean distance between two vectors.
        /// </summary>
        /// <param name="vec1">The first vector</param>
        /// <param name="vec2">The second vector</param>
        /// <returns>The squared distance</returns>
        public static float DistanceSquared(Vector2 vec1, Vector2 vec2)
        {
            float result;
            Vector2.DistanceSquared(ref vec1, ref vec2, out result);
            return result;
        }

        /// <summary>
        /// Compute the squared euclidean distance between two vectors.
        /// </summary>
        /// <param name="vec1">The first vector</param>
        /// <param name="vec2">The second vector</param>
        /// <param name="result">The squared distance</param>
        public static void DistanceSquared(ref Vector2 vec1, ref Vector2 vec2, out float result)
        {
            result = (float)(((double)vec2.X - (double)vec1.X) * ((double)vec2.X - (double)vec1.X) + ((double)vec2.Y - (double)vec1.Y) * ((double)vec2.Y - (double)vec1.Y));
        }

        /// <summary>Scale a vector to unit length</summary>
        /// <param name="vec">The input vector</param>
        /// <returns>The normalized vector</returns>
        public static Vector2 Normalize(Vector2 vec)
        {
            float num = 1f / vec.Length;
            vec.X *= num;
            vec.Y *= num;
            return vec;
        }

        /// <summary>Scale a vector to unit length</summary>
        /// <param name="vec">The input vector</param>
        /// <param name="result">The normalized vector</param>
        public static void Normalize(ref Vector2 vec, out Vector2 result)
        {
            float num = 1f / vec.Length;
            result.X = vec.X * num;
            result.Y = vec.Y * num;
        }

        /// <summary>Scale a vector to approximately unit length</summary>
        /// <param name="vec">The input vector</param>
        /// <returns>The normalized vector</returns>
        public static Vector2 NormalizeFast(Vector2 vec)
        {
            float num = MathHelper.InverseSqrtFast((float)((double)vec.X * (double)vec.X + (double)vec.Y * (double)vec.Y));
            vec.X *= num;
            vec.Y *= num;
            return vec;
        }

        /// <summary>Scale a vector to approximately unit length</summary>
        /// <param name="vec">The input vector</param>
        /// <param name="result">The normalized vector</param>
        public static void NormalizeFast(ref Vector2 vec, out Vector2 result)
        {
            float num = MathHelper.InverseSqrtFast((float)((double)vec.X * (double)vec.X + (double)vec.Y * (double)vec.Y));
            result.X = vec.X * num;
            result.Y = vec.Y * num;
        }

        /// <summary>Calculate the dot (scalar) product of two vectors</summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The dot product of the two inputs</returns>
        public static float Dot(Vector2 left, Vector2 right)
        {
            return (float)((double)left.X * (double)right.X + (double)left.Y * (double)right.Y);
        }

        /// <summary>Calculate the dot (scalar) product of two vectors</summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <param name="result">The dot product of the two inputs</param>
        public static void Dot(ref Vector2 left, ref Vector2 right, out float result)
        {
            result = (float)((double)left.X * (double)right.X + (double)left.Y * (double)right.Y);
        }

        /// <summary>
        /// Calculate the perpendicular dot (scalar) product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The perpendicular dot product of the two inputs</returns>
        public static float PerpDot(Vector2 left, Vector2 right)
        {
            return (float)((double)left.X * (double)right.Y - (double)left.Y * (double)right.X);
        }

        /// <summary>
        /// Calculate the perpendicular dot (scalar) product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <param name="result">The perpendicular dot product of the two inputs</param>
        public static void PerpDot(ref Vector2 left, ref Vector2 right, out float result)
        {
            result = (float)((double)left.X * (double)right.Y - (double)left.Y * (double)right.X);
        }

        /// <summary>
        /// Returns a new Vector that is the linear blend of the 2 given Vectors
        /// </summary>
        /// <param name="a">First input vector</param>
        /// <param name="b">Second input vector</param>
        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
        /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise</returns>
        public static Vector2 Lerp(Vector2 a, Vector2 b, float blend)
        {
            a.X = blend * (b.X - a.X) + a.X;
            a.Y = blend * (b.Y - a.Y) + a.Y;
            return a;
        }

        /// <summary>
        /// Returns a new Vector that is the linear blend of the 2 given Vectors
        /// </summary>
        /// <param name="a">First input vector</param>
        /// <param name="b">Second input vector</param>
        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
        /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
        public static void Lerp(ref Vector2 a, ref Vector2 b, float blend, out Vector2 result)
        {
            result.X = blend * (b.X - a.X) + a.X;
            result.Y = blend * (b.Y - a.Y) + a.Y;
        }

        /// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
        /// <param name="a">First input Vector</param>
        /// <param name="b">Second input Vector</param>
        /// <param name="c">Third input Vector</param>
        /// <param name="u">First Barycentric Coordinate</param>
        /// <param name="v">Second Barycentric Coordinate</param>
        /// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</returns>
        public static Vector2 BaryCentric(Vector2 a, Vector2 b, Vector2 c, float u, float v)
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
        public static void BaryCentric(ref Vector2 a, ref Vector2 b, ref Vector2 c, float u, float v, out Vector2 result)
        {
            result = a;
            Vector2 result1 = b;
            Vector2.Subtract(ref result1, ref a, out result1);
            Vector2.Multiply(ref result1, u, out result1);
            Vector2.Add(ref result, ref result1, out result);
            Vector2 result2 = c;
            Vector2.Subtract(ref result2, ref a, out result2);
            Vector2.Multiply(ref result2, v, out result2);
            Vector2.Add(ref result, ref result2, out result);
        }

        /// <summary>Transforms a vector by a quaternion rotation.</summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 Transform(Vector2 vec, Quaternion quat)
        {
            Vector2 result;
            Vector2.Transform(ref vec, ref quat, out result);
            return result;
        }

        /// <summary>Transforms a vector by a quaternion rotation.</summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <param name="result">The result of the operation.</param>
        public static void Transform(ref Vector2 vec, ref Quaternion quat, out Vector2 result)
        {
            Quaternion result1 = new Quaternion(vec.X, vec.Y, 0.0f, 0.0f);
            Quaternion result2;
            Quaternion.Invert(ref quat, out result2);
            Quaternion result3;
            Quaternion.Multiply(ref quat, ref result1, out result3);
            Quaternion.Multiply(ref result3, ref result2, out result1);
            result.X = result1.X;
            result.Y = result1.Y;
        }

        /// <summary>
        /// Gets or sets an Vector2 with the Y and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Yx
        {
            get
            {
                return new Vector2(this.Y, this.X);
            }
            set
            {
                this.Y = value.X;
                this.X = value.Y;
            }
        }

        /// <summary>Adds the specified instances.</summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>Result of addition.</returns>
        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        /// <summary>Subtracts the specified instances.</summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>Result of subtraction.</returns>
        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        /// <summary>Negates the specified instance.</summary>
        /// <param name="vec">Operand.</param>
        /// <returns>Result of negation.</returns>
        public static Vector2 operator -(Vector2 vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            return vec;
        }

        /// <summary>Multiplies the specified instance by a scalar.</summary>
        /// <param name="vec">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector2 operator *(Vector2 vec, float scale)
        {
            vec.X *= scale;
            vec.Y *= scale;
            return vec;
        }

        /// <summary>Multiplies the specified instance by a scalar.</summary>
        /// <param name="scale">Left operand.</param>
        /// <param name="vec">Right operand.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector2 operator *(float scale, Vector2 vec)
        {
            vec.X *= scale;
            vec.Y *= scale;
            return vec;
        }

        /// <summary>
        /// Component-wise multiplication between the specified instance by a scale vector.
        /// </summary>
        /// <param name="scale">Left operand.</param>
        /// <param name="vec">Right operand.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector2 operator *(Vector2 vec, Vector2 scale)
        {
            vec.X *= scale.X;
            vec.Y *= scale.Y;
            return vec;
        }

        /// <summary>Divides the specified instance by a scalar.</summary>
        /// <param name="vec">Left operand</param>
        /// <param name="scale">Right operand</param>
        /// <returns>Result of the division.</returns>
        public static Vector2 operator /(Vector2 vec, float scale)
        {
            vec.X /= scale;
            vec.Y /= scale;
            return vec;
        }

        /// <summary>Compares the specified instances for equality.</summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>True if both instances are equal; false otherwise.</returns>
        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.Equals(right);
        }

        /// <summary>Compares the specified instances for inequality.</summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>True if both instances are not equal; false otherwise.</returns>
        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a System.String that represents the current Vector2.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}{2} {1})", (object)this.X, (object)this.Y, (object)Vector2.listSeparator);
        }

        /// <summary>Returns the hashcode for this instance.</summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() * 397 ^ this.Y.GetHashCode();
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector2))
            {
                return false;
            }

            return this.Equals((Vector2)obj);
        }

        /// <summary>Indicates whether the current vector is equal to another vector.</summary>
        /// <param name="other">A vector to compare with this vector.</param>
        /// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
        public bool Equals(Vector2 other)
        {
            if ((double)this.X == (double)other.X)
            {
                return (double)this.Y == (double)other.Y;
            }

            return false;
        }
    }
}