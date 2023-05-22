namespace Arcane.Cache.Json.Motion;

using System.Text.Json.Serialization;

public class Motion
{
    [JsonPropertyName("motion_file")]
    public string MotionFile { get; set; } = string.Empty;

    [JsonPropertyName("motion_smoothed_count")]
    public int MotionSmoothedCount { get; set; }

    [JsonPropertyName("motion_smoothed_value")]
    public int MotionSmoothedValue { get; set; }

    [JsonPropertyName("motion_smoothed_factor")]
    public float MotionSmoothedFactor { get; set; }

    [JsonPropertyName("motion_sound")]
    public uint MotionSound { get; set; }

    [JsonPropertyName("motion_sheath")]
    public int MotionSheath { get; set; }

    [JsonPropertyName("motion_reset_loc")]
    public bool MotionResetLoc { get; set; }

    [JsonPropertyName("motion_leave_ground")]
    public bool MotionLeaveGround { get; set; }

    [JsonPropertyName("motion_force")]
    public float MotionForce { get; set; }

    [JsonPropertyName("motion_disable_blend")]
    public bool MotionDisableBlend { get; set; }

    [JsonPropertyName("motion_parts")]
    public List<string> MotionParts { get; set; } = new ();

    [JsonPropertyName("motion_smoothing")]
    public List<List<float>> MotionSmoothing { get; set; } = new();

    [JsonPropertyName("motion_target_frames")]
    public int[] MotionTargetFrames { get; set; } = Array.Empty<int>();
}
