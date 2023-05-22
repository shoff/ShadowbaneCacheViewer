namespace Arcane.Data.Mongo.Entities;

using MongoDB.Bson.Serialization.Attributes;

public class RenderTextureSetEntity
{
    [BsonId]
    public uint TextureId { get; set; }
    public string TextureType { get; set; } = string.Empty;
    public string TextureTransparent { get; set; } = string.Empty;
    public bool TextureCompress { get; set; }
    public bool TextureNormalMap { get; set; }
    public bool TextureDetailNormalMap { get; set; }
    public bool TextureCreateMipMaps { get; set; }
    public bool TextureWrap { get; set; }

    [BsonRequired]
    public string TexturePath { get; set; } = string.Empty;
}