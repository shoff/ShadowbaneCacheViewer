namespace Shadowbane.Cache;

using System.Collections.Generic;
using System.Numerics;

public interface IMesh
{
    float MeshSize { get; }
    uint Identity { get; }
    uint VertexCount { get; set; }
    uint VertexBufferSize { get; set; }
    IList<ITexture> Textures { get; }
    ICollection<Vector3> Vertices { get; }
    ulong VerticesOffset { get; set; }
    ICollection<Vector3> Normals { get; }
    ICollection<Vector2> TextureVectors { get; }
    ulong IndicesOffset { get; set; }
    IList<Index> Indices { get; }
    ulong NormalsOffset { get; set; }
    uint NormalsCount { get; set; }
    uint NormalsBufferSize { get; set; }
    ulong TextureOffset { get; set; }
    uint TextureCoordinatesCount { get; set; }
    uint NumberOfIndices { get; set; }
    Vector3 Scale { get; set; }
    Vector3 Position { get; set; }
    Vector3[] Bounds { get; }
    void ApplyPosition();
    void ApplyScale();
    void SetBounds();
    void SetBounds(Vector3 min, Vector3 max);
    string GetMeshInformation();
}