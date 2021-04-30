namespace Shadowbane.Cache.Loader.Models
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public class Texture
    {
        // we need a texture loader that reads the texture data and sends it here rather than 
        // having to open an new reader for every texture.
        public Texture(ReadOnlyMemory<byte> data, int id)
        {
            this.TextureId = id;

            using var reader = data.CreateBinaryReaderUtf8();
            this.Width = reader.ReadInt32();
            this.Height = reader.ReadInt32();
            this.Depth = reader.ReadInt32();
            this.Image = this.TextureMap(reader);
        }

        public Bitmap Image { get; }
        public int TextureId { get; }
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }

        public PixelFormat PixelFormat { get; }

        private Bitmap TextureMap(BinaryReader reader)
        {

            reader.BaseStream.Position += 26;
            PixelFormat format;

            //if (this.Depth == 1)
            //{
            //    format = PixelFormat.Alpha; /* Gl.GL_LUMINANCE;*/
            //    // for now just return as this is not valid for GDI bitmaps :(
            //    return null;
            //}

            format = this.Depth == 4 ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb;
            Bitmap myBitmap = null;
            try
            {
                myBitmap = new Bitmap(this.Width, this.Height, format);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Color clr;
            var pd = new PixelData();

            switch (format)
            {
                case PixelFormat.Format24bppRgb:
                    for (var y = 0; y < myBitmap?.Height; y++)
                    {
                        for (var x = 0; x < myBitmap.Width; x++)
                        {
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