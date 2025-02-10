using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Endpoints;

namespace FilesService.Features;

public static class GetPresignedUrl
{
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("files/{key:guid}/presigned-url", Handler);
        }

        private record GetPresignedUrlRequest(string BucketName);

        private async Task<IResult> Handler(
            GetPresignedUrlRequest request,
            Guid key,
            IAmazonS3 s3Client,
            CancellationToken ct)
        {
            try
            {
                GetPreSignedUrlRequest getPresignedUrlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = request.BucketName,
                    Key = key.ToString(),
                    Verb = HttpVerb.GET,
                    Expires = DateTime.UtcNow.AddDays(14),
                    Protocol = Protocol.HTTP,
                };

                string? presignedUrl = await s3Client.GetPreSignedURLAsync(getPresignedUrlRequest);

                return Results.Ok(new
                {
                    key,
                    presignedUrl
                });
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"S3: get presigned url failed: \r\t\n{ex}");
            }
        }
    }
}
