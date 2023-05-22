namespace Arcane.Data.Mongo.Entities;

public class ItemClassReqEntity
{
    public bool Restrict { get; set; }
    public object[] Classes { get; set; } = Array.Empty<object>();
}