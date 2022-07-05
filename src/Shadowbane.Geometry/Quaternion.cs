namespace Shadowbane.Geometry;

using System;

public struct Quaternion : IEquatable<Quaternion>
{
    public static readonly Quaternion identity = new(0.0f, 0.0f, 0.0f, 1f);
    public Vector3 xyz;
    public float w;

    private Quaternion(Vector3 v, float w)
    {
        this.xyz = v;
        this.w = w;
    }
    public Quaternion(float x, float y, float z, float w)
    {
        this = new Quaternion(new Vector3(x, y, z), w);
    }

    private Quaternion(float rotationX, float rotationY, float rotationZ)
    {
        rotationX *= 0.5f;
        rotationY *= 0.5f;
        rotationZ *= 0.5f;
        var num1 = (float)Math.Cos(rotationX);
        var num2 = (float)Math.Cos(rotationY);
        var num3 = (float)Math.Cos(rotationZ);
        var num4 = (float)Math.Sin(rotationX);
        var num5 = (float)Math.Sin(rotationY);
        var num6 = (float)Math.Sin(rotationZ);
        this.w = (float)(num1 * (double)num2 * num3 - num4 * (double)num5 * num6);
        this.xyz.X = (float)(num4 * (double)num2 * num3 + num1 * (double)num5 * num6);
        this.xyz.Y = (float)(num1 * (double)num5 * num3 - num4 * (double)num2 * num6);
        this.xyz.Z = (float)(num1 * (double)num2 * num6 + num4 * (double)num5 * num3);
    }

    private Quaternion(Vector3 eulerAngles)
    {
        this = new Quaternion(eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
    }
    public float X
    {
        get => this.xyz.X;
        set => this.xyz.X = value;
    }
    public float Y
    {
        get => this.xyz.Y;
        set => this.xyz.Y = value;
    }
    public float Z
    {
        get => this.xyz.Z;
        set => this.xyz.Z = value;
    }
    public void ToAxisAngle(out Vector3 axis, out float angle)
    {
        var axisAngle = this.ToAxisAngle();
        axis = axisAngle.Xyz;
        angle = axisAngle.W;
    }

    private Vector4 ToAxisAngle()
    {
        var quaternion = this;
        if (Math.Abs(quaternion.w) > 1.0)
        {
            quaternion.Normalize();
        }

        var vector4 = new Vector4
        {
            W = 2f * (float)Math.Acos(quaternion.w)
        };
        var num = (float)Math.Sqrt(1.0 - quaternion.w * (double)quaternion.w);
        vector4.Xyz = num <= 9.99999974737875E-05 ? Vector3.UnitX : quaternion.xyz / num;
        return vector4;
    }

    private float Length => (float)Math.Sqrt(this.w * (double)this.w + this.xyz.LengthSquared);

    private float LengthSquared => this.w * this.w + this.xyz.LengthSquared;

    public Quaternion Normalized()
    {
        var quaternion = this;
        quaternion.Normalize();
        return quaternion;
    }

    private void Invert()
    {
        this.w = -this.w;
    }
    public Quaternion Inverted()
    {
        var quaternion = this;
        quaternion.Invert();
        return quaternion;
    }
    public void Normalize()
    {
        var num = 1f / this.Length;
        this.xyz *= num;
        this.w *= num;
    }
    public void Conjugate()
    {
        this.xyz = -this.xyz;
    }
    public static Quaternion Add(Quaternion left, Quaternion right)
    {
        return new Quaternion(left.xyz + right.xyz, left.w + right.w);
    }
    public static void Add(ref Quaternion left, ref Quaternion right, out Quaternion result)
    {
        result = new Quaternion(left.xyz + right.xyz, left.w + right.w);
    }
    public static Quaternion Sub(Quaternion left, Quaternion right)
    {
        return new Quaternion(left.xyz - right.xyz, left.w - right.w);
    }
    public static void Sub(ref Quaternion left, ref Quaternion right, out Quaternion result)
    {
        result = new Quaternion(left.xyz - right.xyz, left.w - right.w);
    }
    public static Quaternion Multiply(Quaternion left, Quaternion right)
    {
        Multiply(ref left, ref right, out var result);
        return result;
    }
    public static void Multiply(ref Quaternion left, ref Quaternion right, out Quaternion result)
    {
        result = new Quaternion(right.w * left.xyz + left.w * right.xyz + Vector3.Cross(left.xyz, right.xyz), left.w * right.w - Vector3.Dot(left.xyz, right.xyz));
    }

    private static void Multiply(ref Quaternion quaternion, float scale, out Quaternion result)
    {
        result = new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.w * scale);
    }
    public static Quaternion Multiply(Quaternion quaternion, float scale)
    {
        return new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.w * scale);
    }
    public static Quaternion Conjugate(Quaternion q)
    {
        return new Quaternion(-q.xyz, q.w);
    }
    public static void Conjugate(ref Quaternion q, out Quaternion result)
    {
        result = new Quaternion(-q.xyz, q.w);
    }
    public static Quaternion Invert(Quaternion q)
    {
        Invert(ref q, out var result);
        return result;
    }
    public static void Invert(ref Quaternion q, out Quaternion result)
    {
        var lengthSquared = q.LengthSquared;
        if (lengthSquared != 0.0)
        {
            var num = 1f / lengthSquared;
            result = new Quaternion(q.xyz * -num, q.w * num);
        }
        else
        {
            result = q;
        }
    }

    private static Quaternion Normalize(Quaternion q)
    {
        Normalize(ref q, out var result);
        return result;
    }

    private static void Normalize(ref Quaternion q, out Quaternion result)
    {
        var num = 1f / q.Length;
        result = new Quaternion(q.xyz * num, q.w * num);
    }
    public static Quaternion FromAxisAngle(Vector3 axis, float angle)
    {
        if (axis.LengthSquared == 0.0)
        {
            return identity;
        }

        var quaternion = identity;
        angle *= 0.5f;
        axis.Normalize();
        quaternion.xyz = axis * (float)Math.Sin(angle);
        quaternion.w = (float)Math.Cos(angle);
        return Normalize(quaternion);
    }
    public static Quaternion FromEulerAngles(float pitch, float yaw, float roll)
    {
        return new Quaternion(pitch, yaw, roll);
    }
    public static Quaternion FromEulerAngles(Vector3 eulerAngles)
    {
        return new Quaternion(eulerAngles);
    }
    public static void FromEulerAngles(ref Vector3 eulerAngles, out Quaternion result)
    {
        var num1 = (float)Math.Cos(eulerAngles.X * 0.5);
        var num2 = (float)Math.Cos(eulerAngles.Y * 0.5);
        var num3 = (float)Math.Cos(eulerAngles.Z * 0.5);
        var num4 = (float)Math.Sin(eulerAngles.X * 0.5);
        var num5 = (float)Math.Sin(eulerAngles.Y * 0.5);
        var num6 = (float)Math.Sin(eulerAngles.Z * 0.5);
        result.w = (float)(num1 * (double)num2 * num3 - num4 * (double)num5 * num6);
        result.xyz.X = (float)(num4 * (double)num2 * num3 + num1 * (double)num5 * num6);
        result.xyz.Y = (float)(num1 * (double)num5 * num3 - num4 * (double)num2 * num6);
        result.xyz.Z = (float)(num1 * (double)num2 * num6 + num4 * (double)num5 * num3);
    }
    public static Quaternion FromMatrix(Matrix3 matrix)
    {
        FromMatrix(ref matrix, out var result);
        return result;
    }

    private static void FromMatrix(ref Matrix3 matrix, out Quaternion result)
    {
        var trace = matrix.Trace;
        if (trace > 0.0)
        {
            var num1 = (float)Math.Sqrt(trace + 1.0) * 2f;
            var num2 = 1f / num1;
            result.w = num1 * 0.25f;
            result.xyz.X = (matrix.Row2.Y - matrix.Row1.Z) * num2;
            result.xyz.Y = (matrix.Row0.Z - matrix.Row2.X) * num2;
            result.xyz.Z = (matrix.Row1.X - matrix.Row0.Y) * num2;
        }
        else
        {
            var x = matrix.Row0.X;
            var y = matrix.Row1.Y;
            var z = matrix.Row2.Z;
            if (x > (double)y && x > (double)z)
            {
                var num1 = (float)Math.Sqrt(1.0 + x - y - z) * 2f;
                var num2 = 1f / num1;
                result.w = (matrix.Row2.Y - matrix.Row1.Z) * num2;
                result.xyz.X = num1 * 0.25f;
                result.xyz.Y = (matrix.Row0.Y + matrix.Row1.X) * num2;
                result.xyz.Z = (matrix.Row0.Z + matrix.Row2.X) * num2;
            }
            else if (y > (double)z)
            {
                var num1 = (float)Math.Sqrt(1.0 + y - x - z) * 2f;
                var num2 = 1f / num1;
                result.w = (matrix.Row0.Z - matrix.Row2.X) * num2;
                result.xyz.X = (matrix.Row0.Y + matrix.Row1.X) * num2;
                result.xyz.Y = num1 * 0.25f;
                result.xyz.Z = (matrix.Row1.Z + matrix.Row2.Y) * num2;
            }
            else
            {
                var num1 = (float)Math.Sqrt(1.0 + z - x - y) * 2f;
                var num2 = 1f / num1;
                result.w = (matrix.Row1.X - matrix.Row0.Y) * num2;
                result.xyz.X = (matrix.Row0.Z + matrix.Row2.X) * num2;
                result.xyz.Y = (matrix.Row1.Z + matrix.Row2.Y) * num2;
                result.xyz.Z = num1 * 0.25f;
            }
        }
    }
    public static Quaternion Slerp(Quaternion q1, Quaternion q2, float blend)
    {
        if (q1.LengthSquared == 0.0)
        {
            if (q2.LengthSquared == 0.0)
            {
                return identity;
            }

            return q2;
        }
        if (q2.LengthSquared == 0.0)
        {
            return q1;
        }

        var num1 = q1.w * q2.w + Vector3.Dot(q1.xyz, q2.xyz);
        if (num1 >= 1.0 || num1 <= -1.0)
        {
            return q1;
        }

        if (num1 < 0.0)
        {
            q2.xyz = -q2.xyz;
            q2.w = -q2.w;
            num1 = -num1;
        }
        float num2;
        float num3;
        if (num1 < 0.990000009536743)
        {
            var num4 = (float)Math.Acos(num1);
            var num5 = 1f / (float)Math.Sin(num4);
            num2 = (float)Math.Sin(num4 * (1.0 - blend)) * num5;
            num3 = (float)Math.Sin(num4 * (double)blend) * num5;
        }
        else
        {
            num2 = 1f - blend;
            num3 = blend;
        }
        var q = new Quaternion(
            num2 * q1.xyz + num3 * q2.xyz, 
            (float)(num2 * (double)q1.w + num3 * (double)q2.w));
        if (q.LengthSquared > 0.0)
        {
            return Normalize(q);
        }

        return identity;
    }
    public static Quaternion operator +(Quaternion left, Quaternion right)
    {
        left.xyz += right.xyz;
        left.w += right.w;
        return left;
    }
    public static Quaternion operator -(Quaternion left, Quaternion right)
    {
        left.xyz -= right.xyz;
        left.w -= right.w;
        return left;
    }
    public static Quaternion operator *(Quaternion left, Quaternion right)
    {
        Multiply(ref left, ref right, out left);
        return left;
    }
    public static Quaternion operator *(Quaternion quaternion, float scale)
    {
        Multiply(ref quaternion, scale, out quaternion);
        return quaternion;
    }
    public static Quaternion operator *(float scale, Quaternion quaternion)
    {
        return new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.w * scale);
    }
    public static bool operator ==(Quaternion left, Quaternion right)
    {
        return left.Equals(right);
    }
    public static bool operator !=(Quaternion left, Quaternion right)
    {
        return !left.Equals(right);
    }
    public override string ToString()
    {
        return $"V: {this.xyz}, W: {this.w}";
    }
    public override bool Equals(object? other)
    {
        var quaternion = other as Quaternion?;
        if (quaternion == null)
        {
            return false;
        }

        return this == quaternion;
    }
    public override int GetHashCode()
    {
        return this.xyz.GetHashCode() * 397 ^ this.w.GetHashCode();
    }
    public bool Equals(Quaternion other)
    {
        if (this.xyz == other.xyz)
        {
            return Math.Abs((double)this.w - other.w) < TOLERANCE;
        }

        return false;
    }

    private const double TOLERANCE = 0.0000001;
}