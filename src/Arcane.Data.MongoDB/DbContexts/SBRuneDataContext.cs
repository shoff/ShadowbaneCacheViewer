namespace Arcane.Data.Mongo.DbContexts;

using System.Diagnostics.CodeAnalysis;
using Arcane.Data.Mongo.Entities;
using MongoDB.Driver;

[ExcludeFromCodeCoverage(Justification = "We don't currently have a strategy for integration style tests")]
public sealed class SBRuneDataContext 
{
    private readonly IMongoDatabase db;
    private readonly SBRuneStoreOptions options;
    
    public SBRuneDataContext(IMongoClient client, SBRuneStoreOptions? options)
    {
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        options ??= new SBRuneStoreOptions();

        if (string.IsNullOrWhiteSpace(options.DatabaseName))
        {
            throw new ArgumentException($"DatabaseName is missing in {nameof(SBRuneStoreOptions)}");
        }

        this.db = client.GetDatabase(options.DatabaseName);
        this.options = options;
        this.Client = client;
    }

    public IMongoClient Client { get; }

    public IMongoCollection<RuneEntity> Runes  => 
        this.db.GetCollection<RuneEntity>(this.options.RunesCollectionName);
}

public sealed class SBRuneStoreOptions
{
    public string? DatabaseName { get; set; } = "Shadowbane";
    public string RunesCollectionName { get; set; } = "Runes";
}
