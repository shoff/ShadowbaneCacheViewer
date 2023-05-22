namespace Arcane.Data.Mongo.Entities;

public class RenderTemplateEntity
{
    public bool TemplateObjectCanFade { get; set; }
    public string TemplateTracker { get; set; } = string.Empty;
    public bool TemplateIlluminated { get; set; }
    public float? TemplateBoneLength { get; set; }
    public int? TemplateClipMap { get; set; }
    public int? TemplateLightTwoSide { get; set; }
    public int? TemplateCullFace { get; set; }
    public int? TemplateSpecularMap { get; set; }
    public float? TemplateShininess { get; set; }
    public bool TemplateHasMesh { get; set; }
    public ICollection<MeshEntity> MeshSet { get; set; } = new List<MeshEntity>();
}