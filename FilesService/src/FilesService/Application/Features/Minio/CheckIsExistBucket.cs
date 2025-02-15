using FilesService.Application.Endpoints;
using FilesService.Infrastructure.MongoDB;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.Minio;

public static class CheckIsExistBucket
{
    private record CheckIsExistBucketRequest(IEnumerable<Guid> Ids);

    public sealed class Endpoint : IEndpoint
    {
        //Использую POST, потому что у GET нет Body
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files", Handler);
        }
    }
    private static async Task<IResult> Handler(
           [FromBody] GetFilesDataByIdsRequest request,
           MongoDbRepository repository,
           CancellationToken ct)
    {
        var result = await repository.Get(request.Ids, ct);
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return Results.BadRequest();
    }
}
