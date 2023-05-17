namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class ItemUserPowerAction
{
    [JsonPropertyName("power")]
    public string Power { get; set; } = string.Empty;
   
    [JsonPropertyName("arguments")]
    public int[] Arguments { get; set; } = Array.Empty<int>();
}