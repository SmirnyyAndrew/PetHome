using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Endpoints;

namespace FilesService.Features;

public static class CompleteMultipartUpload
{
    public record PartETagInfo(int PartNumber, string ETag);
     
    private record CompleteMultipartRequest(
       string BucketName,
       string UploadId,
       List<PartETagInfo> Parts);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/complite-multipart/presigned", Handler);
        }
    }
    private static async Task<IResult> Handler(
           Guid key,
           CompleteMultipartRequest request,
           IAmazonS3 s3Client,
           CancellationToken ct)
    {
        try
        {
            var presignedRequest = new CompleteMultipartUploadRequest
            {
                BucketName = request.BucketName,
                Key = key.ToString(),
                UploadId = request.UploadId,
                PartETags = request.Parts
                    .Select(p => new PartETag(p.PartNumber, p.ETag)).ToList()
            };

            var response = await s3Client.CompleteMultipartUploadAsync(
                presignedRequest, ct);

            return Results.Ok(new
            {
                key,
                response.Location
            });
        }
        catch (Exception ex)
        {
            return Results.BadRequest($"S3: complete multipart upload failed: \r\t\n{ex}");
        }
    }
}
