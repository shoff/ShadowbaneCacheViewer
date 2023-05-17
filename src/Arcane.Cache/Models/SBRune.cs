namespace Arcane.Cache.Models;

using Arcane.Cache.Json.CObjects;

public class SBRune
{
    public uint RuneId { get; set; }
    public string ObjName { get; set; } = string.Empty;
    public bool Pickable { get; set; }
    public float Gravity { get; set; }
    public float CullDistance { get; set; }
    public float[] Scale { get; set; } = Array.Empty<float>();
    // TODO flatten this with the renderable that matches the id
    public uint RenderObject { get; set; }
    public SBRenderable? Renderable { get; set; }
    public bool DoubleFusion { get; set; }
    public float[] ForwardVector { get; set; } = Array.Empty<float>();
    public string TrackingName { get; set; } = string.Empty;
    public float MaxTrackingDistance { get; set; }
    public uint Icon { get; set; }
    public float GravityF { get; set; }
    public object[] SoundEvents { get; set; } = Array.Empty<object>();
    public object[] ArcHardpointList { get; set; } = Array.Empty<object>();
    public ObjSparseData? SparseData { get; set; }
    public int RenderObjectLowDetail { get; set; }
    public float[] DefaultAlignment { get; set; } = Array.Empty<float>();
    public object? SoundTable { get; set; }
    public float HealthCurrent { get; set; }
    public float HealthFull { get; set; }
    public CombatAttackResist CombatAttackResist { get; set; } = new();
    public object[] CombatPowers { get; set; } = Array.Empty<object>();
    public string ItemType { get; set; } = string.Empty;
    public int ItemEqSlotsValue { get; set; }
    public bool ItemEqSlotsType { get; set; }
    public string[] ItemEqSlotsOr { get; set; } = Array.Empty<string>();
    public string[] ItemEqSlotsAnd { get; set; } = Array.Empty<string>();
    public bool ItemTakeable { get; set; }
    public int ItemValue { get; set; }
    public int ItemWT { get; set; }
    public float ItemPassiveDefenseMod { get; set; }
    public string ItemBaseName { get; set; } = string.Empty;
    public string ItemDsc { get; set; } = string.Empty;
    public int ItemRenderObjectFemale { get; set; }
    public float ItemHealthFull { get; set; }
    public int ItemSkillUsed { get; set; }
    public int ItemSkillMasteryUsed { get; set; }
    public int ItemParryAnimId { get; set; }
    public object[] ItemOfferingInfo { get; set; } = Array.Empty<object>();
    public string[] ItemFlags { get; set; } = Array.Empty<string>();
    public object[] ItemUseFlags { get; set; } = Array.Empty<object>();
    public int ItemPostItemId { get; set; }
    public int ItemInitialCharges { get; set; }
    public int ItemBookArcana { get; set; }
    public object[] ItemSkillReq { get; set; } = Array.Empty<object>();
    public ItemRaceReq? ItemRaceReq { get; set; }
    public ItemClassReq? ItemClassReq { get; set; }
    public ItemDiscReq? ItemDiscReq { get; set; }
    public object[] ItemAttrReq { get; set; } = Array.Empty<object>();
    public int ItemLevelReq { get; set; }
    public int ItemRankReq { get; set; }
    public string ItemSexReq { get; set; } = string.Empty;
    public ItemUserPowerAction[] ItemUserPowerActions { get; set; } = Array.Empty<ItemUserPowerAction>();
    public object[] ItemPowerGrant { get; set; } = Array.Empty<object>();
    public object[] ItemPowerAction { get; set; } = Array.Empty<object>();
    public bool ItemSheathable { get; set; }
    public bool ItemHasStub { get; set; }
    public object[] ItemOfferingAdjustments { get; set; } = Array.Empty<object>();
    public object[] ItemResourceCosts { get; set; } = Array.Empty<object>();
    public int ItemBaneRank { get; set; }
    public bool ItemIgnoreSavedActions { get; set; }
    public string RuneType { get; set; } = string.Empty;
    public object? RuneSubType { get; set; }
    public bool RuneIsStandardCharacterCreation { get; set; }
    public int RuneCreationCost { get; set; }
    public int RuneRank { get; set; }
    public int RunePracsPerLevel { get; set; }
    public float RuneExpReqToLevel { get; set; }
    public string RuneSex { get; set; } = string.Empty;
    public int RuneClassIcon { get; set; }
    public int RuneHealth { get; set; }
    public int RuneMana { get; set; }
    public int RuneStamina { get; set; }
    public float RuneMinDamage { get; set; }
    public float RuneMaxDamage { get; set; }
    public int RuneAttack { get; set; }
    public int RuneDefense { get; set; }
    public int RuneLevel { get; set; }
    public RuneSpeed? RuneSpeed { get; set; }
    public RuneGroup? RuneGroup { get; set; }
    public string RuneDsc { get; set; } = string.Empty;
    public string RuneFxTxt { get; set; } = string.Empty;
    public long RuneGroupTactics { get; set; }
    public long RuneGroupRoleSet { get; set; }
    public object[] RuneEnemyMonsterTypes { get; set; } = Array.Empty<object>();
    public object[] RuneGroupeeMonsterTypes { get; set; } = Array.Empty<object>();
    public object[] RuneHelperMonsterTypes { get; set; } = Array.Empty<object>();
    public object[] RuneNotEnemyMonsterTypes { get; set; } = Array.Empty<object>();
    public object[] RuneEnemyGender { get; set; } = Array.Empty<object>();
    public RuneSkillGrant[] RuneSkillGrants { get; set; } = Array.Empty<RuneSkillGrant>();
    public RuneSkillAdj[] RuneSkillAdjs { get; set; } = Array.Empty<RuneSkillAdj>();
    public RuneAttrAdj[] RuneAttrAdjs { get; set; } = Array.Empty<RuneAttrAdj>();
    public RuneMaxAttrAdj[] RuneMaxAttrAdjs { get; set; } = Array.Empty<RuneMaxAttrAdj>();
    public float[][] RuneNaturalattacks { get; set; } = Array.Empty<float[]>();
    public object[] RuneEnchantmentType { get; set; } = Array.Empty<object>();
    public object[] RuneInventoryContents { get; set; } = Array.Empty<object>();
    public bool RuneRenderable { get; set; }
    public float[] RuneScaleFactor { get; set; } = Array.Empty<float>();
    public int RuneSkeleton { get; set; }
    public bool RuneSlopeHugger { get; set; }
    public bool RuneCanFly { get; set; }
    public int RuneDeathEffect { get; set; }
    public int RuneTombstoneId { get; set; }
    public RuneBodyParts[] RuneBodyPartsArray { get; set; } = Array.Empty<RuneBodyParts>();
    public int[] RuneHair { get; set; } = Array.Empty<int>();
    public int[] RuneBeard { get; set; } = Array.Empty<int>();
    public int RuneNaturalPowerAttack { get; set; }
    public RuneSparseData? RuneSparseData { get; set; }
}