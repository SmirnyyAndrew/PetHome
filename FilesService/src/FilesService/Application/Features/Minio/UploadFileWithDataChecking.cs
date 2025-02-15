using FilesService.Application.Endpoints;
using FilesService.Application.Interfaces;
using FilesService.Core.Request.Minio;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.Minio;

public static class UploadFileWithDataChecking
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("upload-file-with-checking", Handler);
        }
    }
    private static async Task<IResult> Handler(
           [FromForm] IFormFile formFile,
           [FromForm] UploadFileRequest request,
           IFilesProvider fileProvider,
           CancellationToken ct)
    {
        await using Stream stream = formFile.OpenReadStream();

        var result = await fileProvider.UploadFileWithDataChecking(stream, request, ct);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok();
    }
}
