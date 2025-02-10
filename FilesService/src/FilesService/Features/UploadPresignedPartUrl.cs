using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Endpoints;

namespace FilesService.Features;

public static class UploadPresignedPartUrl
{
    private record UploadPresignedPartUrlRequest(
       string BucketName,
       string UploadId,
       int PartNumber);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key}/part-presigned", Handler);
        }
    }
    private static async Task<IResult> Handler(
           string key,
           UploadPresignedPartUrlRequest request,
           IAmazonS3 s3Client,
           CancellationToken ct)
    {
        try
        {

            GetPreSignedUrlRequest presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = request.BucketName,
                Key = key,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddDays(14), 
                Protocol = Protocol.HTTP,
                UploadId = request.UploadId,
                PartNumber = request.PartNumber
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
            return Results.BadRequest($"S3: part upload presigned url failed: \r\t\n{ex.Message}");
        }
    }
}
