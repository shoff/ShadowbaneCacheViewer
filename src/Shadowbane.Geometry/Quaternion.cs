namespace Shadowbane.Geometry
{
    using System;

    public struct Quaternion : IEquatable<Quaternion>
    {
        public static readonly Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1f);
        public Vector3 Xyz;
        public float W;
        public Quaternion(Vector3 v, float w)
        {
            this.Xyz = v;
            this.W = w;
        }
        public Quaternion(float x, float y, float z, float w)
        {
            this = new Quaternion(new Vector3(x, y, z), w);
        }
        public Quaternion(float rotationX, float rotationY, float rotationZ)
        {
            rotationX *= 0.5f;
            rotationY *= 0.5f;
            rotationZ *= 0.5f;
            float num1 = (float)Math.Cos((double)rotationX);
            float num2 = (float)Math.Cos((double)rotationY);
            float num3 = (float)Math.Cos((double)rotationZ);
            float num4 = (float)Math.Sin((double)rotationX);
            float num5 = (float)Math.Sin((double)rotationY);
            float num6 = (float)Math.Sin((double)rotationZ);
            this.W = (float)((double)num1 * (double)num2 * (double)num3 - (double)num4 * (double)num5 * (double)num6);
            this.Xyz.X = (float)((double)num4 * (double)num2 * (double)num3 + (double)num1 * (double)num5 * (double)num6);
            this.Xyz.Y = (float)((double)num1 * (double)num5 * (double)num3 - (double)num4 * (double)num2 * (double)num6);
            this.Xyz.Z = (float)((double)num1 * (double)num2 * (double)num6 + (double)num4 * (double)num5 * (double)num3);
        }
        public Quaternion(Vector3 eulerAngles)
        {
            this = new Quaternion(eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
        }
        public float X
        {
            get
            {
                return this.Xyz.X;
            }
            set
            {
                this.Xyz.X = value;
            }
        }
        public float Y
        {
            get
            {
                return this.Xyz.Y;
            }
            set
            {
                this.Xyz.Y = value;
            }
        }
        public float Z
        {
            get
            {
                return this.Xyz.Z;
            }
            set
            {
                this.Xyz.Z = value;
            }
        }
        public void ToAxisAngle(out Vector3 axis, out float angle)
        {
            Vector4 axisAngle = this.ToAxisAngle();
            axis = axisAngle.Xyz;
            angle = axisAngle.W;
        }
        public Vector4 ToAxisAngle()
        {
            Quaternion quaternion = this;
            if ((double)Math.Abs(quaternion.W) > 1.0)
            {
                quaternion.Normalize();
            }

            Vector4 vector4 = new Vector4();
            vector4.W = 2f * (float)Math.Acos((double)quaternion.W);
            float num = (float)Math.Sqrt(1.0 - (double)quaternion.W * (double)quaternion.W);
            vector4.Xyz = (double)num <= 9.99999974737875E-05 ? Vector3.UnitX : quaternion.Xyz / num;
            return vector4;
        }
        public float Length
        {
            get
            {
                return (float)Math.Sqrt((double)this.W * (double)this.W + (double)this.Xyz.LengthSquared);
            }
        }
        public float LengthSquared
        {
            get
            {
                return this.W * this.W + this.Xyz.LengthSquared;
            }
        }
        public Quaternion Normalized()
        {
            Quaternion quaternion = this;
            quaternion.Normalize();
            return quaternion;
        }
        public void Invert()
        {
            this.W = -this.W;
        }
        public Quaternion Inverted()
        {
            Quaternion quaternion = this;
            quaternion.Invert();
            return quaternion;
        }
        public void Normalize()
        {
            float num = 1f / this.Length;
            this.Xyz *= num;
            this.W *= num;
        }
        public void Conjugate()
        {
            this.Xyz = -this.Xyz;
        }
        public static Quaternion Add(Quaternion left, Quaternion right)
        {
            return new Quaternion(left.Xyz + right.Xyz, left.W + right.W);
        }
        public static void Add(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            result = new Quaternion(left.Xyz + right.Xyz, left.W + right.W);
        }
        public static Quaternion Sub(Quaternion left, Quaternion right)
        {
            return new Quaternion(left.Xyz - right.Xyz, left.W - right.W);
        }
        public static void Sub(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            result = new Quaternion(left.Xyz - right.Xyz, left.W - right.W);
        }
        public static Quaternion Multiply(Quaternion left, Quaternion right)
        {
            Quaternion result;
            Quaternion.Multiply(ref left, ref right, out result);
            return result;
        }
        public static void Multiply(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            result = new Quaternion(right.W * left.Xyz + left.W * right.Xyz + Vector3.Cross(left.Xyz, right.Xyz), left.W * right.W - Vector3.Dot(left.Xyz, right.Xyz));
        }
        public static void Multiply(ref Quaternion quaternion, float scale, out Quaternion result)
        {
            result = new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
        }
        public static Quaternion Multiply(Quaternion quaternion, float scale)
        {
            return new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
        }
        public static Quaternion Conjugate(Quaternion q)
        {
            return new Quaternion(-q.Xyz, q.W);
        }
        public static void Conjugate(ref Quaternion q, out Quaternion result)
        {
            result = new Quaternion(-q.Xyz, q.W);
        }
        public static Quaternion Invert(Quaternion q)
        {
            Quaternion result;
            Quaternion.Invert(ref q, out result);
            return result;
        }
        public static void Invert(ref Quaternion q, out Quaternion result)
        {
            float lengthSquared = q.LengthSquared;
            if ((double)lengthSquared != 0.0)
            {
                float num = 1f / lengthSquared;
                result = new Quaternion(q.Xyz * -num, q.W * num);
            }
            else
            {
                result = q;
            }
        }
        public static Quaternion Normalize(Quaternion q)
        {
            Quaternion result;
            Quaternion.Normalize(ref q, out result);
            return result;
        }
        public static void Normalize(ref Quaternion q, out Quaternion result)
        {
            float num = 1f / q.Length;
            result = new Quaternion(q.Xyz * num, q.W * num);
        }
        public static Quaternion FromAxisAngle(Vector3 axis, float angle)
        {
            if ((double)axis.LengthSquared == 0.0)
            {
                return Quaternion.Identity;
            }

            Quaternion identity = Quaternion.Identity;
            angle *= 0.5f;
            axis.Normalize();
            identity.Xyz = axis * (float)Math.Sin((double)angle);
            identity.W = (float)Math.Cos((double)angle);
            return Quaternion.Normalize(identity);
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
            float num1 = (float)Math.Cos((double)eulerAngles.X * 0.5);
            float num2 = (float)Math.Cos((double)eulerAngles.Y * 0.5);
            float num3 = (float)Math.Cos((double)eulerAngles.Z * 0.5);
            float num4 = (float)Math.Sin((double)eulerAngles.X * 0.5);
            float num5 = (float)Math.Sin((double)eulerAngles.Y * 0.5);
            float num6 = (float)Math.Sin((double)eulerAngles.Z * 0.5);
            result.W = (float)((double)num1 * (double)num2 * (double)num3 - (double)num4 * (double)num5 * (double)num6);
            result.Xyz.X = (float)((double)num4 * (double)num2 * (double)num3 + (double)num1 * (double)num5 * (double)num6);
            result.Xyz.Y = (float)((double)num1 * (double)num5 * (double)num3 - (double)num4 * (double)num2 * (double)num6);
            result.Xyz.Z = (float)((double)num1 * (double)num2 * (double)num6 + (double)num4 * (double)num5 * (double)num3);
        }
        public static Quaternion FromMatrix(Matrix3 matrix)
        {
            Quaternion result;
            Quaternion.FromMatrix(ref matrix, out result);
            return result;
        }
        public static void FromMatrix(ref Matrix3 matrix, out Quaternion result)
        {
            float trace = matrix.Trace;
            if ((double)trace > 0.0)
            {
                float num1 = (float)Math.Sqrt((double)trace + 1.0) * 2f;
                float num2 = 1f / num1;
                result.W = num1 * 0.25f;
                result.Xyz.X = (matrix.Row2.Y - matrix.Row1.Z) * num2;
                result.Xyz.Y = (matrix.Row0.Z - matrix.Row2.X) * num2;
                result.Xyz.Z = (matrix.Row1.X - matrix.Row0.Y) * num2;
            }
            else
            {
                float x = matrix.Row0.X;
                float y = matrix.Row1.Y;
                float z = matrix.Row2.Z;
                if ((double)x > (double)y && (double)x > (double)z)
                {
                    float num1 = (float)Math.Sqrt(1.0 + (double)x - (double)y - (double)z) * 2f;
                    float num2 = 1f / num1;
                    result.W = (matrix.Row2.Y - matrix.Row1.Z) * num2;
                    result.Xyz.X = num1 * 0.25f;
                    result.Xyz.Y = (matrix.Row0.Y + matrix.Row1.X) * num2;
                    result.Xyz.Z = (matrix.Row0.Z + matrix.Row2.X) * num2;
                }
                else if ((double)y > (double)z)
                {
                    float num1 = (float)Math.Sqrt(1.0 + (double)y - (double)x - (double)z) * 2f;
                    float num2 = 1f / num1;
                    result.W = (matrix.Row0.Z - matrix.Row2.X) * num2;
                    result.Xyz.X = (matrix.Row0.Y + matrix.Row1.X) * num2;
                    result.Xyz.Y = num1 * 0.25f;
                    result.Xyz.Z = (matrix.Row1.Z + matrix.Row2.Y) * num2;
                }
                else
                {
                    float num1 = (float)Math.Sqrt(1.0 + (double)z - (double)x - (double)y) * 2f;
                    float num2 = 1f / num1;
                    result.W = (matrix.Row1.X - matrix.Row0.Y) * num2;
                    result.Xyz.X = (matrix.Row0.Z + matrix.Row2.X) * num2;
                    result.Xyz.Y = (matrix.Row1.Z + matrix.Row2.Y) * num2;
                    result.Xyz.Z = num1 * 0.25f;
                }
            }
        }
        public static Quaternion Slerp(Quaternion q1, Quaternion q2, float blend)
        {
            if ((double)q1.LengthSquared == 0.0)
            {
                if ((double)q2.LengthSquared == 0.0)
                {
                    return Quaternion.Identity;
                }

                return q2;
            }
            if ((double)q2.LengthSquared == 0.0)
            {
                return q1;
            }

            float num1 = q1.W * q2.W + Vector3.Dot(q1.Xyz, q2.Xyz);
            if ((double)num1 >= 1.0 || (double)num1 <= -1.0)
            {
                return q1;
            }

            if ((double)num1 < 0.0)
            {
                q2.Xyz = -q2.Xyz;
                q2.W = -q2.W;
                num1 = -num1;
            }
            float num2;
            float num3;
            if ((double)num1 < 0.990000009536743)
            {
                float num4 = (float)Math.Acos((double)num1);
                float num5 = 1f / (float)Math.Sin((double)num4);
                num2 = (float)Math.Sin((double)num4 * (1.0 - (double)blend)) * num5;
                num3 = (float)Math.Sin((double)num4 * (double)blend) * num5;
            }
            else
            {
                num2 = 1f - blend;
                num3 = blend;
            }
            Quaternion q = new Quaternion(num2 * q1.Xyz + num3 * q2.Xyz, (float)((double)num2 * (double)q1.W + (double)num3 * (double)q2.W));
            if ((double)q.LengthSquared > 0.0)
            {
                return Quaternion.Normalize(q);
            }

            return Quaternion.Identity;
        }
        public static Quaternion operator +(Quaternion left, Quaternion right)
        {
            left.Xyz += right.Xyz;
            left.W += right.W;
            return left;
        }
        public static Quaternion operator -(Quaternion left, Quaternion right)
        {
            left.Xyz -= right.Xyz;
            left.W -= right.W;
            return left;
        }
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            Quaternion.Multiply(ref left, ref right, out left);
            return left;
        }
        public static Quaternion operator *(Quaternion quaternion, float scale)
        {
            Quaternion.Multiply(ref quaternion, scale, out quaternion);
            return quaternion;
        }
        public static Quaternion operator *(float scale, Quaternion quaternion)
        {
            return new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
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
            return string.Format("V: {0}, W: {1}", (object)this.Xyz, (object)this.W);
        }
        public override bool Equals(object other)
        {
            if (!(other is Quaternion))
            {
                return false;
            }

            return this == (Quaternion)other;
        }
        public override int GetHashCode()
        {
            return this.Xyz.GetHashCode() * 397 ^ this.W.GetHashCode();
        }
        public bool Equals(Quaternion other)
        {
            if (this.Xyz == other.Xyz)
            {
                return (double)this.W == (double)other.W;
            }

            return false;
        }
    }
}