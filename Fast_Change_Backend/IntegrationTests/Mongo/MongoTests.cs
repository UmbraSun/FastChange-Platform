using FluentAssertions;
using IntegrationTests.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IntegrationTests.Mongo;

[Collection(nameof(MongoCollection))]
public sealed class MongoTests
{
    private readonly IntegrationFixture _fixture;

    public MongoTests(IntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Mongo_Should_Insert_Document()
    {
        var collection = _fixture.Mongo.Database.GetCollection<BsonDocument>("health");
        var document = new BsonDocument
        {
            ["value"] = "ok"
        };

        await collection.InsertOneAsync(document);
        var count = await collection.CountDocumentsAsync(FilterDefinition<BsonDocument>.Empty);
        count.Should().Be(1);
    }
}
