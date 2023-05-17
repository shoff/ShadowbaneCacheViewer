namespace Arcane.Cache.Json.Skeleton;

using System.Text.Json.Serialization;

public class Skeleton
{
    [JsonPropertyName("skeleton_name")]
    public string SkeletonName { get; set; } = string.Empty;

    [JsonPropertyName("skeleton_motion")]
    public int[][] SkeletonMotion { get; set; } = Array.Empty<int[]>();

    [JsonPropertyName("skeleton_root")]
    public SkeletonRoot? SkeletonRoot { get; set; }
}