namespace Arcane.Cache.Json.CObjects;

public class RuneSkillAdj
{
    public string skill_type { get; set; } = string.Empty;
    public int[][] skill_adjusts { get; set; } = Array.Empty<int[]>();
}