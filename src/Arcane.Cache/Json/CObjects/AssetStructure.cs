namespace Arcane.Cache.Json.CObjects;


public class AssetStructure
{
    public string obj_name { get; set; } = string.Empty;
    public bool obj_pickable { get; set; }
    public float obj_gravity { get; set; }
    public float obj_cull_distance { get; set; }
    public float[] obj_scale { get; set; } = Array.Empty<float>();
    public int obj_render_object { get; set; }
    public bool obj_double_fusion { get; set; }
    public float[] obj_forward_vector { get; set; } = Array.Empty<float>();
    public string obj_tracking_name { get; set; } = string.Empty;
    public float obj_max_tracking_distance { get; set; }
    public int obj_icon { get; set; }
    public float obj_gravity_f { get; set; }
    public object[] obj_sound_events { get; set; } = Array.Empty<object>();
    public object[][] obj_arc_hardpoint_list { get; set; } = Array.Empty<object[]>();
    public ObjSparseData? obj_sparse_data { get; set; }
    public int obj_render_object_low_detail { get; set; }
    public float[] obj_default_alignment { get; set; } = Array.Empty<float>();
    public int obj_sound_table { get; set; }
    public float combat_health_current { get; set; }
    public float combat_health_full { get; set; }
    public CombatAttackResist? combat_attack_resist { get; set; } = new();
    public object[] combat_powers { get; set; } = Array.Empty<object>();
    public float static_platform_height { get; set; }
    public bool static_has_platform { get; set; }
    public bool static_collision_detect { get; set; }
    public bool static_has_sound { get; set; }
    public StructureFloors[] structure_floors { get; set; } = Array.Empty<StructureFloors>();
    public object[] structure_holes { get; set; } = Array.Empty<object>();
    public StructureLevels[] structure_levels { get; set; } = Array.Empty<StructureLevels>();
    public object[] structure_doors { get; set; } = Array.Empty<object>();
    public object[] structure_region_triggers { get; set; } = Array.Empty<object>();
    public int asset_structure_template_id { get; set; }
}