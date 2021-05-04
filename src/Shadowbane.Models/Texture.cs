namespace Shadowbane.Models
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using Cache;

    public class Texture
    {
        public Texture(ReadOnlyMemory<byte> data, int id)
        {
            this.TextureId = id;

            using (var reader = data.CreateBinaryReaderUtf32(0))
            {
                this.Width = reader.ReadInt32();
                this.Height = reader.ReadInt32();
                this.Depth = reader.ReadInt32();
            }

            this.Image = this.TextureMap(data);
        }

        public Bitmap Image { get; }

        public int TextureId { get; set; }

        public CacheIndex CacheIndexIdentity { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }

        public PixelFormat PixelFormat => PixelFormat.DontCare;

        public Bitmap TextureMap(ReadOnlyMemory<byte> buffer)
        {
            using var reader = buffer.CreateBinaryReaderUtf32(26);
            //reader.BaseStream.Position += 26;
            PixelFormat format;
            if (this.Depth == 1)
            {
                format = PixelFormat.Alpha; /* Gl.GL_LUMINANCE;*/
                // for now just return as this is not valid for GDI bitmaps :(
                return null;
            }

            format = this.Depth == 4 ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb;
            var myBitmap = new Bitmap(this.Width, this.Height, format);
                
            Color clr;
            var pd = new PixelData();

            switch (format)
            {
                case PixelFormat.Format24bppRgb:
                    for (var y = 0; y < myBitmap?.Height; y++)
                    {
                        for (var x = 0; x < myBitmap.Width; x++)
                        {
                            // pd.blue = reader.ReadByte();
                            // pd.green = reader.ReadByte();
                            // pd.red = reader.ReadByte();
                            pd.red = reader.ReadByte();
                            pd.green = reader.ReadByte();
                            pd.blue = reader.ReadByte();
                            clr = Color.FromArgb(pd.red, pd.green, pd.blue);
                            myBitmap.SetPixel(x, y, clr);
                        }
                    }

                    break;
                case PixelFormat.Format32bppArgb:
                    for (var y = 0; y < myBitmap?.Height; y++)
                    {
                        for (var x = 0; x < myBitmap.Width; x++)
                        {
                            // pd.alpha = reader.ReadByte();
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

            myBitmap?.RotateFlip(RotateFlipType.Rotate180FlipX);
            return myBitmap;
        }
    }
}