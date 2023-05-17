namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class Rune
{
    [JsonIgnore]
    public uint RuneId { get; set; }

    [JsonPropertyName("obj_name")]
    public string ObjName { get; set; } = string.Empty;
    
    [JsonPropertyName("obj_pickable")]
    public bool Pickable { get; set; }
    
    [JsonPropertyName("obj_gravity")]
    public float Gravity { get; set; }
    
    [JsonPropertyName("obj_cull_distance")]
    public float CullDistance { get; set; }
    
    [JsonPropertyName("obj_scale")]
    public float[] Scale { get; set; } = Array.Empty<float>();
    
    // TODO flatten this with the renderable that matches the id
    [JsonPropertyName("obj_render_object")]
    public int RenderObject { get; set; }
    
    [JsonPropertyName("obj_double_fusion")]
    public bool DoubleFusion { get; set; }
    
    [JsonPropertyName("obj_forward_vector")]
    public float[] ForwardVector { get; set; } = Array.Empty<float>();
    
    [JsonPropertyName("obj_tracking_name")]
    public string TrackingName { get; set; } = string.Empty;
    
    [JsonPropertyName("obj_max_tracking_distance")]
    public float MaxTrackingDistance { get; set; }
    
    [JsonPropertyName("obj_icon")]
    public int Icon { get; set; }
    
    [JsonPropertyName("obj_gravity_f")]
    public float GravityF { get; set; }
    
    [JsonPropertyName("obj_sound_events")]
    public object[] SoundEvents { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("obj_arc_hardpoint_list")]
    public object[] ArcHardpointList { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("obj_sparse_data")]
    public ObjSparseData? SparseData { get; set; }
    
    [JsonPropertyName("obj_render_object_low_detail")]
    public int RenderObjectLowDetail { get; set; }
    
    [JsonPropertyName("obj_default_alignment")]
    public float[] DefaultAlignment { get; set; } = Array.Empty<float>();
    
    [JsonPropertyName("obj_sound_table")]
    public object? SoundTable { get; set; }
    
    [JsonPropertyName("combat_health_current")]
    public float HealthCurrent { get; set; }
    
    [JsonPropertyName("combat_health_full")]
    public float HealthFull { get; set; }
    
    [JsonPropertyName("combat_attack_resist")]
    public CombatAttackResist CombatAttackResist { get; set; } = new();
    
    [JsonPropertyName("combat_powers")]
    public object[] CombatPowers { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("item_type")]
    public string ItemType { get; set; } = string.Empty;
    
    [JsonPropertyName("item_eq_slots_value")]
    public int ItemEqSlotsValue { get; set; }
    
    [JsonPropertyName("item_eq_slots_type")]
    public bool ItemEqSlotsType { get; set; }
    
    [JsonPropertyName("item_eq_slots_or")]
    public string[] ItemEqSlotsOr { get; set; } = Array.Empty<string>();
    
    [JsonPropertyName("item_eq_slots_and")]
    public string[] ItemEqSlotsAnd { get; set; } = Array.Empty<string>();
    
    [JsonPropertyName("item_takeable")]
    public bool ItemTakeable { get; set; }
    
    [JsonPropertyName("item_value")]
    public int ItemValue { get; set; }
    
    [JsonPropertyName("item_wt")]
    public int ItemWT { get; set; }
    
    [JsonPropertyName("item_passive_defense_mod")]
    public float ItemPassiveDefenseMod { get; set; }
    
    [JsonPropertyName("item_base_name")]
    public string ItemBaseName { get; set; } = string.Empty;
    
    [JsonPropertyName("item_dsc")]
    public string ItemDsc { get; set; } = string.Empty;
    
    [JsonPropertyName("item_render_object_female")]
    public int ItemRenderObjectFemale { get; set; }
    
    [JsonPropertyName("item_health_full")]
    public float ItemHealthFull { get; set; }
    
    [JsonPropertyName("item_skill_used")]
    public int ItemSkillUsed { get; set; }
    
    [JsonPropertyName("item_skill_mastery_used")]
    public int ItemSkillMasteryUsed { get; set; }
    
    [JsonPropertyName("item_parry_anim_id")]
    public int ItemParryAnimId { get; set; }
    
    [JsonPropertyName("item_offering_info")]
    public object[] ItemOfferingInfo { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("item_flags")]
    public string[] ItemFlags { get; set; } = Array.Empty<string>();
    
    [JsonPropertyName("item_use_flags")]
    public object[] ItemUseFlags { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("item_post_item_id")]
    public int ItemPostItemId { get; set; }
    
    [JsonPropertyName("item_initial_charges")]
    public int ItemInitialCharges { get; set; }
    
    [JsonPropertyName("item_book_arcana")]
    public int ItemBookArcana { get; set; }
    
    [JsonPropertyName("item_skill_req")]
    public object[] ItemSkillReq { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("item_race_req")]
    public ItemRaceReq? ItemRaceReq { get; set; }
    
    [JsonPropertyName("item_class_req")]
    public ItemClassReq? ItemClassReq { get; set; }
    
    [JsonPropertyName("item_disc_req")]
    public ItemDiscReq? ItemDiscReq { get; set; }
    
    [JsonPropertyName("item_attr_req")]
    public object[] ItemAttrReq { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("item_level_req")]
    public int ItemLevelReq { get; set; }
    
    [JsonPropertyName("item_rank_req")]
    public int ItemRankReq { get; set; }
    
    [JsonPropertyName("item_sex_req")]
    public string ItemSexReq { get; set; } = string.Empty;
    
    [JsonPropertyName("item_user_power_action")]
    public ItemUserPowerAction[] ItemUserPowerActions { get; set; } = Array.Empty<ItemUserPowerAction>();
    
    [JsonPropertyName("item_power_grant")]
    public object[] ItemPowerGrant { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("item_power_action")]
    public object[] ItemPowerAction { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("item_sheathable")]
    public bool ItemSheathable { get; set; }
    
    [JsonPropertyName("item_has_stub")]
    public bool ItemHasStub { get; set; }
    
    [JsonPropertyName("item_offering_adjustments")]
    public object[] ItemOfferingAdjustments { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("item_resource_costs")]
    public object[] ItemResourceCosts { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("item_bane_rank")]
    public int ItemBaneRank { get; set; }
    
    [JsonPropertyName("item_ignore_saved_actions")]
    public bool ItemIgnoreSavedActions { get; set; }
    
    [JsonPropertyName("rune_type")]
    public string RuneType { get; set; } = string.Empty;
    
    [JsonPropertyName("rune_sub_type")]
    public object? RuneSubType { get; set; }
    
    [JsonPropertyName("rune_is_standard_character_creation")]
    public bool RuneIsStandardCharacterCreation { get; set; }
    
    [JsonPropertyName("rune_creation_cost")]
    public int RuneCreationCost { get; set; }
    
    [JsonPropertyName("rune_rank")]
    public int RuneRank { get; set; }
    
    [JsonPropertyName("rune_pracs_per_level")]
    public int RunePracsPerLevel { get; set; }
    
    [JsonPropertyName("rune_exp_req_to_level")]
    public float RuneExpReqToLevel { get; set; }
    
    [JsonPropertyName("rune_sex")]
    public string RuneSex { get; set; } = string.Empty;
    
    [JsonPropertyName("rune_class_icon")]
    public int RuneClassIcon { get; set; }
    
    [JsonPropertyName("rune_health")]
    public int RuneHealth { get; set; }
    
    [JsonPropertyName("rune_mana")]
    public int RuneMana { get; set; }
    
    [JsonPropertyName("rune_stamina")]
    public int RuneStamina { get; set; }
    
    [JsonPropertyName("rune_min_damage")]
    public float RuneMinDamage { get; set; }
    
    [JsonPropertyName("rune_max_damage")]
    public float RuneMaxDamage { get; set; }
    
    [JsonPropertyName("rune_attack")]
    public int RuneAttack { get; set; }
    
    [JsonPropertyName("rune_defense")]
    public int RuneDefense { get; set; }
    
    [JsonPropertyName("rune_level")]
    public int RuneLevel { get; set; }
    
    [JsonPropertyName("rune_speed")]
    public RuneSpeed? RuneSpeed { get; set; }
    
    [JsonPropertyName("rune_group")]
    public RuneGroup? RuneGroup { get; set; }
    
    [JsonPropertyName("rune_dsc")]
    public string RuneDsc { get; set; } = string.Empty;
    
    [JsonPropertyName("rune_fx_txt")]
    public string RuneFxTxt { get; set; } = string.Empty;
    
    [JsonPropertyName("rune_group_tactics")]
    public long RuneGroupTactics { get; set; }
    
    [JsonPropertyName("rune_group_role_set")]
    public long RuneGroupRoleSet { get; set; }
    
    [JsonPropertyName("rune_enemy_monster_types")]
    public object[] RuneEnemyMonsterTypes { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("rune_groupee_monster_types")]
    public object[] RuneGroupeeMonsterTypes { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("rune_helper_monster_types")]
    public object[] RuneHelperMonsterTypes { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("rune_not_enemy_monster_types")]
    public object[] RuneNotEnemyMonsterTypes { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("rune_enemy_gender")]
    public object[] RuneEnemyGender { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("rune_skill_grant")]
    public RuneSkillGrant[] RuneSkillGrants { get; set; } = Array.Empty<RuneSkillGrant>();
    
    [JsonPropertyName("rune_skill_adj")]
    public RuneSkillAdj[] RuneSkillAdjs { get; set; } = Array.Empty<RuneSkillAdj>();
    
    [JsonPropertyName("rune_attr_adj")]
    public RuneAttrAdj[] RuneAttrAdjs { get; set; } = Array.Empty<RuneAttrAdj>();
    
    [JsonPropertyName("rune_max_attr_adj")]
    public RuneMaxAttrAdj[] RuneMaxAttrAdjs { get; set; } = Array.Empty<RuneMaxAttrAdj>();
    
    [JsonPropertyName("rune_naturalattacks")]
    public float[][] RuneNaturalattacks { get; set; } = Array.Empty<float[]>();
    
    [JsonPropertyName("rune_enchantment_type")]
    public object[] RuneEnchantmentType { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("rune_inventory_contents")]
    public object[] RuneInventoryContents { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("rune_renderable")]
    public bool RuneRenderable { get; set; }
    
    [JsonPropertyName("rune_scale_factor")]
    public float[] RuneScaleFactor { get; set; } = Array.Empty<float>();
    
    [JsonPropertyName("rune_skeleton")]
    public int RuneSkeleton { get; set; }
    
    [JsonPropertyName("rune_slope_hugger")]
    public bool RuneSlopeHugger { get; set; }
    
    [JsonPropertyName("rune_can_fly")]
    public bool RuneCanFly { get; set; }
    
    [JsonPropertyName("rune_death_effect")]
    public int RuneDeathEffect { get; set; }
    
    [JsonPropertyName("rune_tombstone_id")]
    public int RuneTombstoneId { get; set; }
    
    [JsonPropertyName("rune_body_parts")]
    public RuneBodyParts[] RuneBodyPartsArray { get; set; } = Array.Empty<RuneBodyParts>();
    
    [JsonPropertyName("rune_hair")]
    public int[] RuneHair { get; set; } = Array.Empty<int>();
    
    [JsonPropertyName("rune_beard")]
    public int[] RuneBeard { get; set; } = Array.Empty<int>();
    
    [JsonPropertyName("rune_natural_power_attack")]
    public int RuneNaturalPowerAttack { get; set; }
    
    [JsonPropertyName("rune_sparse_data")]
    public RuneSparseData? RuneSparseData { get; set; }
}