namespace Shadowbane.Geometry;

using System;

public struct Matrix3 : IEquatable<Matrix3>
{
    public static readonly Matrix3 Identity = new(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);
    public static readonly Matrix3 Zero = new(Vector3.Zero, Vector3.Zero, Vector3.Zero);
    public Vector3 Row0;
    public Vector3 Row1;
    public Vector3 Row2;
    public Matrix3(Vector3 row0, Vector3 row1, Vector3 row2)
    {
        this.Row0 = row0;
        this.Row1 = row1;
        this.Row2 = row2;
    }
    public Matrix3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
    {
        this.Row0 = new Vector3(m00, m01, m02);
        this.Row1 = new Vector3(m10, m11, m12);
        this.Row2 = new Vector3(m20, m21, m22);
    }
    public Matrix3(Matrix4 matrix)
    {
        this.Row0 = matrix.Row0.Xyz;
        this.Row1 = matrix.Row1.Xyz;
        this.Row2 = matrix.Row2.Xyz;
    }
    public float Determinant
    {
        get
        {
            float x1 = this.Row0.X;
            float y1 = this.Row0.Y;
            float z1 = this.Row0.Z;
            float x2 = this.Row1.X;
            float y2 = this.Row1.Y;
            float z2 = this.Row1.Z;
            float x3 = this.Row2.X;
            float y3 = this.Row2.Y;
            float z3 = this.Row2.Z;
            return (float)(x1 * (double)y2 * z3 + y1 * (double)z2 * x3 + z1 * (double)x2 * y3 - z1 * (double)y2 * x3 - x1 * (double)z2 * y3 - y1 * (double)x2 * z3);
        }
    }
    public Vector3 Column0 => new(this.Row0.X, this.Row1.X, this.Row2.X);
    public Vector3 Column1 => new(this.Row0.Y, this.Row1.Y, this.Row2.Y);
    public Vector3 Column2 => new(this.Row0.Z, this.Row1.Z, this.Row2.Z);
    public float M11
    {
        get => this.Row0.X;
        set => this.Row0.X = value;
    }
    public float M12
    {
        get => this.Row0.Y;
        set => this.Row0.Y = value;
    }
    public float M13
    {
        get => this.Row0.Z;
        set => this.Row0.Z = value;
    }
    public float M21
    {
        get => this.Row1.X;
        set => this.Row1.X = value;
    }
    public float M22
    {
        get => this.Row1.Y;
        set => this.Row1.Y = value;
    }
    public float M23
    {
        get => this.Row1.Z;
        set => this.Row1.Z = value;
    }
    public float M31
    {
        get => this.Row2.X;
        set => this.Row2.X = value;
    }
    public float M32
    {
        get => this.Row2.Y;
        set => this.Row2.Y = value;
    }
    public float M33
    {
        get => this.Row2.Z;
        set => this.Row2.Z = value;
    }
    public Vector3 Diagonal
    {
        get => new(this.Row0.X, this.Row1.Y, this.Row2.Z);
        set
        {
            this.Row0.X = value.X;
            this.Row1.Y = value.Y;
            this.Row2.Z = value.Z;
        }
    }
    public float Trace => this.Row0.X + this.Row1.Y + this.Row2.Z;
    public float this[int rowIndex, int columnIndex]
    {
        get
        {
            switch (rowIndex)
            {
                case 0:
                    return this.Row0[columnIndex];
                case 1:
                    return this.Row1[columnIndex];
                case 2:
                    return this.Row2[columnIndex];
                default:
                    throw new IndexOutOfRangeException("You tried to access this matrix at: (" + rowIndex + ", " + columnIndex + ")");
            }
        }
        set
        {
            switch (rowIndex)
            {
                case 0:
                    this.Row0[columnIndex] = value;
                    break;
                case 1:
                    this.Row1[columnIndex] = value;
                    break;
                case 2:
                    this.Row2[columnIndex] = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("You tried to set this matrix at: (" + rowIndex + ", " + columnIndex + ")");
            }
        }
    }
    public void Invert()
    {
        this = Matrix3.Invert(this);
    }
    public void Transpose()
    {
        this = Matrix3.Transpose(this);
    }
    public Matrix3 Normalized()
    {
        Matrix3 matrix3 = this;
        matrix3.Normalize();
        return matrix3;
    }
    public void Normalize()
    {
        float determinant = this.Determinant;
        this.Row0 /= determinant;
        this.Row1 /= determinant;
        this.Row2 /= determinant;
    }
    public Matrix3 Inverted()
    {
        Matrix3 matrix3 = this;
        if (matrix3.Determinant != 0.0)
        {
            matrix3.Invert();
        }

        return matrix3;
    }
    public Matrix3 ClearScale()
    {
        Matrix3 matrix3 = this;
        matrix3.Row0 = matrix3.Row0.Normalized();
        matrix3.Row1 = matrix3.Row1.Normalized();
        matrix3.Row2 = matrix3.Row2.Normalized();
        return matrix3;
    }
    public Matrix3 ClearRotation()
    {
        Matrix3 matrix3 = this;
        matrix3.Row0 = new Vector3(matrix3.Row0.Length, 0.0f, 0.0f);
        matrix3.Row1 = new Vector3(0.0f, matrix3.Row1.Length, 0.0f);
        matrix3.Row2 = new Vector3(0.0f, 0.0f, matrix3.Row2.Length);
        return matrix3;
    }
    public Vector3 ExtractScale()
    {
        return new Vector3(this.Row0.Length, this.Row1.Length, this.Row2.Length);
    }
    public Quaternion ExtractRotation(bool normalizeRow = true)
    {
        Vector3 vector31 = this.Row0;
        Vector3 vector32 = this.Row1;
        Vector3 vector33 = this.Row2;
        if (normalizeRow)
        {
            vector31 = vector31.Normalized();
            vector32 = vector32.Normalized();
            vector33 = vector33.Normalized();
        }
        Quaternion quaternion = new Quaternion();
        double d = 0.25 * (vector31[0] + (double)vector32[1] + vector33[2] + 1.0);
        if (d > 0.0)
        {
            double num1 = Math.Sqrt(d);
            quaternion.w = (float)num1;
            double num2 = 1.0 / (4.0 * num1);
            quaternion.X = (float)((vector32[2] - (double)vector33[1]) * num2);
            quaternion.Y = (float)((vector33[0] - (double)vector31[2]) * num2);
            quaternion.Z = (float)((vector31[1] - (double)vector32[0]) * num2);
        }
        else if (vector31[0] > (double)vector32[1] && vector31[0] > (double)vector33[2])
        {
            double num1 = 2.0 * Math.Sqrt(1.0 + vector31[0] - vector32[1] - vector33[2]);
            quaternion.X = (float)(0.25 * num1);
            double num2 = 1.0 / num1;
            quaternion.w = (float)((vector33[1] - (double)vector32[2]) * num2);
            quaternion.Y = (float)((vector32[0] + (double)vector31[1]) * num2);
            quaternion.Z = (float)((vector33[0] + (double)vector31[2]) * num2);
        }
        else if (vector32[1] > (double)vector33[2])
        {
            double num1 = 2.0 * Math.Sqrt(1.0 + vector32[1] - vector31[0] - vector33[2]);
            quaternion.Y = (float)(0.25 * num1);
            double num2 = 1.0 / num1;
            quaternion.w = (float)((vector33[0] - (double)vector31[2]) * num2);
            quaternion.X = (float)((vector32[0] + (double)vector31[1]) * num2);
            quaternion.Z = (float)((vector33[1] + (double)vector32[2]) * num2);
        }
        else
        {
            double num1 = 2.0 * Math.Sqrt(1.0 + vector33[2] - vector31[0] - vector32[1]);
            quaternion.Z = (float)(0.25 * num1);
            double num2 = 1.0 / num1;
            quaternion.w = (float)((vector32[0] - (double)vector31[1]) * num2);
            quaternion.X = (float)((vector33[0] + (double)vector31[2]) * num2);
            quaternion.Y = (float)((vector33[1] + (double)vector32[2]) * num2);
        }
        quaternion.Normalize();
        return quaternion;
    }
    public static void CreateFromAxisAngle(Vector3 axis, float angle, out Matrix3 result)
    {
        axis.Normalize();
        float x = axis.X;
        float y = axis.Y;
        float z = axis.Z;
        float num1 = (float)Math.Cos(-(double)angle);
        float num2 = (float)Math.Sin(-(double)angle);
        float num3 = 1f - num1;
        float num4 = num3 * x * x;
        float num5 = num3 * x * y;
        float num6 = num3 * x * z;
        float num7 = num3 * y * y;
        float num8 = num3 * y * z;
        float num9 = num3 * z * z;
        float num10 = num2 * x;
        float num11 = num2 * y;
        float num12 = num2 * z;
        result.Row0.X = num4 + num1;
        result.Row0.Y = num5 - num12;
        result.Row0.Z = num6 + num11;
        result.Row1.X = num5 + num12;
        result.Row1.Y = num7 + num1;
        result.Row1.Z = num8 - num10;
        result.Row2.X = num6 - num11;
        result.Row2.Y = num8 + num10;
        result.Row2.Z = num9 + num1;
    }
    public static Matrix3 CreateFromAxisAngle(Vector3 axis, float angle)
    {
        Matrix3 result;
        Matrix3.CreateFromAxisAngle(axis, angle, out result);
        return result;
    }
    public static void CreateFromQuaternion(ref Quaternion q, out Matrix3 result)
    {
        Vector3 axis;
        float angle;
        q.ToAxisAngle(out axis, out angle);
        Matrix3.CreateFromAxisAngle(axis, angle, out result);
    }
    public static Matrix3 CreateFromQuaternion(Quaternion q)
    {
        Matrix3 result;
        Matrix3.CreateFromQuaternion(ref q, out result);
        return result;
    }
    public static void CreateRotationX(float angle, out Matrix3 result)
    {
        float num1 = (float)Math.Cos(angle);
        float num2 = (float)Math.Sin(angle);
        result = Matrix3.Identity;
        result.Row1.Y = num1;
        result.Row1.Z = num2;
        result.Row2.Y = -num2;
        result.Row2.Z = num1;
    }
    public static Matrix3 CreateRotationX(float angle)
    {
        Matrix3 result;
        Matrix3.CreateRotationX(angle, out result);
        return result;
    }
    public static void CreateRotationY(float angle, out Matrix3 result)
    {
        float num1 = (float)Math.Cos(angle);
        float num2 = (float)Math.Sin(angle);
        result = Matrix3.Identity;
        result.Row0.X = num1;
        result.Row0.Z = -num2;
        result.Row2.X = num2;
        result.Row2.Z = num1;
    }
    public static Matrix3 CreateRotationY(float angle)
    {
        Matrix3 result;
        Matrix3.CreateRotationY(angle, out result);
        return result;
    }
    public static void CreateRotationZ(float angle, out Matrix3 result)
    {
        float num1 = (float)Math.Cos(angle);
        float num2 = (float)Math.Sin(angle);
        result = Matrix3.Identity;
        result.Row0.X = num1;
        result.Row0.Y = num2;
        result.Row1.X = -num2;
        result.Row1.Y = num1;
    }
    public static Matrix3 CreateRotationZ(float angle)
    {
        Matrix3 result;
        Matrix3.CreateRotationZ(angle, out result);
        return result;
    }
    public static Matrix3 CreateScale(float scale)
    {
        Matrix3 result;
        Matrix3.CreateScale(scale, out result);
        return result;
    }
    public static Matrix3 CreateScale(Vector3 scale)
    {
        Matrix3 result;
        Matrix3.CreateScale(ref scale, out result);
        return result;
    }
    public static Matrix3 CreateScale(float x, float y, float z)
    {
        Matrix3 result;
        Matrix3.CreateScale(x, y, z, out result);
        return result;
    }
    public static void CreateScale(float scale, out Matrix3 result)
    {
        result = Matrix3.Identity;
        result.Row0.X = scale;
        result.Row1.Y = scale;
        result.Row2.Z = scale;
    }
    public static void CreateScale(ref Vector3 scale, out Matrix3 result)
    {
        result = Matrix3.Identity;
        result.Row0.X = scale.X;
        result.Row1.Y = scale.Y;
        result.Row2.Z = scale.Z;
    }
    public static void CreateScale(float x, float y, float z, out Matrix3 result)
    {
        result = Matrix3.Identity;
        result.Row0.X = x;
        result.Row1.Y = y;
        result.Row2.Z = z;
    }
    public static Matrix3 Add(Matrix3 left, Matrix3 right)
    {
        Matrix3.Add(ref left, ref right, out var result);
        return result;
    }
    public static void Add(ref Matrix3 left, ref Matrix3 right, out Matrix3 result)
    {
        Vector3.Add(ref left.Row0, ref right.Row0, out result.Row0);
        Vector3.Add(ref left.Row1, ref right.Row1, out result.Row1);
        Vector3.Add(ref left.Row2, ref right.Row2, out result.Row2);
    }
    public static Matrix3 Multiply(Matrix3 left, Matrix3 right)
    {
        Matrix3 result;
        Matrix3.Multiply(ref left, ref right, out result);
        return result;
    }
    public static void Multiply(ref Matrix3 left, ref Matrix3 right, out Matrix3 result)
    {
        float x1 = left.Row0.X;
        float y1 = left.Row0.Y;
        float z1 = left.Row0.Z;
        float x2 = left.Row1.X;
        float y2 = left.Row1.Y;
        float z2 = left.Row1.Z;
        float x3 = left.Row2.X;
        float y3 = left.Row2.Y;
        float z3 = left.Row2.Z;
        float x4 = right.Row0.X;
        float y4 = right.Row0.Y;
        float z4 = right.Row0.Z;
        float x5 = right.Row1.X;
        float y5 = right.Row1.Y;
        float z5 = right.Row1.Z;
        float x6 = right.Row2.X;
        float y6 = right.Row2.Y;
        float z6 = right.Row2.Z;
        result.Row0.X = (float)(x1 * (double)x4 + y1 * (double)x5 + z1 * (double)x6);
        result.Row0.Y = (float)(x1 * (double)y4 + y1 * (double)y5 + z1 * (double)y6);
        result.Row0.Z = (float)(x1 * (double)z4 + y1 * (double)z5 + z1 * (double)z6);
        result.Row1.X = (float)(x2 * (double)x4 + y2 * (double)x5 + z2 * (double)x6);
        result.Row1.Y = (float)(x2 * (double)y4 + y2 * (double)y5 + z2 * (double)y6);
        result.Row1.Z = (float)(x2 * (double)z4 + y2 * (double)z5 + z2 * (double)z6);
        result.Row2.X = (float)(x3 * (double)x4 + y3 * (double)x5 + z3 * (double)x6);
        result.Row2.Y = (float)(x3 * (double)y4 + y3 * (double)y5 + z3 * (double)y6);
        result.Row2.Z = (float)(x3 * (double)z4 + y3 * (double)z5 + z3 * (double)z6);
    }
    public static void Invert(ref Matrix3 mat, out Matrix3 result)
    {
        int[] numArray1 = new int[3];
        int[] numArray2 = new int[3];
        int[] numArray3 = new int[3] { -1, -1, -1 };
        float[,] numArray4 = new float[3, 3]
        {
            {
                mat.Row0.X,
                mat.Row0.Y,
                mat.Row0.Z
            },
            {
                mat.Row1.X,
                mat.Row1.Y,
                mat.Row1.Z
            },
            {
                mat.Row2.X,
                mat.Row2.Y,
                mat.Row2.Z
            }
        };
        int index1 = 0;
        int index2 = 0;
        for (int index3 = 0; index3 < 3; ++index3)
        {
            float num1 = 0.0f;
            for (int index4 = 0; index4 < 3; ++index4)
            {
                if (numArray3[index4] != 0)
                {
                    for (int index5 = 0; index5 < 3; ++index5)
                    {
                        if (numArray3[index5] == -1)
                        {
                            float num2 = Math.Abs(numArray4[index4, index5]);
                            if (num2 > (double)num1)
                            {
                                num1 = num2;
                                index2 = index4;
                                index1 = index5;
                            }
                        }
                        else if (numArray3[index5] > 0)
                        {
                            result = mat;
                            return;
                        }
                    }
                }
            }
            ++numArray3[index1];
            if (index2 != index1)
            {
                for (int index4 = 0; index4 < 3; ++index4)
                {
                    float num2 = numArray4[index2, index4];
                    numArray4[index2, index4] = numArray4[index1, index4];
                    numArray4[index1, index4] = num2;
                }
            }
            numArray2[index3] = index2;
            numArray1[index3] = index1;
            float num3 = numArray4[index1, index1];
            if (num3 == 0.0)
            {
                throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
            }

            float num4 = 1f / num3;
            numArray4[index1, index1] = 1f;
            for (int index4 = 0; index4 < 3; ++index4)
            {
                numArray4[index1, index4] *= num4;
            }

            for (int index4 = 0; index4 < 3; ++index4)
            {
                if (index1 != index4)
                {
                    float num2 = numArray4[index4, index1];
                    numArray4[index4, index1] = 0.0f;
                    for (int index5 = 0; index5 < 3; ++index5)
                    {
                        numArray4[index4, index5] -= numArray4[index1, index5] * num2;
                    }
                }
            }
        }
        for (int index3 = 2; index3 >= 0; --index3)
        {
            int index4 = numArray2[index3];
            int index5 = numArray1[index3];
            for (int index6 = 0; index6 < 3; ++index6)
            {
                float num = numArray4[index6, index4];
                numArray4[index6, index4] = numArray4[index6, index5];
                numArray4[index6, index5] = num;
            }
        }
        result.Row0.X = numArray4[0, 0];
        result.Row0.Y = numArray4[0, 1];
        result.Row0.Z = numArray4[0, 2];
        result.Row1.X = numArray4[1, 0];
        result.Row1.Y = numArray4[1, 1];
        result.Row1.Z = numArray4[1, 2];
        result.Row2.X = numArray4[2, 0];
        result.Row2.Y = numArray4[2, 1];
        result.Row2.Z = numArray4[2, 2];
    }
    public static Matrix3 Invert(Matrix3 mat)
    {
        Matrix3 result;
        Matrix3.Invert(ref mat, out result);
        return result;
    }
    public static Matrix3 Transpose(Matrix3 mat)
    {
        return new Matrix3(mat.Column0, mat.Column1, mat.Column2);
    }
    public static void Transpose(ref Matrix3 mat, out Matrix3 result)
    {
        result.Row0.X = mat.Row0.X;
        result.Row0.Y = mat.Row1.X;
        result.Row0.Z = mat.Row2.X;
        result.Row1.X = mat.Row0.Y;
        result.Row1.Y = mat.Row1.Y;
        result.Row1.Z = mat.Row2.Y;
        result.Row2.X = mat.Row0.Z;
        result.Row2.Y = mat.Row1.Z;
        result.Row2.Z = mat.Row2.Z;
    }
    public static Matrix3 operator *(Matrix3 left, Matrix3 right)
    {
        return Matrix3.Multiply(left, right);
    }
    public static bool operator ==(Matrix3 left, Matrix3 right)
    {
        return left.Equals(right);
    }
    public static bool operator !=(Matrix3 left, Matrix3 right)
    {
        return !left.Equals(right);
    }
    public override string ToString()
    {
        return string.Format("{0}\n{1}\n{2}", this.Row0, this.Row1, this.Row2);
    }
    public override int GetHashCode()
    {
        return (this.Row0.GetHashCode() * 397 ^ this.Row1.GetHashCode()) * 397 ^ this.Row2.GetHashCode();
    }
    public override bool Equals(object? obj)
    {
        if (!(obj is Matrix3))
        {
            return false;
        }

        return this.Equals((Matrix3)obj);
    }
    public bool Equals(Matrix3 other)
    {
        if (this.Row0 == other.Row0 && this.Row1 == other.Row1)
        {
            return this.Row2 == other.Row2;
        }

        return false;
    }
}