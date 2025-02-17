using FilesService.Application.Endpoints;
using FilesService.Core.Interfaces;
using FilesService.Core.Request.Minio;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.Minio;

public static class DownloadFiles
{ 
    public sealed class Endpoint : IEndpoint
    { 
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("minio/download-files", Handler);
        }
    }
    private static async Task<IResult> Handler(
           [FromBody] DownloadFilesRequest request,  
           IMinioFilesHttpClient fileProvider,
           CancellationToken ct)
    {
        var result = await fileProvider.DownloadFiles(request, ct);
        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok();
    }
}
