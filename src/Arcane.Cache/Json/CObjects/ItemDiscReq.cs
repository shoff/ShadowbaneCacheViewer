namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class ItemDiscReq
{
    [JsonPropertyName("restrict")]
    public bool Restrict { get; set; }

    [JsonPropertyName("discs")]
    public object[] Discs { get; set; } = Array.Empty<object>();
}