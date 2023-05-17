namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class RuneSkillGrant
{
    [JsonPropertyName("skill_type")]
    public string SkillType { get; set; } = string.Empty;
   
    [JsonPropertyName("skill_value")]
    public int SkillValue { get; set; }
    
    [JsonPropertyName("skill_granted_attrs")]
    public object[] SkillGrantedAttrs { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("skill_granted_skills")]
    public object[] SkillGrantedSkills { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("skill_granted_powers")]
    public object[] SkillGrantedPowers { get; set; } = Array.Empty<object>();
    
    [JsonPropertyName("skill_monster_types")]
    public object[] SkillMonsterTypes { get; set; } = Array.Empty<object>();
}