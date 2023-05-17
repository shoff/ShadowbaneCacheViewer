namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class Structure
{
    [JsonPropertyName("obj_name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("obj_pickable")]
    public bool Pickable { get; set; }

    [JsonPropertyName("obj_gravity")]
    public float Gravity { get; set; }

    [JsonPropertyName("obj_cull_distance")]
    public float CullDistance { get; set; }

    [JsonPropertyName("obj_scale")]
    public float[] Scale { get; set; } = Array.Empty<float>();

    [JsonPropertyName("obj_render_object")]
    public uint RenderObject { get; set; }

    [JsonPropertyName("obj_double_fusion")]
    public bool DoubleFusion { get; set; } // booo fuck advertisers

    [JsonPropertyName("obj_forward_vector")]
    public float[] ForwardVector { get; set; } = Array.Empty<float>();

    [JsonPropertyName("obj_tracking_name")]
    public string TrackingName { get; set; } = string.Empty;

    [JsonPropertyName("obj_max_tracking_distance")]
    public float MaxTrackingDistance { get; set; }

    [JsonPropertyName("obj_icon")]
    public uint Icon { get; set; }

    [JsonPropertyName("obj_gravity_f")]
    public float GravityF { get; set; }

    [JsonPropertyName("obj_sound_events")]
    public object[] SoundEvents { get; set; } = Array.Empty<object>();

    [JsonPropertyName("obj_arc_hardpoint_list")]
    public object[] ArcHardpointList { get; set; } = Array.Empty<object>();

    [JsonPropertyName("obj_sparse_data")]
    public ObjSparseData? SparseData { get; set; }

    [JsonPropertyName("obj_render_object_low_detail")]
    public uint RenderObjectLowDetail { get; set; }

    [JsonPropertyName("obj_default_alignment")]
    public float[] DefaultAlignment { get; set; } = Array.Empty<float>();

    [JsonPropertyName("obj_sound_table")]
    public uint SoundTable { get; set; }

    [JsonPropertyName("combat_health_current")]
    public float CombatHealthCurrent { get; set; }

    [JsonPropertyName("combat_health_full")]
    public float CombatHealthFull { get; set; }

    [JsonPropertyName("combat_attack_resist")]
    public CombatAttackResist CombatAttackResist { get; set; } = new();

    [JsonPropertyName("combat_powers")]
    public object[] CombatPowers { get; set; } = Array.Empty<object>();

    [JsonPropertyName("static_platform_height")]
    public float StaticPlatformHeight { get; set; }

    [JsonPropertyName("static_has_platform")]
    public bool StaticHasPlatform { get; set; }

    [JsonPropertyName("static_collision_detect")]
    public bool StaticCollisionDetect { get; set; }

    [JsonPropertyName("static_has_sound")]
    public bool StaticHasSound { get; set; }

    [JsonPropertyName("structure_floors")]
    public StructureFloors[] StructureFloors { get; set; } = Array.Empty<StructureFloors>();

    [JsonPropertyName("structure_holes")]
    public object[] StructureHoles { get; set; } = Array.Empty<object>();

    [JsonPropertyName("structure_levels")]
    public StructureLevels[] StructureLevels { get; set; } = Array.Empty<StructureLevels>();

    [JsonPropertyName("structure_doors")]
    public object[] StructureDoors { get; set; } = Array.Empty<object>();

    [JsonPropertyName("structure_region_triggers")]
    public object[] StructureRegionTriggers { get; set; } = Array.Empty<object>();
}