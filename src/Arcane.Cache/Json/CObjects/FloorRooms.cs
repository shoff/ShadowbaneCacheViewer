namespace Arcane.Cache.Json.CObjects;

public class FloorRooms
{
    public float[][] region_points { get; set; } = Array.Empty<float[]>();
    public float[] region_render_scale { get; set; } = Array.Empty<float>();
    public string region_content_behavior { get; set; } = string.Empty;
    public string region_state { get; set; } = string.Empty;
    public bool region_render_flipped { get; set; }
    public bool region_has_stairs { get; set; }
    public int region_unknown2 { get; set; }
    public int region_unknown3 { get; set; }
    public float[] region_unknown4 { get; set; } = Array.Empty<float>();
}