using FilesService.Infrastructure.MongoDB.Documents;
using MongoDB.Driver;

namespace FilesService.Infrastructure.MongoDB;

public class MongoDbContext(IMongoClient mongoClient)
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("mongodb");

    public IMongoCollection<FileData> Files
        => _database.GetCollection<FileData>("files"); 
}
