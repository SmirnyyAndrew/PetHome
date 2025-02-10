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
            app.MapPost("files/part-presigned", Handler);
        }
    }
    private static async Task<IResult> Handler(
           UploadPresignedPartUrlRequest request,
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
                //ContentType = request.ContentType,
                Protocol = Protocol.HTTP, 
                UploadId = request.UploadId,
                PartNumber = request.PartNumber
            };

            string? presignedUrl = await s3Client.GetPreSignedURLAsync(presignedRequest);

            return Results.Ok(new
            {
                key,
                presignedUrl
            });
        }
        catch (Exception ex)
        {
            return Results.BadRequest($"S3: part upload presigned url failed: \r\t\n{ex}");
        }
    }
}
