namespace Arcane.Data.Mongo.Entities;

public class ItemRaceReqEntity
{
    public bool Restrict { get; set; }
    public object[] Races { get; set; } = Array.Empty<object>();
}