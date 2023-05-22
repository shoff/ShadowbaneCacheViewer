namespace Arcane.Data.Mongo.Entities;

public class RuneSkillGrantEntity
{
    public string SkillType { get; set; } = string.Empty;
    public int SkillValue { get; set; }
    public object[] SkillGrantedAttrs { get; set; } = Array.Empty<object>();
    public object[] SkillGrantedSkills { get; set; } = Array.Empty<object>();
    public object[] SkillGrantedPowers { get; set; } = Array.Empty<object>();
    public object[] SkillMonsterTypes { get; set; } = Array.Empty<object>();
}