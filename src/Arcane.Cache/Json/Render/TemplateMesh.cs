namespace Arcane.Cache.Json.Render;

using System.Text.Json.Serialization;

public class TemplateMesh
{
    [JsonPropertyName("mesh_set")]
    public MeshSet[] MeshSet { get; set; } = Array.Empty<MeshSet>();
}