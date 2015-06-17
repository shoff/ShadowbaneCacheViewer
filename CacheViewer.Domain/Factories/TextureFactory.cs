
namespace CacheViewer.Domain.Factories
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using ArraySegments;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Models;

    /// <summary>
    /// </summary>
    public class TextureFactory
    {
        private static readonly TextureFactory instance = new TextureFactory();
        private readonly Textures textureArchive;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureFactory"/> class.
        /// </summary>
        private TextureFactory()
        {
            this.textureArchive = (Textures)ArchiveFactory.Instance.Build(CacheFile.Textures);
        }

        /// <summary>
        /// Builds the specified identity.
        /// </summary>
        /// <param name="identity">
        /// The identity.
        /// </param>
        /// <param name="storeBuffer">
        /// if set to <c>true</c> [store buffer].
        /// </param>
        /// <returns>
        /// </returns>
        public Texture Build(int identity, bool storeBuffer = true)
        {
            return new Texture(this.textureArchive[identity].Item1, identity);
        }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        /// <value>
        /// The indexes.
        /// </value>
        public CacheIndex[] Indexes
        {
            get { return this.textureArchive.CacheIndices.ToArray(); }
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">
        /// The identifier.
        /// </param>
        /// <returns>
        /// </returns>
        public CacheAsset GetById(int id)
        {
            return this.textureArchive[id];
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static TextureFactory Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Sets the cache use.
        /// </summary>
        /// <param name="useCache">
        /// if set to <c>true</c> [use cache].
        /// </param>
        public void SetCacheUse(bool useCache)
        {
            this.textureArchive.UseCache = useCache;
        }
        

        /// <summary>
        /// Textures the map.
        /// </summary>
        /// <param name="identity">
        /// The identity.
        /// </param>
        /// <returns>
        /// </returns>
        public Bitmap TextureMap(int identity)
        {
            ArraySegment<byte> buffer = this.textureArchive[identity].Item1;
            using (BinaryReader reader = buffer.CreateBinaryReader())
            {
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();
                int depth = reader.ReadInt32();
                reader.BaseStream.Position += 14;

                PixelFormat format;
                if (depth == 1)
                {
                    format = PixelFormat.Alpha; /* Gl.GL_LUMINANCE;*/
                    return null;
                }

                if (depth == 4)
                {
                    format = PixelFormat.Format32bppArgb; /* Gl.GL_RGBA;*/
                }
                else
                {
                    format = PixelFormat.Format24bppRgb; /*Gl.GL_RGB;*/
                }

                var myBitmap = new Bitmap(width, height, format);

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
        /// Gets the identity array.
        /// </summary>
        /// <value>
        /// The identity array.
        /// </value>
        public int[] IdentityArray
        {
            get { return this.textureArchive.IdentityArray; }
        }
    }
}