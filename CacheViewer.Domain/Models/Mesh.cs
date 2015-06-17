using System.Collections.Generic;
using CacheViewer.Domain.Archive;
using CacheViewer.Domain.Parsers;
using SlimDX;


namespace CacheViewer.Domain.Models
{
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
        /// <value>
        /// The vertices.
        /// </value>
        public Vector3[] Vertices { get; set; }

        /// <summary>
        /// Gets or sets the normals.
        /// </summary>
        /// <value>
        /// The normals.
        /// </value>
        public Vector3[] Normals { get; set; }

        /// <summary>
        /// Gets or sets the texture vectors.
        /// </summary>
        /// <value>
        /// The texture vectors.
        /// </value>
        public Vector2[] TextureVectors { get; set; }

        /// <summary>
        /// Gets or sets the indices.
        /// </summary>
        /// <value>
        /// The indices.
        /// </value>
        public List<WavefrontVertex> Indices { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the normals count.
        /// </summary>
        /// <value>
        /// The normals count.
        /// </value>
        public uint NormalsCount { get; set; }

        /// <summary>
        /// Gets or sets the size of the normals buffer.
        /// </summary>
        /// <value>
        /// The size of the normals buffer.
        /// </value>
        public uint NormalsBufferSize { get; set; }

        /// <summary>
        /// Gets or sets the texture coordinates count.
        /// </summary>
        /// <value>
        /// The texture coordinates count.
        /// </value>
        public uint TextureCoordinatesCount { get; set; }

        /// <summary>
        /// Gets or sets the unknown data.
        /// </summary>
        /// <value>
        /// The unknown data.
        /// </value>
        public byte[] UnknownData { get; set; }

        /// <summary>
        /// Gets or sets the offset to unknown data.
        /// </summary>
        /// <value>
        /// The offset to unknown data.
        /// </value>
        public long OffsetToUnknownData { get; set; }

        /// <summary>
        /// Gets or sets the number of indices.
        /// </summary>
        /// <value>
        /// The number of indices.
        /// </value>
        public uint NumberOfIndices { get; set; }
    }
}