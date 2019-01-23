namespace CacheViewer.Domain.Models
{
    using System.Collections.Generic;
    using System.Text;
    using Archive;
    using OpenTK;
    using Parsers;

    public class Mesh
    {
        public CacheIndex CacheIndex { get; set; }
        public uint VertexCount { get; set; }
        public uint VertexBufferSize { get; set; }
        public MeshHeader Header { get; set; }
        public List<Texture> Textures { get; set; } = new List<Texture>();
        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public ulong VerticesOffset { get; set; }
        public List<Vector3> Normals { get; set; } = new List<Vector3>();
        public List<Vector2> TextureVectors { get; set; } = new List<Vector2>();
        public ulong IndicesOffset { get; set; }
        public List<WavefrontVertex> Indices { get; set; } = new List<WavefrontVertex>();
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