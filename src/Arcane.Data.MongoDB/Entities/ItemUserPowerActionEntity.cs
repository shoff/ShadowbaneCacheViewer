namespace Arcane.Data.Mongo.Entities;

public class ItemUserPowerActionEntity
{
    public string Power { get; set; } = string.Empty;
    public int[] Arguments { get; set; } = Array.Empty<int>();
}