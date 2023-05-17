namespace Arcane.Cache.Json.CObjects;

using System.Text.Json.Serialization;

public class StructureFloors
{
    [JsonPropertyName("floor_level_number")]
    public int FloorLevelNumber { get; set; }

    [JsonPropertyName("floor_exits")]
    public FloorExits[] FloorExits { get; set; } = Array.Empty<FloorExits>();

    [JsonPropertyName("floor_rooms")]
    public object[] FloorRooms { get; set; } = Array.Empty<object>();
}