namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class ItemClassReq
{
    [JsonPropertyName("restrict")]
    public bool Restrict { get; set; }
  
    [JsonPropertyName("classes")]
    public object[] Classes { get; set; } = Array.Empty<object>();
}