namespace Shadowbane.Geometry;

using System;

internal static class MathHelper
{
    public const float PI = 3.141593f;
    public const float PI_OVER2 = 1.570796f;
    public const float PI_OVER3 = 1.047198f;
    public const float PI_OVER4 = 0.7853982f;
    public const float PI_OVER6 = 0.5235988f;
    public const float TWO_PI = 6.283185f;
    public const float THREE_PI_OVER2 = 4.712389f;
    public const float E = 2.718282f;
    public const float LOG10_E = 0.4342945f;
    public const float LOG2_E = 1.442695f;
    private const int V = 1597463174;

    public static long NextPowerOfTwo(long n)
    {
        if (n < 0L)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Must be positive.");
        }

        return (long)Math.Pow(2.0, Math.Ceiling(Math.Log(n, 2.0)));
    }
        
    public static int NextPowerOfTwo(int n)
    {
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Must be positive.");
        }

        return (int)Math.Pow(2.0, Math.Ceiling(Math.Log(n, 2.0)));
    }

    public static float NextPowerOfTwo(float n)
    {
        if (n < 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Must be positive.");
        }

        return (float)Math.Pow(2.0, Math.Ceiling(Math.Log(n, 2.0)));
    }

    public static double NextPowerOfTwo(double n)
    {
        if (n < 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Must be positive.");
        }

        return Math.Pow(2.0, Math.Ceiling(Math.Log(n, 2.0)));
    }

    public static long Factorial(int n)
    {
        long num = 1;
        for (; n > 1; --n)
        {
            num *= n;
        }

        return num;
    }

    public static long BinomialCoefficient(int n, int k)
    {
        return MathHelper.Factorial(n) / (MathHelper.Factorial(k) * MathHelper.Factorial(n - k));
    }

    public static unsafe float InverseSqrtFast(float x)
    {
        float num = 0.5f * x;
        int* v = (int*)&x;
        var v1 = *v >> 1;
        var v2 = V - v1;
        x = *(float*)&v2;
        x *= (float)(1.5 - num * (double)x * x);
        return x;
    }

    public static double InverseSqrtFast(double x)
    {
        return InverseSqrtFast((float)x);
    }

    public static double RadiansToDegrees(double radians)
    {
        return radians * (180.0 / Math.PI);
    }

    public static float RadiansToDegrees(float radians)
    {
        return radians * 57.29578f;
    }
        
    public static double DegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180.0);
    }
        
    public static float DegreesToRadians(float degrees)
    {
        return degrees * ((float)Math.PI / 180f);
    }
        
    public static void Swap(ref double a, ref double b)
    {
        double num = a;
        a = b;
        b = num;
    }

    public static void Swap(ref float a, ref float b)
    {
        float num = a;
        a = b;
        b = num;
    }

    public static int Clamp(int n, int min, int max)
    {
        return Math.Max(Math.Min(n, max), min);
    }

    public static float Clamp(float n, float min, float max)
    {
        return Math.Max(Math.Min(n, max), min);
    }

    public static double Clamp(double n, double min, double max)
    {
        return Math.Max(Math.Min(n, max), min);
    }

    private static unsafe int FloatToInt32Bits(float f)
    {
        return *(int*)&f;
    }

    public static bool ApproximatelyEqual(float a, float b, int maxDeltaBits)
    {
        long num1 = FloatToInt32Bits(a);
        if (num1 < 0L)
        {
            num1 = int.MinValue - num1;
        }

        long num2 = FloatToInt32Bits(b);
        if (num2 < 0L)
        {
            num2 = int.MinValue - num2;
        }

        return Math.Abs(num1 - num2) <= 1 << maxDeltaBits;
    }
        
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

    public static bool ApproximatelyEquivalent(float a, float b, float tolerance)
    {
        if ((double)a == (double)b)
        {
            return true;
        }

        return (double)Math.Abs(a - b) <= (double)tolerance;
    }

    public static bool ApproximatelyEquivalent(double a, double b, double tolerance)
    {
        if (a == b)
        {
            return true;
        }

        return Math.Abs(a - b) <= tolerance;
    }
}