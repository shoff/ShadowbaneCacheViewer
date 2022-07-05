namespace Shadowbane.Models;

using System;
using System.Drawing.Imaging;
using SixLabors.ImageSharp.Processing;
using System.IO;
using Cache;
using Geometry;
using Microsoft.Toolkit.HighPerformance;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Color=SixLabors.ImageSharp.Color;
using Image=SixLabors.ImageSharp.Image;

public class Texture : ITexture
{
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

    public Image? TextureMap(BinaryReader reader, ReadOnlyMemory<byte> buffer)
    {
        var format = this.GetFormat();
        if (format == PixelFormat.Undefined || this.Width < 1 || this.Height < 1)
        {
            //format = PixelFormat.Alpha; /* Gl.GL_LUMINANCE;*/
            // for now just return as this is not valid for GDI bitmaps :(
            return null;
        }
        var pd = new PixelData();
        switch (format)
        {
            // NFI yet how to read these, not my top priority.
            case PixelFormat.Indexed:
                var image = Image.Load(reader.ReadBytes(this.Width * this.Height));
                image.Mutate(x => x
                    .Resize(this.Width,this.Height)
                    .AutoOrient()
                    .RotateFlip(RotateMode.Rotate180, FlipMode.Vertical));
                //
                // for (var y = 0; y < this.Height; y++)
                // {
                //     for (var x = 0; x < this.Width; x++)
                //     {
                //         // Clearly this is wrong
                //         var colorByte = reader.ReadByte();
                //         KnownColor kc;
                //         try
                //         {
                //             kc = (KnownColor)(int)colorByte;
                //         }
                //         catch
                //         {
                //             kc = KnownColor.Black;
                //         }
                //
                //         clr = Color.FromKnownColor(kc);
                //         myBitmap.SetPixel(x, y, clr);
                //     }
                // }
                return image;

            case PixelFormat.Format24bppRgb:

                Image<Rgb24> image1 = new Image<Rgb24>(this.Width, this.Height);
                var remainingBytes = reader.BaseStream.Length - reader.BaseStream.Position;
                var bytesNeeded = this.Height * (this.Width * 3);
                if (remainingBytes < bytesNeeded)
                {
                    // grrr why?
                    return null;
                }

                for (var y = 0; y < image1.Height; y++)
                {
                    for (var x = 0; x < image1.Width; x++)
                    {
                        // TODO this shouldn't be happening.
                        if (reader.CanRead(3))
                        {
                            pd.red = reader.ReadByte();
                            pd.green = reader.ReadByte();
                            pd.blue = reader.ReadByte();
                        }
                        else
                        {
                            pd.red = 0;
                            pd.green = 0;
                            pd.blue = 0;
                        }

                        var clr = Color.FromRgb(pd.red, pd.green, pd.blue);
                        image1[y, x] = clr;
                    }
                }
                return image1;
            
            case PixelFormat.Format32bppArgb:
                Image<Argb32> image2 = new Image<Argb32>(this.Width, this.Height);

                var remainingBytes1 = reader.BaseStream.Length - reader.BaseStream.Position;
                var bytesNeeded1 = this.Height * (this.Width * 4);
                if (remainingBytes1 < bytesNeeded1)
                {
                    // grrr why?
                    return null;
                }

                for (var y = 0; y < this?.Height; y++)
                {
                    for (var x = 0; x < this.Width; x++)
                    {
                        pd.red = reader.ReadByte();
                        pd.green = reader.ReadByte();
                        pd.blue = reader.ReadByte();
                        pd.alpha = reader.ReadByte();
                        
                        var clr = Color.FromRgb(pd.red, pd.green, pd.blue);
                        image2[y, x] = clr;
                    }
                }
                return image2;
        }
        return null;
    }

    public Image? TextureMap(ReadOnlyMemory<byte> buffer)
    {
        using var reader = buffer.CreateBinaryReaderUtf32(26);
        return TextureMap(reader, buffer);
    }

    public PixelFormat PixelFormat
    {
        get => this.GetFormat();
    }

    private  PixelFormat GetFormat()
    {
        return this.Depth switch
        {
            1 => PixelFormat.Undefined,
            4 => PixelFormat.Format32bppArgb,
            65536 => PixelFormat.Indexed,
            _ => PixelFormat.Format24bppRgb
        };
    }
}

