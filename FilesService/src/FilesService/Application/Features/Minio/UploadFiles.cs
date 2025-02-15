using FilesService.Application.Endpoints;
using FilesService.Application.Interfaces;
using FilesService.Core.Request.Minio;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.Minio;

public static class UploadFiles
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("upload-files", Handler);
        }
    }
    private static async Task<IResult> Handler(
           [FromForm] IEnumerable<IFormFile> formFiles,
           [FromForm] UploadFilesRequest request,
           IFilesProvider fileProvider,
           CancellationToken ct)
    {
        List<Stream> streams = formFiles.Select(x => x.OpenReadStream()).ToList();

        var result = await fileProvider.UploadFiles(streams, request, ct);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }
}
