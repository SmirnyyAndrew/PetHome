using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Application.Endpoints;
using FilesService.Core.Request.AmazonS3;
using FilesService.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.AmazonS3;

public static class UploadPresignedUrl
{  
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("amazon/files/presigned", Handler);
        }
    }
    private static async Task<IResult> Handler(
           [FromBody] UploadPresignedUrlRequest request,
           IAmazonS3 s3Client,
           CancellationToken ct)
    {
        try
        {
            string key = Guid.NewGuid().ToString();

            GetPreSignedUrlRequest presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = request.BucketName,
                Key = key,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddDays(14),
                ContentType = request.ContentType,
                Protocol = Protocol.HTTP,
                Metadata =
                {
                    ["file-name"] = request.ContentType
                }
            };

            string? presignedUrl = await s3Client.GetPreSignedURLAsync(presignedRequest);

            FileUrlResponse response = new FileUrlResponse(key, presignedUrl); 
            return Results.Ok(response);
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3: upload presigned url failed: \r\t\n{ex.Message}");
        }
    }
}
