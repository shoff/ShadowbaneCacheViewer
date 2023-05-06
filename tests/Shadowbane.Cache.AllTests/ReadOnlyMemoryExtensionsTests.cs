namespace Shadowbane.Cache.AllTests;

using System;
using System.Buffers;
using AutoFixture;
using CacheTypes;
using Xunit;

public class ReadOnlyMemoryExtensionsTests : CacheBaseTest
{
    [Fact]
    public void Uncompress_Returns_Same_Values_Sent_To_Compress()
    {
        var originalArray = ArrayPool<byte>.Shared.Rent(50);
        for (int i = 0; i < 50; i++)
        {
            originalArray[i] = this.fixture.Create<byte>();
        }

        var originalMemory = new ReadOnlyMemory<byte>(originalArray);
        var compressedMemory = originalMemory.Compress();
        var uncompressedMemory = compressedMemory.Uncompress((uint) originalArray.Length);
        for (int i = 0; i < 50; i++)
        {
            Assert.Equal(originalMemory.Span[i], uncompressedMemory.Span[i]);
        }
    }
}