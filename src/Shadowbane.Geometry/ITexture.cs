namespace Shadowbane.Geometry;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public interface ITexture
{
    Bitmap Image { get; }
    uint TextureId { get; }
    int Width { get; }
    int Height { get; }
    int Depth { get; }
    PixelFormat PixelFormat { get; }
    Bitmap TextureMap(BinaryReader reader, ReadOnlyMemory<byte> buffer);
    Bitmap TextureMap(ReadOnlyMemory<byte> buffer);
}