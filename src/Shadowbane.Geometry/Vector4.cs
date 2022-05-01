namespace Shadowbane.Geometry;

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using static MathHelper;

public struct Vector4 : IEquatable<Vector4>
{
    public static readonly Vector4 UnitX = new(1f, 0.0f, 0.0f, 0.0f);
    public static readonly Vector4 UnitY = new(0.0f, 1f, 0.0f, 0.0f);
    public static readonly Vector4 UnitZ = new(0.0f, 0.0f, 1f, 0.0f);
    public static readonly Vector4 UnitW = new(0.0f, 0.0f, 0.0f, 1f);
    public static readonly Vector4 Zero = new(0.0f, 0.0f, 0.0f, 0.0f);
    public static readonly Vector4 One = new(1f, 1f, 1f, 1f);
    public static readonly int SizeInBytes = Marshal.SizeOf((object)new Vector4());
    private static readonly string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
    public float X;
    public float Y;
    public float Z;
    public float W;
    public Vector4(float value)
    {
        this.X = value;
        this.Y = value;
        this.Z = value;
        this.W = value;
    }
    public Vector4(float x, float y, float z, float w)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.W = w;
    }
    public Vector4(Vector2 v)
    {
        this.X = v.X;
        this.Y = v.Y;
        this.Z = 0.0f;
        this.W = 0.0f;
    }
    public Vector4(Vector3 v)
    {
        this.X = v.X;
        this.Y = v.Y;
        this.Z = v.Z;
        this.W = 0.0f;
    }
    public Vector4(Vector3 v, float w)
    {
        this.X = v.X;
        this.Y = v.Y;
        this.Z = v.Z;
        this.W = w;
    }
    public Vector4(Vector4 v)
    {
        this.X = v.X;
        this.Y = v.Y;
        this.Z = v.Z;
        this.W = v.W;
    }
    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return this.X;
                case 1:
                    return this.Y;
                case 2:
                    return this.Z;
                case 3:
                    return this.W;
                default:
                    throw new IndexOutOfRangeException("You tried to access this vector at index: " + index);
            }
        }
        set
        {
            switch (index)
            {
                case 0:
                    this.X = value;
                    break;
                case 1:
                    this.Y = value;
                    break;
                case 2:
                    this.Z = value;
                    break;
                case 3:
                    this.W = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("You tried to set this vector at index: " + index);
            }
        }
    }
    public float Length => (float)Math.Sqrt(this.X * (double)this.X + this.Y * (double)this.Y + this.Z * (double)this.Z + this.W * (double)this.W);
    public float LengthFast => 1f / InverseSqrtFast((float)(this.X * (double)this.X + this.Y * (double)this.Y + this.Z * (double)this.Z + this.W * (double)this.W));
    public float LengthSquared => (float)(this.X * (double)this.X + this.Y * (double)this.Y + this.Z * (double)this.Z + this.W * (double)this.W);
    public Vector4 Normalized()
    {
        Vector4 vector4 = this;
        vector4.Normalize();
        return vector4;
    }
    public void Normalize()
    {
        float num = 1f / this.Length;
        this.X *= num;
        this.Y *= num;
        this.Z *= num;
        this.W *= num;
    }
    public void NormalizeFast()
    {
        float num = InverseSqrtFast((float)(this.X * (double)this.X + this.Y * (double)this.Y + this.Z * (double)this.Z + this.W * (double)this.W));
        this.X *= num;
        this.Y *= num;
        this.Z *= num;
        this.W *= num;
    }
    public static Vector4 Add(Vector4 a, Vector4 b)
    {
        Add(ref a, ref b, out a);
        return a;
    }
    public static void Add(ref Vector4 a, ref Vector4 b, out Vector4 result)
    {
        result.X = a.X + b.X;
        result.Y = a.Y + b.Y;
        result.Z = a.Z + b.Z;
        result.W = a.W + b.W;
    }
    public static Vector4 Subtract(Vector4 a, Vector4 b)
    {
        Subtract(ref a, ref b, out a);
        return a;
    }
    public static void Subtract(ref Vector4 a, ref Vector4 b, out Vector4 result)
    {
        result.X = a.X - b.X;
        result.Y = a.Y - b.Y;
        result.Z = a.Z - b.Z;
        result.W = a.W - b.W;
    }
    public static Vector4 Multiply(Vector4 vector, float scale)
    {
        Multiply(ref vector, scale, out vector);
        return vector;
    }
    public static void Multiply(ref Vector4 vector, float scale, out Vector4 result)
    {
        result.X = vector.X * scale;
        result.Y = vector.Y * scale;
        result.Z = vector.Z * scale;
        result.W = vector.W * scale;
    }
    public static Vector4 Multiply(Vector4 vector, Vector4 scale)
    {
        Multiply(ref vector, ref scale, out vector);
        return vector;
    }
    public static void Multiply(ref Vector4 vector, ref Vector4 scale, out Vector4 result)
    {
        result.X = vector.X * scale.X;
        result.Y = vector.Y * scale.Y;
        result.Z = vector.Z * scale.Z;
        result.W = vector.W * scale.W;
    }
    public static Vector4 Divide(Vector4 vector, float scale)
    {
        Divide(ref vector, scale, out vector);
        return vector;
    }
    public static void Divide(ref Vector4 vector, float scale, out Vector4 result)
    {
        result.X = vector.X / scale;
        result.Y = vector.Y / scale;
        result.Z = vector.Z / scale;
        result.W = vector.W / scale;
    }
    public static Vector4 Divide(Vector4 vector, Vector4 scale)
    {
        Divide(ref vector, ref scale, out vector);
        return vector;
    }
    public static void Divide(ref Vector4 vector, ref Vector4 scale, out Vector4 result)
    {
        result.X = vector.X / scale.X;
        result.Y = vector.Y / scale.Y;
        result.Z = vector.Z / scale.Z;
        result.W = vector.W / scale.W;
    }
    public static Vector4 ComponentMin(Vector4 a, Vector4 b)
    {
        a.X = (double)a.X < (double)b.X ? a.X : b.X;
        a.Y = (double)a.Y < (double)b.Y ? a.Y : b.Y;
        a.Z = (double)a.Z < (double)b.Z ? a.Z : b.Z;
        a.W = (double)a.W < (double)b.W ? a.W : b.W;
        return a;
    }
    public static void ComponentMin(ref Vector4 a, ref Vector4 b, out Vector4 result)
    {
        result.X = (double)a.X < (double)b.X ? a.X : b.X;
        result.Y = (double)a.Y < (double)b.Y ? a.Y : b.Y;
        result.Z = (double)a.Z < (double)b.Z ? a.Z : b.Z;
        result.W = (double)a.W < (double)b.W ? a.W : b.W;
    }
    public static Vector4 ComponentMax(Vector4 a, Vector4 b)
    {
        a.X = (double)a.X > (double)b.X ? a.X : b.X;
        a.Y = (double)a.Y > (double)b.Y ? a.Y : b.Y;
        a.Z = (double)a.Z > (double)b.Z ? a.Z : b.Z;
        a.W = (double)a.W > (double)b.W ? a.W : b.W;
        return a;
    }
    public static void ComponentMax(ref Vector4 a, ref Vector4 b, out Vector4 result)
    {
        result.X = (double)a.X > (double)b.X ? a.X : b.X;
        result.Y = (double)a.Y > (double)b.Y ? a.Y : b.Y;
        result.Z = (double)a.Z > (double)b.Z ? a.Z : b.Z;
        result.W = (double)a.W > (double)b.W ? a.W : b.W;
    }
    public static Vector4 MagnitudeMin(Vector4 left, Vector4 right)
    {
        if (left.LengthSquared >= (double)right.LengthSquared)
        {
            return right;
        }

        return left;
    }
    public static void MagnitudeMin(ref Vector4 left, ref Vector4 right, out Vector4 result)
    {
        result = (double)left.LengthSquared < (double)right.LengthSquared ? left : right;
    }
    public static Vector4 MagnitudeMax(Vector4 left, Vector4 right)
    {
        if (left.LengthSquared < (double)right.LengthSquared)
        {
            return right;
        }

        return left;
    }
    public static void MagnitudeMax(ref Vector4 left, ref Vector4 right, out Vector4 result)
    {
        result = (double)left.LengthSquared >= (double)right.LengthSquared ? left : right;
    }
    public static Vector4 Clamp(Vector4 vec, Vector4 min, Vector4 max)
    {
        vec.X = (double)vec.X < (double)min.X ? min.X : ((double)vec.X > (double)max.X ? max.X : vec.X);
        vec.Y = (double)vec.Y < (double)min.Y ? min.Y : ((double)vec.Y > (double)max.Y ? max.Y : vec.Y);
        vec.Z = (double)vec.Z < (double)min.Z ? min.Z : ((double)vec.Z > (double)max.Z ? max.Z : vec.Z);
        vec.W = (double)vec.W < (double)min.W ? min.W : ((double)vec.W > (double)max.W ? max.W : vec.W);
        return vec;
    }
    public static void Clamp(ref Vector4 vec, ref Vector4 min, ref Vector4 max, out Vector4 result)
    {
        result.X = (double)vec.X < (double)min.X ? min.X : ((double)vec.X > (double)max.X ? max.X : vec.X);
        result.Y = (double)vec.Y < (double)min.Y ? min.Y : ((double)vec.Y > (double)max.Y ? max.Y : vec.Y);
        result.Z = (double)vec.Z < (double)min.Z ? min.Z : ((double)vec.Z > (double)max.Z ? max.Z : vec.Z);
        result.W = (double)vec.W < (double)min.W ? min.W : ((double)vec.W > (double)max.W ? max.W : vec.W);
    }
    public static Vector4 Normalize(Vector4 vec)
    {
        float num = 1f / vec.Length;
        vec.X *= num;
        vec.Y *= num;
        vec.Z *= num;
        vec.W *= num;
        return vec;
    }
    public static void Normalize(ref Vector4 vec, out Vector4 result)
    {
        float num = 1f / vec.Length;
        result.X = vec.X * num;
        result.Y = vec.Y * num;
        result.Z = vec.Z * num;
        result.W = vec.W * num;
    }
    public static Vector4 NormalizeFast(Vector4 vec)
    {
        float num = InverseSqrtFast((float)(vec.X * (double)vec.X + vec.Y * (double)vec.Y + vec.Z * (double)vec.Z + vec.W * (double)vec.W));
        vec.X *= num;
        vec.Y *= num;
        vec.Z *= num;
        vec.W *= num;
        return vec;
    }
    public static void NormalizeFast(ref Vector4 vec, out Vector4 result)
    {
        float num = InverseSqrtFast((float)(vec.X * (double)vec.X + vec.Y * (double)vec.Y + vec.Z * (double)vec.Z + vec.W * (double)vec.W));
        result.X = vec.X * num;
        result.Y = vec.Y * num;
        result.Z = vec.Z * num;
        result.W = vec.W * num;
    }
    public static float Dot(Vector4 left, Vector4 right)
    {
        return (float)(left.X * (double)right.X + left.Y * (double)right.Y + left.Z * (double)right.Z + left.W * (double)right.W);
    }
    public static void Dot(ref Vector4 left, ref Vector4 right, out float result)
    {
        result = (float)(left.X * (double)right.X + left.Y * (double)right.Y + left.Z * (double)right.Z + left.W * (double)right.W);
    }
    public static Vector4 Lerp(Vector4 a, Vector4 b, float blend)
    {
        a.X = blend * (b.X - a.X) + a.X;
        a.Y = blend * (b.Y - a.Y) + a.Y;
        a.Z = blend * (b.Z - a.Z) + a.Z;
        a.W = blend * (b.W - a.W) + a.W;
        return a;
    }
    public static void Lerp(ref Vector4 a, ref Vector4 b, float blend, out Vector4 result)
    {
        result.X = blend * (b.X - a.X) + a.X;
        result.Y = blend * (b.Y - a.Y) + a.Y;
        result.Z = blend * (b.Z - a.Z) + a.Z;
        result.W = blend * (b.W - a.W) + a.W;
    }
    public static Vector4 BaryCentric(Vector4 a, Vector4 b, Vector4 c, float u, float v)
    {
        return a + u * (b - a) + v * (c - a);
    }
    public static void BaryCentric(ref Vector4 a, ref Vector4 b, ref Vector4 c, float u, float v, out Vector4 result)
    {
        result = a;
        Vector4 result1 = b;
        Subtract(ref result1, ref a, out result1);
        Multiply(ref result1, u, out result1);
        Add(ref result, ref result1, out result);
        Vector4 result2 = c;
        Subtract(ref result2, ref a, out result2);
        Multiply(ref result2, v, out result2);
        Add(ref result, ref result2, out result);
    }
    public static Vector4 Transform(Vector4 vec, Matrix4 mat)
    {
        Transform(ref vec, ref mat, out var result);
        return result;
    }
    public static void Transform(ref Vector4 vec, ref Matrix4 mat, out Vector4 result)
    {
        result = new Vector4((float)(vec.X * (double)mat.Row0.X + vec.Y * (double)mat.Row1.X + vec.Z * (double)mat.Row2.X + vec.W * (double)mat.Row3.X), (float)(vec.X * (double)mat.Row0.Y + vec.Y * (double)mat.Row1.Y + vec.Z * (double)mat.Row2.Y + vec.W * (double)mat.Row3.Y), (float)(vec.X * (double)mat.Row0.Z + vec.Y * (double)mat.Row1.Z + vec.Z * (double)mat.Row2.Z + vec.W * (double)mat.Row3.Z), (float)(vec.X * (double)mat.Row0.W + vec.Y * (double)mat.Row1.W + vec.Z * (double)mat.Row2.W + vec.W * (double)mat.Row3.W));
    }
    public static Vector4 Transform(Vector4 vec, Quaternion quaternion)
    {
        Transform(ref vec, ref quaternion, out var result);
        return result;
    }
    public static void Transform(ref Vector4 vec, ref Quaternion quaternion, out Vector4 result)
    {
        Quaternion result1 = new Quaternion(vec.X, vec.Y, vec.Z, vec.W);
        Quaternion.Invert(ref quaternion, out var result2);
        Quaternion.Multiply(ref quaternion, ref result1, out var result3);
        Quaternion.Multiply(ref result3, ref result2, out result1);
        result.X = result1.X;
        result.Y = result1.Y;
        result.Z = result1.Z;
        result.W = result1.W;
    }
    public static Vector4 Transform(Matrix4 mat, Vector4 vec)
    {
        Transform(ref mat, ref vec, out var result);
        return result;
    }
    public static void Transform(ref Matrix4 mat, ref Vector4 vec, out Vector4 result)
    {
        result = new Vector4((float)(mat.Row0.X * (double)vec.X + mat.Row0.Y * (double)vec.Y + mat.Row0.Z * (double)vec.Z + mat.Row0.W * (double)vec.W), (float)(mat.Row1.X * (double)vec.X + mat.Row1.Y * (double)vec.Y + mat.Row1.Z * (double)vec.Z + mat.Row1.W * (double)vec.W), (float)(mat.Row2.X * (double)vec.X + mat.Row2.Y * (double)vec.Y + mat.Row2.Z * (double)vec.Z + mat.Row2.W * (double)vec.W), (float)(mat.Row3.X * (double)vec.X + mat.Row3.Y * (double)vec.Y + mat.Row3.Z * (double)vec.Z + mat.Row3.W * (double)vec.W));
    }
    public Vector2 Xy
    {
        get => new(this.X, this.Y);
        set
        {
            this.X = value.X;
            this.Y = value.Y;
        }
    }
    public Vector2 Xz
    {
        get => new(this.X, this.Z);
        set
        {
            this.X = value.X;
            this.Z = value.Y;
        }
    }
    public Vector2 Xw
    {
        get => new(this.X, this.W);
        set
        {
            this.X = value.X;
            this.W = value.Y;
        }
    }
    public Vector2 Yx
    {
        get => new(this.Y, this.X);
        set
        {
            this.Y = value.X;
            this.X = value.Y;
        }
    }
    public Vector2 Yz
    {
        get => new(this.Y, this.Z);
        set
        {
            this.Y = value.X;
            this.Z = value.Y;
        }
    }
    public Vector2 Yw
    {
        get => new(this.Y, this.W);
        set
        {
            this.Y = value.X;
            this.W = value.Y;
        }
    }
    public Vector2 Zx
    {
        get => new(this.Z, this.X);
        set
        {
            this.Z = value.X;
            this.X = value.Y;
        }
    }
    public Vector2 Zy
    {
        get => new(this.Z, this.Y);
        set
        {
            this.Z = value.X;
            this.Y = value.Y;
        }
    }
    public Vector2 Zw
    {
        get => new(this.Z, this.W);
        set
        {
            this.Z = value.X;
            this.W = value.Y;
        }
    }
    public Vector2 Wx
    {
        get => new(this.W, this.X);
        set
        {
            this.W = value.X;
            this.X = value.Y;
        }
    }
    public Vector2 Wy
    {
        get => new(this.W, this.Y);
        set
        {
            this.W = value.X;
            this.Y = value.Y;
        }
    }
    public Vector2 Wz
    {
        get => new(this.W, this.Z);
        set
        {
            this.W = value.X;
            this.Z = value.Y;
        }
    }
    public Vector3 Xyz
    {
        get => new(this.X, this.Y, this.Z);
        set
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = value.Z;
        }
    }
    public Vector3 Xyw
    {
        get => new(this.X, this.Y, this.W);
        set
        {
            this.X = value.X;
            this.Y = value.Y;
            this.W = value.Z;
        }
    }
    public Vector3 Xzy
    {
        get => new(this.X, this.Z, this.Y);
        set
        {
            this.X = value.X;
            this.Z = value.Y;
            this.Y = value.Z;
        }
    }
    public Vector3 Xzw
    {
        get => new(this.X, this.Z, this.W);
        set
        {
            this.X = value.X;
            this.Z = value.Y;
            this.W = value.Z;
        }
    }
    public Vector3 Xwz
    {
        get => new(this.X, this.W, this.Z);
        set
        {
            this.X = value.X;
            this.W = value.Y;
            this.Z = value.Z;
        }
    }
    public Vector3 Yxz
    {
        get => new(this.Y, this.X, this.Z);
        set
        {
            this.Y = value.X;
            this.X = value.Y;
            this.Z = value.Z;
        }
    }
    public Vector3 Yxw
    {
        get => new(this.Y, this.X, this.W);
        set
        {
            this.Y = value.X;
            this.X = value.Y;
            this.W = value.Z;
        }
    }
    public Vector3 Yzx
    {
        get => new(this.Y, this.Z, this.X);
        set
        {
            this.Y = value.X;
            this.Z = value.Y;
            this.X = value.Z;
        }
    }
    public Vector3 Yzw
    {
        get => new(this.Y, this.Z, this.W);
        set
        {
            this.Y = value.X;
            this.Z = value.Y;
            this.W = value.Z;
        }
    }
    public Vector3 Ywx
    {
        get => new(this.Y, this.W, this.X);
        set
        {
            this.Y = value.X;
            this.W = value.Y;
            this.X = value.Z;
        }
    }
    public Vector3 Ywz
    {
        get => new(this.Y, this.W, this.Z);
        set
        {
            this.Y = value.X;
            this.W = value.Y;
            this.Z = value.Z;
        }
    }
    public Vector3 Zxy
    {
        get => new(this.Z, this.X, this.Y);
        set
        {
            this.Z = value.X;
            this.X = value.Y;
            this.Y = value.Z;
        }
    }
    public Vector3 Zxw
    {
        get => new(this.Z, this.X, this.W);
        set
        {
            this.Z = value.X;
            this.X = value.Y;
            this.W = value.Z;
        }
    }
    public Vector3 Zyx
    {
        get => new(this.Z, this.Y, this.X);
        set
        {
            this.Z = value.X;
            this.Y = value.Y;
            this.X = value.Z;
        }
    }
    public Vector3 Zyw
    {
        get => new(this.Z, this.Y, this.W);
        set
        {
            this.Z = value.X;
            this.Y = value.Y;
            this.W = value.Z;
        }
    }
    public Vector3 Zwx
    {
        get => new(this.Z, this.W, this.X);
        set
        {
            this.Z = value.X;
            this.W = value.Y;
            this.X = value.Z;
        }
    }
    public Vector3 Zwy
    {
        get => new(this.Z, this.W, this.Y);
        set
        {
            this.Z = value.X;
            this.W = value.Y;
            this.Y = value.Z;
        }
    }
    public Vector3 Wxy
    {
        get => new(this.W, this.X, this.Y);
        set
        {
            this.W = value.X;
            this.X = value.Y;
            this.Y = value.Z;
        }
    }
    public Vector3 Wxz
    {
        get => new(this.W, this.X, this.Z);
        set
        {
            this.W = value.X;
            this.X = value.Y;
            this.Z = value.Z;
        }
    }
    public Vector3 Wyx
    {
        get => new(this.W, this.Y, this.X);
        set
        {
            this.W = value.X;
            this.Y = value.Y;
            this.X = value.Z;
        }
    }
    public Vector3 Wyz
    {
        get => new(this.W, this.Y, this.Z);
        set
        {
            this.W = value.X;
            this.Y = value.Y;
            this.Z = value.Z;
        }
    }
    public Vector3 Wzx
    {
        get => new(this.W, this.Z, this.X);
        set
        {
            this.W = value.X;
            this.Z = value.Y;
            this.X = value.Z;
        }
    }
    public Vector3 Wzy
    {
        get => new(this.W, this.Z, this.Y);
        set
        {
            this.W = value.X;
            this.Z = value.Y;
            this.Y = value.Z;
        }
    }
    public Vector4 Xywz
    {
        get => new(this.X, this.Y, this.W, this.Z);
        set
        {
            this.X = value.X;
            this.Y = value.Y;
            this.W = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Xzyw
    {
        get => new(this.X, this.Z, this.Y, this.W);
        set
        {
            this.X = value.X;
            this.Z = value.Y;
            this.Y = value.Z;
            this.W = value.W;
        }
    }
    public Vector4 Xzwy
    {
        get => new(this.X, this.Z, this.W, this.Y);
        set
        {
            this.X = value.X;
            this.Z = value.Y;
            this.W = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Xwyz
    {
        get => new(this.X, this.W, this.Y, this.Z);
        set
        {
            this.X = value.X;
            this.W = value.Y;
            this.Y = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Xwzy
    {
        get => new(this.X, this.W, this.Z, this.Y);
        set
        {
            this.X = value.X;
            this.W = value.Y;
            this.Z = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Yxzw
    {
        get => new(this.Y, this.X, this.Z, this.W);
        set
        {
            this.Y = value.X;
            this.X = value.Y;
            this.Z = value.Z;
            this.W = value.W;
        }
    }
    public Vector4 Yxwz
    {
        get => new(this.Y, this.X, this.W, this.Z);
        set
        {
            this.Y = value.X;
            this.X = value.Y;
            this.W = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Yyzw
    {
        get => new(this.Y, this.Y, this.Z, this.W);
        set
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = value.Z;
            this.W = value.W;
        }
    }
    public Vector4 Yywz
    {
        get => new(this.Y, this.Y, this.W, this.Z);
        set
        {
            this.X = value.X;
            this.Y = value.Y;
            this.W = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Yzxw
    {
        get => new(this.Y, this.Z, this.X, this.W);
        set
        {
            this.Y = value.X;
            this.Z = value.Y;
            this.X = value.Z;
            this.W = value.W;
        }
    }
    public Vector4 Yzwx
    {
        get => new(this.Y, this.Z, this.W, this.X);
        set
        {
            this.Y = value.X;
            this.Z = value.Y;
            this.W = value.Z;
            this.X = value.W;
        }
    }
    public Vector4 Ywxz
    {
        get => new(this.Y, this.W, this.X, this.Z);
        set
        {
            this.Y = value.X;
            this.W = value.Y;
            this.X = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Ywzx
    {
        get => new(this.Y, this.W, this.Z, this.X);
        set
        {
            this.Y = value.X;
            this.W = value.Y;
            this.Z = value.Z;
            this.X = value.W;
        }
    }
    public Vector4 Zxyw
    {
        get => new(this.Z, this.X, this.Y, this.W);
        set
        {
            this.Z = value.X;
            this.X = value.Y;
            this.Y = value.Z;
            this.W = value.W;
        }
    }
    public Vector4 Zxwy
    {
        get => new(this.Z, this.X, this.W, this.Y);
        set
        {
            this.Z = value.X;
            this.X = value.Y;
            this.W = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Zyxw
    {
        get => new(this.Z, this.Y, this.X, this.W);
        set
        {
            this.Z = value.X;
            this.Y = value.Y;
            this.X = value.Z;
            this.W = value.W;
        }
    }
    public Vector4 Zywx
    {
        get => new(this.Z, this.Y, this.W, this.X);
        set
        {
            this.Z = value.X;
            this.Y = value.Y;
            this.W = value.Z;
            this.X = value.W;
        }
    }
    public Vector4 Zwxy
    {
        get => new(this.Z, this.W, this.X, this.Y);
        set
        {
            this.Z = value.X;
            this.W = value.Y;
            this.X = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Zwyx
    {
        get => new(this.Z, this.W, this.Y, this.X);
        set
        {
            this.Z = value.X;
            this.W = value.Y;
            this.Y = value.Z;
            this.X = value.W;
        }
    }
    public Vector4 Zwzy
    {
        get => new(this.Z, this.W, this.Z, this.Y);
        set
        {
            this.X = value.X;
            this.W = value.Y;
            this.Z = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Wxyz
    {
        get => new(this.W, this.X, this.Y, this.Z);
        set
        {
            this.W = value.X;
            this.X = value.Y;
            this.Y = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Wxzy
    {
        get => new(this.W, this.X, this.Z, this.Y);
        set
        {
            this.W = value.X;
            this.X = value.Y;
            this.Z = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Wyxz
    {
        get => new(this.W, this.Y, this.X, this.Z);
        set
        {
            this.W = value.X;
            this.Y = value.Y;
            this.X = value.Z;
            this.Z = value.W;
        }
    }
    public Vector4 Wyzx
    {
        get => new(this.W, this.Y, this.Z, this.X);
        set
        {
            this.W = value.X;
            this.Y = value.Y;
            this.Z = value.Z;
            this.X = value.W;
        }
    }
    public Vector4 Wzxy
    {
        get => new(this.W, this.Z, this.X, this.Y);
        set
        {
            this.W = value.X;
            this.Z = value.Y;
            this.X = value.Z;
            this.Y = value.W;
        }
    }
    public Vector4 Wzyx
    {
        get => new(this.W, this.Z, this.Y, this.X);
        set
        {
            this.W = value.X;
            this.Z = value.Y;
            this.Y = value.Z;
            this.X = value.W;
        }
    }
    public Vector4 Wzyw
    {
        get => new(this.W, this.Z, this.Y, this.W);
        set
        {
            this.X = value.X;
            this.Z = value.Y;
            this.Y = value.Z;
            this.W = value.W;
        }
    }
    public static Vector4 operator +(Vector4 left, Vector4 right)
    {
        left.X += right.X;
        left.Y += right.Y;
        left.Z += right.Z;
        left.W += right.W;
        return left;
    }
    public static Vector4 operator -(Vector4 left, Vector4 right)
    {
        left.X -= right.X;
        left.Y -= right.Y;
        left.Z -= right.Z;
        left.W -= right.W;
        return left;
    }
    public static Vector4 operator -(Vector4 vec)
    {
        vec.X = -vec.X;
        vec.Y = -vec.Y;
        vec.Z = -vec.Z;
        vec.W = -vec.W;
        return vec;
    }
    public static Vector4 operator *(Vector4 vec, float scale)
    {
        vec.X *= scale;
        vec.Y *= scale;
        vec.Z *= scale;
        vec.W *= scale;
        return vec;
    }
    public static Vector4 operator *(float scale, Vector4 vec)
    {
        vec.X *= scale;
        vec.Y *= scale;
        vec.Z *= scale;
        vec.W *= scale;
        return vec;
    }
    public static Vector4 operator *(Vector4 vec, Vector4 scale)
    {
        vec.X *= scale.X;
        vec.Y *= scale.Y;
        vec.Z *= scale.Z;
        vec.W *= scale.W;
        return vec;
    }
    public static Vector4 operator *(Vector4 vec, Matrix4 mat)
    {
        Transform(ref vec, ref mat, out var result);
        return result;
    }
    public static Vector4 operator *(Matrix4 mat, Vector4 vec)
    {
        Transform(ref mat, ref vec, out var result);
        return result;
    }
    public static Vector4 operator *(Quaternion quat, Vector4 vec)
    {
        Transform(ref vec, ref quat, out var result);
        return result;
    }
    public static Vector4 operator /(Vector4 vec, float scale)
    {
        vec.X /= scale;
        vec.Y /= scale;
        vec.Z /= scale;
        vec.W /= scale;
        return vec;
    }
    public static bool operator ==(Vector4 left, Vector4 right)
    {
        return left.Equals(right);
    }
    public static bool operator !=(Vector4 left, Vector4 right)
    {
        return !left.Equals(right);
    }
    public static unsafe explicit operator float*(Vector4 v)
    {
        return &v.X;
    }
    public static unsafe explicit operator IntPtr(Vector4 v)
    {
        return (IntPtr)(&v.X);
    }
    public override string ToString()
    {
        return string.Format("({0}{4} {1}{4} {2}{4} {3})", (object)this.X, (object)this.Y, (object)this.Z, (object)this.W, (object)listSeparator);
    }
    public override int GetHashCode()
    {
        return ((this.X.GetHashCode() * 397 ^ this.Y.GetHashCode()) * 397 ^ this.Z.GetHashCode()) * 397 ^ this.W.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        if (!(obj is Vector4))
        {
            return false;
        }

        return this.Equals((Vector4)obj);
    }
    public bool Equals(Vector4 other)
    {
        if (this.X == (double)other.X && this.Y == (double)other.Y && this.Z == (double)other.Z)
        {
            return this.W == (double)other.W;
        }

        return false;
    }
}