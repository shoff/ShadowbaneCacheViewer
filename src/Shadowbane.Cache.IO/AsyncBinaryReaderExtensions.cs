﻿namespace Shadowbane.Cache.IO;

using System.Text;
using System;
using CommunityToolkit.HighPerformance;


public static class AsyncBinaryReaderExtensions
{
    public static AsyncBinaryReader CreateAsyncBinaryReaderUtf32(this ReadOnlyMemory<byte> segment, long cacheIndexOffset = 0)
    {
        var reader = new AsyncBinaryReader(segment.AsStream(), Encoding.UTF32);
        reader.BaseStream.Position = cacheIndexOffset;
        return reader;
    }
}