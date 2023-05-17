namespace Arcane.Cache.Json.Render;

using System.Text.Json.Serialization;

public class RenderTextureSet
{
    [JsonPropertyName("texture_type")]
    public string TextureType { get; set; } = string.Empty;

    [JsonPropertyName("texture_data")]
    public TextureData? TextureData { get; set; }
}