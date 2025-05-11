using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Application.Endpoints;
using FilesService.Core.Request.AmazonS3;
using FilesService.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.AmazonS3.MultipartUpload;

public static class GetPresignedUrl
{
    public sealed class EndPoint : IEndpoint
    {

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("amazon/files/{key}/presigned", Handler);
        }
    }

    /// <summary>
    /// Получить ссылку для загрузки файла
    /// </summary>
    /// <param name="key">Название файла в Amazon s3</param>
    /// <param name="request"></param>
    /// <param name="s3Client"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
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

            FileUrlResponse response = new FileUrlResponse(key, presignedUrl);
            return Results.Ok(response);
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3: get presigned url failed: \r\t\n{ex.Message}");
        }
    }
}

