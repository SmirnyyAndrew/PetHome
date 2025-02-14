using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FilesService.Core.Models;

public class FileData
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; init; }
    public required string StoragePath { get; init; }
    public required DateTime UploadDate { get; init; }
    public required long FileSize { get; init; }
    public required string ContentType { get; init; }
}
