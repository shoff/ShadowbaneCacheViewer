namespace Arcane.Data.Mongo.Entities;

public class ItemDiscReqEntity
{
    public bool Restrict { get; set; }
    public object[] Discs { get; set; } = Array.Empty<object>();
}