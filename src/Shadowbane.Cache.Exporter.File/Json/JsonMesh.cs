namespace Shadowbane.Cache.Exporter.File.Json;

using System.Collections.Generic;
using Models;

public class JsonMesh
{
    public uint Identity { get; set; }
    public uint VertexCount { get; set; }
    public uint VertexBufferSize { get; set; }
    public MeshHeader Header { get; set; }
    public IList<JsonTexture> Textures { get; set; } = new List<JsonTexture>();
    public ICollection<JsonVector3> Vertices { get; set; } = new List<JsonVector3>();
    public ulong VerticesOffset { get; set; }
    public ICollection<JsonVector3> Normals { get; set; } = new List<JsonVector3>();
    public ICollection<JsonVector2> TextureVectors { get; set; } = new List<JsonVector2>();
    public ulong IndicesOffset { get; set; }
    public IList<JsonIndex> Indices { get; set; } = new List<JsonIndex>();
    public ulong NormalsOffset { get; set; }
    public uint NormalsCount { get; set; }
    public uint NormalsBufferSize { get; set; }
    public ulong TextureOffset { get; set; }
    public uint TextureCoordinatesCount { get; set; }
    public byte[]? UnknownData { get; set; }
    public long OffsetToUnknownData { get; set; }
    public int NumberOfIndices { get; set; }
    public JsonVector3 Scale { get; set; }
    public JsonVector3 Position { get; set; }
    public JsonVector3[] Bounds { get; set; }
    public float MeshSize { get; set; }
}