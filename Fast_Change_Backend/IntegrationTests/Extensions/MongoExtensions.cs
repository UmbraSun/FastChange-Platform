using IntegrationTests.Infrastructure;
using MongoDB.Driver;

namespace IntegrationTests.Extensions;

public static class MongoExtensions
{
    public static IMongoCollection<TDocument> GetCollection<TDocument>(
        this MongoFixture fixture,
        string name)
    {
        return fixture.Database.GetCollection<TDocument>(name);
    }
}
