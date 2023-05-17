namespace Arcane.Cache.Json.Render;

using System.Text.Json.Serialization;

public class MeshSet
{
    [JsonPropertyName("polymesh_id")]
    public uint PolymeshId { get; set; }

    [JsonPropertyName("polymesh_decal")]
    public bool PolymeshDecal { get; set; }

    [JsonPropertyName("polymesh_double_sided")]
    public bool PolymeshDoubleSided { get; set; }
}