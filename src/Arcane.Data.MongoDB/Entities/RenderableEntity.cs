namespace Arcane.Data.Mongo.Entities;

using MongoDB.Bson.Serialization.Attributes;

public class RenderableEntity
{
    [BsonId]
    public uint RederableId { get; set; }
    public RenderTemplateEntity? RenderTemplate { get; set; }
    public string RenderTargetBone { get; set; } = string.Empty;
    public float[] RenderScale { get; set; } = Array.Empty<float>();
    public int RenderHasLoc { get; set; }
    public float[] RenderLoc { get; set; } = Array.Empty<float>();
    public RenderableEntity[] RenderChildren { get; set; } = Array.Empty<RenderableEntity>();
    public bool RenderHasTextureSet { get; set; }
    public RenderTextureSetEntity[] RenderTextureSets { get; set; } = Array.Empty<RenderTextureSetEntity>();
    public bool RenderCollides { get; set; }
    public bool RenderCalculateBoundingBox { get; set; }
    public bool RenderNationCrest { get; set; }
    public bool RenderGuildCrest { get; set; }
    public bool RenderBumped { get; set; }
    public bool RenderVpActive { get; set; }
    public bool RenderHasLightEffects { get; set; }
}