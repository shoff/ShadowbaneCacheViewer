namespace CacheViewer.Domain.Parsers
{
    using System.Diagnostics.Contracts;

    /// <summary>
    ///     A struct representing an Wavefront OBJ vertex. 
    /// </summary>
    /// <remarks>
    /// OBJ vertices are indexed vertices so instead of vectors 
    /// it has an index for the position, texture coordinate and normal.
    /// Each of those indices points to a location in a list of vectors.
    /// </remarks>
    public class WavefrontVertex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WavefrontVertex"/> struct.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="textureCoordinate">The TextureCoordinate.</param>
        /// <param name="normal">The normal.</param>
        public WavefrontVertex(int position, int textureCoordinate, int normal)
        {
            this.Position = position;
            this.TextureCoordinate = textureCoordinate;
            this.Normal = normal;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the normal.
        /// </summary>
        /// <value>
        /// The normal.
        /// </value>
        public int Normal { get; set; }

        /// <summary>
        /// Gets or sets the TextureCoordinate.
        /// </summary>
        /// <value>
        /// The TextureCoordinate.
        /// </value>
        public int TextureCoordinate { get; set; }
    }
}