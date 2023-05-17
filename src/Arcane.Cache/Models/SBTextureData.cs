namespace Arcane.Cache.Models;

public class SBTextureData
{
    public uint TextureId { get; set; }
    public string TextureTransparent { get; set; } = string.Empty;
    public bool TextureCompress { get; set; }
    public bool TextureNormalMap { get; set; }
    public bool TextureDetailNormalMap { get; set; }
    public bool TextureCreateMipMaps { get; set; }
    public bool TextureWrap { get; set; }
    public string TexturePath { get; set; } = string.Empty;
}