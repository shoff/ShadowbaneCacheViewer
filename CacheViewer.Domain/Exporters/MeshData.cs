namespace CacheViewer.Domain.Exporters
{
    using System.Collections.Generic;
    using System.Linq;
    using Geometry;
    using Models;
    using Parsers;

    public class MeshData
    {
        public MeshData() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MeshData" /> class.
        /// </summary>
        /// <param name="mesh">The Mesh.</param>
        public MeshData(Mesh mesh)
        {
            this.Positions = mesh.Vertices.ToList();
            this.Normals = mesh.Normals.ToList();
            this.TextureCoordinates = mesh.TextureVectors.ToList();
            this.Indices = mesh.Indices;
        }

        public List<Vector3> Positions { get; set; } = new List<Vector3>();
        public List<Vector3> Normals { get; set; } = new List<Vector3>();
        public List<Vector2> TextureCoordinates { get; set; } = new List<Vector2>();
        public List<Index> Indices { get; set; } = new List<Index>();
    }
}