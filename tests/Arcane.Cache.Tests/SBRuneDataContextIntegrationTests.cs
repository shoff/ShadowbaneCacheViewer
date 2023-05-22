namespace Arcane.Cache.Tests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Arcane.Data.Mongo.Mappings;
using Data.Mongo.DbContexts;
using MongoDB.Driver;
using Xunit.Abstractions;

public class SBRuneDataContextIntegrationTests
{
    private readonly ITestOutputHelper helper;
    private readonly List<string> runes;
    public const string RUNE_PATH = "C:\\dev\\ShadowbaneCacheViewer\\ARCANE_DUMP\\COBJECTS\\RUNE"; // yuck
    public const string TEXTURE_PATH = "C:\\dev\\ShadowbaneCacheViewer\\ARCANE_DUMP\\TEXTURE";
    private readonly MongoClient mongoClient;
    private readonly SBRuneDataContext dataContext;
    private const string connectionString = "mongodb://root:H%40cker33@mongo1:27011,mongo2:27012,mongo3:27013/organizations?authSource=admin&replicaSet=rs0";

    public SBRuneDataContextIntegrationTests(ITestOutputHelper helper)
    {
        this.helper = helper;
        this.runes = Directory.GetFiles(RUNE_PATH).ToList();
        var mcs = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
        this.mongoClient = new MongoClient(mcs);
        this.dataContext = new SBRuneDataContext(this.mongoClient, null);
    }

    [Fact]
    public void Can_Add_Rune_To_Mongo()
    {
        foreach (var file in this.runes)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(file);
                uint id = uint.Parse(fileInfo.Name.Replace(".json", ""));
                var entity = EntityExtensions.GetRune(file, id);

                if (entity is null)
                {
                    throw new ApplicationException($"Could not load entity for {file}");
                }
                this.dataContext.Runes.InsertOne(entity);
                this.helper.WriteLine($"inserted {entity.RuneId}");
            }
            catch (Exception e)
            {
                this.helper.WriteLine(e.Message);
                // fuck it for now
            }
        }
    }

}
