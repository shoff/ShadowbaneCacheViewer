namespace Shadowbane.Geometry;

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using static MathHelper;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct Vector3 : IEquatable<Vector3>
{
    public static readonly Vector3 UnitX = new(1f, 0.0f, 0.0f);
    public static readonly Vector3 UnitY = new(0.0f, 1f, 0.0f);
    public static readonly Vector3 UnitZ = new(0.0f, 0.0f, 1f);
    public static readonly Vector3 Zero = new(0.0f, 0.0f, 0.0f);
    public static readonly Vector3 One = new(1f, 1f, 1f);
    public static readonly int SizeInBytes = Marshal.SizeOf((object)new Vector3());
    private static readonly string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

    public float X;
    public float Y;
    public float Z;

    public Vector3(float value)
    {
        this.X = value;
        this.Y = value;
        this.Z = value;
    }
    public Vector3(float x, float y, float z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
    public Vector3(Vector2 v)
    {
        this.X = v.x;
        this.Y = v.y;
        this.Z = 0.0f;
    }
    public Vector3(Vector3 v)
    {
        this.X = v.X;
        this.Y = v.Y;
        this.Z = v.Z;
    }
    public Vector3(Vector4 v)
    {
        this.X = v.X;
        this.Y = v.Y;
        this.Z = v.Z;
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

            if (index == 2)
            {
                return this.Z;
            }

            throw new IndexOutOfRangeException("You tried to access this vector at index: " + index);
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
                    throw new IndexOutOfRangeException("You tried to set this vector at index: " + index);
                }

                this.Z = value;
            }
        }
    }
    public float Length => (float)Math.Sqrt(this.X * (double)this.X + this.Y * (double)this.Y + this.Z * (double)this.Z);
    public float LengthFast => 1f / InverseSqrtFast((float)(this.X * (double)this.X + this.Y * (double)this.Y + this.Z * (double)this.Z));
    public float LengthSquared => (float)(this.X * (double)this.X + this.Y * (double)this.Y + this.Z * (double)this.Z);
    public Vector3 Normalized()
    {
        Vector3 vector3 = this;
        vector3.Normalize();
        return vector3;
    }
    public void Normalize()
    {
        float num = 1f / this.Length;
        this.X *= num;
        this.Y *= num;
        this.Z *= num;
    }
    public void NormalizeFast()
    {
        float num = InverseSqrtFast((float)(this.X * (double)this.X + this.Y * (double)this.Y + this.Z * (double)this.Z));
        this.X *= num;
        this.Y *= num;
        this.Z *= num;
    }
    public static Vector3 Add(Vector3 a, Vector3 b)
    {
        Add(ref a, ref b, out a);
        return a;
    }
    public static void Add(ref Vector3 a, ref Vector3 b, out Vector3 result)
    {
        result.X = a.X + b.X;
        result.Y = a.Y + b.Y;
        result.Z = a.Z + b.Z;
    }
    public static Vector3 Subtract(Vector3 a, Vector3 b)
    {
        Subtract(ref a, ref b, out a);
        return a;
    }
    public static void Subtract(ref Vector3 a, ref Vector3 b, out Vector3 result)
    {
        result.X = a.X - b.X;
        result.Y = a.Y - b.Y;
        result.Z = a.Z - b.Z;
    }
    public static Vector3 Multiply(Vector3 vector, float scale)
    {
        Multiply(ref vector, scale, out vector);
        return vector;
    }
    public static void Multiply(ref Vector3 vector, float scale, out Vector3 result)
    {
        result.X = vector.X * scale;
        result.Y = vector.Y * scale;
        result.Z = vector.Z * scale;
    }
    public static Vector3 Multiply(Vector3 vector, Vector3 scale)
    {
        Multiply(ref vector, ref scale, out vector);
        return vector;
    }
    public static void Multiply(ref Vector3 vector, ref Vector3 scale, out Vector3 result)
    {
        result.X = vector.X * scale.X;
        result.Y = vector.Y * scale.Y;
        result.Z = vector.Z * scale.Z;
    }
    public static Vector3 Divide(Vector3 vector, float scale)
    {
        Divide(ref vector, scale, out vector);
        return vector;
    }
    public static void Divide(ref Vector3 vector, float scale, out Vector3 result)
    {
        result.X = vector.X / scale;
        result.Y = vector.Y / scale;
        result.Z = vector.Z / scale;
    }
    public static Vector3 Divide(Vector3 vector, Vector3 scale)
    {
        Divide(ref vector, ref scale, out vector);
        return vector;
    }
    public static void Divide(ref Vector3 vector, ref Vector3 scale, out Vector3 result)
    {
        result.X = vector.X / scale.X;
        result.Y = vector.Y / scale.Y;
        result.Z = vector.Z / scale.Z;
    }
    public static Vector3 ComponentMin(Vector3 a, Vector3 b)
    {
        a.X = (double)a.X < (double)b.X ? a.X : b.X;
        a.Y = (double)a.Y < (double)b.Y ? a.Y : b.Y;
        a.Z = (double)a.Z < (double)b.Z ? a.Z : b.Z;
        return a;
    }
    public static void ComponentMin(ref Vector3 a, ref Vector3 b, out Vector3 result)
    {
        result.X = (double)a.X < (double)b.X ? a.X : b.X;
        result.Y = (double)a.Y < (double)b.Y ? a.Y : b.Y;
        result.Z = (double)a.Z < (double)b.Z ? a.Z : b.Z;
    }
    public static Vector3 ComponentMax(Vector3 a, Vector3 b)
    {
        a.X = (double)a.X > (double)b.X ? a.X : b.X;
        a.Y = (double)a.Y > (double)b.Y ? a.Y : b.Y;
        a.Z = (double)a.Z > (double)b.Z ? a.Z : b.Z;
        return a;
    }
    public static void ComponentMax(ref Vector3 a, ref Vector3 b, out Vector3 result)
    {
        result.X = (double)a.X > (double)b.X ? a.X : b.X;
        result.Y = (double)a.Y > (double)b.Y ? a.Y : b.Y;
        result.Z = (double)a.Z > (double)b.Z ? a.Z : b.Z;
    }
    public static Vector3 MagnitudeMin(Vector3 left, Vector3 right)
    {
        if (left.LengthSquared >= (double)right.LengthSquared)
        {
            return right;
        }

        return left;
    }
    public static void MagnitudeMin(ref Vector3 left, ref Vector3 right, out Vector3 result)
    {
        result = (double)left.LengthSquared < (double)right.LengthSquared ? left : right;
    }
    public static Vector3 MagnitudeMax(Vector3 left, Vector3 right)
    {
        if (left.LengthSquared < (double)right.LengthSquared)
        {
            return right;
        }

        return left;
    }
    public static void MagnitudeMax(ref Vector3 left, ref Vector3 right, out Vector3 result)
    {
        result = (double)left.LengthSquared >= (double)right.LengthSquared ? left : right;
    }
    [Obsolete("Use MagnitudeMin() instead.")]
    public static Vector3 Min(Vector3 left, Vector3 right)
    {
        if (left.LengthSquared >= (double)right.LengthSquared)
        {
            return right;
        }

        return left;
    }
    [Obsolete("Use MagnitudeMax() instead.")]
    public static Vector3 Max(Vector3 left, Vector3 right)
    {
        if (left.LengthSquared < (double)right.LengthSquared)
        {
            return right;
        }

        return left;
    }
    public static Vector3 Clamp(Vector3 vec, Vector3 min, Vector3 max)
    {
        vec.X = (double)vec.X < (double)min.X ? min.X : ((double)vec.X > (double)max.X ? max.X : vec.X);
        vec.Y = (double)vec.Y < (double)min.Y ? min.Y : ((double)vec.Y > (double)max.Y ? max.Y : vec.Y);
        vec.Z = (double)vec.Z < (double)min.Z ? min.Z : ((double)vec.Z > (double)max.Z ? max.Z : vec.Z);
        return vec;
    }
    public static void Clamp(ref Vector3 vec, ref Vector3 min, ref Vector3 max, out Vector3 result)
    {
        result.X = (double)vec.X < (double)min.X ? min.X : ((double)vec.X > (double)max.X ? max.X : vec.X);
        result.Y = (double)vec.Y < (double)min.Y ? min.Y : ((double)vec.Y > (double)max.Y ? max.Y : vec.Y);
        result.Z = (double)vec.Z < (double)min.Z ? min.Z : ((double)vec.Z > (double)max.Z ? max.Z : vec.Z);
    }
    public static float Distance(Vector3 vec1, Vector3 vec2)
    {
        Distance(ref vec1, ref vec2, out var result);
        return result;
    }
    public static void Distance(ref Vector3 vec1, ref Vector3 vec2, out float result)
    {
        result = (float)Math.Sqrt((vec2.X - (double)vec1.X) * (vec2.X - (double)vec1.X) + (vec2.Y - (double)vec1.Y) * (vec2.Y - (double)vec1.Y) + (vec2.Z - (double)vec1.Z) * (vec2.Z - (double)vec1.Z));
    }
    public static float DistanceSquared(Vector3 vec1, Vector3 vec2)
    {
        DistanceSquared(ref vec1, ref vec2, out var result);
        return result;
    }
    public static void DistanceSquared(ref Vector3 vec1, ref Vector3 vec2, out float result)
    {
        result = (float)((vec2.X - (double)vec1.X) * (vec2.X - (double)vec1.X) + (vec2.Y - (double)vec1.Y) * (vec2.Y - (double)vec1.Y) + (vec2.Z - (double)vec1.Z) * (vec2.Z - (double)vec1.Z));
    }
    public static Vector3 Normalize(Vector3 vec)
    {
        float num = 1f / vec.Length;
        vec.X *= num;
        vec.Y *= num;
        vec.Z *= num;
        return vec;
    }
    public static void Normalize(ref Vector3 vec, out Vector3 result)
    {
        float num = 1f / vec.Length;
        result.X = vec.X * num;
        result.Y = vec.Y * num;
        result.Z = vec.Z * num;
    }
    public static Vector3 NormalizeFast(Vector3 vec)
    {
        float num = InverseSqrtFast((float)(vec.X * (double)vec.X + vec.Y * (double)vec.Y + vec.Z * (double)vec.Z));
        vec.X *= num;
        vec.Y *= num;
        vec.Z *= num;
        return vec;
    }
    public static void NormalizeFast(ref Vector3 vec, out Vector3 result)
    {
        float num = InverseSqrtFast((float)(vec.X * (double)vec.X + vec.Y * (double)vec.Y + vec.Z * (double)vec.Z));
        result.X = vec.X * num;
        result.Y = vec.Y * num;
        result.Z = vec.Z * num;
    }
    public static float Dot(Vector3 left, Vector3 right)
    {
        return (float)(left.X * (double)right.X + left.Y * (double)right.Y + left.Z * (double)right.Z);
    }
    public static void Dot(ref Vector3 left, ref Vector3 right, out float result)
    {
        result = (float)(left.X * (double)right.X + left.Y * (double)right.Y + left.Z * (double)right.Z);
    }
    public static Vector3 Cross(Vector3 left, Vector3 right)
    {
        Cross(ref left, ref right, out var result);
        return result;
    }
    public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result)
    {
        result.X = (float)(left.Y * (double)right.Z - left.Z * (double)right.Y);
        result.Y = (float)(left.Z * (double)right.X - left.X * (double)right.Z);
        result.Z = (float)(left.X * (double)right.Y - left.Y * (double)right.X);
    }
    public static Vector3 Lerp(Vector3 a, Vector3 b, float blend)
    {
        a.X = blend * (b.X - a.X) + a.X;
        a.Y = blend * (b.Y - a.Y) + a.Y;
        a.Z = blend * (b.Z - a.Z) + a.Z;
        return a;
    }
    public static void Lerp(ref Vector3 a, ref Vector3 b, float blend, out Vector3 result)
    {
        result.X = blend * (b.X - a.X) + a.X;
        result.Y = blend * (b.Y - a.Y) + a.Y;
        result.Z = blend * (b.Z - a.Z) + a.Z;
    }
    public static Vector3 BaryCentric(Vector3 a, Vector3 b, Vector3 c, float u, float v)
    {
        return a + u * (b - a) + v * (c - a);
    }
    public static void BaryCentric(ref Vector3 a, ref Vector3 b, ref Vector3 c, float u, float v, out Vector3 result)
    {
        result = a;
        Vector3 result1 = b;
        Subtract(ref result1, ref a, out result1);
        Multiply(ref result1, u, out result1);
        Add(ref result, ref result1, out result);
        Vector3 result2 = c;
        Subtract(ref result2, ref a, out result2);
        Multiply(ref result2, v, out result2);
        Add(ref result, ref result2, out result);
    }
    public static Vector3 TransformVector(Vector3 vec, Matrix4 mat)
    {
        TransformVector(ref vec, ref mat, out var result);
        return result;
    }
    public static void TransformVector(ref Vector3 vec, ref Matrix4 mat, out Vector3 result)
    {
        result.X = (float)(vec.X * (double)mat.Row0.X + vec.Y * (double)mat.Row1.X + vec.Z * (double)mat.Row2.X);
        result.Y = (float)(vec.X * (double)mat.Row0.Y + vec.Y * (double)mat.Row1.Y + vec.Z * (double)mat.Row2.Y);
        result.Z = (float)(vec.X * (double)mat.Row0.Z + vec.Y * (double)mat.Row1.Z + vec.Z * (double)mat.Row2.Z);
    }
    public static Vector3 TransformNormal(Vector3 norm, Matrix4 mat)
    {
        TransformNormal(ref norm, ref mat, out var result);
        return result;
    }
    public static void TransformNormal(ref Vector3 norm, ref Matrix4 mat, out Vector3 result)
    {
        Matrix4 invMat = Matrix4.Invert(mat);
        TransformNormalInverse(ref norm, ref invMat, out result);
    }
    public static Vector3 TransformNormalInverse(Vector3 norm, Matrix4 invMat)
    {
        TransformNormalInverse(ref norm, ref invMat, out var result);
        return result;
    }
    public static void TransformNormalInverse(ref Vector3 norm, ref Matrix4 invMat, out Vector3 result)
    {
        result.X = (float)(norm.X * (double)invMat.Row0.X + norm.Y * (double)invMat.Row0.Y + norm.Z * (double)invMat.Row0.Z);
        result.Y = (float)(norm.X * (double)invMat.Row1.X + norm.Y * (double)invMat.Row1.Y + norm.Z * (double)invMat.Row1.Z);
        result.Z = (float)(norm.X * (double)invMat.Row2.X + norm.Y * (double)invMat.Row2.Y + norm.Z * (double)invMat.Row2.Z);
    }
    public static Vector3 TransformPosition(Vector3 pos, Matrix4 mat)
    {
        TransformPosition(ref pos, ref mat, out var result);
        return result;
    }
    public static void TransformPosition(ref Vector3 pos, ref Matrix4 mat, out Vector3 result)
    {
        result.X = (float)(pos.X * (double)mat.Row0.X + pos.Y * (double)mat.Row1.X + pos.Z * (double)mat.Row2.X) + mat.Row3.X;
        result.Y = (float)(pos.X * (double)mat.Row0.Y + pos.Y * (double)mat.Row1.Y + pos.Z * (double)mat.Row2.Y) + mat.Row3.Y;
        result.Z = (float)(pos.X * (double)mat.Row0.Z + pos.Y * (double)mat.Row1.Z + pos.Z * (double)mat.Row2.Z) + mat.Row3.Z;
    }
    public static Vector3 Transform(Vector3 vec, Matrix3 mat)
    {
        Transform(ref vec, ref mat, out var result);
        return result;
    }
    public static void Transform(ref Vector3 vec, ref Matrix3 mat, out Vector3 result)
    {
        result.X = (float)(vec.X * (double)mat.Row0.X + vec.Y * (double)mat.Row1.X + vec.Z * (double)mat.Row2.X);
        result.Y = (float)(vec.X * (double)mat.Row0.Y + vec.Y * (double)mat.Row1.Y + vec.Z * (double)mat.Row2.Y);
        result.Z = (float)(vec.X * (double)mat.Row0.Z + vec.Y * (double)mat.Row1.Z + vec.Z * (double)mat.Row2.Z);
    }
    public static Vector3 Transform(Vector3 vec, Quaternion quat)
    {
        Transform(ref vec, ref quat, out var result);
        return result;
    }
    public static void Transform(ref Vector3 vec, ref Quaternion quat, out Vector3 result)
    {
        Vector3 xyz = quat.xyz;
        Cross(ref xyz, ref vec, out var result1);
        Multiply(ref vec, quat.w, out var result2);
        Add(ref result1, ref result2, out result1);
        Cross(ref xyz, ref result1, out result2);
        Multiply(ref result2, 2f, out result2);
        Add(ref vec, ref result2, out result);
    }
    public static Vector3 Transform(Matrix3 mat, Vector3 vec)
    {
        Transform(ref vec, ref mat, out var result);
        return result;
    }
    public static void Transform(ref Matrix3 mat, ref Vector3 vec, out Vector3 result)
    {
        result.X = (float)(mat.Row0.X * (double)vec.X + mat.Row0.Y * (double)vec.Y + mat.Row0.Z * (double)vec.Z);
        result.Y = (float)(mat.Row1.X * (double)vec.X + mat.Row1.Y * (double)vec.Y + mat.Row1.Z * (double)vec.Z);
        result.Z = (float)(mat.Row2.X * (double)vec.X + mat.Row2.Y * (double)vec.Y + mat.Row2.Z * (double)vec.Z);
    }
    public static Vector3 TransformPerspective(Vector3 vec, Matrix4 mat)
    {
        TransformPerspective(ref vec, ref mat, out var result);
        return result;
    }
    public static void TransformPerspective(ref Vector3 vec, ref Matrix4 mat, out Vector3 result)
    {
        Vector4 result1 = new Vector4(vec.X, vec.Y, vec.Z, 1f);
        Vector4.Transform(ref result1, ref mat, out result1);
        result.X = result1.X / result1.W;
        result.Y = result1.Y / result1.W;
        result.Z = result1.Z / result1.W;
    }
    public static float CalculateAngle(Vector3 first, Vector3 second)
    {
        CalculateAngle(ref first, ref second, out var result);
        return result;
    }
    public static void CalculateAngle(ref Vector3 first, ref Vector3 second, out float result)
    {
        Dot(ref first, ref second, out var result1);
        result = (float)Math.Acos(MathHelper.Clamp(result1 / (first.Length * (double)second.Length), -1.0, 1.0));
    }
    public static Vector3 Project(Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix4 worldViewProjection)
    {
        Vector4 vector41;
        vector41.X = (float)(vector.X * (double)worldViewProjection.M11 + vector.Y * (double)worldViewProjection.M21 + vector.Z * (double)worldViewProjection.M31) + worldViewProjection.M41;
        vector41.Y = (float)(vector.X * (double)worldViewProjection.M12 + vector.Y * (double)worldViewProjection.M22 + vector.Z * (double)worldViewProjection.M32) + worldViewProjection.M42;
        vector41.Z = (float)(vector.X * (double)worldViewProjection.M13 + vector.Y * (double)worldViewProjection.M23 + vector.Z * (double)worldViewProjection.M33) + worldViewProjection.M43;
        vector41.W = (float)(vector.X * (double)worldViewProjection.M14 + vector.Y * (double)worldViewProjection.M24 + vector.Z * (double)worldViewProjection.M34) + worldViewProjection.M44;
        Vector4 vector42 = vector41 / vector41.W;
        vector42.X = x + width * (float)((vector42.X + 1.0) / 2.0);
        vector42.Y = y + height * (float)((vector42.Y + 1.0) / 2.0);
        vector42.Z = minZ + (float)((maxZ - (double)minZ) * ((vector42.Z + 1.0) / 2.0));
        return new Vector3(vector42.X, vector42.Y, vector42.Z);
    }
    public static Vector3 Unproject(Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix4 inverseWorldViewProjection)
    {
        Vector4 vector41;
        vector41.X = (float)((vector.X - (double)x) / width * 2.0 - 1.0);
        vector41.Y = (float)((vector.Y - (double)y) / height * 2.0 - 1.0);
        vector41.Z = (float)(vector.Z / (maxZ - (double)minZ) * 2.0 - 1.0);
        vector41.X = (float)(vector41.X * (double)inverseWorldViewProjection.M11 + vector41.Y * (double)inverseWorldViewProjection.M21 + vector41.Z * (double)inverseWorldViewProjection.M31) + inverseWorldViewProjection.M41;
        vector41.Y = (float)(vector41.X * (double)inverseWorldViewProjection.M12 + vector41.Y * (double)inverseWorldViewProjection.M22 + vector41.Z * (double)inverseWorldViewProjection.M32) + inverseWorldViewProjection.M42;
        vector41.Z = (float)(vector41.X * (double)inverseWorldViewProjection.M13 + vector41.Y * (double)inverseWorldViewProjection.M23 + vector41.Z * (double)inverseWorldViewProjection.M33) + inverseWorldViewProjection.M43;
        vector41.W = (float)(vector41.X * (double)inverseWorldViewProjection.M14 + vector41.Y * (double)inverseWorldViewProjection.M24 + vector41.Z * (double)inverseWorldViewProjection.M34) + inverseWorldViewProjection.M44;
        Vector4 vector42 = vector41 / vector41.W;
        return new Vector3(vector42.X, vector42.Y, vector42.Z);
    }
    public Vector2 Xy
    {
        get => new(this.X, this.Y);
        set
        {
            this.X = value.x;
            this.Y = value.y;
        }
    }
    public Vector2 Xz
    {
        get => new(this.X, this.Z);
        set
        {
            this.X = value.x;
            this.Z = value.y;
        }
    }
    public Vector2 Yx
    {
        get => new(this.Y, this.X);
        set
        {
            this.Y = value.x;
            this.X = value.y;
        }
    }
    public Vector2 Yz
    {
        get => new(this.Y, this.Z);
        set
        {
            this.Y = value.x;
            this.Z = value.y;
        }
    }
    public Vector2 Zx
    {
        get => new(this.Z, this.X);
        set
        {
            this.Z = value.x;
            this.X = value.y;
        }
    }
    public Vector2 Zy
    {
        get => new(this.Z, this.Y);
        set
        {
            this.Z = value.x;
            this.Y = value.y;
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
    public static Vector3 operator +(Vector3 left, Vector3 right)
    {
        left.X += right.X;
        left.Y += right.Y;
        left.Z += right.Z;
        return left;
    }
    public static Vector3 operator -(Vector3 left, Vector3 right)
    {
        left.X -= right.X;
        left.Y -= right.Y;
        left.Z -= right.Z;
        return left;
    }
    public static Vector3 operator -(Vector3 vec)
    {
        vec.X = -vec.X;
        vec.Y = -vec.Y;
        vec.Z = -vec.Z;
        return vec;
    }
    public static Vector3 operator *(Vector3 vec, float scale)
    {
        vec.X *= scale;
        vec.Y *= scale;
        vec.Z *= scale;
        return vec;
    }
    public static Vector3 operator *(float scale, Vector3 vec)
    {
        vec.X *= scale;
        vec.Y *= scale;
        vec.Z *= scale;
        return vec;
    }
    public static Vector3 operator *(Vector3 vec, Vector3 scale)
    {
        vec.X *= scale.X;
        vec.Y *= scale.Y;
        vec.Z *= scale.Z;
        return vec;
    }
    public static Vector3 operator *(Vector3 vec, Matrix3 mat)
    {
        Transform(ref vec, ref mat, out var result);
        return result;
    }
    public static Vector3 operator *(Matrix3 mat, Vector3 vec)
    {
        Transform(ref mat, ref vec, out var result);
        return result;
    }
    public static Vector3 operator *(Quaternion quaternion, Vector3 vec)
    {
        Transform(ref vec, ref quaternion, out var result);
        return result;
    }
    public static Vector3 operator /(Vector3 vec, float scale)
    {
        vec.X /= scale;
        vec.Y /= scale;
        vec.Z /= scale;
        return vec;
    }
    public static bool operator ==(Vector3 left, Vector3 right)
    {
        return left.Equals(right);
    }
    public static bool operator !=(Vector3 left, Vector3 right)
    {
        return !left.Equals(right);
    }
    public override string ToString()
    {
        return $"({this.X}{listSeparator} {this.Y}{listSeparator} { this.Z})";
    }
    public override int GetHashCode()
    {
        return (this.X.GetHashCode() * 397 ^ this.Y.GetHashCode()) * 397 ^ this.Z.GetHashCode();
    }
    public override bool Equals(object? obj)
    {
        if (!(obj is Vector3))
        {
            return false;
        }

        return this.Equals((Vector3)obj);
    }
    public bool Equals(Vector3 other)
    {
        if (this.X == (double)other.X && this.Y == (double)other.Y)
        {
            return this.Z == (double)other.Z;
        }

        return false;
    }
}