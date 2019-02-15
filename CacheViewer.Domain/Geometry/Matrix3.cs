namespace CacheViewer.Domain.Geometry
{
    using System;

    /// <summary>
    /// Represents a 3x3 matrix containing 3D rotation and scale.
    /// </summary>
    [Serializable]
    public struct Matrix3 : IEquatable<Matrix3>
    {
        /// <summary>The identity matrix.</summary>
        public static readonly Matrix3 Identity = new Matrix3(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);
        /// <summary>The zero matrix.</summary>
        public static readonly Matrix3 Zero = new Matrix3(Vector3.Zero, Vector3.Zero, Vector3.Zero);
        /// <summary>First row of the matrix.</summary>
        public Vector3 Row0;
        /// <summary>Second row of the matrix.</summary>
        public Vector3 Row1;
        /// <summary>Third row of the matrix.</summary>
        public Vector3 Row2;

        /// <summary>Constructs a new instance.</summary>
        /// <param name="row0">Top row of the matrix</param>
        /// <param name="row1">Second row of the matrix</param>
        /// <param name="row2">Bottom row of the matrix</param>
        public Matrix3(Vector3 row0, Vector3 row1, Vector3 row2)
        {
            this.Row0 = row0;
            this.Row1 = row1;
            this.Row2 = row2;
        }

        /// <summary>Constructs a new instance.</summary>
        /// <param name="m00">First item of the first row of the matrix.</param>
        /// <param name="m01">Second item of the first row of the matrix.</param>
        /// <param name="m02">Third item of the first row of the matrix.</param>
        /// <param name="m10">First item of the second row of the matrix.</param>
        /// <param name="m11">Second item of the second row of the matrix.</param>
        /// <param name="m12">Third item of the second row of the matrix.</param>
        /// <param name="m20">First item of the third row of the matrix.</param>
        /// <param name="m21">Second item of the third row of the matrix.</param>
        /// <param name="m22">Third item of the third row of the matrix.</param>
        public Matrix3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
        {
            this.Row0 = new Vector3(m00, m01, m02);
            this.Row1 = new Vector3(m10, m11, m12);
            this.Row2 = new Vector3(m20, m21, m22);
        }

        /// <summary>Constructs a new instance.</summary>
        /// <param name="matrix">A Matrix4 to take the upper-left 3x3 from.</param>
        public Matrix3(Matrix4 matrix)
        {
            this.Row0 = matrix.Row0.Xyz;
            this.Row1 = matrix.Row1.Xyz;
            this.Row2 = matrix.Row2.Xyz;
        }

        /// <summary>Gets the determinant of this matrix.</summary>
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
                return (float)((double)x1 * (double)y2 * (double)z3 + (double)y1 * (double)z2 * (double)x3 + (double)z1 * (double)x2 * (double)y3 - (double)z1 * (double)y2 * (double)x3 - (double)x1 * (double)z2 * (double)y3 - (double)y1 * (double)x2 * (double)z3);
            }
        }

        /// <summary>Gets the first column of this matrix.</summary>
        public Vector3 Column0 => new Vector3(this.Row0.X, this.Row1.X, this.Row2.X);

        /// <summary>Gets the second column of this matrix.</summary>
        public Vector3 Column1 => new Vector3(this.Row0.Y, this.Row1.Y, this.Row2.Y);

        /// <summary>Gets the third column of this matrix.</summary>
        public Vector3 Column2 => new Vector3(this.Row0.Z, this.Row1.Z, this.Row2.Z);

        /// <summary>
        /// Gets or sets the value at row 1, column 1 of this instance.
        /// </summary>
        public float M11
        {
            get => this.Row0.X;
            set => this.Row0.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 1, column 2 of this instance.
        /// </summary>
        public float M12
        {
            get => this.Row0.Y;
            set => this.Row0.Y = value;
        }

        /// <summary>
        /// Gets or sets the value at row 1, column 3 of this instance.
        /// </summary>
        public float M13
        {
            get => this.Row0.Z;
            set => this.Row0.Z = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 1 of this instance.
        /// </summary>
        public float M21
        {
            get => this.Row1.X;
            set => this.Row1.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 2 of this instance.
        /// </summary>
        public float M22
        {
            get => this.Row1.Y;
            set => this.Row1.Y = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 3 of this instance.
        /// </summary>
        public float M23
        {
            get => this.Row1.Z;
            set => this.Row1.Z = value;
        }

        /// <summary>
        /// Gets or sets the value at row 3, column 1 of this instance.
        /// </summary>
        public float M31
        {
            get => this.Row2.X;
            set => this.Row2.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 3, column 2 of this instance.
        /// </summary>
        public float M32
        {
            get => this.Row2.Y;
            set => this.Row2.Y = value;
        }

        /// <summary>
        /// Gets or sets the value at row 3, column 3 of this instance.
        /// </summary>
        public float M33
        {
            get => this.Row2.Z;
            set => this.Row2.Z = value;
        }

        /// <summary>
        /// Gets or sets the values along the main diagonal of the matrix.
        /// </summary>
        public Vector3 Diagonal
        {
            get => new Vector3(this.Row0.X, this.Row1.Y, this.Row2.Z);
            set
            {
                this.Row0.X = value.X;
                this.Row1.Y = value.Y;
                this.Row2.Z = value.Z;
            }
        }

        /// <summary>
        /// Gets the trace of the matrix, the sum of the values along the diagonal.
        /// </summary>
        public float Trace => this.Row0.X + this.Row1.Y + this.Row2.Z;

        /// <summary>Gets or sets the value at a specified row and column.</summary>
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
                        throw new IndexOutOfRangeException("You tried to access this matrix at: (" + (object)rowIndex + ", " + (object)columnIndex + ")");
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
                        throw new IndexOutOfRangeException("You tried to set this matrix at: (" + (object)rowIndex + ", " + (object)columnIndex + ")");
                }
            }
        }

        /// <summary>Converts this instance into its inverse.</summary>
        public void Invert()
        {
            this = Matrix3.Invert(this);
        }

        /// <summary>Converts this instance into its transpose.</summary>
        public void Transpose()
        {
            this = Matrix3.Transpose(this);
        }

        /// <summary>Returns a normalised copy of this instance.</summary>
        public Matrix3 Normalized()
        {
            Matrix3 matrix3 = this;
            matrix3.Normalize();
            return matrix3;
        }

        /// <summary>
        /// Divides each element in the Matrix by the <see cref="P:OpenTK.Matrix3.Determinant" />.
        /// </summary>
        public void Normalize()
        {
            float determinant = this.Determinant;
            this.Row0 /= determinant;
            this.Row1 /= determinant;
            this.Row2 /= determinant;
        }

        /// <summary>Returns an inverted copy of this instance.</summary>
        public Matrix3 Inverted()
        {
            Matrix3 matrix3 = this;
            if ((double)matrix3.Determinant != 0.0)
            {
                matrix3.Invert();
            }

            return matrix3;
        }

        /// <summary>Returns a copy of this Matrix3 without scale.</summary>
        public Matrix3 ClearScale()
        {
            Matrix3 matrix3 = this;
            matrix3.Row0 = matrix3.Row0.Normalized();
            matrix3.Row1 = matrix3.Row1.Normalized();
            matrix3.Row2 = matrix3.Row2.Normalized();
            return matrix3;
        }

        /// <summary>Returns a copy of this Matrix3 without rotation.</summary>
        public Matrix3 ClearRotation()
        {
            Matrix3 matrix3 = this;
            matrix3.Row0 = new Vector3(matrix3.Row0.Length, 0.0f, 0.0f);
            matrix3.Row1 = new Vector3(0.0f, matrix3.Row1.Length, 0.0f);
            matrix3.Row2 = new Vector3(0.0f, 0.0f, matrix3.Row2.Length);
            return matrix3;
        }

        /// <summary>Returns the scale component of this instance.</summary>
        public Vector3 ExtractScale()
        {
            return new Vector3(this.Row0.Length, this.Row1.Length, this.Row2.Length);
        }

        /// <summary>
        /// Returns the rotation component of this instance. Quite slow.
        /// </summary>
        /// <param name="row_normalise">Whether the method should row-normalise (i.e. remove scale from) the Matrix. Pass false if you know it's already normalised.</param>
        public Quaternion ExtractRotation(bool row_normalise = true)
        {
            Vector3 vector3_1 = this.Row0;
            Vector3 vector3_2 = this.Row1;
            Vector3 vector3_3 = this.Row2;
            if (row_normalise)
            {
                vector3_1 = vector3_1.Normalized();
                vector3_2 = vector3_2.Normalized();
                vector3_3 = vector3_3.Normalized();
            }
            Quaternion quaternion = new Quaternion();
            double d = 0.25 * ((double)vector3_1[0] + (double)vector3_2[1] + (double)vector3_3[2] + 1.0);
            if (d > 0.0)
            {
                double num1 = Math.Sqrt(d);
                quaternion.W = (float)num1;
                double num2 = 1.0 / (4.0 * num1);
                quaternion.X = (float)(((double)vector3_2[2] - (double)vector3_3[1]) * num2);
                quaternion.Y = (float)(((double)vector3_3[0] - (double)vector3_1[2]) * num2);
                quaternion.Z = (float)(((double)vector3_1[1] - (double)vector3_2[0]) * num2);
            }
            else if ((double)vector3_1[0] > (double)vector3_2[1] && (double)vector3_1[0] > (double)vector3_3[2])
            {
                double num1 = 2.0 * Math.Sqrt(1.0 + (double)vector3_1[0] - (double)vector3_2[1] - (double)vector3_3[2]);
                quaternion.X = (float)(0.25 * num1);
                double num2 = 1.0 / num1;
                quaternion.W = (float)(((double)vector3_3[1] - (double)vector3_2[2]) * num2);
                quaternion.Y = (float)(((double)vector3_2[0] + (double)vector3_1[1]) * num2);
                quaternion.Z = (float)(((double)vector3_3[0] + (double)vector3_1[2]) * num2);
            }
            else if ((double)vector3_2[1] > (double)vector3_3[2])
            {
                double num1 = 2.0 * Math.Sqrt(1.0 + (double)vector3_2[1] - (double)vector3_1[0] - (double)vector3_3[2]);
                quaternion.Y = (float)(0.25 * num1);
                double num2 = 1.0 / num1;
                quaternion.W = (float)(((double)vector3_3[0] - (double)vector3_1[2]) * num2);
                quaternion.X = (float)(((double)vector3_2[0] + (double)vector3_1[1]) * num2);
                quaternion.Z = (float)(((double)vector3_3[1] + (double)vector3_2[2]) * num2);
            }
            else
            {
                double num1 = 2.0 * Math.Sqrt(1.0 + (double)vector3_3[2] - (double)vector3_1[0] - (double)vector3_2[1]);
                quaternion.Z = (float)(0.25 * num1);
                double num2 = 1.0 / num1;
                quaternion.W = (float)(((double)vector3_2[0] - (double)vector3_1[1]) * num2);
                quaternion.X = (float)(((double)vector3_3[0] + (double)vector3_1[2]) * num2);
                quaternion.Y = (float)(((double)vector3_3[1] + (double)vector3_2[2]) * num2);
            }
            quaternion.Normalize();
            return quaternion;
        }

        /// <summary>
        /// Build a rotation matrix from the specified axis/angle rotation.
        /// </summary>
        /// <param name="axis">The axis to rotate about.</param>
        /// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
        /// <param name="result">A matrix instance.</param>
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

        /// <summary>
        /// Build a rotation matrix from the specified axis/angle rotation.
        /// </summary>
        /// <param name="axis">The axis to rotate about.</param>
        /// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
        /// <returns>A matrix instance.</returns>
        public static Matrix3 CreateFromAxisAngle(Vector3 axis, float angle)
        {
            Matrix3 result;
            Matrix3.CreateFromAxisAngle(axis, angle, out result);
            return result;
        }

        /// <summary>Build a rotation matrix from the specified quaternion.</summary>
        /// <param name="q">Quaternion to translate.</param>
        /// <param name="result">Matrix result.</param>
        public static void CreateFromQuaternion(ref Quaternion q, out Matrix3 result)
        {
            Vector3 axis;
            float angle;
            q.ToAxisAngle(out axis, out angle);
            Matrix3.CreateFromAxisAngle(axis, angle, out result);
        }

        /// <summary>Build a rotation matrix from the specified quaternion.</summary>
        /// <param name="q">Quaternion to translate.</param>
        /// <returns>A matrix instance.</returns>
        public static Matrix3 CreateFromQuaternion(Quaternion q)
        {
            Matrix3 result;
            Matrix3.CreateFromQuaternion(ref q, out result);
            return result;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the x-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix3 instance.</param>
        public static void CreateRotationX(float angle, out Matrix3 result)
        {
            float num1 = (float)Math.Cos((double)angle);
            float num2 = (float)Math.Sin((double)angle);
            result = Matrix3.Identity;
            result.Row1.Y = num1;
            result.Row1.Z = num2;
            result.Row2.Y = -num2;
            result.Row2.Z = num1;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the x-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix3 instance.</returns>
        public static Matrix3 CreateRotationX(float angle)
        {
            Matrix3 result;
            Matrix3.CreateRotationX(angle, out result);
            return result;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the y-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix3 instance.</param>
        public static void CreateRotationY(float angle, out Matrix3 result)
        {
            float num1 = (float)Math.Cos((double)angle);
            float num2 = (float)Math.Sin((double)angle);
            result = Matrix3.Identity;
            result.Row0.X = num1;
            result.Row0.Z = -num2;
            result.Row2.X = num2;
            result.Row2.Z = num1;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the y-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix3 instance.</returns>
        public static Matrix3 CreateRotationY(float angle)
        {
            Matrix3 result;
            Matrix3.CreateRotationY(angle, out result);
            return result;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the z-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix3 instance.</param>
        public static void CreateRotationZ(float angle, out Matrix3 result)
        {
            float num1 = (float)Math.Cos((double)angle);
            float num2 = (float)Math.Sin((double)angle);
            result = Matrix3.Identity;
            result.Row0.X = num1;
            result.Row0.Y = num2;
            result.Row1.X = -num2;
            result.Row1.Y = num1;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the z-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix3 instance.</returns>
        public static Matrix3 CreateRotationZ(float angle)
        {
            Matrix3 result;
            Matrix3.CreateRotationZ(angle, out result);
            return result;
        }

        /// <summary>Creates a scale matrix.</summary>
        /// <param name="scale">Single scale factor for the x, y, and z axes.</param>
        /// <returns>A scale matrix.</returns>
        public static Matrix3 CreateScale(float scale)
        {
            Matrix3 result;
            Matrix3.CreateScale(scale, out result);
            return result;
        }

        /// <summary>Creates a scale matrix.</summary>
        /// <param name="scale">Scale factors for the x, y, and z axes.</param>
        /// <returns>A scale matrix.</returns>
        public static Matrix3 CreateScale(Vector3 scale)
        {
            Matrix3 result;
            Matrix3.CreateScale(ref scale, out result);
            return result;
        }

        /// <summary>Creates a scale matrix.</summary>
        /// <param name="x">Scale factor for the x axis.</param>
        /// <param name="y">Scale factor for the y axis.</param>
        /// <param name="z">Scale factor for the z axis.</param>
        /// <returns>A scale matrix.</returns>
        public static Matrix3 CreateScale(float x, float y, float z)
        {
            Matrix3 result;
            Matrix3.CreateScale(x, y, z, out result);
            return result;
        }

        /// <summary>Creates a scale matrix.</summary>
        /// <param name="scale">Single scale factor for the x, y, and z axes.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(float scale, out Matrix3 result)
        {
            result = Matrix3.Identity;
            result.Row0.X = scale;
            result.Row1.Y = scale;
            result.Row2.Z = scale;
        }

        /// <summary>Creates a scale matrix.</summary>
        /// <param name="scale">Scale factors for the x, y, and z axes.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(ref Vector3 scale, out Matrix3 result)
        {
            result = Matrix3.Identity;
            result.Row0.X = scale.X;
            result.Row1.Y = scale.Y;
            result.Row2.Z = scale.Z;
        }

        /// <summary>Creates a scale matrix.</summary>
        /// <param name="x">Scale factor for the x axis.</param>
        /// <param name="y">Scale factor for the y axis.</param>
        /// <param name="z">Scale factor for the z axis.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(float x, float y, float z, out Matrix3 result)
        {
            result = Matrix3.Identity;
            result.Row0.X = x;
            result.Row1.Y = y;
            result.Row2.Z = z;
        }

        /// <summary>Adds two instances.</summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <returns>A new instance that is the result of the addition.</returns>
        public static Matrix3 Add(Matrix3 left, Matrix3 right)
        {
            Matrix3 result;
            Matrix3.Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>Adds two instances.</summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <param name="result">A new instance that is the result of the addition.</param>
        public static void Add(ref Matrix3 left, ref Matrix3 right, out Matrix3 result)
        {
            Vector3.Add(ref left.Row0, ref right.Row0, out result.Row0);
            Vector3.Add(ref left.Row1, ref right.Row1, out result.Row1);
            Vector3.Add(ref left.Row2, ref right.Row2, out result.Row2);
        }

        /// <summary>Multiplies two instances.</summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication</returns>
        public static Matrix3 Mult(Matrix3 left, Matrix3 right)
        {
            Matrix3 result;
            Matrix3.Mult(ref left, ref right, out result);
            return result;
        }

        /// <summary>Multiplies two instances.</summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication</param>
        public static void Mult(ref Matrix3 left, ref Matrix3 right, out Matrix3 result)
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
            result.Row0.X = (float)((double)x1 * (double)x4 + (double)y1 * (double)x5 + (double)z1 * (double)x6);
            result.Row0.Y = (float)((double)x1 * (double)y4 + (double)y1 * (double)y5 + (double)z1 * (double)y6);
            result.Row0.Z = (float)((double)x1 * (double)z4 + (double)y1 * (double)z5 + (double)z1 * (double)z6);
            result.Row1.X = (float)((double)x2 * (double)x4 + (double)y2 * (double)x5 + (double)z2 * (double)x6);
            result.Row1.Y = (float)((double)x2 * (double)y4 + (double)y2 * (double)y5 + (double)z2 * (double)y6);
            result.Row1.Z = (float)((double)x2 * (double)z4 + (double)y2 * (double)z5 + (double)z2 * (double)z6);
            result.Row2.X = (float)((double)x3 * (double)x4 + (double)y3 * (double)x5 + (double)z3 * (double)x6);
            result.Row2.Y = (float)((double)x3 * (double)y4 + (double)y3 * (double)y5 + (double)z3 * (double)y6);
            result.Row2.Z = (float)((double)x3 * (double)z4 + (double)y3 * (double)z5 + (double)z3 * (double)z6);
        }

        /// <summary>Calculate the inverse of the given matrix</summary>
        /// <param name="mat">The matrix to invert</param>
        /// <param name="result">The inverse of the given matrix if it has one, or the input if it is singular</param>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the Matrix3 is singular.</exception>
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
                                if ((double)num2 > (double)num1)
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
                if ((double)num3 == 0.0)
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

        /// <summary>Calculate the inverse of the given matrix</summary>
        /// <param name="mat">The matrix to invert</param>
        /// <returns>The inverse of the given matrix if it has one, or the input if it is singular</returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the Matrix4 is singular.</exception>
        public static Matrix3 Invert(Matrix3 mat)
        {
            Matrix3 result;
            Matrix3.Invert(ref mat, out result);
            return result;
        }

        /// <summary>Calculate the transpose of the given matrix</summary>
        /// <param name="mat">The matrix to transpose</param>
        /// <returns>The transpose of the given matrix</returns>
        public static Matrix3 Transpose(Matrix3 mat)
        {
            return new Matrix3(mat.Column0, mat.Column1, mat.Column2);
        }

        /// <summary>Calculate the transpose of the given matrix</summary>
        /// <param name="mat">The matrix to transpose</param>
        /// <param name="result">The result of the calculation</param>
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

        /// <summary>Matrix multiplication</summary>
        /// <param name="left">left-hand operand</param>
        /// <param name="right">right-hand operand</param>
        /// <returns>A new Matrix3d which holds the result of the multiplication</returns>
        public static Matrix3 operator *(Matrix3 left, Matrix3 right)
        {
            return Matrix3.Mult(left, right);
        }

        /// <summary>Compares two instances for equality.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(Matrix3 left, Matrix3 right)
        {
            return left.Equals(right);
        }

        /// <summary>Compares two instances for inequality.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equal right; false otherwise.</returns>
        public static bool operator !=(Matrix3 left, Matrix3 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a System.String that represents the current Matrix3d.
        /// </summary>
        /// <returns>The string representation of the matrix.</returns>
        public override string ToString()
        {
            return string.Format("{0}\n{1}\n{2}", (object)this.Row0, (object)this.Row1, (object)this.Row2);
        }

        /// <summary>Returns the hashcode for this instance.</summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            return (this.Row0.GetHashCode() * 397 ^ this.Row1.GetHashCode()) * 397 ^ this.Row2.GetHashCode();
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix3))
            {
                return false;
            }

            return this.Equals((Matrix3)obj);
        }

        /// <summary>Indicates whether the current matrix is equal to another matrix.</summary>
        /// <param name="other">A matrix to compare with this matrix.</param>
        /// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
        public bool Equals(Matrix3 other)
        {
            if (this.Row0 == other.Row0 && this.Row1 == other.Row1)
            {
                return this.Row2 == other.Row2;
            }

            return false;
        }
    }

    /// <summary>
    /// Represents a 4x4 matrix containing 3D rotation, scale, transform, and projection.
    /// </summary>
    /// <seealso cref="T:OpenTK.Matrix4d" />
    [Serializable]
    public struct Matrix4 : IEquatable<Matrix4>
    {
        /// <summary>The identity matrix.</summary>
        public static readonly Matrix4 Identity = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);
        /// <summary>The zero matrix.</summary>
        public static readonly Matrix4 Zero = new Matrix4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero);
        /// <summary>Top row of the matrix.</summary>
        public Vector4 Row0;
        /// <summary>2nd row of the matrix.</summary>
        public Vector4 Row1;
        /// <summary>3rd row of the matrix.</summary>
        public Vector4 Row2;
        /// <summary>Bottom row of the matrix.</summary>
        public Vector4 Row3;

        /// <summary>Constructs a new instance.</summary>
        /// <param name="row0">Top row of the matrix.</param>
        /// <param name="row1">Second row of the matrix.</param>
        /// <param name="row2">Third row of the matrix.</param>
        /// <param name="row3">Bottom row of the matrix.</param>
        public Matrix4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
        {
            this.Row0 = row0;
            this.Row1 = row1;
            this.Row2 = row2;
            this.Row3 = row3;
        }

        /// <summary>Constructs a new instance.</summary>
        /// <param name="m00">First item of the first row of the matrix.</param>
        /// <param name="m01">Second item of the first row of the matrix.</param>
        /// <param name="m02">Third item of the first row of the matrix.</param>
        /// <param name="m03">Fourth item of the first row of the matrix.</param>
        /// <param name="m10">First item of the second row of the matrix.</param>
        /// <param name="m11">Second item of the second row of the matrix.</param>
        /// <param name="m12">Third item of the second row of the matrix.</param>
        /// <param name="m13">Fourth item of the second row of the matrix.</param>
        /// <param name="m20">First item of the third row of the matrix.</param>
        /// <param name="m21">Second item of the third row of the matrix.</param>
        /// <param name="m22">Third item of the third row of the matrix.</param>
        /// <param name="m23">First item of the third row of the matrix.</param>
        /// <param name="m30">Fourth item of the fourth row of the matrix.</param>
        /// <param name="m31">Second item of the fourth row of the matrix.</param>
        /// <param name="m32">Third item of the fourth row of the matrix.</param>
        /// <param name="m33">Fourth item of the fourth row of the matrix.</param>
        public Matrix4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
        {
            this.Row0 = new Vector4(m00, m01, m02, m03);
            this.Row1 = new Vector4(m10, m11, m12, m13);
            this.Row2 = new Vector4(m20, m21, m22, m23);
            this.Row3 = new Vector4(m30, m31, m32, m33);
        }

        /// <summary>Constructs a new instance.</summary>
        /// <param name="topLeft">The top left 3x3 of the matrix.</param>
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

        /// <summary>Gets the determinant of this matrix.</summary>
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
                return (float)((double)x1 * (double)y2 * (double)z3 * (double)w4 - (double)x1 * (double)y2 * (double)w3 * (double)z4 + (double)x1 * (double)z2 * (double)w3 * (double)y4 - (double)x1 * (double)z2 * (double)y3 * (double)w4 + (double)x1 * (double)w2 * (double)y3 * (double)z4 - (double)x1 * (double)w2 * (double)z3 * (double)y4 - (double)y1 * (double)z2 * (double)w3 * (double)x4 + (double)y1 * (double)z2 * (double)x3 * (double)w4 - (double)y1 * (double)w2 * (double)x3 * (double)z4 + (double)y1 * (double)w2 * (double)z3 * (double)x4 - (double)y1 * (double)x2 * (double)z3 * (double)w4 + (double)y1 * (double)x2 * (double)w3 * (double)z4 + (double)z1 * (double)w2 * (double)x3 * (double)y4 - (double)z1 * (double)w2 * (double)y3 * (double)x4 + (double)z1 * (double)x2 * (double)y3 * (double)w4 - (double)z1 * (double)x2 * (double)w3 * (double)y4 + (double)z1 * (double)y2 * (double)w3 * (double)x4 - (double)z1 * (double)y2 * (double)x3 * (double)w4 - (double)w1 * (double)x2 * (double)y3 * (double)z4 + (double)w1 * (double)x2 * (double)z3 * (double)y4 - (double)w1 * (double)y2 * (double)z3 * (double)x4 + (double)w1 * (double)y2 * (double)x3 * (double)z4 - (double)w1 * (double)z2 * (double)x3 * (double)y4 + (double)w1 * (double)z2 * (double)y3 * (double)x4);
            }
        }

        /// <summary>Gets the first column of this matrix.</summary>
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

        /// <summary>Gets the second column of this matrix.</summary>
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

        /// <summary>Gets the third column of this matrix.</summary>
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

        /// <summary>Gets the fourth column of this matrix.</summary>
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

        /// <summary>
        /// Gets or sets the value at row 1, column 1 of this instance.
        /// </summary>
        public float M11
        {
            get => this.Row0.X;
            set => this.Row0.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 1, column 2 of this instance.
        /// </summary>
        public float M12
        {
            get => this.Row0.Y;
            set => this.Row0.Y = value;
        }

        /// <summary>
        /// Gets or sets the value at row 1, column 3 of this instance.
        /// </summary>
        public float M13
        {
            get => this.Row0.Z;
            set => this.Row0.Z = value;
        }

        /// <summary>
        /// Gets or sets the value at row 1, column 4 of this instance.
        /// </summary>
        public float M14
        {
            get => this.Row0.W;
            set => this.Row0.W = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 1 of this instance.
        /// </summary>
        public float M21
        {
            get => this.Row1.X;
            set => this.Row1.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 2 of this instance.
        /// </summary>
        public float M22
        {
            get => this.Row1.Y;
            set => this.Row1.Y = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 3 of this instance.
        /// </summary>
        public float M23
        {
            get => this.Row1.Z;
            set => this.Row1.Z = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 4 of this instance.
        /// </summary>
        public float M24
        {
            get => this.Row1.W;
            set => this.Row1.W = value;
        }

        /// <summary>
        /// Gets or sets the value at row 3, column 1 of this instance.
        /// </summary>
        public float M31
        {
            get => this.Row2.X;
            set => this.Row2.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 3, column 2 of this instance.
        /// </summary>
        public float M32
        {
            get => this.Row2.Y;
            set => this.Row2.Y = value;
        }

        /// <summary>
        /// Gets or sets the value at row 3, column 3 of this instance.
        /// </summary>
        public float M33
        {
            get => this.Row2.Z;
            set => this.Row2.Z = value;
        }

        /// <summary>
        /// Gets or sets the value at row 3, column 4 of this instance.
        /// </summary>
        public float M34
        {
            get => this.Row2.W;
            set => this.Row2.W = value;
        }

        /// <summary>
        /// Gets or sets the value at row 4, column 1 of this instance.
        /// </summary>
        public float M41
        {
            get => this.Row3.X;
            set => this.Row3.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 4, column 2 of this instance.
        /// </summary>
        public float M42
        {
            get => this.Row3.Y;
            set => this.Row3.Y = value;
        }

        /// <summary>
        /// Gets or sets the value at row 4, column 3 of this instance.
        /// </summary>
        public float M43
        {
            get => this.Row3.Z;
            set => this.Row3.Z = value;
        }

        /// <summary>
        /// Gets or sets the value at row 4, column 4 of this instance.
        /// </summary>
        public float M44
        {
            get => this.Row3.W;
            set => this.Row3.W = value;
        }

        /// <summary>
        /// Gets or sets the values along the main diagonal of the matrix.
        /// </summary>
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

        /// <summary>
        /// Gets the trace of the matrix, the sum of the values along the diagonal.
        /// </summary>
        public float Trace => this.Row0.X + this.Row1.Y + this.Row2.Z + this.Row3.W;

        /// <summary>Gets or sets the value at a specified row and column.</summary>
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
                        throw new IndexOutOfRangeException("You tried to access this matrix at: (" + (object)rowIndex + ", " + (object)columnIndex + ")");
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
                        throw new IndexOutOfRangeException("You tried to set this matrix at: (" + (object)rowIndex + ", " + (object)columnIndex + ")");
                }
            }
        }

        /// <summary>Converts this instance into its inverse.</summary>
        public void Invert()
        {
            this = Matrix4.Invert(this);
        }

        /// <summary>Converts this instance into its transpose.</summary>
        public void Transpose()
        {
            this = Matrix4.Transpose(this);
        }

        /// <summary>Returns a normalised copy of this instance.</summary>
        public Matrix4 Normalized()
        {
            Matrix4 matrix4 = this;
            matrix4.Normalize();
            return matrix4;
        }

        /// <summary>
        /// Divides each element in the Matrix by the <see cref="P:OpenTK.Matrix4.Determinant" />.
        /// </summary>
        public void Normalize()
        {
            float determinant = this.Determinant;
            this.Row0 /= determinant;
            this.Row1 /= determinant;
            this.Row2 /= determinant;
            this.Row3 /= determinant;
        }

        /// <summary>Returns an inverted copy of this instance.</summary>
        public Matrix4 Inverted()
        {
            Matrix4 matrix4 = this;
            if ((double)matrix4.Determinant != 0.0)
            {
                matrix4.Invert();
            }

            return matrix4;
        }

        /// <summary>Returns a copy of this Matrix4 without translation.</summary>
        public Matrix4 ClearTranslation()
        {
            Matrix4 matrix4 = this;
            matrix4.Row3.Xyz = Vector3.Zero;
            return matrix4;
        }

        /// <summary>Returns a copy of this Matrix4 without scale.</summary>
        public Matrix4 ClearScale()
        {
            Matrix4 matrix4 = this;
            matrix4.Row0.Xyz = matrix4.Row0.Xyz.Normalized();
            matrix4.Row1.Xyz = matrix4.Row1.Xyz.Normalized();
            matrix4.Row2.Xyz = matrix4.Row2.Xyz.Normalized();
            return matrix4;
        }

        /// <summary>Returns a copy of this Matrix4 without rotation.</summary>
        public Matrix4 ClearRotation()
        {
            Matrix4 matrix4 = this;
            matrix4.Row0.Xyz = new Vector3(matrix4.Row0.Xyz.Length, 0.0f, 0.0f);
            matrix4.Row1.Xyz = new Vector3(0.0f, matrix4.Row1.Xyz.Length, 0.0f);
            matrix4.Row2.Xyz = new Vector3(0.0f, 0.0f, matrix4.Row2.Xyz.Length);
            return matrix4;
        }

        /// <summary>Returns a copy of this Matrix4 without projection.</summary>
        public Matrix4 ClearProjection()
        {
            Matrix4 matrix4 = this;
            matrix4.Column3 = Vector4.Zero;
            return matrix4;
        }

        /// <summary>Returns the translation component of this instance.</summary>
        public Vector3 ExtractTranslation()
        {
            return this.Row3.Xyz;
        }

        /// <summary>Returns the scale component of this instance.</summary>
        public Vector3 ExtractScale()
        {
            Vector3 xyz = this.Row0.Xyz;
            double length1 = (double)xyz.Length;
            xyz = this.Row1.Xyz;
            double length2 = (double)xyz.Length;
            xyz = this.Row2.Xyz;
            double length3 = (double)xyz.Length;
            return new Vector3((float)length1, (float)length2, (float)length3);
        }

        /// <summary>
        /// Returns the rotation component of this instance. Quite slow.
        /// </summary>
        /// <param name="rowNormalise">Whether the method should row-normalise (i.e. remove scale from) the Matrix. Pass false if you know it's already normalised.</param>
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
            double d = 0.25 * ((double)vector3_1[0] + (double)vector3_2[1] + (double)vector3_3[2] + 1.0);
            if (d > 0.0)
            {
                double num1 = Math.Sqrt(d);
                quaternion.W = (float)num1;
                double num2 = 1.0 / (4.0 * num1);
                quaternion.X = (float)(((double)vector3_2[2] - (double)vector3_3[1]) * num2);
                quaternion.Y = (float)(((double)vector3_3[0] - (double)vector3_1[2]) * num2);
                quaternion.Z = (float)(((double)vector3_1[1] - (double)vector3_2[0]) * num2);
            }
            else if ((double)vector3_1[0] > (double)vector3_2[1] && (double)vector3_1[0] > (double)vector3_3[2])
            {
                double num1 = 2.0 * Math.Sqrt(1.0 + (double)vector3_1[0] - (double)vector3_2[1] - (double)vector3_3[2]);
                quaternion.X = (float)(0.25 * num1);
                double num2 = 1.0 / num1;
                quaternion.W = (float)(((double)vector3_3[1] - (double)vector3_2[2]) * num2);
                quaternion.Y = (float)(((double)vector3_2[0] + (double)vector3_1[1]) * num2);
                quaternion.Z = (float)(((double)vector3_3[0] + (double)vector3_1[2]) * num2);
            }
            else if ((double)vector3_2[1] > (double)vector3_3[2])
            {
                double num1 = 2.0 * Math.Sqrt(1.0 + (double)vector3_2[1] - (double)vector3_1[0] - (double)vector3_3[2]);
                quaternion.Y = (float)(0.25 * num1);
                double num2 = 1.0 / num1;
                quaternion.W = (float)(((double)vector3_3[0] - (double)vector3_1[2]) * num2);
                quaternion.X = (float)(((double)vector3_2[0] + (double)vector3_1[1]) * num2);
                quaternion.Z = (float)(((double)vector3_3[1] + (double)vector3_2[2]) * num2);
            }
            else
            {
                double num1 = 2.0 * Math.Sqrt(1.0 + (double)vector3_3[2] - (double)vector3_1[0] - (double)vector3_2[1]);
                quaternion.Z = (float)(0.25 * num1);
                double num2 = 1.0 / num1;
                quaternion.W = (float)(((double)vector3_2[0] - (double)vector3_1[1]) * num2);
                quaternion.X = (float)(((double)vector3_3[0] + (double)vector3_1[2]) * num2);
                quaternion.Y = (float)(((double)vector3_3[1] + (double)vector3_2[2]) * num2);
            }
            quaternion.Normalize();
            return quaternion;
        }

        /// <summary>Returns the projection component of this instance.</summary>
        public Vector4 ExtractProjection()
        {
            return this.Column3;
        }

        /// <summary>
        /// Build a rotation matrix from the specified axis/angle rotation.
        /// </summary>
        /// <param name="axis">The axis to rotate about.</param>
        /// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
        /// <param name="result">A matrix instance.</param>
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

        /// <summary>
        /// Build a rotation matrix from the specified axis/angle rotation.
        /// </summary>
        /// <param name="axis">The axis to rotate about.</param>
        /// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
        /// <returns>A matrix instance.</returns>
        public static Matrix4 CreateFromAxisAngle(Vector3 axis, float angle)
        {
            Matrix4 result;
            Matrix4.CreateFromAxisAngle(axis, angle, out result);
            return result;
        }

        /// <summary>Builds a rotation matrix from a quaternion.</summary>
        /// <param name="q">The quaternion to rotate by.</param>
        /// <param name="result">A matrix instance.</param>
        public static void CreateFromQuaternion(ref Quaternion q, out Matrix4 result)
        {
            Vector3 axis;
            float angle;
            q.ToAxisAngle(out axis, out angle);
            Matrix4.CreateFromAxisAngle(axis, angle, out result);
        }

        /// <summary>Builds a rotation matrix from a quaternion.</summary>
        /// <param name="q">The quaternion to rotate by.</param>
        /// <returns>A matrix instance.</returns>
        public static Matrix4 CreateFromQuaternion(Quaternion q)
        {
            Matrix4 result;
            Matrix4.CreateFromQuaternion(ref q, out result);
            return result;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the x-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateRotationX(float angle, out Matrix4 result)
        {
            float num1 = (float)Math.Cos((double)angle);
            float num2 = (float)Math.Sin((double)angle);
            result = Matrix4.Identity;
            result.Row1.Y = num1;
            result.Row1.Z = num2;
            result.Row2.Y = -num2;
            result.Row2.Z = num1;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the x-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix4 instance.</returns>
        public static Matrix4 CreateRotationX(float angle)
        {
            Matrix4 result;
            Matrix4.CreateRotationX(angle, out result);
            return result;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the y-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateRotationY(float angle, out Matrix4 result)
        {
            float num1 = (float)Math.Cos((double)angle);
            float num2 = (float)Math.Sin((double)angle);
            result = Matrix4.Identity;
            result.Row0.X = num1;
            result.Row0.Z = -num2;
            result.Row2.X = num2;
            result.Row2.Z = num1;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the y-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix4 instance.</returns>
        public static Matrix4 CreateRotationY(float angle)
        {
            Matrix4 result;
            Matrix4.CreateRotationY(angle, out result);
            return result;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the z-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateRotationZ(float angle, out Matrix4 result)
        {
            float num1 = (float)Math.Cos((double)angle);
            float num2 = (float)Math.Sin((double)angle);
            result = Matrix4.Identity;
            result.Row0.X = num1;
            result.Row0.Y = num2;
            result.Row1.X = -num2;
            result.Row1.Y = num1;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the z-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix4 instance.</returns>
        public static Matrix4 CreateRotationZ(float angle)
        {
            Matrix4 result;
            Matrix4.CreateRotationZ(angle, out result);
            return result;
        }

        /// <summary>Creates a translation matrix.</summary>
        /// <param name="x">X translation.</param>
        /// <param name="y">Y translation.</param>
        /// <param name="z">Z translation.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateTranslation(float x, float y, float z, out Matrix4 result)
        {
            result = Matrix4.Identity;
            result.Row3.X = x;
            result.Row3.Y = y;
            result.Row3.Z = z;
        }

        /// <summary>Creates a translation matrix.</summary>
        /// <param name="vector">The translation vector.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateTranslation(ref Vector3 vector, out Matrix4 result)
        {
            result = Matrix4.Identity;
            result.Row3.X = vector.X;
            result.Row3.Y = vector.Y;
            result.Row3.Z = vector.Z;
        }

        /// <summary>Creates a translation matrix.</summary>
        /// <param name="x">X translation.</param>
        /// <param name="y">Y translation.</param>
        /// <param name="z">Z translation.</param>
        /// <returns>The resulting Matrix4 instance.</returns>
        public static Matrix4 CreateTranslation(float x, float y, float z)
        {
            Matrix4 result;
            Matrix4.CreateTranslation(x, y, z, out result);
            return result;
        }

        /// <summary>Creates a translation matrix.</summary>
        /// <param name="vector">The translation vector.</param>
        /// <returns>The resulting Matrix4 instance.</returns>
        public static Matrix4 CreateTranslation(Vector3 vector)
        {
            Matrix4 result;
            Matrix4.CreateTranslation(vector.X, vector.Y, vector.Z, out result);
            return result;
        }

        /// <summary>Creates a scale matrix.</summary>
        /// <param name="scale">Single scale factor for the x, y, and z axes.</param>
        /// <returns>A scale matrix.</returns>
        public static Matrix4 CreateScale(float scale)
        {
            Matrix4 result;
            Matrix4.CreateScale(scale, out result);
            return result;
        }

        /// <summary>Creates a scale matrix.</summary>
        /// <param name="scale">Scale factors for the x, y, and z axes.</param>
        /// <returns>A scale matrix.</returns>
        public static Matrix4 CreateScale(Vector3 scale)
        {
            Matrix4 result;
            Matrix4.CreateScale(ref scale, out result);
            return result;
        }

        /// <summary>Creates a scale matrix.</summary>
        /// <param name="x">Scale factor for the x axis.</param>
        /// <param name="y">Scale factor for the y axis.</param>
        /// <param name="z">Scale factor for the z axis.</param>
        /// <returns>A scale matrix.</returns>
        public static Matrix4 CreateScale(float x, float y, float z)
        {
            Matrix4 result;
            Matrix4.CreateScale(x, y, z, out result);
            return result;
        }

        /// <summary>Creates a scale matrix.</summary>
        /// <param name="scale">Single scale factor for the x, y, and z axes.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(float scale, out Matrix4 result)
        {
            result = Matrix4.Identity;
            result.Row0.X = scale;
            result.Row1.Y = scale;
            result.Row2.Z = scale;
        }

        /// <summary>Creates a scale matrix.</summary>
        /// <param name="scale">Scale factors for the x, y, and z axes.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(ref Vector3 scale, out Matrix4 result)
        {
            result = Matrix4.Identity;
            result.Row0.X = scale.X;
            result.Row1.Y = scale.Y;
            result.Row2.Z = scale.Z;
        }

        /// <summary>Creates a scale matrix.</summary>
        /// <param name="x">Scale factor for the x axis.</param>
        /// <param name="y">Scale factor for the y axis.</param>
        /// <param name="z">Scale factor for the z axis.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(float x, float y, float z, out Matrix4 result)
        {
            result = Matrix4.Identity;
            result.Row0.X = x;
            result.Row1.Y = y;
            result.Row2.Z = z;
        }

        /// <summary>Creates an orthographic projection matrix.</summary>
        /// <param name="width">The width of the projection volume.</param>
        /// <param name="height">The height of the projection volume.</param>
        /// <param name="zNear">The near edge of the projection volume.</param>
        /// <param name="zFar">The far edge of the projection volume.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateOrthographic(float width, float height, float zNear, float zFar, out Matrix4 result)
        {
            Matrix4.CreateOrthographicOffCenter((float)(-(double)width / 2.0), width / 2f, (float)(-(double)height / 2.0), height / 2f, zNear, zFar, out result);
        }

        /// <summary>Creates an orthographic projection matrix.</summary>
        /// <param name="width">The width of the projection volume.</param>
        /// <param name="height">The height of the projection volume.</param>
        /// <param name="zNear">The near edge of the projection volume.</param>
        /// <param name="zFar">The far edge of the projection volume.</param>
        /// <rereturns>The resulting Matrix4 instance.</rereturns>
        public static Matrix4 CreateOrthographic(float width, float height, float zNear, float zFar)
        {
            Matrix4 result;
            Matrix4.CreateOrthographicOffCenter((float)(-(double)width / 2.0), width / 2f, (float)(-(double)height / 2.0), height / 2f, zNear, zFar, out result);
            return result;
        }

        /// <summary>Creates an orthographic projection matrix.</summary>
        /// <param name="left">The left edge of the projection volume.</param>
        /// <param name="right">The right edge of the projection volume.</param>
        /// <param name="bottom">The bottom edge of the projection volume.</param>
        /// <param name="top">The top edge of the projection volume.</param>
        /// <param name="zNear">The near edge of the projection volume.</param>
        /// <param name="zFar">The far edge of the projection volume.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear, float zFar, out Matrix4 result)
        {
            result = Matrix4.Identity;
            float num1 = (float)(1.0 / ((double)right - (double)left));
            float num2 = (float)(1.0 / ((double)top - (double)bottom));
            float num3 = (float)(1.0 / ((double)zFar - (double)zNear));
            result.Row0.X = 2f * num1;
            result.Row1.Y = 2f * num2;
            result.Row2.Z = -2f * num3;
            result.Row3.X = (float)-((double)right + (double)left) * num1;
            result.Row3.Y = (float)-((double)top + (double)bottom) * num2;
            result.Row3.Z = (float)-((double)zFar + (double)zNear) * num3;
        }

        /// <summary>Creates an orthographic projection matrix.</summary>
        /// <param name="left">The left edge of the projection volume.</param>
        /// <param name="right">The right edge of the projection volume.</param>
        /// <param name="bottom">The bottom edge of the projection volume.</param>
        /// <param name="top">The top edge of the projection volume.</param>
        /// <param name="zNear">The near edge of the projection volume.</param>
        /// <param name="zFar">The far edge of the projection volume.</param>
        /// <returns>The resulting Matrix4 instance.</returns>
        public static Matrix4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            Matrix4 result;
            Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, zNear, zFar, out result);
            return result;
        }

        /// <summary>Creates a perspective projection matrix.</summary>
        /// <param name="fovy">Angle of the field of view in the y direction (in radians)</param>
        /// <param name="aspect">Aspect ratio of the view (width / height)</param>
        /// <param name="zNear">Distance to the near clip plane</param>
        /// <param name="zFar">Distance to the far clip plane</param>
        /// <param name="result">A projection matrix that transforms camera space to raster space</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown under the following conditions:
        /// <list type="bullet">
        /// <item>fovy is zero, less than zero or larger than Math.PI</item>
        /// <item>aspect is negative or zero</item>
        /// <item>zNear is negative or zero</item>
        /// <item>zFar is negative or zero</item>
        /// <item>zNear is larger than zFar</item>
        /// </list>
        /// </exception>
        public static void CreatePerspectiveFieldOfView(float fovy, float aspect, float zNear, float zFar, out Matrix4 result)
        {
            if ((double)fovy <= 0.0 || (double)fovy > Math.PI)
            {
                throw new ArgumentOutOfRangeException(nameof(fovy));
            }

            if ((double)aspect <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(aspect));
            }

            if ((double)zNear <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(zNear));
            }

            if ((double)zFar <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(zFar));
            }

            float top = zNear * (float)Math.Tan(0.5 * (double)fovy);
            float bottom = -top;
            Matrix4.CreatePerspectiveOffCenter(bottom * aspect, top * aspect, bottom, top, zNear, zFar, out result);
        }

        /// <summary>Creates a perspective projection matrix.</summary>
        /// <param name="fovy">Angle of the field of view in the y direction (in radians)</param>
        /// <param name="aspect">Aspect ratio of the view (width / height)</param>
        /// <param name="zNear">Distance to the near clip plane</param>
        /// <param name="zFar">Distance to the far clip plane</param>
        /// <returns>A projection matrix that transforms camera space to raster space</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown under the following conditions:
        /// <list type="bullet">
        /// <item>fovy is zero, less than zero or larger than Math.PI</item>
        /// <item>aspect is negative or zero</item>
        /// <item>zNear is negative or zero</item>
        /// <item>zFar is negative or zero</item>
        /// <item>zNear is larger than zFar</item>
        /// </list>
        /// </exception>
        public static Matrix4 CreatePerspectiveFieldOfView(float fovy, float aspect, float zNear, float zFar)
        {
            Matrix4 result;
            Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, zNear, zFar, out result);
            return result;
        }

        /// <summary>Creates an perspective projection matrix.</summary>
        /// <param name="left">Left edge of the view frustum</param>
        /// <param name="right">Right edge of the view frustum</param>
        /// <param name="bottom">Bottom edge of the view frustum</param>
        /// <param name="top">Top edge of the view frustum</param>
        /// <param name="zNear">Distance to the near clip plane</param>
        /// <param name="zFar">Distance to the far clip plane</param>
        /// <param name="result">A projection matrix that transforms camera space to raster space</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown under the following conditions:
        /// <list type="bullet">
        /// <item>zNear is negative or zero</item>
        /// <item>zFar is negative or zero</item>
        /// <item>zNear is larger than zFar</item>
        /// </list>
        /// </exception>
        public static void CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float zNear, float zFar, out Matrix4 result)
        {
            if ((double)zNear <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(zNear));
            }

            if ((double)zFar <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(zFar));
            }

            if ((double)zNear >= (double)zFar)
            {
                throw new ArgumentOutOfRangeException(nameof(zNear));
            }

            float num1 = (float)(2.0 * (double)zNear / ((double)right - (double)left));
            float num2 = (float)(2.0 * (double)zNear / ((double)top - (double)bottom));
            float num3 = (float)(((double)right + (double)left) / ((double)right - (double)left));
            float num4 = (float)(((double)top + (double)bottom) / ((double)top - (double)bottom));
            float num5 = (float)(-((double)zFar + (double)zNear) / ((double)zFar - (double)zNear));
            float num6 = (float)(-(2.0 * (double)zFar * (double)zNear) / ((double)zFar - (double)zNear));
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

        /// <summary>Creates an perspective projection matrix.</summary>
        /// <param name="left">Left edge of the view frustum</param>
        /// <param name="right">Right edge of the view frustum</param>
        /// <param name="bottom">Bottom edge of the view frustum</param>
        /// <param name="top">Top edge of the view frustum</param>
        /// <param name="zNear">Distance to the near clip plane</param>
        /// <param name="zFar">Distance to the far clip plane</param>
        /// <returns>A projection matrix that transforms camera space to raster space</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown under the following conditions:
        /// <list type="bullet">
        /// <item>zNear is negative or zero</item>
        /// <item>zFar is negative or zero</item>
        /// <item>zNear is larger than zFar</item>
        /// </list>
        /// </exception>
        public static Matrix4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            Matrix4 result;
            Matrix4.CreatePerspectiveOffCenter(left, right, bottom, top, zNear, zFar, out result);
            return result;
        }

        /// <summary>Build a world space to camera space matrix</summary>
        /// <param name="eye">Eye (camera) position in world space</param>
        /// <param name="target">Target position in world space</param>
        /// <param name="up">Up vector in world space (should not be parallel to the camera direction, that is target - eye)</param>
        /// <returns>A Matrix4 that transforms world space to camera space</returns>
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
            matrix4.Row3.X = (float)-((double)right.X * (double)eye.X + (double)right.Y * (double)eye.Y + (double)right.Z * (double)eye.Z);
            matrix4.Row3.Y = (float)-((double)vector3_2.X * (double)eye.X + (double)vector3_2.Y * (double)eye.Y + (double)vector3_2.Z * (double)eye.Z);
            matrix4.Row3.Z = (float)-((double)vector3_1.X * (double)eye.X + (double)vector3_1.Y * (double)eye.Y + (double)vector3_1.Z * (double)eye.Z);
            matrix4.Row3.W = 1f;
            return matrix4;
        }

        /// <summary>Build a world space to camera space matrix</summary>
        /// <param name="eyeX">Eye (camera) position in world space</param>
        /// <param name="eyeY">Eye (camera) position in world space</param>
        /// <param name="eyeZ">Eye (camera) position in world space</param>
        /// <param name="targetX">Target position in world space</param>
        /// <param name="targetY">Target position in world space</param>
        /// <param name="targetZ">Target position in world space</param>
        /// <param name="upX">Up vector in world space (should not be parallel to the camera direction, that is target - eye)</param>
        /// <param name="upY">Up vector in world space (should not be parallel to the camera direction, that is target - eye)</param>
        /// <param name="upZ">Up vector in world space (should not be parallel to the camera direction, that is target - eye)</param>
        /// <returns>A Matrix4 that transforms world space to camera space</returns>
        public static Matrix4 LookAt(float eyeX, float eyeY, float eyeZ, float targetX, float targetY, float targetZ, float upX, float upY, float upZ)
        {
            return Matrix4.LookAt(new Vector3(eyeX, eyeY, eyeZ), new Vector3(targetX, targetY, targetZ), new Vector3(upX, upY, upZ));
        }

        /// <summary>Adds two instances.</summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <returns>A new instance that is the result of the addition.</returns>
        public static Matrix4 Add(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Matrix4.Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>Adds two instances.</summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <param name="result">A new instance that is the result of the addition.</param>
        public static void Add(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            result.Row0 = left.Row0 + right.Row0;
            result.Row1 = left.Row1 + right.Row1;
            result.Row2 = left.Row2 + right.Row2;
            result.Row3 = left.Row3 + right.Row3;
        }

        /// <summary>Subtracts one instance from another.</summary>
        /// <param name="left">The left operand of the subraction.</param>
        /// <param name="right">The right operand of the subraction.</param>
        /// <returns>A new instance that is the result of the subraction.</returns>
        public static Matrix4 Subtract(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Matrix4.Subtract(ref left, ref right, out result);
            return result;
        }

        /// <summary>Subtracts one instance from another.</summary>
        /// <param name="left">The left operand of the subraction.</param>
        /// <param name="right">The right operand of the subraction.</param>
        /// <param name="result">A new instance that is the result of the subraction.</param>
        public static void Subtract(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            result.Row0 = left.Row0 - right.Row0;
            result.Row1 = left.Row1 - right.Row1;
            result.Row2 = left.Row2 - right.Row2;
            result.Row3 = left.Row3 - right.Row3;
        }

        /// <summary>Multiplies two instances.</summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        public static Matrix4 Mult(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Matrix4.Mult(ref left, ref right, out result);
            return result;
        }

        /// <summary>Multiplies two instances.</summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
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
            result.Row0.X = (float)((double)x1 * (double)x5 + (double)y1 * (double)x6 + (double)z1 * (double)x7 + (double)w1 * (double)x8);
            result.Row0.Y = (float)((double)x1 * (double)y5 + (double)y1 * (double)y6 + (double)z1 * (double)y7 + (double)w1 * (double)y8);
            result.Row0.Z = (float)((double)x1 * (double)z5 + (double)y1 * (double)z6 + (double)z1 * (double)z7 + (double)w1 * (double)z8);
            result.Row0.W = (float)((double)x1 * (double)w5 + (double)y1 * (double)w6 + (double)z1 * (double)w7 + (double)w1 * (double)w8);
            result.Row1.X = (float)((double)x2 * (double)x5 + (double)y2 * (double)x6 + (double)z2 * (double)x7 + (double)w2 * (double)x8);
            result.Row1.Y = (float)((double)x2 * (double)y5 + (double)y2 * (double)y6 + (double)z2 * (double)y7 + (double)w2 * (double)y8);
            result.Row1.Z = (float)((double)x2 * (double)z5 + (double)y2 * (double)z6 + (double)z2 * (double)z7 + (double)w2 * (double)z8);
            result.Row1.W = (float)((double)x2 * (double)w5 + (double)y2 * (double)w6 + (double)z2 * (double)w7 + (double)w2 * (double)w8);
            result.Row2.X = (float)((double)x3 * (double)x5 + (double)y3 * (double)x6 + (double)z3 * (double)x7 + (double)w3 * (double)x8);
            result.Row2.Y = (float)((double)x3 * (double)y5 + (double)y3 * (double)y6 + (double)z3 * (double)y7 + (double)w3 * (double)y8);
            result.Row2.Z = (float)((double)x3 * (double)z5 + (double)y3 * (double)z6 + (double)z3 * (double)z7 + (double)w3 * (double)z8);
            result.Row2.W = (float)((double)x3 * (double)w5 + (double)y3 * (double)w6 + (double)z3 * (double)w7 + (double)w3 * (double)w8);
            result.Row3.X = (float)((double)x4 * (double)x5 + (double)y4 * (double)x6 + (double)z4 * (double)x7 + (double)w4 * (double)x8);
            result.Row3.Y = (float)((double)x4 * (double)y5 + (double)y4 * (double)y6 + (double)z4 * (double)y7 + (double)w4 * (double)y8);
            result.Row3.Z = (float)((double)x4 * (double)z5 + (double)y4 * (double)z6 + (double)z4 * (double)z7 + (double)w4 * (double)z8);
            result.Row3.W = (float)((double)x4 * (double)w5 + (double)y4 * (double)w6 + (double)z4 * (double)w7 + (double)w4 * (double)w8);
        }

        /// <summary>Multiplies an instance by a scalar.</summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication</returns>
        public static Matrix4 Mult(Matrix4 left, float right)
        {
            Matrix4 result;
            Matrix4.Mult(ref left, right, out result);
            return result;
        }

        /// <summary>Multiplies an instance by a scalar.</summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication</param>
        public static void Mult(ref Matrix4 left, float right, out Matrix4 result)
        {
            result.Row0 = left.Row0 * right;
            result.Row1 = left.Row1 * right;
            result.Row2 = left.Row2 * right;
            result.Row3 = left.Row3 * right;
        }

        /// <summary>Calculate the inverse of the given matrix</summary>
        /// <param name="mat">The matrix to invert</param>
        /// <param name="result">The inverse of the given matrix if it has one, or the input if it is singular</param>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the Matrix4 is singular.</exception>
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
                *numPtr1 = (float)((double)numPtr2[5] * (double)numPtr2[10] * (double)numPtr2[15] - (double)numPtr2[5] * (double)numPtr2[11] * (double)numPtr2[14] - (double)numPtr2[9] * (double)numPtr2[6] * (double)numPtr2[15] + (double)numPtr2[9] * (double)numPtr2[7] * (double)numPtr2[14] + (double)numPtr2[13] * (double)numPtr2[6] * (double)numPtr2[11] - (double)numPtr2[13] * (double)numPtr2[7] * (double)numPtr2[10]);
                numPtr1[4] = (float)(-(double)numPtr2[4] * (double)numPtr2[10] * (double)numPtr2[15] + (double)numPtr2[4] * (double)numPtr2[11] * (double)numPtr2[14] + (double)numPtr2[8] * (double)numPtr2[6] * (double)numPtr2[15] - (double)numPtr2[8] * (double)numPtr2[7] * (double)numPtr2[14] - (double)numPtr2[12] * (double)numPtr2[6] * (double)numPtr2[11] + (double)numPtr2[12] * (double)numPtr2[7] * (double)numPtr2[10]);
                numPtr1[8] = (float)((double)numPtr2[4] * (double)numPtr2[9] * (double)numPtr2[15] - (double)numPtr2[4] * (double)numPtr2[11] * (double)numPtr2[13] - (double)numPtr2[8] * (double)numPtr2[5] * (double)numPtr2[15] + (double)numPtr2[8] * (double)numPtr2[7] * (double)numPtr2[13] + (double)numPtr2[12] * (double)numPtr2[5] * (double)numPtr2[11] - (double)numPtr2[12] * (double)numPtr2[7] * (double)numPtr2[9]);
                numPtr1[12] = (float)(-(double)numPtr2[4] * (double)numPtr2[9] * (double)numPtr2[14] + (double)numPtr2[4] * (double)numPtr2[10] * (double)numPtr2[13] + (double)numPtr2[8] * (double)numPtr2[5] * (double)numPtr2[14] - (double)numPtr2[8] * (double)numPtr2[6] * (double)numPtr2[13] - (double)numPtr2[12] * (double)numPtr2[5] * (double)numPtr2[10] + (double)numPtr2[12] * (double)numPtr2[6] * (double)numPtr2[9]);
                *(float*)((IntPtr)numPtr1 + 4) = (float)(-(double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[10] * (double)numPtr2[15] + (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[11] * (double)numPtr2[14] + (double)numPtr2[9] * (double)numPtr2[2] * (double)numPtr2[15] - (double)numPtr2[9] * (double)numPtr2[3] * (double)numPtr2[14] - (double)numPtr2[13] * (double)numPtr2[2] * (double)numPtr2[11] + (double)numPtr2[13] * (double)numPtr2[3] * (double)numPtr2[10]);
                numPtr1[5] = (float)((double)*numPtr2 * (double)numPtr2[10] * (double)numPtr2[15] - (double)*numPtr2 * (double)numPtr2[11] * (double)numPtr2[14] - (double)numPtr2[8] * (double)numPtr2[2] * (double)numPtr2[15] + (double)numPtr2[8] * (double)numPtr2[3] * (double)numPtr2[14] + (double)numPtr2[12] * (double)numPtr2[2] * (double)numPtr2[11] - (double)numPtr2[12] * (double)numPtr2[3] * (double)numPtr2[10]);
                numPtr1[9] = (float)(-(double)*numPtr2 * (double)numPtr2[9] * (double)numPtr2[15] + (double)*numPtr2 * (double)numPtr2[11] * (double)numPtr2[13] + (double)numPtr2[8] * (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[15] - (double)numPtr2[8] * (double)numPtr2[3] * (double)numPtr2[13] - (double)numPtr2[12] * (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[11] + (double)numPtr2[12] * (double)numPtr2[3] * (double)numPtr2[9]);
                numPtr1[13] = (float)((double)*numPtr2 * (double)numPtr2[9] * (double)numPtr2[14] - (double)*numPtr2 * (double)numPtr2[10] * (double)numPtr2[13] - (double)numPtr2[8] * (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[14] + (double)numPtr2[8] * (double)numPtr2[2] * (double)numPtr2[13] + (double)numPtr2[12] * (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[10] - (double)numPtr2[12] * (double)numPtr2[2] * (double)numPtr2[9]);
                numPtr1[2] = (float)((double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[6] * (double)numPtr2[15] - (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[7] * (double)numPtr2[14] - (double)numPtr2[5] * (double)numPtr2[2] * (double)numPtr2[15] + (double)numPtr2[5] * (double)numPtr2[3] * (double)numPtr2[14] + (double)numPtr2[13] * (double)numPtr2[2] * (double)numPtr2[7] - (double)numPtr2[13] * (double)numPtr2[3] * (double)numPtr2[6]);
                numPtr1[6] = (float)(-(double)*numPtr2 * (double)numPtr2[6] * (double)numPtr2[15] + (double)*numPtr2 * (double)numPtr2[7] * (double)numPtr2[14] + (double)numPtr2[4] * (double)numPtr2[2] * (double)numPtr2[15] - (double)numPtr2[4] * (double)numPtr2[3] * (double)numPtr2[14] - (double)numPtr2[12] * (double)numPtr2[2] * (double)numPtr2[7] + (double)numPtr2[12] * (double)numPtr2[3] * (double)numPtr2[6]);
                numPtr1[10] = (float)((double)*numPtr2 * (double)numPtr2[5] * (double)numPtr2[15] - (double)*numPtr2 * (double)numPtr2[7] * (double)numPtr2[13] - (double)numPtr2[4] * (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[15] + (double)numPtr2[4] * (double)numPtr2[3] * (double)numPtr2[13] + (double)numPtr2[12] * (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[7] - (double)numPtr2[12] * (double)numPtr2[3] * (double)numPtr2[5]);
                numPtr1[14] = (float)(-(double)*numPtr2 * (double)numPtr2[5] * (double)numPtr2[14] + (double)*numPtr2 * (double)numPtr2[6] * (double)numPtr2[13] + (double)numPtr2[4] * (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[14] - (double)numPtr2[4] * (double)numPtr2[2] * (double)numPtr2[13] - (double)numPtr2[12] * (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[6] + (double)numPtr2[12] * (double)numPtr2[2] * (double)numPtr2[5]);
                numPtr1[3] = (float)(-(double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[6] * (double)numPtr2[11] + (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[7] * (double)numPtr2[10] + (double)numPtr2[5] * (double)numPtr2[2] * (double)numPtr2[11] - (double)numPtr2[5] * (double)numPtr2[3] * (double)numPtr2[10] - (double)numPtr2[9] * (double)numPtr2[2] * (double)numPtr2[7] + (double)numPtr2[9] * (double)numPtr2[3] * (double)numPtr2[6]);
                numPtr1[7] = (float)((double)*numPtr2 * (double)numPtr2[6] * (double)numPtr2[11] - (double)*numPtr2 * (double)numPtr2[7] * (double)numPtr2[10] - (double)numPtr2[4] * (double)numPtr2[2] * (double)numPtr2[11] + (double)numPtr2[4] * (double)numPtr2[3] * (double)numPtr2[10] + (double)numPtr2[8] * (double)numPtr2[2] * (double)numPtr2[7] - (double)numPtr2[8] * (double)numPtr2[3] * (double)numPtr2[6]);
                numPtr1[11] = (float)(-(double)*numPtr2 * (double)numPtr2[5] * (double)numPtr2[11] + (double)*numPtr2 * (double)numPtr2[7] * (double)numPtr2[9] + (double)numPtr2[4] * (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[11] - (double)numPtr2[4] * (double)numPtr2[3] * (double)numPtr2[9] - (double)numPtr2[8] * (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[7] + (double)numPtr2[8] * (double)numPtr2[3] * (double)numPtr2[5]);
                numPtr1[15] = (float)((double)*numPtr2 * (double)numPtr2[5] * (double)numPtr2[10] - (double)*numPtr2 * (double)numPtr2[6] * (double)numPtr2[9] - (double)numPtr2[4] * (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[10] + (double)numPtr2[4] * (double)numPtr2[2] * (double)numPtr2[9] + (double)numPtr2[8] * (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr2[6] - (double)numPtr2[8] * (double)numPtr2[2] * (double)numPtr2[5]);
                float num1 = (float)((double)*numPtr2 * (double)*numPtr1 + (double)*(float*)((IntPtr)numPtr2 + 4) * (double)numPtr1[4] + (double)numPtr2[2] * (double)numPtr1[8] + (double)numPtr2[3] * (double)numPtr1[12]);
                if ((double)num1 == 0.0)
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

        /// <summary>Calculate the inverse of the given matrix</summary>
        /// <param name="mat">The matrix to invert</param>
        /// <returns>The inverse of the given matrix if it has one, or the input if it is singular</returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the Matrix4 is singular.</exception>
        public static Matrix4 Invert(Matrix4 mat)
        {
            Matrix4 result;
            Matrix4.Invert(ref mat, out result);
            return result;
        }

        /// <summary>Calculate the transpose of the given matrix</summary>
        /// <param name="mat">The matrix to transpose</param>
        /// <returns>The transpose of the given matrix</returns>
        public static Matrix4 Transpose(Matrix4 mat)
        {
            return new Matrix4(mat.Column0, mat.Column1, mat.Column2, mat.Column3);
        }

        /// <summary>Calculate the transpose of the given matrix</summary>
        /// <param name="mat">The matrix to transpose</param>
        /// <param name="result">The result of the calculation</param>
        public static void Transpose(ref Matrix4 mat, out Matrix4 result)
        {
            result.Row0 = mat.Column0;
            result.Row1 = mat.Column1;
            result.Row2 = mat.Column2;
            result.Row3 = mat.Column3;
        }

        /// <summary>Matrix multiplication</summary>
        /// <param name="left">left-hand operand</param>
        /// <param name="right">right-hand operand</param>
        /// <returns>A new Matrix4 which holds the result of the multiplication</returns>
        public static Matrix4 operator *(Matrix4 left, Matrix4 right)
        {
            return Matrix4.Mult(left, right);
        }

        /// <summary>Matrix-scalar multiplication</summary>
        /// <param name="left">left-hand operand</param>
        /// <param name="right">right-hand operand</param>
        /// <returns>A new Matrix4 which holds the result of the multiplication</returns>
        public static Matrix4 operator *(Matrix4 left, float right)
        {
            return Matrix4.Mult(left, right);
        }

        /// <summary>Matrix addition</summary>
        /// <param name="left">left-hand operand</param>
        /// <param name="right">right-hand operand</param>
        /// <returns>A new Matrix4 which holds the result of the addition</returns>
        public static Matrix4 operator +(Matrix4 left, Matrix4 right)
        {
            return Matrix4.Add(left, right);
        }

        /// <summary>Matrix subtraction</summary>
        /// <param name="left">left-hand operand</param>
        /// <param name="right">right-hand operand</param>
        /// <returns>A new Matrix4 which holds the result of the subtraction</returns>
        public static Matrix4 operator -(Matrix4 left, Matrix4 right)
        {
            return Matrix4.Subtract(left, right);
        }

        /// <summary>Compares two instances for equality.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(Matrix4 left, Matrix4 right)
        {
            return left.Equals(right);
        }

        /// <summary>Compares two instances for inequality.</summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equal right; false otherwise.</returns>
        public static bool operator !=(Matrix4 left, Matrix4 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a System.String that represents the current Matrix4.
        /// </summary>
        /// <returns>The string representation of the matrix.</returns>
        public override string ToString()
        {
            return string.Format("{0}\n{1}\n{2}\n{3}", (object)this.Row0, (object)this.Row1, (object)this.Row2, (object)this.Row3);
        }

        /// <summary>Returns the hashcode for this instance.</summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            return ((this.Row0.GetHashCode() * 397 ^ this.Row1.GetHashCode()) * 397 ^ this.Row2.GetHashCode()) * 397 ^ this.Row3.GetHashCode();
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare tresult.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix4))
            {
                return false;
            }

            return this.Equals((Matrix4)obj);
        }

        /// <summary>Indicates whether the current matrix is equal to another matrix.</summary>
        /// <param name="other">An matrix to compare with this matrix.</param>
        /// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
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