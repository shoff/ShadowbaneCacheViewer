namespace Arcane.Data.Mongo.Entities;

public class RuneSkillAdjEntity
{
    public string skill_type { get; set; } = string.Empty;
    public int[][] skill_adjusts { get; set; } = Array.Empty<int[]>();
}