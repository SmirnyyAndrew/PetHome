using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Application.Endpoints;
using FilesService.Infrastructure.MongoDB;
using FilesService.Infrastructure.MongoDB.Documents;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.AmazonS3.MultipartUpload;

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
            app.MapPost("files/{key}/complite-multipart/presigned", Handler);
        }
    }
    private static async Task<IResult> Handler(
          [FromRoute] string key,
          [FromBody] CompleteMultipartRequest request,
           IAmazonS3 s3Client,
           MongoDbRepository repository,
           CancellationToken ct)
    {
        try
        {
            Guid fileId = Guid.NewGuid();

            //TODO: job проверки файла в mongo и s3

            var presignedRequest = new CompleteMultipartUploadRequest
            {
                BucketName = request.BucketName,
                Key = key,
                UploadId = request.UploadId,
                PartETags = request.Parts
                    .Select(p => new PartETag(p.PartNumber, p.ETag)).ToList()
            };

            var response = await s3Client.CompleteMultipartUploadAsync(
                presignedRequest, ct);


            GetObjectMetadataRequest metaDataRequest = new GetObjectMetadataRequest
            {
                BucketName = request.BucketName,
                Key = key,
            };
            var metaData = await s3Client.GetObjectMetadataAsync(metaDataRequest, cancellationToken: ct);

            FileData fileData = new FileData
            {
                Id = fileId,
                StoragePath = key,
                UploadDate = DateTime.UtcNow,
                FileSize = metaData.Headers.ContentLength,
                ContentType = metaData.Headers.ContentType,
            };

            await repository.Add(fileData, ct);

            return Results.Ok(new
            {
                key,
                location = response.Location
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3: complete multipart upload failed: \r\t\n{ex.Message}");
        }
    }
}
