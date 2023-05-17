namespace Arcane.Cache.Json.CObjects;

public class TemplateRankInfo
{
    public long rank_hirelings { get; set; }
    public int rank_shrines { get; set; }
    public int rank_spires { get; set; }
    public int rank_barracks { get; set; }
    public long rank_rank { get; set; }
    public int rank_health { get; set; }
    public bool rank_automatic { get; set; }
    public float rank_energy_k1 { get; set; }
    public float rank_energy_k2 { get; set; }
    public float rank_level_val { get; set; }
    public long[][] rank_building_id { get; set; } = Array.Empty<long[]>();
    public int rank_formula { get; set; }
    public object[] rank_placement_limit { get; set; } = Array.Empty<object>();
}