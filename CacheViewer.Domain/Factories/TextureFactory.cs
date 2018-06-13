namespace CacheViewer.Domain.Factories
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using Archive;
    using Models;
    using Nito.ArraySegments;

    /// <summary>
    /// </summary>
    public class TextureFactory
    {
        private readonly Textures textureArchive;

        /// <summary>Initializes a new instance of the <see cref="TextureFactory" /> class.</summary>
        private TextureFactory()
        {
            this.textureArchive = (Textures) ArchiveFactory.Instance.Build(CacheFile.Textures);
        }

        /// <summary>Gets the indexes.</summary>
        public CacheIndex[] Indexes => this.textureArchive.CacheIndices.ToArray();

        /// <summary>Gets the instance.</summary>
        public static TextureFactory Instance { get; } = new TextureFactory();

        /// <summary>
        ///     Gets the identity array.
        /// </summary>
        /// <value>
        ///     The identity array.
        /// </value>
        public int[] IdentityArray => this.textureArchive.IdentityArray;

        /// <summary>Builds the specified identity.</summary>
        /// <param name="identity">
        ///     The identity.
        /// </param>
        /// <param name="storeBuffer">
        ///     if set to <c>true</c> [store buffer].
        /// </param>
        /// <returns>
        /// </returns>
        public Texture Build(int identity, bool storeBuffer = true)
        {
            return new Texture(this.textureArchive[identity].Item1, identity);
        }

        /// <summary>Gets the by identifier.</summary>
        /// <param name="id">
        ///     The identifier.
        /// </param>
        /// <returns>
        /// </returns>
        public CacheAsset GetById(int id)
        {
            return this.textureArchive[id];
        }

        /// <summary>Sets the cache use.</summary>
        /// <param name="useCache">
        ///     if set to <c>true</c> [use cache].
        /// </param>
        public void SetCacheUse(bool useCache)
        {
            this.textureArchive.UseCache = useCache;
        }

        /// <summary>Textures the map.</summary>
        /// <param name="identity">
        ///     The identity.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="NotSupportedException">The stream does not support seeking. </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public Bitmap TextureMap(int identity)
        {
            var buffer = this.textureArchive[identity].Item1;

            using (var reader = buffer.CreateBinaryReader())
            {
                var width = reader.ReadInt32();
                var height = reader.ReadInt32();
                var depth = reader.ReadInt32();
                reader.BaseStream.Position += 14;

                PixelFormat format;
                if (depth == 1)
                {
                    format = PixelFormat.Alpha; /* Gl.GL_LUMINANCE;*/
                    return null;
                }

                format = depth == 4 ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb;

                var myBitmap = new Bitmap(width, height, format);

                Color clr;
                var pd = new PixelData();

                switch (format)
                {
                    case PixelFormat.Format24bppRgb:
                        for (var y = 0; y < myBitmap.Height; y++)
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
                        for (var y = 0; y < myBitmap.Height; y++)
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

                myBitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                return myBitmap;
            }
        }
    }
}