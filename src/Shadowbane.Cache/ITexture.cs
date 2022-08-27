namespace Shadowbane.Cache;

using System;
using System.IO;
using SixLabors.ImageSharp;

public interface ITexture
{
    Image? Image { get; }
    uint TextureId { get; }
    int Width { get; }
    int Height { get; }
    int Depth { get; }
    Image? TextureMap(BinaryReader reader, ReadOnlyMemory<byte> buffer);
    Image? TextureMap(ReadOnlyMemory<byte> buffer);
}

public struct PixelFormats
{
    
}