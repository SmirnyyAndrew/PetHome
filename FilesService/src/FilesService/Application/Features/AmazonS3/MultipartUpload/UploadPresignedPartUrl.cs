using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Application.Endpoints;
using FilesService.Core.Request.AmazonS3.MultipartUpload;
using FilesService.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.AmazonS3.MultipartUpload;

public static class UploadPresignedPartUrl
{ 
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("amazon/files/{key}/part-presigned", Handler);
        }
    }

    /// <summary>
    /// Получить ссылку для загрузки части файла
    /// </summary>
    /// <param name="key">Название файла в Amazon s3</param>
    /// <param name="request"></param>
    /// <param name="s3Client"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private static async Task<IResult> Handler(
           [FromRoute] string key,
           [FromBody] UploadPresignedPartUrlRequest request,
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

            FileUrlResponse response = new FileUrlResponse(key, presignedUrl); 
            return Results.Ok(response);
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3: part upload presigned url failed: \r\t\n{ex.Message}");
        }
    }
}
