using FilesService.Application.Endpoints;
using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;
using FilesService.Core.Request.Minio;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.Minio;


public static class UploadFile
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("upload-file", Handler);
        }
    }
    private static async Task<IResult> Handler(
           IFormFile file,
           [FromQuery] string bucketName,
           [FromQuery] bool createBucketIfNotExist,
           IFilesProvider fileProvider,
           CancellationToken ct)
    {
        await using Stream stream = file.OpenReadStream();

        MinioFileInfoDto minioFileInfoDto = new MinioFileInfoDto(bucketName, file.Name);
        UploadFileRequest request = new UploadFileRequest(minioFileInfoDto, createBucketIfNotExist);
        var result = await fileProvider.UploadFile(stream, request, ct);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }
}
