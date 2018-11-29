namespace CacheViewer.Domain.Models
{
    using System.Collections.Generic;
    using System.Text;
    using Archive;
    using OpenTK;
    using Parsers;

    /// <summary>
    /// </summary>
    public class Mesh
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Mesh" /> class.
        /// </summary>
        public Mesh()
        {
            this.Textures = new List<Texture>();
            this.Indices = new List<WavefrontVertex>();
            this.Normals = new List<Vector3>();
            this.Vertices = new List<Vector3>();
            this.TextureVectors = new List<Vector2>();
        }

        public CacheIndex CacheIndex { get; set; }
        public uint VertexCount { get; set; }
        public uint VertexBufferSize { get; set; }
        public MeshHeader Header { get; set; }
        public List<Texture> Textures { get; set; }
        public List<Vector3> Vertices { get; set; }

        public ulong VerticesOffset { get; set; }

        public List<Vector3> Normals { get; set; }
        public List<Vector2> TextureVectors { get; set; }

        public ulong IndicesOffset { get; set; }
        public List<WavefrontVertex> Indices { get; set; }
        public int Id { get; set; }

        public ulong NormalsOffset { get; set; }
        public uint NormalsCount { get; set; }
        public uint NormalsBufferSize { get; set; }
        public ulong TextureOffset { get; set; }
        public uint TextureCoordinatesCount { get; set; }
        public byte[] UnknownData { get; set; }
        public long OffsetToUnknownData { get; set; }
        public uint NumberOfIndices { get; set; }

        public string GetMeshInformation()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.CacheIndex.ToString());
            sb.Append($"Vertex count {this.VertexCount} - Vertex offset {this.VerticesOffset}\r\n");
            sb.Append($"Normals count {this.NormalsCount} - Normals offset {this.NormalsOffset}\r\n");
            sb.Append($"Indices count {this.Indices.Count} - Indices offset {this.IndicesOffset}\r\n");
            sb.Append($"Texture count {this.TextureCoordinatesCount} - Texture offset {this.TextureOffset}");
            return sb.ToString();
        }
    }
}