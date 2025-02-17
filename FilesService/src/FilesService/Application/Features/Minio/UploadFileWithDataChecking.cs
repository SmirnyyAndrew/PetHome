using FilesService.Application.Endpoints;
using FilesService.Application.Interfaces;
using FilesService.Core.Dto.File;
using FilesService.Core.Request.Minio;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.Minio;

public static class UploadFileWithDataChecking
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("upload-file-with-data-checking", Handler);
        }
    }
    private static async Task<IResult> Handler(
           IFormFile formFile,
           [FromQuery] string bucketName,
           [FromQuery] bool createBucketIfNotExist,
           IFilesProvider fileProvider,
           CancellationToken ct)
    {
        await using Stream stream = formFile.OpenReadStream();
        MinioFileInfoDto minioFileInfoDto = new MinioFileInfoDto(bucketName, formFile.Name);
        UploadFileRequest request = new UploadFileRequest(minioFileInfoDto, createBucketIfNotExist);
        
        var result = await fileProvider.UploadFileWithDataChecking(stream, request, ct);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok();
    }
}
