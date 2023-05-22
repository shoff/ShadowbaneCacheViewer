namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class RuneBodyParts
{
    [JsonPropertyName("body_part_render")]
    public uint BodyPartRender { get; set; }
    
    [JsonPropertyName("body_part_position")]
    public float[] BodyPartPosition { get; set; } = Array.Empty<float>();

    // on humanoid BodyPartPosition = :
    // [0] = LOWERBACK
    // [1] = UPPERBACK  
    // [2] = RSHOULDERJOINT
    // [3] = RHUMERUS
    // [4] = RRADIUS
    // [5] = RWRIST
    // [6] = RHAND
    // [7] = RFINGERS
    // [8] = RTHUMB
    // [9] = NECKJOINT
    // [10] = NECK
    // [11] = HEAD
    // [12] = RFEMUR
    // [13] = RTIBIA
    // [14] = RFOOT
    // [15] = RTOES

}