namespace Shadowbane.Models;

using System;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Numerics;
using Cache;
using Microsoft.Toolkit.HighPerformance;
using Serilog;
using SixLabors.ImageSharp;

using Color = SixLabors.ImageSharp.Color;
using Image = SixLabors.ImageSharp.Image;

public class Texture : ITexture
{
    private static readonly ILogger logger = Log.Logger = new LoggerConfiguration()
        // add console as logging target
        .WriteTo.File($"{AppDomain.CurrentDomain.BaseDirectory}\\Logs\\Texture.log", rollingInterval: RollingInterval.Day)
        // set default minimum level
        .MinimumLevel.Debug()
        .CreateLogger();

    public static string ImageSavePath => $"{AppDomain.CurrentDomain.BaseDirectory}\\Images\\";

    public Texture(ReadOnlyMemory<byte> data, uint id)
    {
        this.TextureId = id;
        using var reader = new BinaryReader(data.AsStream());
        this.Width = reader.ReadInt32();
        this.Height = reader.ReadInt32();
        this.Depth = reader.ReadInt32();

        // this.PixelFormat = this.Depth switch
        // {
        //     1 => PixelFormat.Undefined,
        //     4 => PixelFormat.Format32bppArgb,
        //     65536 => PixelFormat.Indexed,
        //     _ => PixelFormat.Format24bppRgb
        // };
        reader.BaseStream.Position += 14;
        this.Image = this.TextureMap(reader, data);
    }

    public Image? Image { get; }

    public uint TextureId { get; }

    public int Width { get; }

    public int Height { get; }

    public int Depth { get; }

    public bool IsValid => this.Image is { Width: > 0, Height: > 0 };

    // ReSharper disable once CognitiveComplexity
    public Image? TextureMap(BinaryReader reader, ReadOnlyMemory<byte> data)
    {
        var format = this.GetFormat();
        if (format == PixelFormat.Undefined || this.Width < 1 || this.Height < 1)
        {
            // format = PixelFormat.Alpha; /* Gl.GL_LUMINANCE;*/
            // for now just return as this is not valid for GDI bitmaps :(
            return null;
        }

        // var pd = new PixelData();
        switch (format)
        {
            // NFI yet how to read these, not my top priority.
            case PixelFormat.Indexed:
                var image = Image.Load(reader.ReadBytes(this.Width * this.Height));
                image.Mutate(x => x
                    .Resize(this.Width, this.Height)
                    .AutoOrient()
                    .RotateFlip(RotateMode.Rotate180, FlipMode.Vertical));
                image.SaveAsGif($"{ImageSavePath}\\{this.TextureId}.gif");
                return image;

            case PixelFormat.Format24bppRgb:

                Image<Rgb24> image1 = new Image<Rgb24>(this.Width, this.Height);
                var remainingBytes = reader.BaseStream.Length - reader.BaseStream.Position;
                var bytesNeeded = this.Height * (this.Width * 3);
                if (remainingBytes < bytesNeeded)
                {
                    // grrr why?
                    logger.Error($"Format24bppRgb should have {bytesNeeded} bytes, but only has {remainingBytes}");
                    return null;
                }
                
                // TODO this shouldn't be happening.
                if (!reader.CanRead(3))
                {
                    logger.Error(
                        $"Unable to complete image creation for texture id {this.TextureId} could not read data!");
                    break;
                }

                image1.ProcessPixelRows(accessor =>
                {
                    for (var y = 0; y < image1.Height; y++)
                    {
                        var rowSpan = accessor.GetRowSpan(y);
                        for (var x = 0; x < rowSpan.Length; x++)
                        {
                            // Get a reference to the pixel at position x
                            ref Rgb24 pixel = ref rowSpan[x];
                            pixel.R = reader.ReadByte();
                            pixel.G = reader.ReadByte();
                            pixel.B = reader.ReadByte();
                        }
                    }
                });                
                image1.Mutate(m => m.Flip(FlipMode.Vertical));

                image1.SaveAsPng($"{ImageSavePath}\\{this.TextureId}.png");
                return image1;

            case PixelFormat.Format32bppArgb:
                Image<Rgba32> image2 = new Image<Rgba32>(this.Width, this.Height);

                var remainingBytes1 = reader.BaseStream.Length - reader.BaseStream.Position;
                var bytesNeeded1 = this.Height * (this.Width * 4);
                if (remainingBytes1 < bytesNeeded1)
                {
                    // grrr why?
                    logger.Error($"Format32bppArgb should have {bytesNeeded1} bytes, but only has {remainingBytes1}");
                    return null;
                }

                image2.ProcessPixelRows(accessor =>
                {
                    for (var y = 0; y < image2.Height; y++)
                    {
                        var rowSpan = accessor.GetRowSpan(y);
                        for (var x = 0; x < rowSpan.Length; x++)
                        {
                            // Get a reference to the pixel at position x
                            ref Rgba32 pixel = ref rowSpan[x];
                            pixel.R = reader.ReadByte();
                            pixel.G = reader.ReadByte();
                            pixel.B = reader.ReadByte();
                            pixel.A = reader.ReadByte();
                        }
                    }
                });
                image2.Mutate(m => m.Flip(FlipMode.Vertical));

                // image2 = Image.LoadPixelData<Rgba32>(data.Span, this.Width, this.Height);
                //for (var y = 0; y < this.Height; y++)
                //{
                //    for (var x = 0; x < this.Width; x++)
                //    {
                //        //pd.red = reader.ReadByte();
                //        //pd.green = reader.ReadByte();
                //        //pd.blue = reader.ReadByte();
                //        //pd.alpha = reader.ReadByte();

                //        var clr = Color.FromRgba(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                //        image2[y, x] = clr;
                //    }
                //}
                image2.SaveAsPng($"{ImageSavePath}\\{this.TextureId}.png");

                return image2;
        }

        return null;
    }

    public Image? TextureMap(ReadOnlyMemory<byte> buffer)
    {
        using var reader = buffer.CreateBinaryReaderUtf32(26);
        return TextureMap(reader, buffer);
    }

    public PixelFormat PixelFormat => this.Depth switch
    {
        1 => PixelFormat.Alpha,
        4 => PixelFormat.Format32bppArgb,
        _ => PixelFormat.Format24bppRgb
    };

    private PixelFormat GetFormat() => this.Depth switch
    {
        1 => PixelFormat.Alpha,
        4 => PixelFormat.Format32bppArgb,
        _ => PixelFormat.Format24bppRgb
    };
}
public enum PixelFormat : int
{
    Undefined = 0,
    Max = 15, // 0x0000000F
    Indexed = 65536, // 0x00010000
    Gdi = 131072, // 0x00020000
    Format16bppRgb555 = 135173, // 0x00021005
    Format16bppRgb565 = 135174, // 0x00021006
    Format24bppRgb = 137224, // 0x00021808
    Format32bppRgb = 139273, // 0x00022009
    Format1bppIndexed = 196865, // 0x00030101
    Format4bppIndexed = 197634, // 0x00030402
    Format8bppIndexed = 198659, // 0x00030803
    Alpha = 262144, // 0x00040000
    Format16bppArgb1555 = 397319, // 0x00061007
    PAlpha = 524288, // 0x00080000
    Format32bppPArgb = 925707, // 0x000E200B
    Extended = 1048576, // 0x00100000
    Format16bppGrayScale = 1052676, // 0x00101004
    Format48bppRgb = 1060876, // 0x0010300C
    Format64bppPArgb = 1851406, // 0x001C400E
    Canonical = 2097152, // 0x00200000
    Format32bppArgb = 2498570, // 0x0026200A
    Format64bppArgb = 3424269, // 0x0034400D
}

