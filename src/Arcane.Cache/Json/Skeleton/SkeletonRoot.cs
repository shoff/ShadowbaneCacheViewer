namespace Arcane.Cache.Json.Skeleton;

using System.Text.Json.Serialization;

public class SkeletonRoot
{
    [JsonPropertyName("bone_id")]
    public uint BoneId { get; set; }

    [JsonPropertyName("bone_name")]
    public string BoneName { get; set; } = string.Empty;

    [JsonPropertyName("bone_direction")]
    public float[] BoneDirection { get; set; } = Array.Empty<float>();

    [JsonPropertyName("bone_length")]
    public float BoneLength { get; set; }

    [JsonPropertyName("bone_axis")]
    public float[] BoneAxis { get; set; } = Array.Empty<float>();

    [JsonPropertyName("bone_dof")]
    public string BoneDof { get; set; } = string.Empty;

    [JsonPropertyName("bone_order")]
    public float[] BoneOrder { get; set; } = Array.Empty<float>();

    [JsonPropertyName("bone_position")]
    public float[] BonePosition { get; set; } = Array.Empty<float>();

    [JsonPropertyName("bone_orientation")]
    public float[] BoneOrientation { get; set; } = Array.Empty<float>();

    [JsonPropertyName("bone_u0")]
    public bool BoneU0 { get; set; }

    [JsonPropertyName("bone_u1")]
    public bool BoneU1 { get; set; }

    [JsonPropertyName("bone_hierarchy")]
    public BoneHierarchy[] BoneHierarchyImpl { get; set; } = Array.Empty<BoneHierarchy>();
}