using FilesService.Application.Endpoints;
using FilesService.Application.Interfaces;
using FilesService.Core.Dto.File;
using FilesService.Core.Request.Minio;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.Minio;

public static class GetFilePresignedPath
{ 
    public sealed class Endpoint : IEndpoint
    { 
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("file-presigned-path", Handler);
        }
    }
    private static async Task<IResult> Handler(
           [FromBody] MinioFilesInfoDto fileInfoDto,
           IFilesProvider fileProvider,
           CancellationToken ct)
    {
        var result = await fileProvider.GetFilePresignedPath(fileInfoDto, ct);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }
}
