namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class RuneBodyParts
{
    [JsonPropertyName("body_part_render")]
    public uint BodyPartRender { get; set; }
    
    [JsonPropertyName("body_part_position")]
    public float[] BodyPartPosition { get; set; } = Array.Empty<float>();
}