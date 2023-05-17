namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class Item
{
    [JsonIgnore]
    public uint ItemId { get; set; }
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
    public object[] obj_arc_hardpoint_list { get; set; } = Array.Empty<object>();
    public ObjSparseData? obj_sparse_data { get; set; }
    public int obj_render_object_low_detail { get; set; }
    public float[] obj_default_alignment { get; set; } = Array.Empty<float>();
    public int obj_sound_table { get; set; }
    public float combat_health_current { get; set; }
    public float combat_health_full { get; set; }
    public CombatAttackResist combat_attack_resist { get; set; } = new();
    public object[] combat_powers { get; set; } = Array.Empty<object>();
    public string item_type { get; set; } = string.Empty;
    public int item_eq_slots_value { get; set; }
    public bool item_eq_slots_type { get; set; }
    public string[] item_eq_slots_or { get; set; } = Array.Empty<string>();
    public string[] item_eq_slots_and { get; set; } = Array.Empty<string>();
    public bool item_takeable { get; set; }
    public int item_value { get; set; }
    public int item_wt { get; set; }
    public float item_passive_defense_mod { get; set; }
    public string item_base_name { get; set; } = string.Empty;
    public string item_dsc { get; set; } = string.Empty;
    public int item_render_object_female { get; set; }
    public float item_health_full { get; set; }
    public object item_skill_used { get; set; } = string.Empty;
    public object? item_skill_mastery_used { get; set; }
    public int item_parry_anim_id { get; set; }
    public object[] item_offering_info { get; set; } = Array.Empty<object>();
    public string[] item_flags { get; set; } = Array.Empty<string>();
    public object[] item_use_flags { get; set; } = Array.Empty<object>();
    public int item_post_item_id { get; set; }
    public int item_initial_charges { get; set; }
    public int item_book_arcana { get; set; }
    public object[] item_skill_req { get; set; } = Array.Empty<object>();
    public ItemRaceReq? item_race_req { get; set; }
    public ItemClassReq? item_class_req { get; set; }
    public ItemDiscReq? item_disc_req { get; set; }
    public object[] item_attr_req { get; set; } = Array.Empty<object>();
    public int item_level_req { get; set; }
    public int item_rank_req { get; set; }
    public string item_sex_req { get; set; } = string.Empty;
    public object[] item_user_power_action { get; set; } = Array.Empty<object>();
    public object[] item_power_grant { get; set; } = Array.Empty<object>();
    public object[] item_power_action { get; set; } = Array.Empty<object>();
    public bool item_sheathable { get; set; }
    public object? item_has_stub { get; set; }
    public object[] item_offering_adjustments { get; set; } = Array.Empty<object>();
    public object[] item_resource_costs { get; set; } = Array.Empty<object>();
    public int item_bane_rank { get; set; }
    public bool item_ignore_saved_actions { get; set; }
}
