namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class RuneGroup
{
    [JsonPropertyName("group_type")]
    public int GroupType { get; set; }
    
    [JsonPropertyName("group_is_faction")]
    public bool GroupIsFaction { get; set; }
    
    [JsonPropertyName("group_is_guild")]
    public bool GroupIsGuild { get; set; }
}