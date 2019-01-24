namespace CacheViewer.Domain.Geometry
{
    using System;

    /// <summary>Contains common mathematical functions and constants.</summary>
    public static class MathHelper
    {
        /// <summary>
        /// Defines the value of Pi as a <see cref="T:System.Single" />.
        /// </summary>
        public const float Pi = 3.141593f;
        /// <summary>
        /// Defines the value of Pi divided by two as a <see cref="T:System.Single" />.
        /// </summary>
        public const float PiOver2 = 1.570796f;
        /// <summary>
        /// Defines the value of Pi divided by three as a <see cref="T:System.Single" />.
        /// </summary>
        public const float PiOver3 = 1.047198f;
        /// <summary>
        /// Definesthe value of  Pi divided by four as a <see cref="T:System.Single" />.
        /// </summary>
        public const float PiOver4 = 0.7853982f;
        /// <summary>
        /// Defines the value of Pi divided by six as a <see cref="T:System.Single" />.
        /// </summary>
        public const float PiOver6 = 0.5235988f;
        /// <summary>
        /// Defines the value of Pi multiplied by two as a <see cref="T:System.Single" />.
        /// </summary>
        public const float TwoPi = 6.283185f;
        /// <summary>
        /// Defines the value of Pi multiplied by 3 and divided by two as a <see cref="T:System.Single" />.
        /// </summary>
        public const float ThreePiOver2 = 4.712389f;
        /// <summary>
        /// Defines the value of E as a <see cref="T:System.Single" />.
        /// </summary>
        public const float E = 2.718282f;
        /// <summary>Defines the base-10 logarithm of E.</summary>
        public const float Log10E = 0.4342945f;
        /// <summary>Defines the base-2 logarithm of E.</summary>
        public const float Log2E = 1.442695f;
        private const int V = 1597463174;

        /// <summary>
        /// Returns the next power of two that is greater than or equal to the specified number.
        /// </summary>
        /// <param name="n">The specified number.</param>
        /// <returns>The next power of two.</returns>
        public static long NextPowerOfTwo(long n)
        {
            if (n < 0L)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Must be positive.");
            }

            return (long)Math.Pow(2.0, Math.Ceiling(Math.Log((double)n, 2.0)));
        }

        /// <summary>
        /// Returns the next power of two that is greater than or equal to the specified number.
        /// </summary>
        /// <param name="n">The specified number.</param>
        /// <returns>The next power of two.</returns>
        public static int NextPowerOfTwo(int n)
        {
            if (n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Must be positive.");
            }

            return (int)Math.Pow(2.0, Math.Ceiling(Math.Log((double)n, 2.0)));
        }

        /// <summary>
        /// Returns the next power of two that is greater than or equal to the specified number.
        /// </summary>
        /// <param name="n">The specified number.</param>
        /// <returns>The next power of two.</returns>
        public static float NextPowerOfTwo(float n)
        {
            if ((double)n < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Must be positive.");
            }

            return (float)Math.Pow(2.0, Math.Ceiling(Math.Log((double)n, 2.0)));
        }

        /// <summary>
        /// Returns the next power of two that is greater than or equal to the specified number.
        /// </summary>
        /// <param name="n">The specified number.</param>
        /// <returns>The next power of two.</returns>
        public static double NextPowerOfTwo(double n)
        {
            if (n < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Must be positive.");
            }

            return Math.Pow(2.0, Math.Ceiling(Math.Log(n, 2.0)));
        }

        /// <summary>Calculates the factorial of a given natural number.</summary>
        /// <param name="n">The number.</param>
        /// <returns>n!</returns>
        public static long Factorial(int n)
        {
            long num = 1;
            for (; n > 1; --n)
            {
                num *= (long)n;
            }

            return num;
        }

        /// <summary>
        /// Calculates the binomial coefficient <paramref name="n" /> above <paramref name="k" />.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <param name="k">The k.</param>
        /// <returns>n! / (k! * (n - k)!)</returns>
        public static long BinomialCoefficient(int n, int k)
        {
            return MathHelper.Factorial(n) / (MathHelper.Factorial(k) * MathHelper.Factorial(n - k));
        }

        /// <summary>
        /// Returns an approximation of the inverse square root of left number.
        /// </summary>
        /// <param name="x">A number.</param>
        /// <returns>An approximation of the inverse square root of the specified number, with an upper error bound of 0.001</returns>
        /// <remarks>
        /// This is an improved implementation of the the method known as Carmack's inverse square root
        /// which is found in the Quake III source code. This implementation comes from
        /// http://www.codemaestro.com/reviews/review00000105.html. For the history of this method, see
        /// http://www.beyond3d.com/content/articles/8/
        /// </remarks>
        public static unsafe float InverseSqrtFast(float x)
        {
            float num = 0.5f * x;
            int* v = (int*)&x;
            var v1 = *v >> 1;
            var v2 = V - v1;
            x = *(float*)&v2;
            x *= (float)(1.5 - (double)num * (double)x * (double)x);
            return x;
        }

        /// <summary>
        /// Returns an approximation of the inverse square root of left number.
        /// </summary>
        /// <param name="x">A number.</param>
        /// <returns>An approximation of the inverse square root of the specified number, with an upper error bound of 0.001</returns>
        /// <remarks>
        /// This is an improved implementation of the the method known as Carmack's inverse square root
        /// which is found in the Quake III source code. This implementation comes from
        /// http://www.codemaestro.com/reviews/review00000105.html. For the history of this method, see
        /// http://www.beyond3d.com/content/articles/8/
        /// </remarks>
        public static double InverseSqrtFast(double x)
        {
            return (double)MathHelper.InverseSqrtFast((float)x);
        }

        /// <summary>Convert degrees to radians</summary>
        /// <param name="degrees">An angle in degrees</param>
        /// <returns>The angle expressed in radians</returns>
        public static float DegreesToRadians(float degrees)
        {
            return degrees * ((float)Math.PI / 180f);
        }

        /// <summary>Convert radians to degrees</summary>
        /// <param name="radians">An angle in radians</param>
        /// <returns>The angle expressed in degrees</returns>
        public static float RadiansToDegrees(float radians)
        {
            return radians * 57.29578f;
        }

        /// <summary>Convert degrees to radians</summary>
        /// <param name="degrees">An angle in degrees</param>
        /// <returns>The angle expressed in radians</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.0);
        }

        /// <summary>Convert radians to degrees</summary>
        /// <param name="radians">An angle in radians</param>
        /// <returns>The angle expressed in degrees</returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians * (180.0 / Math.PI);
        }

        /// <summary>Swaps two double values.</summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        public static void Swap(ref double a, ref double b)
        {
            double num = a;
            a = b;
            b = num;
        }

        /// <summary>Swaps two float values.</summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        public static void Swap(ref float a, ref float b)
        {
            float num = a;
            a = b;
            b = num;
        }

        /// <summary>Clamps a number between a minimum and a maximum.</summary>
        /// <param name="n">The number to clamp.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>min, if n is lower than min; max, if n is higher than max; n otherwise.</returns>
        public static int Clamp(int n, int min, int max)
        {
            return Math.Max(Math.Min(n, max), min);
        }

        /// <summary>Clamps a number between a minimum and a maximum.</summary>
        /// <param name="n">The number to clamp.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>min, if n is lower than min; max, if n is higher than max; n otherwise.</returns>
        public static float Clamp(float n, float min, float max)
        {
            return Math.Max(Math.Min(n, max), min);
        }

        /// <summary>Clamps a number between a minimum and a maximum.</summary>
        /// <param name="n">The number to clamp.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>min, if n is lower than min; max, if n is higher than max; n otherwise.</returns>
        public static double Clamp(double n, double min, double max)
        {
            return Math.Max(Math.Min(n, max), min);
        }

        private static unsafe int FloatToInt32Bits(float f)
        {
            return *(int*)&f;
        }

        /// <summary>
        /// Approximates floating point equality with a maximum number of different bits.
        /// This is typically used in place of an epsilon comparison.
        /// see: https://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/
        /// see: https://stackoverflow.com/questions/3874627/floating-point-comparison-functions-for-c-sharp
        /// </summary>
        /// <param name="a">the first value to compare</param>
        /// <param name="b">&gt;the second value to compare</param>
        /// <param name="maxDeltaBits">the number of floating point bits to check</param>
        /// <returns></returns>
        public static bool ApproximatelyEqual(float a, float b, int maxDeltaBits)
        {
            long num1 = (long)MathHelper.FloatToInt32Bits(a);
            if (num1 < 0L)
            {
                num1 = (long)int.MinValue - num1;
            }

            long num2 = (long)MathHelper.FloatToInt32Bits(b);
            if (num2 < 0L)
            {
                num2 = (long)int.MinValue - num2;
            }

            return Math.Abs(num1 - num2) <= (long)(1 << maxDeltaBits);
        }

        /// <summary>
        /// Approximates double-precision floating point equality by an epsilon (maximum error) value.
        /// This method is designed as a "fits-all" solution and attempts to handle as many cases as possible.
        /// </summary>
        /// <param name="a">The first float.</param>
        /// <param name="b">The second float.</param>
        /// <param name="epsilon">The maximum error between the two.</param>
        /// <returns><value>true</value> if the values are approximately equal within the error margin; otherwise, <value>false</value>.</returns>
        public static bool ApproximatelyEqualEpsilon(double a, double b, double epsilon)
        {
            double num1 = Math.Abs(a);
            double num2 = Math.Abs(b);
            double num3 = Math.Abs(a - b);
            if (a == b)
            {
                return true;
            }

            if (a == 0.0 || b == 0.0 || num3 < 2.2250738585072E-308)
            {
                return num3 < epsilon * 2.2250738585072E-308;
            }

            return num3 / Math.Min(num1 + num2, double.MaxValue) < epsilon;
        }

        /// <summary>
        /// Approximates single-precision floating point equality by an epsilon (maximum error) value.
        /// This method is designed as a "fits-all" solution and attempts to handle as many cases as possible.
        /// </summary>
        /// <param name="a">The first float.</param>
        /// <param name="b">The second float.</param>
        /// <param name="epsilon">The maximum error between the two.</param>
        /// <returns><value>true</value> if the values are approximately equal within the error margin; otherwise, <value>false</value>.</returns>
        public static bool ApproximatelyEqualEpsilon(float a, float b, float epsilon)
        {
            float num1 = Math.Abs(a);
            float num2 = Math.Abs(b);
            float num3 = Math.Abs(a - b);
            if ((double)a == (double)b)
            {
                return true;
            }

            if ((double)a == 0.0 || (double)b == 0.0 || (double)num3 < 1.17549435082229E-38)
            {
                return (double)num3 < (double)epsilon * 1.17549435082229E-38;
            }

            return (double)(num3 / Math.Min(num1 + num2, float.MaxValue)) < (double)epsilon;
        }

        /// <summary>
        /// Approximates equivalence between two single-precision floating-point numbers on a direct human scale.
        /// It is important to note that this does not approximate equality - instead, it merely checks whether or not
        /// two numbers could be considered equivalent to each other within a certain tolerance. The tolerance is
        /// inclusive.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare·</param>
        /// <param name="tolerance">The tolerance within which the two values would be considered equivalent.</param>
        /// <returns>Whether or not the values can be considered equivalent within the tolerance.</returns>
        public static bool ApproximatelyEquivalent(float a, float b, float tolerance)
        {
            if ((double)a == (double)b)
            {
                return true;
            }

            return (double)Math.Abs(a - b) <= (double)tolerance;
        }

        /// <summary>
        /// Approximates equivalence between two double-precision floating-point numbers on a direct human scale.
        /// It is important to note that this does not approximate equality - instead, it merely checks whether or not
        /// two numbers could be considered equivalent to each other within a certain tolerance. The tolerance is
        /// inclusive.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare·</param>
        /// <param name="tolerance">The tolerance within which the two values would be considered equivalent.</param>
        /// <returns>Whether or not the values can be considered equivalent within the tolerance.</returns>
        public static bool ApproximatelyEquivalent(double a, double b, double tolerance)
        {
            if (a == b)
            {
                return true;
            }

            return Math.Abs(a - b) <= tolerance;
        }
    }
}