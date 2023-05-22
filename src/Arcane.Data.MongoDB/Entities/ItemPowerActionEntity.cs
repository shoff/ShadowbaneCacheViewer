namespace Arcane.Data.Mongo.Entities;

public class ItemPowerActionEntity
{
    public string PowerType { get; set; } = string.Empty;
    public int[] PowerArguments {get;set; } = Array.Empty<int>();
}