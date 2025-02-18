using FilesService.Application.Endpoints;
using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.Minio;

public static class DeleteFile
{ 
    public sealed class Endpoint : IEndpoint
    { 
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("minio/file", Handler);
        }
    }
    private static async Task<IResult> Handler(
           [FromBody] MinioFilesInfoDto fileInfoDto,
           IMinioFilesHttpClient fileProvider,
           CancellationToken ct)
    {   
        var result = await fileProvider.DeleteFile(fileInfoDto,ct);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);
         
        return Results.Ok();
    }
}
