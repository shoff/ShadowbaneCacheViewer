namespace Shadowbane.Geometry;

using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
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