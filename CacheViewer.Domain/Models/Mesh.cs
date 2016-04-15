namespace CacheViewer.Domain.Models
{
    using System.Collections.Generic;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Parsers;
    using SlimDX;

    /// <summary>
    /// </summary>
    public class Mesh
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mesh"/> class.
        /// </summary>
        public Mesh()
        {
            this.Textures = new List<Texture>();
            this.Indices = new List<WavefrontVertex>();
            this.Normals = new List<Vector3>();
            this.Vertices = new List<Vector3>();
            this.TextureVectors = new List<Vector2>();
        }

        /// <summary>
        /// Gets or sets the index of the cache.
        /// </summary>
        /// <value>
        /// The index of the cache.
        /// </value>
        public CacheIndex CacheIndex { get; set; }

        /// <summary>
        /// Gets or sets the vertex count.
        /// </summary>
        /// <value>
        /// The vertex count.
        /// </value>
        public uint VertexCount { get; set; }

        /// <summary>
        /// Gets or sets the size of the vertex buffer.
        /// </summary>
        /// <value>
        /// The size of the vertex buffer.
        /// </value>
        public uint VertexBufferSize { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public MeshHeader Header { get; set; }

        /// <summary>
        /// Gets or sets the textures.
        /// </summary>
        /// <value>
        /// The textures.
        /// </value>
        public List<Texture> Textures { get; set; }

        /// <summary>
        /// Gets or sets the vertices.
        /// </summary>
        public List<Vector3> Vertices { get; set; }

        /// <summary>
        /// Gets or sets the normals.
        /// </summary>
        public List<Vector3> Normals { get; set; }

        /// <summary>
        /// Gets or sets the texture vectors.
        /// </summary>
        public List<Vector2> TextureVectors { get; set; }

        /// <summary>
        /// Gets or sets the indices.
        /// </summary>
        public List<WavefrontVertex> Indices { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the normals count.
        /// </summary>
        public uint NormalsCount { get; set; }

        /// <summary>
        /// Gets or sets the size of the normals buffer.
        /// </summary>
        public uint NormalsBufferSize { get; set; }

        /// <summary>
        /// Gets or sets the texture coordinates count.
        /// </summary>
        public uint TextureCoordinatesCount { get; set; }

        /// <summary>
        /// Gets or sets the unknown data.
        /// </summary>
        public byte[] UnknownData { get; set; }

        /// <summary>
        /// Gets or sets the offset to unknown data.
        /// </summary>
        public long OffsetToUnknownData { get; set; }

        /// <summary>
        /// Gets or sets the number of indices.
        /// </summary>
        public uint NumberOfIndices { get; set; }
    }
}