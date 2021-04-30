namespace Shadowbane.Geometry
{
    using System;

    public struct Matrix4 : IEquatable<Matrix4>
    {
        public static readonly Matrix4 Identity = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);
        public static readonly Matrix4 Zero = new Matrix4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero);
        public Vector4 Row0;
        public Vector4 Row1;
        public Vector4 Row2;
        public Vector4 Row3;
        
        public Matrix4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
        {
            this.Row0 = row0;
            this.Row1 = row1;
            this.Row2 = row2;
            this.Row3 = row3;
        }
        public Matrix4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
        {
            this.Row0 = new Vector4(m00, m01, m02, m03);
            this.Row1 = new Vector4(m10, m11, m12, m13);
            this.Row2 = new Vector4(m20, m21, m22, m23);
            this.Row3 = new Vector4(m30, m31, m32, m33);
        }
        public Matrix4(Matrix3 topLeft)
        {
            this.Row0.X = topLeft.Row0.X;
            this.Row0.Y = topLeft.Row0.Y;
            this.Row0.Z = topLeft.Row0.Z;
            this.Row0.W = 0.0f;
            this.Row1.X = topLeft.Row1.X;
            this.Row1.Y = topLeft.Row1.Y;
            this.Row1.Z = topLeft.Row1.Z;
            this.Row1.W = 0.0f;
            this.Row2.X = topLeft.Row2.X;
            this.Row2.Y = topLeft.Row2.Y;
            this.Row2.Z = topLeft.Row2.Z;
            this.Row2.W = 0.0f;
            this.Row3.X = 0.0f;
            this.Row3.Y = 0.0f;
            this.Row3.Z = 0.0f;
            this.Row3.W = 1f;
        }
        public float Determinant
        {
            get
            {
                float x1 = this.Row0.X;
                float y1 = this.Row0.Y;
                float z1 = this.Row0.Z;
                float w1 = this.Row0.W;
                float x2 = this.Row1.X;
                float y2 = this.Row1.Y;
                float z2 = this.Row1.Z;
                float w2 = this.Row1.W;
                float x3 = this.Row2.X;
                float y3 = this.Row2.Y;
                float z3 = this.Row2.Z;
                float w3 = this.Row2.W;
                float x4 = this.Row3.X;
                float y4 = this.Row3.Y;
                float z4 = this.Row3.Z;
                float w4 = this.Row3.W;
                return (float)(x1 * (double)y2 * z3 * w4 - x1 * (double)y2 * w3 * z4 + x1 * (double)z2 * w3 * y4 - x1 * (double)z2 * y3 * w4 + x1 * (double)w2 * y3 * z4 - x1 * (double)w2 * z3 * y4 - y1 * (double)z2 * w3 * x4 + y1 * (double)z2 * x3 * w4 - y1 * (double)w2 * x3 * z4 + y1 * (double)w2 * z3 * x4 - y1 * (double)x2 * z3 * w4 + y1 * (double)x2 * w3 * z4 + z1 * (double)w2 * x3 * y4 - z1 * (double)w2 * y3 * x4 + z1 * (double)x2 * y3 * w4 - z1 * (double)x2 * w3 * y4 + z1 * (double)y2 * w3 * x4 - z1 * (double)y2 * x3 * w4 - w1 * (double)x2 * y3 * z4 + w1 * (double)x2 * z3 * y4 - w1 * (double)y2 * z3 * x4 + w1 * (double)y2 * x3 * z4 - w1 * (double)z2 * x3 * y4 + w1 * (double)z2 * y3 * x4);
            }
        }
        public Vector4 Column0
        {
            get => new Vector4(this.Row0.X, this.Row1.X, this.Row2.X, this.Row3.X);
            set
            {
                this.Row0.X = value.X;
                this.Row1.X = value.Y;
                this.Row2.X = value.Z;
                this.Row3.X = value.W;
            }
        }
        public Vector4 Column1
        {
            get => new Vector4(this.Row0.Y, this.Row1.Y, this.Row2.Y, this.Row3.Y);
            set
            {
                this.Row0.Y = value.X;
                this.Row1.Y = value.Y;
                this.Row2.Y = value.Z;
                this.Row3.Y = value.W;
            }
        }
        public Vector4 Column2
        {
            get => new Vector4(this.Row0.Z, this.Row1.Z, this.Row2.Z, this.Row3.Z);
            set
            {
                this.Row0.Z = value.X;
                this.Row1.Z = value.Y;
                this.Row2.Z = value.Z;
                this.Row3.Z = value.W;
            }
        }
        public Vector4 Column3
        {
            get => new Vector4(this.Row0.W, this.Row1.W, this.Row2.W, this.Row3.W);
            set
            {
                this.Row0.W = value.X;
                this.Row1.W = value.Y;
                this.Row2.W = value.Z;
                this.Row3.W = value.W;
            }
        }
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
        public float M14
        {
            get => this.Row0.W;
            set => this.Row0.W = value;
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
        public float M24
        {
            get => this.Row1.W;
            set => this.Row1.W = value;
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
        public float M34
        {
            get => this.Row2.W;
            set => this.Row2.W = value;
        }
        public float M41
        {
            get => this.Row3.X;
            set => this.Row3.X = value;
        }
        public float M42
        {
            get => this.Row3.Y;
            set => this.Row3.Y = value;
        }
        public float M43
        {
            get => this.Row3.Z;
            set => this.Row3.Z = value;
        }
        public float M44
        {
            get => this.Row3.W;
            set => this.Row3.W = value;
        }
        public Vector4 Diagonal
        {
            get => new Vector4(this.Row0.X, this.Row1.Y, this.Row2.Z, this.Row3.W);
            set
            {
                this.Row0.X = value.X;
                this.Row1.Y = value.Y;
                this.Row2.Z = value.Z;
                this.Row3.W = value.W;
            }
        }
        public float Trace => this.Row0.X + this.Row1.Y + this.Row2.Z + this.Row3.W;
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
                    case 3:
                        return this.Row3[columnIndex];
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
                    case 3:
                        this.Row3[columnIndex] = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("You tried to set this matrix at: (" + rowIndex + ", " + columnIndex + ")");
                }
            }
        }
        public void Invert()
        {
            this = Matrix4.Invert(this);
        }
        public void Transpose()
        {
            this = Matrix4.Transpose(this);
        }
        public Matrix4 Normalized()
        {
            Matrix4 matrix4 = this;
            matrix4.Normalize();
            return matrix4;
        }
        public void Normalize()
        {
            float determinant = this.Determinant;
            this.Row0 /= determinant;
            this.Row1 /= determinant;
            this.Row2 /= determinant;
            this.Row3 /= determinant;
        }
        public Matrix4 Inverted()
        {
            Matrix4 matrix4 = this;
            if (matrix4.Determinant != 0.0)
            {
                matrix4.Invert();
            }

            return matrix4;
        }
        public Matrix4 ClearTranslation()
        {
            Matrix4 matrix4 = this;
            matrix4.Row3.Xyz = Vector3.Zero;
            return matrix4;
        }
        public Matrix4 ClearScale()
        {
            Matrix4 matrix4 = this;
            matrix4.Row0.Xyz = matrix4.Row0.Xyz.Normalized();
            matrix4.Row1.Xyz = matrix4.Row1.Xyz.Normalized();
            matrix4.Row2.Xyz = matrix4.Row2.Xyz.Normalized();
            return matrix4;
        }
        public Matrix4 ClearRotation()
        {
            Matrix4 matrix4 = this;
            matrix4.Row0.Xyz = new Vector3(matrix4.Row0.Xyz.Length, 0.0f, 0.0f);
            matrix4.Row1.Xyz = new Vector3(0.0f, matrix4.Row1.Xyz.Length, 0.0f);
            matrix4.Row2.Xyz = new Vector3(0.0f, 0.0f, matrix4.Row2.Xyz.Length);
            return matrix4;
        }
        public Matrix4 ClearProjection()
        {
            Matrix4 matrix4 = this;
            matrix4.Column3 = Vector4.Zero;
            return matrix4;
        }
        public Vector3 ExtractTranslation()
        {
            return this.Row3.Xyz;
        }
        public Vector3 ExtractScale()
        {
            Vector3 xyz = this.Row0.Xyz;
            double length1 = xyz.Length;
            xyz = this.Row1.Xyz;
            double length2 = xyz.Length;
            xyz = this.Row2.Xyz;
            double length3 = xyz.Length;
            return new Vector3((float)length1, (float)length2, (float)length3);
        }
        public Quaternion ExtractRotation(bool rowNormalise = true)
        {
            Vector3 vector3_1 = this.Row0.Xyz;
            Vector3 vector3_2 = this.Row1.Xyz;
            Vector3 vector3_3 = this.Row2.Xyz;
            if (rowNormalise)
            {
                vector3_1 = vector3_1.Normalized();
                vector3_2 = vector3_2.Normalized();
                vector3_3 = vector3_3.Normalized();
            }
            Quaternion quaternion = new Quaternion();
            double d = 0.25 * (vector3_1[0] + (double)vector3_2[1] + vector3_3[2] + 1.0);
            if (d > 0.0)
            {
                double num1 = Math.Sqrt(d);
                quaternion.W = (float)num1;
                double num2 = 1.0 / (4.0 * num1);
                quaternion.X = (float)((vector3_2[2] - (double)vector3_3[1]) * num2);
                quaternion.Y = (float)((vector3_3[0] - (double)vector3_1[2]) * num2);
                quaternion.Z = (float)((vector3_1[1] - (double)vector3_2[0]) * num2);
            }
            else if (vector3_1[0] > (double)vector3_2[1] && vector3_1[0] > (double)vector3_3[2])
            {
                double num1 = 2.0 * Math.Sqrt(1.0 + vector3_1[0] - vector3_2[1] - vector3_3[2]);
                quaternion.X = (float)(0.25 * num1);
                double num2 = 1.0 / num1;
                quaternion.W = (float)((vector3_3[1] - (double)vector3_2[2]) * num2);
                quaternion.Y = (float)((vector3_2[0] + (double)vector3_1[1]) * num2);
                quaternion.Z = (float)((vector3_3[0] + (double)vector3_1[2]) * num2);
            }
            else if (vector3_2[1] > (double)vector3_3[2])
            {
                double num1 = 2.0 * Math.Sqrt(1.0 + vector3_2[1] - vector3_1[0] - vector3_3[2]);
                quaternion.Y = (float)(0.25 * num1);
                double num2 = 1.0 / num1;
                quaternion.W = (float)((vector3_3[0] - (double)vector3_1[2]) * num2);
                quaternion.X = (float)((vector3_2[0] + (double)vector3_1[1]) * num2);
                quaternion.Z = (float)((vector3_3[1] + (double)vector3_2[2]) * num2);
            }
            else
            {
                double num1 = 2.0 * Math.Sqrt(1.0 + vector3_3[2] - vector3_1[0] - vector3_2[1]);
                quaternion.Z = (float)(0.25 * num1);
                double num2 = 1.0 / num1;
                quaternion.W = (float)((vector3_2[0] - (double)vector3_1[1]) * num2);
                quaternion.X = (float)((vector3_3[0] + (double)vector3_1[2]) * num2);
                quaternion.Y = (float)((vector3_3[1] + (double)vector3_2[2]) * num2);
            }
            quaternion.Normalize();
            return quaternion;
        }
        public Vector4 ExtractProjection()
        {
            return this.Column3;
        }
        public static void CreateFromAxisAngle(Vector3 axis, float angle, out Matrix4 result)
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
            result.Row0.W = 0.0f;
            result.Row1.X = num5 + num12;
            result.Row1.Y = num7 + num1;
            result.Row1.Z = num8 - num10;
            result.Row1.W = 0.0f;
            result.Row2.X = num6 - num11;
            result.Row2.Y = num8 + num10;
            result.Row2.Z = num9 + num1;
            result.Row2.W = 0.0f;
            result.Row3 = Vector4.UnitW;
        }
        public static Matrix4 CreateFromAxisAngle(Vector3 axis, float angle)
        {
            Matrix4 result;
            Matrix4.CreateFromAxisAngle(axis, angle, out result);
            return result;
        }
        public static void CreateFromQuaternion(ref Quaternion q, out Matrix4 result)
        {
            Vector3 axis;
            float angle;
            q.ToAxisAngle(out axis, out angle);
            Matrix4.CreateFromAxisAngle(axis, angle, out result);
        }
        public static Matrix4 CreateFromQuaternion(Quaternion q)
        {
            Matrix4 result;
            Matrix4.CreateFromQuaternion(ref q, out result);
            return result;
        }
        public static void CreateRotationX(float angle, out Matrix4 result)
        {
            float num1 = (float)Math.Cos(angle);
            float num2 = (float)Math.Sin(angle);
            result = Matrix4.Identity;
            result.Row1.Y = num1;
            result.Row1.Z = num2;
            result.Row2.Y = -num2;
            result.Row2.Z = num1;
        }
        public static Matrix4 CreateRotationX(float angle)
        {
            Matrix4 result;
            Matrix4.CreateRotationX(angle, out result);
            return result;
        }
        public static void CreateRotationY(float angle, out Matrix4 result)
        {
            float num1 = (float)Math.Cos(angle);
            float num2 = (float)Math.Sin(angle);
            result = Matrix4.Identity;
            result.Row0.X = num1;
            result.Row0.Z = -num2;
            result.Row2.X = num2;
            result.Row2.Z = num1;
        }
        public static Matrix4 CreateRotationY(float angle)
        {
            Matrix4 result;
            Matrix4.CreateRotationY(angle, out result);
            return result;
        }
        public static void CreateRotationZ(float angle, out Matrix4 result)
        {
            float num1 = (float)Math.Cos(angle);
            float num2 = (float)Math.Sin(angle);
            result = Matrix4.Identity;
            result.Row0.X = num1;
            result.Row0.Y = num2;
            result.Row1.X = -num2;
            result.Row1.Y = num1;
        }
        public static Matrix4 CreateRotationZ(float angle)
        {
            Matrix4 result;
            Matrix4.CreateRotationZ(angle, out result);
            return result;
        }
        public static void CreateTranslation(float x, float y, float z, out Matrix4 result)
        {
            result = Matrix4.Identity;
            result.Row3.X = x;
            result.Row3.Y = y;
            result.Row3.Z = z;
        }
        public static void CreateTranslation(ref Vector3 vector, out Matrix4 result)
        {
            result = Matrix4.Identity;
            result.Row3.X = vector.X;
            result.Row3.Y = vector.Y;
            result.Row3.Z = vector.Z;
        }
        public static Matrix4 CreateTranslation(float x, float y, float z)
        {
            Matrix4 result;
            Matrix4.CreateTranslation(x, y, z, out result);
            return result;
        }
        public static Matrix4 CreateTranslation(Vector3 vector)
        {
            Matrix4 result;
            Matrix4.CreateTranslation(vector.X, vector.Y, vector.Z, out result);
            return result;
        }
        public static Matrix4 CreateScale(float scale)
        {
            Matrix4 result;
            Matrix4.CreateScale(scale, out result);
            return result;
        }
        public static Matrix4 CreateScale(Vector3 scale)
        {
            Matrix4 result;
            Matrix4.CreateScale(ref scale, out result);
            return result;
        }
        public static Matrix4 CreateScale(float x, float y, float z)
        {
            Matrix4 result;
            Matrix4.CreateScale(x, y, z, out result);
            return result;
        }
        public static void CreateScale(float scale, out Matrix4 result)
        {
            result = Matrix4.Identity;
            result.Row0.X = scale;
            result.Row1.Y = scale;
            result.Row2.Z = scale;
        }
        public static void CreateScale(ref Vector3 scale, out Matrix4 result)
        {
            result = Matrix4.Identity;
            result.Row0.X = scale.X;
            result.Row1.Y = scale.Y;
            result.Row2.Z = scale.Z;
        }
        public static void CreateScale(float x, float y, float z, out Matrix4 result)
        {
            result = Matrix4.Identity;
            result.Row0.X = x;
            result.Row1.Y = y;
            result.Row2.Z = z;
        }
        public static void CreateOrthographic(float width, float height, float zNear, float zFar, out Matrix4 result)
        {
            Matrix4.CreateOrthographicOffCenter((float)(-(double)width / 2.0), width / 2f, (float)(-(double)height / 2.0), height / 2f, zNear, zFar, out result);
        }
        public static Matrix4 CreateOrthographic(float width, float height, float zNear, float zFar)
        {
            Matrix4 result;
            Matrix4.CreateOrthographicOffCenter((float)(-(double)width / 2.0), width / 2f, (float)(-(double)height / 2.0), height / 2f, zNear, zFar, out result);
            return result;
        }
        public static void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear, float zFar, out Matrix4 result)
        {
            result = Matrix4.Identity;
            float num1 = (float)(1.0 / (right - (double)left));
            float num2 = (float)(1.0 / (top - (double)bottom));
            float num3 = (float)(1.0 / (zFar - (double)zNear));
            result.Row0.X = 2f * num1;
            result.Row1.Y = 2f * num2;
            result.Row2.Z = -2f * num3;
            result.Row3.X = (float)-(right + (double)left) * num1;
            result.Row3.Y = (float)-(top + (double)bottom) * num2;
            result.Row3.Z = (float)-(zFar + (double)zNear) * num3;
        }
        public static Matrix4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            Matrix4 result;
            Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, zNear, zFar, out result);
            return result;
        }
        public static void CreatePerspectiveFieldOfView(float fovy, float aspect, float zNear, float zFar, out Matrix4 result)
        {
            if (fovy <= 0.0 || fovy > Math.PI)
            {
                throw new ArgumentOutOfRangeException(nameof(fovy));
            }

            if (aspect <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(aspect));
            }

            if (zNear <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(zNear));
            }

            if (zFar <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(zFar));
            }

            float top = zNear * (float)Math.Tan(0.5 * fovy);
            float bottom = -top;
            Matrix4.CreatePerspectiveOffCenter(bottom * aspect, top * aspect, bottom, top, zNear, zFar, out result);
        }
        public static Matrix4 CreatePerspectiveFieldOfView(float fovy, float aspect, float zNear, float zFar)
        {
            Matrix4 result;
            Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, zNear, zFar, out result);
            return result;
        }
        public static void CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float zNear, float zFar, out Matrix4 result)
        {
            if (zNear <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(zNear));
            }

            if (zFar <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(zFar));
            }

            if (zNear >= (double)zFar)
            {
                throw new ArgumentOutOfRangeException(nameof(zNear));
            }

            float num1 = (float)(2.0 * zNear / (right - (double)left));
            float num2 = (float)(2.0 * zNear / (top - (double)bottom));
            float num3 = (float)((right + (double)left) / (right - (double)left));
            float num4 = (float)((top + (double)bottom) / (top - (double)bottom));
            float num5 = (float)(-(zFar + (double)zNear) / (zFar - (double)zNear));
            float num6 = (float)(-(2.0 * zFar * zNear) / (zFar - (double)zNear));
            result.Row0.X = num1;
            result.Row0.Y = 0.0f;
            result.Row0.Z = 0.0f;
            result.Row0.W = 0.0f;
            result.Row1.X = 0.0f;
            result.Row1.Y = num2;
            result.Row1.Z = 0.0f;
            result.Row1.W = 0.0f;
            result.Row2.X = num3;
            result.Row2.Y = num4;
            result.Row2.Z = num5;
            result.Row2.W = -1f;
            result.Row3.X = 0.0f;
            result.Row3.Y = 0.0f;
            result.Row3.Z = num6;
            result.Row3.W = 0.0f;
        }
        public static Matrix4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            Matrix4 result;
            Matrix4.CreatePerspectiveOffCenter(left, right, bottom, top, zNear, zFar, out result);
            return result;
        }
        public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 vector3_1 = Vector3.Normalize(eye - target);
            Vector3 right = Vector3.Normalize(Vector3.Cross(up, vector3_1));
            Vector3 vector3_2 = Vector3.Normalize(Vector3.Cross(vector3_1, right));
            Matrix4 matrix4;
            matrix4.Row0.X = right.X;
            matrix4.Row0.Y = vector3_2.X;
            matrix4.Row0.Z = vector3_1.X;
            matrix4.Row0.W = 0.0f;
            matrix4.Row1.X = right.Y;
            matrix4.Row1.Y = vector3_2.Y;
            matrix4.Row1.Z = vector3_1.Y;
            matrix4.Row1.W = 0.0f;
            matrix4.Row2.X = right.Z;
            matrix4.Row2.Y = vector3_2.Z;
            matrix4.Row2.Z = vector3_1.Z;
            matrix4.Row2.W = 0.0f;
            matrix4.Row3.X = (float)-(right.X * (double)eye.X + right.Y * (double)eye.Y + right.Z * (double)eye.Z);
            matrix4.Row3.Y = (float)-(vector3_2.X * (double)eye.X + vector3_2.Y * (double)eye.Y + vector3_2.Z * (double)eye.Z);
            matrix4.Row3.Z = (float)-(vector3_1.X * (double)eye.X + vector3_1.Y * (double)eye.Y + vector3_1.Z * (double)eye.Z);
            matrix4.Row3.W = 1f;
            return matrix4;
        }
        public static Matrix4 LookAt(float eyeX, float eyeY, float eyeZ, float targetX, float targetY, float targetZ, float upX, float upY, float upZ)
        {
            return Matrix4.LookAt(new Vector3(eyeX, eyeY, eyeZ), new Vector3(targetX, targetY, targetZ), new Vector3(upX, upY, upZ));
        }
        public static Matrix4 Add(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Matrix4.Add(ref left, ref right, out result);
            return result;
        }
        public static void Add(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            result.Row0 = left.Row0 + right.Row0;
            result.Row1 = left.Row1 + right.Row1;
            result.Row2 = left.Row2 + right.Row2;
            result.Row3 = left.Row3 + right.Row3;
        }
        public static Matrix4 Subtract(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Matrix4.Subtract(ref left, ref right, out result);
            return result;
        }
        public static void Subtract(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            result.Row0 = left.Row0 - right.Row0;
            result.Row1 = left.Row1 - right.Row1;
            result.Row2 = left.Row2 - right.Row2;
            result.Row3 = left.Row3 - right.Row3;
        }
        public static Matrix4 Multiply(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Matrix4.Multiply(ref left, ref right, out result);
            return result;
        }
        public static void Multiply(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            float x1 = left.Row0.X;
            float y1 = left.Row0.Y;
            float z1 = left.Row0.Z;
            float w1 = left.Row0.W;
            float x2 = left.Row1.X;
            float y2 = left.Row1.Y;
            float z2 = left.Row1.Z;
            float w2 = left.Row1.W;
            float x3 = left.Row2.X;
            float y3 = left.Row2.Y;
            float z3 = left.Row2.Z;
            float w3 = left.Row2.W;
            float x4 = left.Row3.X;
            float y4 = left.Row3.Y;
            float z4 = left.Row3.Z;
            float w4 = left.Row3.W;
            float x5 = right.Row0.X;
            float y5 = right.Row0.Y;
            float z5 = right.Row0.Z;
            float w5 = right.Row0.W;
            float x6 = right.Row1.X;
            float y6 = right.Row1.Y;
            float z6 = right.Row1.Z;
            float w6 = right.Row1.W;
            float x7 = right.Row2.X;
            float y7 = right.Row2.Y;
            float z7 = right.Row2.Z;
            float w7 = right.Row2.W;
            float x8 = right.Row3.X;
            float y8 = right.Row3.Y;
            float z8 = right.Row3.Z;
            float w8 = right.Row3.W;
            result.Row0.X = (float)(x1 * (double)x5 + y1 * (double)x6 + z1 * (double)x7 + w1 * (double)x8);
            result.Row0.Y = (float)(x1 * (double)y5 + y1 * (double)y6 + z1 * (double)y7 + w1 * (double)y8);
            result.Row0.Z = (float)(x1 * (double)z5 + y1 * (double)z6 + z1 * (double)z7 + w1 * (double)z8);
            result.Row0.W = (float)(x1 * (double)w5 + y1 * (double)w6 + z1 * (double)w7 + w1 * (double)w8);
            result.Row1.X = (float)(x2 * (double)x5 + y2 * (double)x6 + z2 * (double)x7 + w2 * (double)x8);
            result.Row1.Y = (float)(x2 * (double)y5 + y2 * (double)y6 + z2 * (double)y7 + w2 * (double)y8);
            result.Row1.Z = (float)(x2 * (double)z5 + y2 * (double)z6 + z2 * (double)z7 + w2 * (double)z8);
            result.Row1.W = (float)(x2 * (double)w5 + y2 * (double)w6 + z2 * (double)w7 + w2 * (double)w8);
            result.Row2.X = (float)(x3 * (double)x5 + y3 * (double)x6 + z3 * (double)x7 + w3 * (double)x8);
            result.Row2.Y = (float)(x3 * (double)y5 + y3 * (double)y6 + z3 * (double)y7 + w3 * (double)y8);
            result.Row2.Z = (float)(x3 * (double)z5 + y3 * (double)z6 + z3 * (double)z7 + w3 * (double)z8);
            result.Row2.W = (float)(x3 * (double)w5 + y3 * (double)w6 + z3 * (double)w7 + w3 * (double)w8);
            result.Row3.X = (float)(x4 * (double)x5 + y4 * (double)x6 + z4 * (double)x7 + w4 * (double)x8);
            result.Row3.Y = (float)(x4 * (double)y5 + y4 * (double)y6 + z4 * (double)y7 + w4 * (double)y8);
            result.Row3.Z = (float)(x4 * (double)z5 + y4 * (double)z6 + z4 * (double)z7 + w4 * (double)z8);
            result.Row3.W = (float)(x4 * (double)w5 + y4 * (double)w6 + z4 * (double)w7 + w4 * (double)w8);
        }
        public static Matrix4 Multiply(Matrix4 left, float right)
        {
            Matrix4 result;
            Matrix4.Multiply(ref left, right, out result);
            return result;
        }
        public static void Multiply(ref Matrix4 left, float right, out Matrix4 result)
        {
            result.Row0 = left.Row0 * right;
            result.Row1 = left.Row1 * right;
            result.Row2 = left.Row2 * right;
            result.Row3 = left.Row3 * right;
        }
        public static unsafe void Invert(ref Matrix4 mat, out Matrix4 result)
        {
            result = mat;
            // ISSUE: untyped stack allocation
            float* numPtr1 = (float*)(new IntPtr(64));
            fixed (Matrix4* matrix4Ptr1 = &mat)
            fixed (Matrix4* matrix4Ptr2 = &result)
            {
                float* numPtr2 = (float*)matrix4Ptr1;
                float* numPtr3 = (float*)matrix4Ptr2;
                *numPtr1 = (float)(numPtr2[5] * (double)numPtr2[10] * numPtr2[15] - numPtr2[5] * (double)numPtr2[11] * numPtr2[14] - numPtr2[9] * (double)numPtr2[6] * numPtr2[15] + numPtr2[9] * (double)numPtr2[7] * numPtr2[14] + numPtr2[13] * (double)numPtr2[6] * numPtr2[11] - numPtr2[13] * (double)numPtr2[7] * numPtr2[10]);
                numPtr1[4] = (float)(-(double)numPtr2[4] * numPtr2[10] * numPtr2[15] + numPtr2[4] * (double)numPtr2[11] * numPtr2[14] + numPtr2[8] * (double)numPtr2[6] * numPtr2[15] - numPtr2[8] * (double)numPtr2[7] * numPtr2[14] - numPtr2[12] * (double)numPtr2[6] * numPtr2[11] + numPtr2[12] * (double)numPtr2[7] * numPtr2[10]);
                numPtr1[8] = (float)(numPtr2[4] * (double)numPtr2[9] * numPtr2[15] - numPtr2[4] * (double)numPtr2[11] * numPtr2[13] - numPtr2[8] * (double)numPtr2[5] * numPtr2[15] + numPtr2[8] * (double)numPtr2[7] * numPtr2[13] + numPtr2[12] * (double)numPtr2[5] * numPtr2[11] - numPtr2[12] * (double)numPtr2[7] * numPtr2[9]);
                numPtr1[12] = (float)(-(double)numPtr2[4] * numPtr2[9] * numPtr2[14] + numPtr2[4] * (double)numPtr2[10] * numPtr2[13] + numPtr2[8] * (double)numPtr2[5] * numPtr2[14] - numPtr2[8] * (double)numPtr2[6] * numPtr2[13] - numPtr2[12] * (double)numPtr2[5] * numPtr2[10] + numPtr2[12] * (double)numPtr2[6] * numPtr2[9]);
                *(float*)((IntPtr)numPtr1 + 4) = (float)(-(double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[10] * numPtr2[15] + *(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[11] * numPtr2[14] + numPtr2[9] * (double)numPtr2[2] * numPtr2[15] - numPtr2[9] * (double)numPtr2[3] * numPtr2[14] - numPtr2[13] * (double)numPtr2[2] * numPtr2[11] + numPtr2[13] * (double)numPtr2[3] * numPtr2[10]);
                numPtr1[5] = (float)(*numPtr2 * (double)numPtr2[10] * numPtr2[15] - *numPtr2 * (double)numPtr2[11] * numPtr2[14] - numPtr2[8] * (double)numPtr2[2] * numPtr2[15] + numPtr2[8] * (double)numPtr2[3] * numPtr2[14] + numPtr2[12] * (double)numPtr2[2] * numPtr2[11] - numPtr2[12] * (double)numPtr2[3] * numPtr2[10]);
                numPtr1[9] = (float)(-(double)*numPtr2 * numPtr2[9] * numPtr2[15] + *numPtr2 * (double)numPtr2[11] * numPtr2[13] + numPtr2[8] * (double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[15] - numPtr2[8] * (double)numPtr2[3] * numPtr2[13] - numPtr2[12] * (double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[11] + numPtr2[12] * (double)numPtr2[3] * numPtr2[9]);
                numPtr1[13] = (float)(*numPtr2 * (double)numPtr2[9] * numPtr2[14] - *numPtr2 * (double)numPtr2[10] * numPtr2[13] - numPtr2[8] * (double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[14] + numPtr2[8] * (double)numPtr2[2] * numPtr2[13] + numPtr2[12] * (double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[10] - numPtr2[12] * (double)numPtr2[2] * numPtr2[9]);
                numPtr1[2] = (float)(*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[6] * numPtr2[15] - *(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[7] * numPtr2[14] - numPtr2[5] * (double)numPtr2[2] * numPtr2[15] + numPtr2[5] * (double)numPtr2[3] * numPtr2[14] + numPtr2[13] * (double)numPtr2[2] * numPtr2[7] - numPtr2[13] * (double)numPtr2[3] * numPtr2[6]);
                numPtr1[6] = (float)(-(double)*numPtr2 * numPtr2[6] * numPtr2[15] + *numPtr2 * (double)numPtr2[7] * numPtr2[14] + numPtr2[4] * (double)numPtr2[2] * numPtr2[15] - numPtr2[4] * (double)numPtr2[3] * numPtr2[14] - numPtr2[12] * (double)numPtr2[2] * numPtr2[7] + numPtr2[12] * (double)numPtr2[3] * numPtr2[6]);
                numPtr1[10] = (float)(*numPtr2 * (double)numPtr2[5] * numPtr2[15] - *numPtr2 * (double)numPtr2[7] * numPtr2[13] - numPtr2[4] * (double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[15] + numPtr2[4] * (double)numPtr2[3] * numPtr2[13] + numPtr2[12] * (double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[7] - numPtr2[12] * (double)numPtr2[3] * numPtr2[5]);
                numPtr1[14] = (float)(-(double)*numPtr2 * numPtr2[5] * numPtr2[14] + *numPtr2 * (double)numPtr2[6] * numPtr2[13] + numPtr2[4] * (double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[14] - numPtr2[4] * (double)numPtr2[2] * numPtr2[13] - numPtr2[12] * (double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[6] + numPtr2[12] * (double)numPtr2[2] * numPtr2[5]);
                numPtr1[3] = (float)(-(double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[6] * numPtr2[11] + *(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[7] * numPtr2[10] + numPtr2[5] * (double)numPtr2[2] * numPtr2[11] - numPtr2[5] * (double)numPtr2[3] * numPtr2[10] - numPtr2[9] * (double)numPtr2[2] * numPtr2[7] + numPtr2[9] * (double)numPtr2[3] * numPtr2[6]);
                numPtr1[7] = (float)(*numPtr2 * (double)numPtr2[6] * numPtr2[11] - *numPtr2 * (double)numPtr2[7] * numPtr2[10] - numPtr2[4] * (double)numPtr2[2] * numPtr2[11] + numPtr2[4] * (double)numPtr2[3] * numPtr2[10] + numPtr2[8] * (double)numPtr2[2] * numPtr2[7] - numPtr2[8] * (double)numPtr2[3] * numPtr2[6]);
                numPtr1[11] = (float)(-(double)*numPtr2 * numPtr2[5] * numPtr2[11] + *numPtr2 * (double)numPtr2[7] * numPtr2[9] + numPtr2[4] * (double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[11] - numPtr2[4] * (double)numPtr2[3] * numPtr2[9] - numPtr2[8] * (double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[7] + numPtr2[8] * (double)numPtr2[3] * numPtr2[5]);
                numPtr1[15] = (float)(*numPtr2 * (double)numPtr2[5] * numPtr2[10] - *numPtr2 * (double)numPtr2[6] * numPtr2[9] - numPtr2[4] * (double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[10] + numPtr2[4] * (double)numPtr2[2] * numPtr2[9] + numPtr2[8] * (double)*(float*)((IntPtr)numPtr2 + 4) * numPtr2[6] - numPtr2[8] * (double)numPtr2[2] * numPtr2[5]);
                float num1 = (float)(*numPtr2 * (double)*numPtr1 + *(float*)((IntPtr)numPtr2 + 4) * (double)numPtr1[4] + numPtr2[2] * (double)numPtr1[8] + numPtr2[3] * (double)numPtr1[12]);
                if (num1 == 0.0)
                {
                    throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
                }

                float num2 = 1f / num1;
                for (int index = 0; index < 16; ++index)
                {
                    numPtr3[index] = numPtr1[index] * num2;
                }
            }
        }
        public static Matrix4 Invert(Matrix4 mat)
        {
            Matrix4 result;
            Matrix4.Invert(ref mat, out result);
            return result;
        }
        public static Matrix4 Transpose(Matrix4 mat)
        {
            return new Matrix4(mat.Column0, mat.Column1, mat.Column2, mat.Column3);
        }
        public static void Transpose(ref Matrix4 mat, out Matrix4 result)
        {
            result.Row0 = mat.Column0;
            result.Row1 = mat.Column1;
            result.Row2 = mat.Column2;
            result.Row3 = mat.Column3;
        }
        public static Matrix4 operator *(Matrix4 left, Matrix4 right)
        {
            return Matrix4.Multiply(left, right);
        }
        public static Matrix4 operator *(Matrix4 left, float right)
        {
            return Matrix4.Multiply(left, right);
        }
        public static Matrix4 operator +(Matrix4 left, Matrix4 right)
        {
            return Matrix4.Add(left, right);
        }
        public static Matrix4 operator -(Matrix4 left, Matrix4 right)
        {
            return Matrix4.Subtract(left, right);
        }
        public static bool operator ==(Matrix4 left, Matrix4 right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Matrix4 left, Matrix4 right)
        {
            return !left.Equals(right);
        }
        public override string ToString()
        {
            return string.Format("{0}\n{1}\n{2}\n{3}", (object)this.Row0, (object)this.Row1, (object)this.Row2, (object)this.Row3);
        }
        public override int GetHashCode()
        {
            return ((this.Row0.GetHashCode() * 397 ^ this.Row1.GetHashCode()) * 397 ^ this.Row2.GetHashCode()) * 397 ^ this.Row3.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix4))
            {
                return false;
            }

            return this.Equals((Matrix4)obj);
        }
        public bool Equals(Matrix4 other)
        {
            if (this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2)
            {
                return this.Row3 == other.Row3;
            }

            return false;
        }
    }
}