namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class ItemRaceReq
{
    [JsonPropertyName("restrict")]
    public bool Restrict { get; set; }

    [JsonPropertyName("races")]
    public object[] Races { get; set; } = Array.Empty<object>();
}