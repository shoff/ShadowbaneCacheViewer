namespace Arcane.Cache.Json.Render;

using System.Text.Json.Serialization;

public class RenderTemplate
{
    [JsonPropertyName("template_object_can_fade")]
    public bool TemplateObjectCanFade { get; set; }

    [JsonPropertyName("template_tracker")]
    public string TemplateTracker { get; set; } = string.Empty;

    [JsonPropertyName("template_illuminated")]
    public bool TemplateIlluminated { get; set; }

    [JsonPropertyName("template_bone_length")]
    public float TemplateBoneLength { get; set; }

    [JsonPropertyName("template_clip_map")]
    public int TemplateClipMap { get; set; }

    [JsonPropertyName("template_light_two_side")]
    public int TemplateLightTwoSide { get; set; }

    [JsonPropertyName("template_cull_face")]
    public int TemplateCullFace { get; set; }

    [JsonPropertyName("template_specular_map")]
    public int TemplateSpecularMap { get; set; }

    [JsonPropertyName("template_shininess")]
    public float TemplateShininess { get; set; }

    [JsonPropertyName("template_has_mesh")]
    public bool TemplateHasMesh { get; set; }

    [JsonPropertyName("template_mesh")]
    public TemplateMesh? TemplateMesh { get; set; }
}