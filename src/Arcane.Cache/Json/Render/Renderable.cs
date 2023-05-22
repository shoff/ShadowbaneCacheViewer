namespace Arcane.Cache.Json.Render;

using System.Text.Json.Serialization;

public class Renderable
{

    [JsonIgnore]
    public uint RederableId { get; set; }

    [JsonPropertyName("render_template")]
    public RenderTemplate? RenderTemplate { get; set; }

    [JsonPropertyName("render_texture_set")]
    public RenderTextureSet[] RenderTextureSets { get; set; } = Array.Empty<RenderTextureSet>();

    [JsonPropertyName("render_target_bone")]
    public string RenderTargetBone { get; set; } = string.Empty;

    [JsonPropertyName("render_scale")]
    public float[] RenderScale { get; set; } = Array.Empty<float>();

    [JsonPropertyName("render_has_loc")]
    public int RenderHasLoc { get; set; }

    [JsonPropertyName("render_loc")]
    public float[] RenderLoc { get; set; } = Array.Empty<float>();

    [JsonPropertyName("render_children")]
    public uint[] RenderChildren { get; set; } = Array.Empty<uint>();

    [JsonPropertyName("render_has_texture_set")]
    public bool RenderHasTextureSet { get; set; }

    [JsonPropertyName("render_collides")]
    public bool RenderCollides { get; set; }

    [JsonPropertyName("render_calculate_bounding_box")]
    public bool RenderCalculateBoundingBox { get; set; }

    [JsonPropertyName("render_nation_crest")]
    public bool RenderNationCrest { get; set; }

    [JsonPropertyName("render_guild_crest")]
    public bool RenderGuildCrest { get; set; }

    [JsonPropertyName("render_bumped")]
    public bool RenderBumped { get; set; }

    [JsonPropertyName("render_vp_active")]
    public bool RenderVpActive { get; set; }

    [JsonPropertyName("render_has_light_effects")]
    public bool RenderHasLightEffects { get; set; }

}