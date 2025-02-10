using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Endpoints;

namespace FilesService.Features;

public static class StartMultipartUpload
{
    private record StartMultipartUploadRequest(
       string BucketName,
       string FileName,
       string ContentType,
       long Size);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/multipart/presigned", Handler);
        }
    }
    private static async Task<IResult> Handler(
           StartMultipartUploadRequest request,
           IAmazonS3 s3Client,
           CancellationToken ct)
    {
        try
        {
            Guid key = Guid.NewGuid();

            var presignedRequest = new InitiateMultipartUploadRequest
            {
                BucketName = request.BucketName,
                Key = key.ToString(),
                ContentType = request.ContentType,
                Metadata =
                {
                    ["file-name"] = request.ContentType
                }
            };

            var response = await s3Client.InitiateMultipartUploadAsync(
                presignedRequest, ct);

            return Results.Ok(new
            {
                key,
                uploadId = response.UploadId
            });
        }
        catch (Exception ex)
        {
            return Results.BadRequest($"S3: start multipart upload failed: \r\t\n{ex}");
        }
    }
}
