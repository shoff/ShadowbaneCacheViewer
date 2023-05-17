namespace Arcane.Cache.Json.Mesh;

using System.Text.Json.Serialization;

public class Mesh
{
    [JsonIgnore]
    public uint MeshId { get; set; }

    [JsonPropertyName("mesh_name")]
    public string MeshName { get; set; } = string.Empty;

    [JsonPropertyName("mesh_distance")]
    public float MeshDistance { get; set; }

    [JsonPropertyName("mesh_start_point")]
    public float[] MeshStartPoint { get; set; } = Array.Empty<float>();

    [JsonPropertyName("mesh_end_point")]
    public float[] MeshEndPoint { get; set; } = Array.Empty<float>();

    [JsonPropertyName("mesh_ref_point")]
    public float[] MeshRefPoint { get; set; } = Array.Empty<float>();

    [JsonPropertyName("mesh_use_face_normals")]
    public bool MeshUseFaceNormals { get; set; }

    [JsonPropertyName("mesh_use_tangent_basis")]
    public bool MeshUseTangentBasis { get; set; }

    [JsonPropertyName("mesh_vertices")]
    public float[][] MeshVertices { get; set; } = Array.Empty<float[]>();

    [JsonPropertyName("mesh_normals")]
    public float[][] MeshNormals { get; set; } = Array.Empty<float[]>();

    [JsonPropertyName("mesh_uv")]
    public float[][] MeshUv { get; set; } = Array.Empty<float[]>();

    [JsonPropertyName("mesh_tangent_vertices")]
    public float[][] MeshTangentVertices { get; set; } = Array.Empty<float[]>();

    [JsonPropertyName("mesh_indices")]
    public int[] MeshIndices { get; set; } = Array.Empty<int>();

    [JsonPropertyName("mesh_extra_indices")]
    public object[] MeshExtraIndices { get; set; } = Array.Empty<object[]>();
}
