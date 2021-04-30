namespace Shadowbane.Geometry
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using static MathHelper;

    public struct Vector2 : IEquatable<Vector2>
    {
        public static readonly Vector2 UnitX = new Vector2(1f, 0.0f);
        public static readonly Vector2 UnitY = new Vector2(0.0f, 1f);
        public static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);
        public static readonly Vector2 One = new Vector2(1f, 1f);
        public static readonly int SizeInBytes = Marshal.SizeOf((object)new Vector2());
        private static readonly string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
        public float X;
        public float Y;
        public Vector2(float value)
        {
            this.X = value;
            this.Y = value;
        }
        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
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

                throw new IndexOutOfRangeException("You tried to access this vector at index: " + index);
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
                        throw new IndexOutOfRangeException("You tried to set this vector at index: " + index);
                    }

                    this.Y = value;
                }
            }
        }
        public float Length => (float)Math.Sqrt(this.X * (double)this.X + this.Y * (double)this.Y);
        public float LengthFast => 1f / InverseSqrtFast((float)(this.X * (double)this.X + this.Y * (double)this.Y));
        public float LengthSquared => (float)(this.X * (double)this.X + this.Y * (double)this.Y);
        public Vector2 PerpendicularRight => new Vector2(this.Y, -this.X);
        public Vector2 PerpendicularLeft => new Vector2(-this.Y, this.X);
        public Vector2 Normalized()
        {
            Vector2 vector2 = this;
            vector2.Normalize();
            return vector2;
        }
        public void Normalize()
        {
            float num = 1f / this.Length;
            this.X *= num;
            this.Y *= num;
        }
        public void NormalizeFast()
        {
            float num = InverseSqrtFast((float)(this.X * (double)this.X + this.Y * (double)this.Y));
            this.X *= num;
            this.Y *= num;
        }
        public static Vector2 Add(Vector2 a, Vector2 b)
        {
            Add(ref a, ref b, out a);
            return a;
        }
        public static void Add(ref Vector2 a, ref Vector2 b, out Vector2 result)
        {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
        }
        public static Vector2 Subtract(Vector2 a, Vector2 b)
        {
            Subtract(ref a, ref b, out a);
            return a;
        }
        public static void Subtract(ref Vector2 a, ref Vector2 b, out Vector2 result)
        {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
        }
        public static Vector2 Multiply(Vector2 vector, float scale)
        {
            Multiply(ref vector, scale, out vector);
            return vector;
        }
        public static void Multiply(ref Vector2 vector, float scale, out Vector2 result)
        {
            result.X = vector.X * scale;
            result.Y = vector.Y * scale;
        }
        public static Vector2 Multiply(Vector2 vector, Vector2 scale)
        {
            Multiply(ref vector, ref scale, out vector);
            return vector;
        }
        public static void Multiply(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
        {
            result.X = vector.X * scale.X;
            result.Y = vector.Y * scale.Y;
        }
        public static Vector2 Divide(Vector2 vector, float scale)
        {
            Divide(ref vector, scale, out vector);
            return vector;
        }
        public static void Divide(ref Vector2 vector, float scale, out Vector2 result)
        {
            result.X = vector.X / scale;
            result.Y = vector.Y / scale;
        }
        public static Vector2 Divide(Vector2 vector, Vector2 scale)
        {
            Divide(ref vector, ref scale, out vector);
            return vector;
        }
        public static void Divide(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
        {
            result.X = vector.X / scale.X;
            result.Y = vector.Y / scale.Y;
        }
        public static Vector2 ComponentMin(Vector2 a, Vector2 b)
        {
            a.X = (double)a.X < (double)b.X ? a.X : b.X;
            a.Y = (double)a.Y < (double)b.Y ? a.Y : b.Y;
            return a;
        }
        public static void ComponentMin(ref Vector2 a, ref Vector2 b, out Vector2 result)
        {
            result.X = (double)a.X < (double)b.X ? a.X : b.X;
            result.Y = (double)a.Y < (double)b.Y ? a.Y : b.Y;
        }
        public static Vector2 ComponentMax(Vector2 a, Vector2 b)
        {
            a.X = (double)a.X > (double)b.X ? a.X : b.X;
            a.Y = (double)a.Y > (double)b.Y ? a.Y : b.Y;
            return a;
        }
        public static void ComponentMax(ref Vector2 a, ref Vector2 b, out Vector2 result)
        {
            result.X = (double)a.X > (double)b.X ? a.X : b.X;
            result.Y = (double)a.Y > (double)b.Y ? a.Y : b.Y;
        }
        public static Vector2 MagnitudeMin(Vector2 left, Vector2 right)
        {
            if (left.LengthSquared >= (double)right.LengthSquared)
            {
                return right;
            }

            return left;
        }
        public static void MagnitudeMin(ref Vector2 left, ref Vector2 right, out Vector2 result)
        {
            result = (double)left.LengthSquared < (double)right.LengthSquared ? left : right;
        }
        public static Vector2 MagnitudeMax(Vector2 left, Vector2 right)
        {
            if (left.LengthSquared < (double)right.LengthSquared)
            {
                return right;
            }

            return left;
        }
        public static void MagnitudeMax(ref Vector2 left, ref Vector2 right, out Vector2 result)
        {
            result = (double)left.LengthSquared >= (double)right.LengthSquared ? left : right;
        }
        [Obsolete("Use MagnitudeMin() instead.")]
        public static Vector2 Min(Vector2 left, Vector2 right)
        {
            if (left.LengthSquared >= (double)right.LengthSquared)
            {
                return right;
            }

            return left;
        }
        [Obsolete("Use MagnitudeMax() instead.")]
        public static Vector2 Max(Vector2 left, Vector2 right)
        {
            if (left.LengthSquared < (double)right.LengthSquared)
            {
                return right;
            }

            return left;
        }
        public static Vector2 Clamp(Vector2 vec, Vector2 min, Vector2 max)
        {
            vec.X = (double)vec.X < (double)min.X ? min.X : ((double)vec.X > (double)max.X ? max.X : vec.X);
            vec.Y = (double)vec.Y < (double)min.Y ? min.Y : ((double)vec.Y > (double)max.Y ? max.Y : vec.Y);
            return vec;
        }
        public static void Clamp(ref Vector2 vec, ref Vector2 min, ref Vector2 max, out Vector2 result)
        {
            result.X = (double)vec.X < (double)min.X ? min.X : ((double)vec.X > (double)max.X ? max.X : vec.X);
            result.Y = (double)vec.Y < (double)min.Y ? min.Y : ((double)vec.Y > (double)max.Y ? max.Y : vec.Y);
        }
        public static float Distance(Vector2 vec1, Vector2 vec2)
        {
            float result;
            Distance(ref vec1, ref vec2, out result);
            return result;
        }
        public static void Distance(ref Vector2 vec1, ref Vector2 vec2, out float result)
        {
            result = (float)Math.Sqrt((vec2.X - (double)vec1.X) * (vec2.X - (double)vec1.X) + (vec2.Y - (double)vec1.Y) * (vec2.Y - (double)vec1.Y));
        }
        public static float DistanceSquared(Vector2 vec1, Vector2 vec2)
        {
            float result;
            DistanceSquared(ref vec1, ref vec2, out result);
            return result;
        }
        public static void DistanceSquared(ref Vector2 vec1, ref Vector2 vec2, out float result)
        {
            result = (float)((vec2.X - (double)vec1.X) * (vec2.X - (double)vec1.X) + (vec2.Y - (double)vec1.Y) * (vec2.Y - (double)vec1.Y));
        }
        public static Vector2 Normalize(Vector2 vec)
        {
            float num = 1f / vec.Length;
            vec.X *= num;
            vec.Y *= num;
            return vec;
        }
        public static void Normalize(ref Vector2 vec, out Vector2 result)
        {
            float num = 1f / vec.Length;
            result.X = vec.X * num;
            result.Y = vec.Y * num;
        }
        public static Vector2 NormalizeFast(Vector2 vec)
        {
            float num = InverseSqrtFast((float)(vec.X * (double)vec.X + vec.Y * (double)vec.Y));
            vec.X *= num;
            vec.Y *= num;
            return vec;
        }
        public static void NormalizeFast(ref Vector2 vec, out Vector2 result)
        {
            float num = InverseSqrtFast((float)(vec.X * (double)vec.X + vec.Y * (double)vec.Y));
            result.X = vec.X * num;
            result.Y = vec.Y * num;
        }
        public static float Dot(Vector2 left, Vector2 right)
        {
            return (float)(left.X * (double)right.X + left.Y * (double)right.Y);
        }
        public static void Dot(ref Vector2 left, ref Vector2 right, out float result)
        {
            result = (float)(left.X * (double)right.X + left.Y * (double)right.Y);
        }
        public static float PerpDot(Vector2 left, Vector2 right)
        {
            return (float)(left.X * (double)right.Y - left.Y * (double)right.X);
        }
        public static void PerpDot(ref Vector2 left, ref Vector2 right, out float result)
        {
            result = (float)(left.X * (double)right.Y - left.Y * (double)right.X);
        }
        public static Vector2 Lerp(Vector2 a, Vector2 b, float blend)
        {
            a.X = blend * (b.X - a.X) + a.X;
            a.Y = blend * (b.Y - a.Y) + a.Y;
            return a;
        }
        public static void Lerp(ref Vector2 a, ref Vector2 b, float blend, out Vector2 result)
        {
            result.X = blend * (b.X - a.X) + a.X;
            result.Y = blend * (b.Y - a.Y) + a.Y;
        }
        public static Vector2 BaryCentric(Vector2 a, Vector2 b, Vector2 c, float u, float v)
        {
            return a + u * (b - a) + v * (c - a);
        }
        public static void BaryCentric(ref Vector2 a, ref Vector2 b, ref Vector2 c, float u, float v, out Vector2 result)
        {
            result = a;
            Vector2 result1 = b;
            Subtract(ref result1, ref a, out result1);
            Multiply(ref result1, u, out result1);
            Add(ref result, ref result1, out result);
            Vector2 result2 = c;
            Subtract(ref result2, ref a, out result2);
            Multiply(ref result2, v, out result2);
            Add(ref result, ref result2, out result);
        }
        public static Vector2 Transform(Vector2 vec, Quaternion quat)
        {
            Vector2 result;
            Transform(ref vec, ref quat, out result);
            return result;
        }
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
        public Vector2 Yx
        {
            get => new Vector2(this.Y, this.X);
            set
            {
                this.Y = value.X;
                this.X = value.Y;
            }
        }
        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }
        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }
        public static Vector2 operator -(Vector2 vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            return vec;
        }
        public static Vector2 operator *(Vector2 vec, float scale)
        {
            vec.X *= scale;
            vec.Y *= scale;
            return vec;
        }
        public static Vector2 operator *(float scale, Vector2 vec)
        {
            vec.X *= scale;
            vec.Y *= scale;
            return vec;
        }
        public static Vector2 operator *(Vector2 vec, Vector2 scale)
        {
            vec.X *= scale.X;
            vec.Y *= scale.Y;
            return vec;
        }
        public static Vector2 operator /(Vector2 vec, float scale)
        {
            vec.X /= scale;
            vec.Y /= scale;
            return vec;
        }
        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !left.Equals(right);
        }
        public override string ToString()
        {
            return string.Format("({0}{2} {1})", this.X, this.Y, listSeparator);
        }
        public override int GetHashCode()
        {
            return this.X.GetHashCode() * 397 ^ this.Y.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Vector2))
            {
                return false;
            }

            return this.Equals((Vector2)obj);
        }
        public bool Equals(Vector2 other)
        {
            if (this.X == (double)other.X)
            {
                return this.Y == (double)other.Y;
            }

            return false;
        }
    }
}