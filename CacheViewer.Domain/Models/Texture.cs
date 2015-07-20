namespace CacheViewer.Domain.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using ArraySegments;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Factories;

    public class Texture
    {
        /// <summary>
        /// Creates the texture.
        /// </summary>
        /// <param name="data">The buffer.</param>
        /// <param name="id">The id.</param>
        /// ahah!!!!!!!!!! we need to get the texture with the option of JUST getting
        /// the data not setting up ilut!!!!!!!!!!!!!!!!!!!
        public Texture(ArraySegment<byte> data, int id)
        {
            this.TextureId = id;

            using (BinaryReader reader = data.CreateBinaryReader())
            {
                this.Width = reader.ReadInt32();
                this.Height = reader.ReadInt32();
                this.Depth = reader.ReadInt32();
            }
        }

        //private void BuildGlTexture(byte[] buffer, int id)
        //{
        //    this.InitDevIl();

        //    using (MemoryStream ms = new MemoryStream(buffer))
        //    {
        //        using (BinaryReader reader = new BinaryReader(ms))
        //        {
        //            // memcpy(&width, data, 4);
        //            // memcpy(&height, data + 4, 4);
        //            // memcpy(&bytes, data + 8, 4);
        //            this.width = reader.ReadInt32();
        //            this.height = reader.ReadInt32();
        //            this.bytes = reader.ReadInt32();

        //            // Get the image data
        //            // bufSize = (size - 26);
        //            // buffer = new unsigned char[ bufSize];
        //            // memcpy(buffer, data + 26, bufSize);

        //            int format;
        //            if (this.bytes == 1)
        //            {
        //                format = Gl.GL_LUMINANCE;
        //            }
        //            else if (this.bytes == 4)
        //            {
        //                format = Gl.GL_RGBA;
        //            }
        //            else
        //            {
        //                format = Gl.GL_RGB;
        //            }

        //            // Create the texture 
        //            Gl.glGenTextures(1, new[]
        //            {
        //                id
        //            });
        //            Gl.glBindTexture(Gl.GL_TEXTURE_2D, id);
        //            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, this.Width, this.Height, 0, format, Gl.GL_UNSIGNED_BYTE,
        //                buffer);
        //            //glTexImage2D(GL_TEXTURE_2D, 0, bytes, width, height, 0, format, GL_UNSIGNED_BYTE, buffer);
        //            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
        //            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
        //        }
        //    }
        //}

        /// <summary>
        ///   Gets or sets the texture id.
        /// </summary>
        /// <value>The texture id.</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TextureId { get; set; }

        /// <summary>
        ///   Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [NotMapped]
        public CacheIndex CacheIndexIdentity { get; set; }

        /// <summary>
        ///   Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }

        /// <summary>
        ///   Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }

        /// <summary>
        /// Gets the depth.
        /// </summary>
        /// <value>
        /// The depth.
        /// </value>
        public int Depth { get; set; }

        /// <summary>
        /// Textures the map.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public Bitmap TextureMap(ArraySegment<byte> buffer)
        {
            using (BinaryReader reader = buffer.CreateBinaryReader())
            {
                reader.BaseStream.Position += 26;
                PixelFormat format;
                if (this.Depth == 1)
                {
                    format = PixelFormat.Alpha; /* Gl.GL_LUMINANCE;*/
                }
                else
                {
                    format = this.Depth == 4 ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb;
                }

                var myBitmap = new Bitmap(this.Width, this.Height, format);

                Color clr;
                var pd = new PixelData();

                switch (format)
                {
                    case PixelFormat.Format24bppRgb:
                        for (int y = 0; y < myBitmap.Height; y++)
                        {
                            for (int x = 0; x < myBitmap.Width; x++)
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
                        for (int y = 0; y < myBitmap.Height; y++)
                        {
                            for (int x = 0; x < myBitmap.Width; x++)
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
                myBitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                return myBitmap;
            }
        }

        /// <summary>
        ///   Binds this instance.
        /// </summary>
        public void Bind()
        {
            //Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.TextureId);
        }

        /// <summary>
        ///   Un-Binds the texture.
        /// </summary>
        public void UnBind()
        {
            // Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
        }
    }
}