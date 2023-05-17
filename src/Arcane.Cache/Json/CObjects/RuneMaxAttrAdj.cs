namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class RuneMaxAttrAdj
{
    [JsonPropertyName("attr_type")]
    public string AttrType { get; set; } = string.Empty;

    [JsonPropertyName("attr_value")]
    public long AttrValue { get; set; }
}