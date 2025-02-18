using CSharpFunctionalExtensions;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FilesService.Infrastructure.MongoDB;

public class MongoDbRepository(MongoDbContext dbContext)
{
    public async Task Add(FileData fileData, CancellationToken ct)
    {
        await dbContext.Files.InsertOneAsync(fileData, cancellationToken: ct);
    }

    public async Task Add(IEnumerable<FileData> filesData, CancellationToken ct)
    {
        await dbContext.Files.InsertManyAsync(filesData, cancellationToken: ct);
    }

    public async Task<Result<FileData>> Get(Guid id, CancellationToken ct)
    {
        return await dbContext.Files.AsQueryable()
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Result<IReadOnlyList<FileData>>> Get(
        IEnumerable<Guid> ids, CancellationToken ct)
    {
        return await dbContext.Files
            .Find(f => ids.ToList().Contains(f.Id))
            .ToListAsync(ct);
    }

    public async Task<UnitResult<Error>> Remove(Guid id, CancellationToken ct)
    {
        var deleteResult = await dbContext.Files
            .DeleteOneAsync(f => f.Id == id, cancellationToken: ct);
        if (deleteResult.DeletedCount == 0)
            return Errors.NotFound($"Файл c id = {id}");

        return Result.Success<Error>();
    }

    public async Task<Result<long, Error>> Remove(IEnumerable<Guid> ids, CancellationToken ct)
    {
        var deleteResult = await dbContext.Files
            .DeleteManyAsync(f => ids.Contains(f.Id), cancellationToken: ct);
        if (deleteResult.DeletedCount == 0)
            return Errors.NotFound("Файлы");

        return deleteResult.DeletedCount;
    }
}
