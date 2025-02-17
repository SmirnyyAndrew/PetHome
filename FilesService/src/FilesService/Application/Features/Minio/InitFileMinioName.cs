using FilesService.Application.Endpoints;
using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;
using FilesService.Core.Request.Minio;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.Minio;


public static class InitFileMinioName
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("minio/init-file-minio-name", Handler);
        }
    }
    private static MinioFileName Handler( 
           [FromQuery] string fileName,
           IMinioFilesHttpClient fileProvider)
    {   
        var result = fileProvider.InitName(fileName);  
        return result;
    }
}
