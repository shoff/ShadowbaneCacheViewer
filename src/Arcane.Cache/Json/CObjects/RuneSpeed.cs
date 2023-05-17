namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class RuneSpeed
{
    [JsonPropertyName("WALK")]
    public float Walk { get; set; }
   
    [JsonPropertyName("RUN")]
    public float Run { get; set; }
    
    [JsonPropertyName("COMBATWALK")]
    public float Combatwalk { get; set; }
    
    [JsonPropertyName("COMBATRUN")]
    public float CombatRun { get; set; }
    
    [JsonPropertyName("FLYWALK")]
    public float FlyWalk { get; set; }
    
    [JsonPropertyName("FLYRUN")]
    public float FlyRun { get; set; }
    
    [JsonPropertyName("SWIM")]
    public float Swim { get; set; }
}