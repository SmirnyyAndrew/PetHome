using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Application.Endpoints;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.AmazonS3.MultipartUpload;

public static class GetPresignedUrl
{
    public sealed class EndPoint : IEndpoint
    {

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key}/presigned", Handler);
        }
    }

    private record GetPresignedUrlRequest(string BucketName);

    private static async Task<IResult> Handler(
            [FromRoute] string key,
            [FromBody] GetPresignedUrlRequest request,
            IAmazonS3 s3Client,
            CancellationToken ct)
    {
        try
        {
            GetPreSignedUrlRequest presignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = request.BucketName,
                Key = key,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddDays(14),
                Protocol = Protocol.HTTP,
            };

            string? presignedUrl = await s3Client.GetPreSignedURLAsync(presignedUrlRequest);

            return Results.Ok(new
            {
                key,
                url = presignedUrl
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3: get presigned url failed: \r\t\n{ex.Message}");
        }
    }
}

