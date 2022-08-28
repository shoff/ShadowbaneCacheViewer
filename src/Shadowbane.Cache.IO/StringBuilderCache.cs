using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System;

namespace Shadowbane.Cache.IO;

internal static class StringBuilderCache
{
    static readonly Type? type = typeof(object).Assembly.GetType("System.Text.StringBuilderCache");

    public static StringBuilder Acquire(int capacity = 16) =>
        acquire.Value(capacity);

    private static readonly Lazy<Func<int, StringBuilder>> acquire = new(CreateAcquireDelegate);

    private static Func<int, StringBuilder> CreateAcquireDelegate()
    {
        var expCapacity = Expression.Parameter(typeof(int));
        var acquireInfo = type.GetMethod(nameof(Acquire),
            BindingFlags.Static | BindingFlags.Public);

        var expCallAcquire = Expression.Call(acquireInfo, expCapacity);
        var lambda = Expression.Lambda(expCallAcquire, expCapacity);
        return (Func<int, StringBuilder>)lambda.Compile();
    }

    public static string GetStringAndRelease(StringBuilder sb) =>
        getStringAndRelease.Value(sb);

    private static readonly Lazy<Func<StringBuilder, string>> getStringAndRelease =
        new Lazy<Func<StringBuilder, string>>(CreateGetStringAndReleaseDelegate);

    private static Func<StringBuilder, string> CreateGetStringAndReleaseDelegate()
    {
        var expSB = Expression.Parameter(typeof(StringBuilder));
        var releaseMethod = type.GetMethod(nameof(GetStringAndRelease),
            BindingFlags.Static | BindingFlags.Public);
        var expCall = Expression.Call(releaseMethod, expSB);
        var lambda = Expression.Lambda(expCall, expSB);
        return (Func<StringBuilder, string>)lambda.Compile();
    }
}