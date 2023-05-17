namespace Arcane.Cache.Json.CObjects;

public class Asset
{
    public int template_max_ranks { get; set; }
    public int template_start_rank { get; set; }
    public int template_asset_type { get; set; }
    public int template_trade_icon { get; set; }
    public int template_landmark { get; set; }
    public bool template_is_maintenance { get; set; }
    public bool template_has_keys { get; set; }
    public bool template_use_hardpoints { get; set; }
    public bool template_is_fort_asset { get; set; }
    public bool template_is_building_of_war { get; set; }
    public bool template_bow_can_place_on_grid { get; set; }
    public bool template_requires_nation_tree_slot { get; set; }
    public bool template_requires_guild_tree_slot { get; set; }
    public bool template_use_fort_grid { get; set; }
    public bool template_is_fort_start { get; set; }
    public bool template_is_cap_asset { get; set; }
    public float[] template_zone_no_build { get; set; } = Array.Empty<float>();
    public float[] template_zone_influence { get; set; } = Array.Empty<float>();
    public float[] template_eject_loc { get; set; } = Array.Empty<float>();
    public float[] template_npc_load { get; set; } = Array.Empty<float>();
    public float[] template_fort_grid_offset { get; set; } = Array.Empty<float>();
    public long template_power_action_id { get; set; }
    public long template_zone_flag { get; set; }
    public long template_spire_event_rule { get; set; }
    public long template_maintenance_set { get; set; }
    public long template_damage_set { get; set; }
    public long template_energy_set { get; set; }
    public string template_use_trigger { get; set; } = string.Empty;
    public bool template_unknown_check1 { get; set; }
    public string template_loot_trigger { get; set; } = string.Empty;
    public bool template_unknown_check2 { get; set; }
    public bool has_embedded_template { get; set; }
    public Asset? template_embed_template { get; set; }
    public int[] template_creator { get; set; } = Array.Empty<int>();
    public int[] template_terrain { get; set; } = Array.Empty<int>();
    public object[] template_valid_npc_type { get; set; } = Array.Empty<object>();
    public int[] template_valid_npc_cat { get; set; } = Array.Empty<int>();
    public TemplateRankInfo[] template_rank_info { get; set; } = Array.Empty<TemplateRankInfo>();
    public object[] template_cap_info { get; set; } = Array.Empty<object>();
    public object[] template_event_rules { get; set; } = Array.Empty<object>();
    public string[] template_architecture { get; set; } = Array.Empty<string>();
    public string template_offering_type { get; set; } = string.Empty;
    public long template_placement_type { get; set; }
    public object[] template_offering_adjustment { get; set; } = Array.Empty<object>();
    public object[] template_resource_limit { get; set; } = Array.Empty<object>();
    public int template_unknown { get; set; }
}