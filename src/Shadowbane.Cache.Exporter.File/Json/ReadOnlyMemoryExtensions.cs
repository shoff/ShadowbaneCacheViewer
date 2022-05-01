namespace Shadowbane.Cache.Exporter.File.Json;

using System;

public static class ReadOnlyMemoryExtensions
{
    public static string ToHexString(this ReadOnlyMemory<byte> segment, string prefix)
    {
        char[] lookup = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        //if (segment.Span == null)
        //{
        //    return "";
        //}

        int i = 0, p = prefix.Length, l = segment.Span.Length;
        char[] c = new char[l * 2 + p];
        byte d;
        for (; i < p; ++i)
        {
            c[i] = prefix[i];
        }

        i = -1;
        --l;
        --p;
        while (i < l)
        {
            d = segment.Span[++i];
            c[++p] = lookup[d >> 4];
            c[++p] = lookup[d & 0xF];
        }
        return new string(c, 0, c.Length);
    }
}