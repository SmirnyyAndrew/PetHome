using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Endpoints;

namespace FilesService.Features;

public static class UploadPresignedUrl
{
    private record UploadPresignedUrlRequest(
       string BucketName,
       string FileName,
       string ContentType,
       long Size);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned", Handler);
        }
    }
    private static async Task<IResult> Handler(
            UploadPresignedUrlRequest request,
           IAmazonS3 s3Client,
           CancellationToken ct)
    {
        try
        {
            Guid key = Guid.NewGuid();

            GetPreSignedUrlRequest presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = request.BucketName,
                Key = key.ToString(),
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

            return Results.Ok(new
            {
                key,
                url = presignedUrl
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3: upload presigned url failed: \r\t\n{ex.Message}");
        }
    }
}
