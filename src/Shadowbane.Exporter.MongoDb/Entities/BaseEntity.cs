using System.Diagnostics.CodeAnalysis;
using BudoHub.MongoDb;
using MongoDB.Bson.Serialization.Attributes;

namespace Shadowbane.Exporter.MongoDb.Entities;

[ExcludeFromCodeCoverage]
public abstract class BaseEntity : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? Id { get; set; }
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTimeOffset Updated { get; set; }
    public string? UpdatedBy { get; set; }
}