namespace Shadowbane.Models
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using Cache;
    using Microsoft.Toolkit.HighPerformance;

    public class Texture
    {
        public Texture(ReadOnlyMemory<byte> data, uint id)
        {
            this.TextureId = id;
            using var reader = new BinaryReader(data.AsStream());
            this.Width = reader.ReadInt32();
            this.Height = reader.ReadInt32();
            this.Depth = reader.ReadInt32();
            this.PixelFormat = this.Depth switch
            {
                1 => PixelFormat.Undefined,
                4 => PixelFormat.Format32bppArgb,
                65536 => PixelFormat.Indexed,
                _ => PixelFormat.Format24bppRgb
            };
            reader.BaseStream.Position += 14;
            this.Image = this.TextureMap(reader, data);
        }

        public Bitmap Image { get; }

        public uint TextureId { get; }

        public int Width { get; }

        public int Height { get; }

        public int Depth { get; }

        public PixelFormat PixelFormat { get; }

        public Bitmap TextureMap(BinaryReader reader, ReadOnlyMemory<byte> buffer)
        {
            if (this.PixelFormat == PixelFormat.Undefined || this.Width < 1 || this.Height < 1)
            {
                //format = PixelFormat.Alpha; /* Gl.GL_LUMINANCE;*/
                // for now just return as this is not valid for GDI bitmaps :(
                return null;
            }

            Bitmap myBitmap;
            if (this.PixelFormat == PixelFormat.Indexed)
            {
                // Ok let's figure this bullshize out, 
                // NFI yet how to read these, not my top priority.
                return null;
            }

            try
            {
                myBitmap = new Bitmap(this.Width, this.Height, this.PixelFormat);
            }
            catch (Exception e)
            {
                return null;
            }

            Color clr;
            var pd = new PixelData();
            switch (this.PixelFormat)
            {
                // NFI yet how to read these, not my top priority.
                case PixelFormat.Indexed:
                    for (var y = 0; y < myBitmap.Height; y++)
                    {
                        for (var x = 0; x < myBitmap.Width; x++)
                        {
                            // Clearly this is wrong
                            var colorByte = reader.ReadByte();
                            KnownColor kc;
                            try
                            {
                                kc = (KnownColor)(int)colorByte;
                            }
                            catch
                            {
                                kc = KnownColor.Black;
                            }

                            clr = Color.FromKnownColor(kc);
                            myBitmap.SetPixel(x, y, clr);
                        }
                    }

                    break;

                case PixelFormat.Format24bppRgb:

                    var remainingBytes = reader.BaseStream.Length - reader.BaseStream.Position;
                    var bytesNeeded = myBitmap.Height * (myBitmap.Width * 3);
                    if (remainingBytes < bytesNeeded)
                    {
                        // grrr why?
                        return null;
                    }

                    for (var y = 0; y < myBitmap.Height; y++)
                    {
                        for (var x = 0; x < myBitmap.Width; x++)
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

                            clr = Color.FromArgb(pd.red, pd.green, pd.blue);
                            myBitmap.SetPixel(x, y, clr);
                        }
                    }

                    break;
                case PixelFormat.Format32bppArgb:
                    var remainingBytes1 = reader.BaseStream.Length - reader.BaseStream.Position;
                    var bytesNeeded1 = myBitmap.Height * (myBitmap.Width * 4);
                    if (remainingBytes1 < bytesNeeded1)
                    {
                        // grrr why?
                        return null;
                    }

                    for (var y = 0; y < myBitmap?.Height; y++)
                    {
                        for (var x = 0; x < myBitmap.Width; x++)
                        {
                            pd.red = reader.ReadByte();
                            pd.green = reader.ReadByte();
                            pd.blue = reader.ReadByte();
                            pd.alpha = reader.ReadByte();
                            clr = Color.FromArgb(pd.alpha, pd.red, pd.green, pd.blue);
                            myBitmap.SetPixel(x, y, clr);
                        }
                    }

                    break;
            }
            myBitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
            return myBitmap;
        }

        public Bitmap TextureMap(ReadOnlyMemory<byte> buffer)
        {
            using var reader = buffer.CreateBinaryReaderUtf32(26);
            return TextureMap(reader, buffer);
        }
    }
}