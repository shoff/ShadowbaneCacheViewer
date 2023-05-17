namespace Arcane.Cache.Json.Render;

using System.Text.Json.Serialization;

public class TextureData
{
    [JsonPropertyName("texture_id")]
    public uint TextureId { get; set; }

    [JsonPropertyName("texture_transparent")]
    public string TextureTransparent { get; set; } = string.Empty;

    [JsonPropertyName("texture_compress")]
    public bool TextureCompress { get; set; }

    [JsonPropertyName("texture_normal_map")]
    public bool TextureNormalMap { get; set; }

    [JsonPropertyName("texture_detail_normal_map")]
    public bool TextureDetailNormalMap { get; set; }

    [JsonPropertyName("texture_create_mip_maps")]
    public bool TextureCreateMipMaps { get; set; }

    [JsonPropertyName("texture_wrap")]
    public bool TextureWrap { get; set; }
}