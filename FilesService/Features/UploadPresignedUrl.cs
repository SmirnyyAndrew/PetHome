using Amazon.S3; 
using FilesService.Endpoints;

namespace FilesService.Features;

public static class UploadPresignedUrl
{
    private record UploadPresignedUrlRequest(string Name, long Size);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned-url", Handler);
        } 
    }
    private static async Task<IResult> Handler(
           UploadPresignedUrlRequest request,
           IAmazonS3 s3Client,
           CancellationToken ct)
    { 
        return Results.Ok("Everything is working");
    }
}
