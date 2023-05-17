namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public struct CombatAttackResist
{
    [JsonPropertyName("UNKNOWN")]
    public float Unknown { get; set; }
    
    [JsonPropertyName("SLASHING")]
    public float Slashing { get; set; }
    
    [JsonPropertyName("CRUSHING")]
    public float Crushing { get; set; }
    
    [JsonPropertyName("PIERCING")]
    public float Piercing { get; set; }
    
    [JsonPropertyName("POISON")]
    public float Poison { get; set; }
   
    [JsonPropertyName("LIGHTNING")]
    public float Lightning { get; set; }
    
    [JsonPropertyName("MAGIC")]
    public float Magic { get; set; }
    
    [JsonPropertyName("FIRE")]
    public float Fire { get; set; }
    
    [JsonPropertyName("COLD")]
    public float Cold { get; set; }
    
    [JsonPropertyName("MENTAL")]
    public float Mental { get; set; }
    
    [JsonPropertyName("HOLY")]
    public float Holy { get; set; }
    
    [JsonPropertyName("SIEGE")]
    public float Siege { get; set; }
    
    [JsonPropertyName("HEALING")]
    public float Healing { get; set; }
    
    [JsonPropertyName("BLEEDING")]
    public float Bleeding { get; set; }
    
    [JsonPropertyName("UNHOLY")]
    public float Unholy { get; set; }
    
    [JsonPropertyName("ANTISIEGE")]
    public float AntiSiege { get; set; }
}