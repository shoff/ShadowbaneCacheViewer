using System.Collections.Generic;
using System.Linq;
using CacheViewer.Domain.Extensions;
using CacheViewer.Domain.Models;
using CacheViewer.Domain.Parsers;
using SlimDX;

namespace CacheViewer.Domain.Exporters
{
    public class MeshData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeshData"/> class.
        /// </summary>
        public MeshData()
        {
            this.Positions = new List<Vector3>();
            this.Normals = new List<Vector3>();
            this.TextureCoordinates = new List<Vector2>();
            this.Indices = new List<WavefrontVertex>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshData"/> class.
        /// </summary>
        /// <param name="mesh">The Mesh.</param>
        public MeshData(Mesh mesh)
        {
            this.Positions = mesh.Vertices.ToList();
            this.Normals = mesh.Normals.ToList();
            this.TextureCoordinates = mesh.TextureVectors.ToList();
            this.Indices = mesh.Indices;
        }

        /// <summary>
        /// Gets or sets the positions.
        /// </summary>
        /// <value>
        /// The positions.
        /// </value>
        public List<Vector3> Positions { get; set; }

        /// <summary>
        /// Gets or sets the normals.
        /// </summary>
        /// <value>
        /// The normals.
        /// </value>
        public List<Vector3> Normals { get; set; }

        /// <summary>
        /// Gets or sets the texture coordinates.
        /// </summary>
        /// <value>
        /// The texture coordinates.
        /// </value>
        public List<Vector2> TextureCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the indices.
        /// </summary>
        /// <value>
        /// The indices.
        /// </value>
        public List<WavefrontVertex> Indices { get; set; }
    }
}