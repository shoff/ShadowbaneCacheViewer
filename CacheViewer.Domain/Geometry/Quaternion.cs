namespace CacheViewer.Domain.Geometry
{
    using System;
    using System.Xml.Serialization;

    /// <summary>Represents a Quaternion.</summary>
    [Serializable]
    public struct Quaternion : IEquatable<Quaternion>
    {
        /// <summary>Defines the identity quaternion.</summary>
        public static readonly Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1f);
        /// <summary>The X, Y and Z components of this instance.</summary>
        public Vector3 Xyz;
        /// <summary>The W component of this instance.</summary>
        public float W;

        /// <summary>Construct a new Quaternion from vector and w components</summary>
        /// <param name="v">The vector part</param>
        /// <param name="w">The w part</param>
        public Quaternion(Vector3 v, float w)
        {
            this.Xyz = v;
            this.W = w;
        }

        /// <summary>Construct a new Quaternion</summary>
        /// <param name="x">The x component</param>
        /// <param name="y">The y component</param>
        /// <param name="z">The z component</param>
        /// <param name="w">The w component</param>
        public Quaternion(float x, float y, float z, float w)
        {
            this = new Quaternion(new Vector3(x, y, z), w);
        }

        /// <summary>
        /// Construct a new Quaternion from given Euler angles in radians.
        /// The rotations will get applied in following order:
        /// 1. around X axis, 2. around Y axis, 3. around Z axis
        /// </summary>
        /// <param name="rotationX">Counterclockwise rotation around X axis in radian</param>
        /// <param name="rotationY">Counterclockwise rotation around Y axis in radian</param>
        /// <param name="rotationZ">Counterclockwise rotation around Z axis in radian</param>
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

        /// <summary>
        /// Construct a new Quaternion from given Euler angles. The rotations will get applied in following order:
        /// 1. Around X, 2. Around Y, 3. Around Z
        /// </summary>
        /// <param name="eulerAngles">The counterclockwise euler angles as a Vector3</param>
        public Quaternion(Vector3 eulerAngles)
        {
            this = new Quaternion(eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
        }

        /// <summary>Gets or sets the X component of this instance.</summary>
        [XmlIgnore]
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

        /// <summary>Gets or sets the Y component of this instance.</summary>
        [XmlIgnore]
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

        /// <summary>Gets or sets the Z component of this instance.</summary>
        [XmlIgnore]
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

        /// <summary>
        /// Convert the current quaternion to axis angle representation
        /// </summary>
        /// <param name="axis">The resultant axis</param>
        /// <param name="angle">The resultant angle</param>
        public void ToAxisAngle(out Vector3 axis, out float angle)
        {
            Vector4 axisAngle = this.ToAxisAngle();
            axis = axisAngle.Xyz;
            angle = axisAngle.W;
        }

        /// <summary>Convert this instance to an axis-angle representation.</summary>
        /// <returns>A Vector4 that is the axis-angle representation of this quaternion.</returns>
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

        /// <summary>Gets the length (magnitude) of the quaternion.</summary>
        /// <seealso cref="P:OpenTK.Quaternion.LengthSquared" />
        public float Length
        {
            get
            {
                return (float)Math.Sqrt((double)this.W * (double)this.W + (double)this.Xyz.LengthSquared);
            }
        }

        /// <summary>Gets the square of the quaternion length (magnitude).</summary>
        public float LengthSquared
        {
            get
            {
                return this.W * this.W + this.Xyz.LengthSquared;
            }
        }

        /// <summary>Returns a copy of the Quaternion scaled to unit length.</summary>
        public Quaternion Normalized()
        {
            Quaternion quaternion = this;
            quaternion.Normalize();
            return quaternion;
        }

        /// <summary>Reverses the rotation angle of this Quaterniond.</summary>
        public void Invert()
        {
            this.W = -this.W;
        }

        /// <summary>
        /// Returns a copy of this Quaterniond with its rotation angle reversed.
        /// </summary>
        public Quaternion Inverted()
        {
            Quaternion quaternion = this;
            quaternion.Invert();
            return quaternion;
        }

        /// <summary>Scales the Quaternion to unit length.</summary>
        public void Normalize()
        {
            float num = 1f / this.Length;
            this.Xyz *= num;
            this.W *= num;
        }

        /// <summary>Inverts the Vector3 component of this Quaternion.</summary>
        public void Conjugate()
        {
            this.Xyz = -this.Xyz;
        }

        /// <summary>Add two quaternions</summary>
        /// <param name="left">The first operand</param>
        /// <param name="right">The second operand</param>
        /// <returns>The result of the addition</returns>
        public static Quaternion Add(Quaternion left, Quaternion right)
        {
            return new Quaternion(left.Xyz + right.Xyz, left.W + right.W);
        }

        /// <summary>Add two quaternions</summary>
        /// <param name="left">The first operand</param>
        /// <param name="right">The second operand</param>
        /// <param name="result">The result of the addition</param>
        public static void Add(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            result = new Quaternion(left.Xyz + right.Xyz, left.W + right.W);
        }

        /// <summary>Subtracts two instances.</summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion Sub(Quaternion left, Quaternion right)
        {
            return new Quaternion(left.Xyz - right.Xyz, left.W - right.W);
        }

        /// <summary>Subtracts two instances.</summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <param name="result">The result of the operation.</param>
        public static void Sub(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            result = new Quaternion(left.Xyz - right.Xyz, left.W - right.W);
        }

        /// <summary>Multiplies two instances.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>A new instance containing the result of the calculation.</returns>
        public static Quaternion Multiply(Quaternion left, Quaternion right)
        {
            Quaternion result;
            Quaternion.Multiply(ref left, ref right, out result);
            return result;
        }

        /// <summary>Multiplies two instances.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <param name="result">A new instance containing the result of the calculation.</param>
        public static void Multiply(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            result = new Quaternion(right.W * left.Xyz + left.W * right.Xyz + Vector3.Cross(left.Xyz, right.Xyz), left.W * right.W - Vector3.Dot(left.Xyz, right.Xyz));
        }

        /// <summary>Multiplies an instance by a scalar.</summary>
        /// <param name="quaternion">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <param name="result">A new instance containing the result of the calculation.</param>
        public static void Multiply(ref Quaternion quaternion, float scale, out Quaternion result)
        {
            result = new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
        }

        /// <summary>Multiplies an instance by a scalar.</summary>
        /// <param name="quaternion">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>A new instance containing the result of the calculation.</returns>
        public static Quaternion Multiply(Quaternion quaternion, float scale)
        {
            return new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
        }

        /// <summary>Get the conjugate of the given quaternion</summary>
        /// <param name="q">The quaternion</param>
        /// <returns>The conjugate of the given quaternion</returns>
        public static Quaternion Conjugate(Quaternion q)
        {
            return new Quaternion(-q.Xyz, q.W);
        }

        /// <summary>Get the conjugate of the given quaternion</summary>
        /// <param name="q">The quaternion</param>
        /// <param name="result">The conjugate of the given quaternion</param>
        public static void Conjugate(ref Quaternion q, out Quaternion result)
        {
            result = new Quaternion(-q.Xyz, q.W);
        }

        /// <summary>Get the inverse of the given quaternion</summary>
        /// <param name="q">The quaternion to invert</param>
        /// <returns>The inverse of the given quaternion</returns>
        public static Quaternion Invert(Quaternion q)
        {
            Quaternion result;
            Quaternion.Invert(ref q, out result);
            return result;
        }

        /// <summary>Get the inverse of the given quaternion</summary>
        /// <param name="q">The quaternion to invert</param>
        /// <param name="result">The inverse of the given quaternion</param>
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

        /// <summary>Scale the given quaternion to unit length</summary>
        /// <param name="q">The quaternion to normalize</param>
        /// <returns>The normalized quaternion</returns>
        public static Quaternion Normalize(Quaternion q)
        {
            Quaternion result;
            Quaternion.Normalize(ref q, out result);
            return result;
        }

        /// <summary>Scale the given quaternion to unit length</summary>
        /// <param name="q">The quaternion to normalize</param>
        /// <param name="result">The normalized quaternion</param>
        public static void Normalize(ref Quaternion q, out Quaternion result)
        {
            float num = 1f / q.Length;
            result = new Quaternion(q.Xyz * num, q.W * num);
        }

        /// <summary>
        /// Build a quaternion from the given axis and angle in radians
        /// </summary>
        /// <param name="axis">The axis to rotate about</param>
        /// <param name="angle">The rotation angle in radians</param>
        /// <returns>The equivalent quaternion</returns>
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

        /// <summary>
        /// Builds a Quaternion from the given euler angles in radians
        /// The rotations will get applied in following order:
        /// 1. pitch (X axis), 2. yaw (Y axis), 3. roll (Z axis)
        /// </summary>
        /// <param name="pitch">The pitch (attitude), counterclockwise rotation around X axis</param>
        /// <param name="yaw">The yaw (heading), counterclockwise rotation around Y axis</param>
        /// <param name="roll">The roll (bank), counterclockwise rotation around Z axis</param>
        /// <returns></returns>
        public static Quaternion FromEulerAngles(float pitch, float yaw, float roll)
        {
            return new Quaternion(pitch, yaw, roll);
        }

        /// <summary>
        /// Builds a Quaternion from the given euler angles in radians.
        /// The rotations will get applied in following order:
        /// 1. X axis, 2. Y axis, 3. Z axis
        /// </summary>
        /// <param name="eulerAngles">The counterclockwise euler angles as a vector</param>
        /// <returns>The equivalent Quaternion</returns>
        public static Quaternion FromEulerAngles(Vector3 eulerAngles)
        {
            return new Quaternion(eulerAngles);
        }

        /// <summary>
        /// Builds a Quaternion from the given euler angles in radians.
        /// The rotations will get applied in following order:
        /// 1. Around X, 2. Around Y, 3. Around Z
        /// </summary>
        /// <param name="eulerAngles">The counterclockwise euler angles a vector</param>
        /// <param name="result">The equivalent Quaternion</param>
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

        /// <summary>Builds a quaternion from the given rotation matrix</summary>
        /// <param name="matrix">A rotation matrix</param>
        /// <returns>The equivalent quaternion</returns>
        public static Quaternion FromMatrix(Matrix3 matrix)
        {
            Quaternion result;
            Quaternion.FromMatrix(ref matrix, out result);
            return result;
        }

        /// <summary>Builds a quaternion from the given rotation matrix</summary>
        /// <param name="matrix">A rotation matrix</param>
        /// <param name="result">The equivalent quaternion</param>
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

        /// <summary>
        /// Do Spherical linear interpolation between two quaternions
        /// </summary>
        /// <param name="q1">The first quaternion</param>
        /// <param name="q2">The second quaternion</param>
        /// <param name="blend">The blend factor</param>
        /// <returns>A smooth blend between the given quaternions</returns>
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

        /// <summary>Adds two instances.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static Quaternion operator +(Quaternion left, Quaternion right)
        {
            left.Xyz += right.Xyz;
            left.W += right.W;
            return left;
        }

        /// <summary>Subtracts two instances.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static Quaternion operator -(Quaternion left, Quaternion right)
        {
            left.Xyz -= right.Xyz;
            left.W -= right.W;
            return left;
        }

        /// <summary>Multiplies two instances.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            Quaternion.Multiply(ref left, ref right, out left);
            return left;
        }

        /// <summary>Multiplies an instance by a scalar.</summary>
        /// <param name="quaternion">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>A new instance containing the result of the calculation.</returns>
        public static Quaternion operator *(Quaternion quaternion, float scale)
        {
            Quaternion.Multiply(ref quaternion, scale, out quaternion);
            return quaternion;
        }

        /// <summary>Multiplies an instance by a scalar.</summary>
        /// <param name="quaternion">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>A new instance containing the result of the calculation.</returns>
        public static Quaternion operator *(float scale, Quaternion quaternion)
        {
            return new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
        }

        /// <summary>Compares two instances for equality.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(Quaternion left, Quaternion right)
        {
            return left.Equals(right);
        }

        /// <summary>Compares two instances for inequality.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equal right; false otherwise.</returns>
        public static bool operator !=(Quaternion left, Quaternion right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a System.String that represents the current Quaternion.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("V: {0}, W: {1}", (object)this.Xyz, (object)this.W);
        }

        /// <summary>
        /// Compares this object instance to another object for equality.
        /// </summary>
        /// <param name="other">The other object to be used in the comparison.</param>
        /// <returns>True if both objects are Quaternions of equal value. Otherwise it returns false.</returns>
        public override bool Equals(object other)
        {
            if (!(other is Quaternion))
            {
                return false;
            }

            return this == (Quaternion)other;
        }

        /// <summary>Provides the hash code for this object.</summary>
        /// <returns>A hash code formed from the bitwise XOR of this objects members.</returns>
        public override int GetHashCode()
        {
            return this.Xyz.GetHashCode() * 397 ^ this.W.GetHashCode();
        }

        /// <summary>
        /// Compares this Quaternion instance to another Quaternion for equality.
        /// </summary>
        /// <param name="other">The other Quaternion to be used in the comparison.</param>
        /// <returns>True if both instances are equal; false otherwise.</returns>
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