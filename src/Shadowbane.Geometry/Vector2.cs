// ReSharper disable MemberCanBePrivate.Global
namespace Shadowbane.Geometry;

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using static MathHelper;

internal struct Vector2 : IEquatable<Vector2>
{
    public static readonly Vector2 unitX = new(1f, 0.0f);
    public static readonly Vector2 unitY = new(0.0f, 1f);
    public static readonly Vector2 zero = new(0.0f, 0.0f);
    public static readonly Vector2 one = new(1f, 1f);
    public static readonly int sizeInBytes = Marshal.SizeOf((object)new Vector2());
    private static readonly string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
    public float x;
    public float y;
    public Vector2(float value)
    {
        this.x = value;
        this.y = value;
    }
    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    public float this[int index]
    {
        get
        {
            if (index == 0)
            {
                return this.x;
            }

            if (index == 1)
            {
                return this.y;
            }

            throw new IndexOutOfRangeException("You tried to access this vector at index: " + index);
        }
        set
        {
            if (index == 0)
            {
                this.x = value;
            }
            else
            {
                if (index != 1)
                {
                    throw new IndexOutOfRangeException("You tried to set this vector at index: " + index);
                }

                this.y = value;
            }
        }
    }
    public float Length => (float)Math.Sqrt(this.x * (double)this.x + this.y * (double)this.y);
    public float LengthFast => 1f / InverseSqrtFast((float)(this.x * (double)this.x + this.y * (double)this.y));
    public float LengthSquared => (float)(this.x * (double)this.x + this.y * (double)this.y);
    public Vector2 PerpendicularRight => new(this.y, -this.x);
    public Vector2 PerpendicularLeft => new(-this.y, this.x);
    public Vector2 Normalized()
    {
        Vector2 vector2 = this;
        vector2.Normalize();
        return vector2;
    }
    public void Normalize()
    {
        float num = 1f / this.Length;
        this.x *= num;
        this.y *= num;
    }
    public void NormalizeFast()
    {
        float num = InverseSqrtFast((float)(this.x * (double)this.x + this.y * (double)this.y));
        this.x *= num;
        this.y *= num;
    }
    public static Vector2 Add(Vector2 a, Vector2 b)
    {
        Add(ref a, ref b, out a);
        return a;
    }
    public static void Add(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
        result.x = a.x + b.x;
        result.y = a.y + b.y;
    }
    public static Vector2 Subtract(Vector2 a, Vector2 b)
    {
        Subtract(ref a, ref b, out a);
        return a;
    }
    public static void Subtract(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
        result.x = a.x - b.x;
        result.y = a.y - b.y;
    }
    public static Vector2 Multiply(Vector2 vector, float scale)
    {
        Multiply(ref vector, scale, out vector);
        return vector;
    }
    public static void Multiply(ref Vector2 vector, float scale, out Vector2 result)
    {
        result.x = vector.x * scale;
        result.y = vector.y * scale;
    }
    public static Vector2 Multiply(Vector2 vector, Vector2 scale)
    {
        Multiply(ref vector, ref scale, out vector);
        return vector;
    }
    public static void Multiply(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
    {
        result.x = vector.x * scale.x;
        result.y = vector.y * scale.y;
    }
    public static Vector2 Divide(Vector2 vector, float scale)
    {
        Divide(ref vector, scale, out vector);
        return vector;
    }
    public static void Divide(ref Vector2 vector, float scale, out Vector2 result)
    {
        result.x = vector.x / scale;
        result.y = vector.y / scale;
    }
    public static Vector2 Divide(Vector2 vector, Vector2 scale)
    {
        Divide(ref vector, ref scale, out vector);
        return vector;
    }
    public static void Divide(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
    {
        result.x = vector.x / scale.x;
        result.y = vector.y / scale.y;
    }
    public static Vector2 ComponentMin(Vector2 a, Vector2 b)
    {
        a.x = a.x < (double)b.x ? a.x : b.x;
        a.y = a.y < (double)b.y ? a.y : b.y;
        return a;
    }
    public static void ComponentMin(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
        result.x = a.x < (double)b.x ? a.x : b.x;
        result.y = a.y < (double)b.y ? a.y : b.y;
    }
    public static Vector2 ComponentMax(Vector2 a, Vector2 b)
    {
        a.x = a.x > (double)b.x ? a.x : b.x;
        a.y = a.y > (double)b.y ? a.y : b.y;
        return a;
    }
    public static void ComponentMax(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
        result.x = a.x > (double)b.x ? a.x : b.x;
        result.y = a.y > (double)b.y ? a.y : b.y;
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
        result = left.LengthSquared < (double)right.LengthSquared ? left : right;
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
        result = left.LengthSquared >= (double)right.LengthSquared ? left : right;
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
        vec.x = vec.x < (double)min.x ? min.x : (vec.x > (double)max.x ? max.x : vec.x);
        vec.y = vec.y < (double)min.y ? min.y : (vec.y > (double)max.y ? max.y : vec.y);
        return vec;
    }
    public static void Clamp(ref Vector2 vec, ref Vector2 min, ref Vector2 max, out Vector2 result)
    {
        result.x = vec.x < (double)min.x ? min.x : (vec.x > (double)max.x ? max.x : vec.x);
        result.y = vec.y < (double)min.y ? min.y : (vec.y > (double)max.y ? max.y : vec.y);
    }
    public static float Distance(Vector2 vec1, Vector2 vec2)
    {
        Distance(ref vec1, ref vec2, out var result);
        return result;
    }
    public static void Distance(ref Vector2 vec1, ref Vector2 vec2, out float result)
    {
        result = (float)Math.Sqrt((vec2.x - (double)vec1.x) * (vec2.x - (double)vec1.x) + (vec2.y - (double)vec1.y) * (vec2.y - (double)vec1.y));
    }
    public static float DistanceSquared(Vector2 vec1, Vector2 vec2)
    {
        DistanceSquared(ref vec1, ref vec2, out var result);
        return result;
    }
    public static void DistanceSquared(ref Vector2 vec1, ref Vector2 vec2, out float result)
    {
        result = (float)((vec2.x - (double)vec1.x) * (vec2.x - (double)vec1.x) + (vec2.y - (double)vec1.y) * (vec2.y - (double)vec1.y));
    }
    public static Vector2 Normalize(Vector2 vec)
    {
        float num = 1f / vec.Length;
        vec.x *= num;
        vec.y *= num;
        return vec;
    }
    public static void Normalize(ref Vector2 vec, out Vector2 result)
    {
        float num = 1f / vec.Length;
        result.x = vec.x * num;
        result.y = vec.y * num;
    }
    public static Vector2 NormalizeFast(Vector2 vec)
    {
        float num = InverseSqrtFast((float)(vec.x * (double)vec.x + vec.y * (double)vec.y));
        vec.x *= num;
        vec.y *= num;
        return vec;
    }
    public static void NormalizeFast(ref Vector2 vec, out Vector2 result)
    {
        float num = InverseSqrtFast((float)(vec.x * (double)vec.x + vec.y * (double)vec.y));
        result.x = vec.x * num;
        result.y = vec.y * num;
    }
    public static float Dot(Vector2 left, Vector2 right)
    {
        return (float)(left.x * (double)right.x + left.y * (double)right.y);
    }
    public static void Dot(ref Vector2 left, ref Vector2 right, out float result)
    {
        result = (float)(left.x * (double)right.x + left.y * (double)right.y);
    }
    public static float PerpDot(Vector2 left, Vector2 right)
    {
        return (float)(left.x * (double)right.y - left.y * (double)right.x);
    }
    public static void PerpDot(ref Vector2 left, ref Vector2 right, out float result)
    {
        result = (float)(left.x * (double)right.y - left.y * (double)right.x);
    }
    public static Vector2 Lerp(Vector2 a, Vector2 b, float blend)
    {
        a.x = blend * (b.x - a.x) + a.x;
        a.y = blend * (b.y - a.y) + a.y;
        return a;
    }
    public static void Lerp(ref Vector2 a, ref Vector2 b, float blend, out Vector2 result)
    {
        result.x = blend * (b.x - a.x) + a.x;
        result.y = blend * (b.y - a.y) + a.y;
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
    public static Vector2 Transform(Vector2 vec, Quaternion quaternion)
    {
        Vector2 result;
        Transform(ref vec, ref quaternion, out result);
        return result;
    }
    public static void Transform(ref Vector2 vec, ref Quaternion quaternion, out Vector2 result)
    {
        Quaternion result1 = new Quaternion(vec.x, vec.y, 0.0f, 0.0f);
        Quaternion result2;
        Quaternion.Invert(ref quaternion, out result2);
        Quaternion result3;
        Quaternion.Multiply(ref quaternion, ref result1, out result3);
        Quaternion.Multiply(ref result3, ref result2, out result1);
        result.x = result1.X;
        result.y = result1.Y;
    }
    public Vector2 Yx
    {
        get => new(this.y, this.x);
        set
        {
            this.y = value.x;
            this.x = value.y;
        }
    }
    public static Vector2 operator +(Vector2 left, Vector2 right)
    {
        left.x += right.x;
        left.y += right.y;
        return left;
    }
    public static Vector2 operator -(Vector2 left, Vector2 right)
    {
        left.x -= right.x;
        left.y -= right.y;
        return left;
    }
    public static Vector2 operator -(Vector2 vec)
    {
        vec.x = -vec.x;
        vec.y = -vec.y;
        return vec;
    }
    public static Vector2 operator *(Vector2 vec, float scale)
    {
        vec.x *= scale;
        vec.y *= scale;
        return vec;
    }
    public static Vector2 operator *(float scale, Vector2 vec)
    {
        vec.x *= scale;
        vec.y *= scale;
        return vec;
    }
    public static Vector2 operator *(Vector2 vec, Vector2 scale)
    {
        vec.x *= scale.x;
        vec.y *= scale.y;
        return vec;
    }
    public static Vector2 operator /(Vector2 vec, float scale)
    {
        vec.x /= scale;
        vec.y /= scale;
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
        return string.Format("({0}{2} {1})", this.x, this.y, listSeparator);
    }
    public override int GetHashCode()
    {
        return this.x.GetHashCode() * 397 ^ this.y.GetHashCode();
    }
    public override bool Equals(object? obj)
    {
        return obj is Vector2 vector2 && this.Equals(vector2);
    }
    public bool Equals(Vector2 other)
    {
        if (Math.Abs(this.x - (double)other.x) < TOLERANCE)
        {
            return Math.Abs(this.y - (double)other.y) < TOLERANCE;
        }

        return false;
    }

    private const double TOLERANCE = 0.0000001; 
}