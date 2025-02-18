using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Infrastructure.MongoDB;
using Hangfire;

namespace FilesService.Application.Jobs;

public class ConfirmConsistencyJob(
    MongoDbRepository repository,
    IAmazonS3 s3Client,
    ILogger<ConfirmConsistencyJob> logger)
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [5, 10, 15])]
    public async Task<IResult> Execute(
        Guid fileId, string key, string bucketName, CancellationToken ct)
    {
        var getFileFromDbResult = await repository.Get(fileId, ct);

        GetObjectMetadataRequest metaDataRequest = new GetObjectMetadataRequest
        {
            BucketName = bucketName,
            Key = key,
        };
        var getFileFromS3Result = await s3Client.GetObjectMetadataAsync(metaDataRequest, cancellationToken: ct);

        bool isUploadSuccess = getFileFromDbResult.IsSuccess && getFileFromS3Result != null;

        if (isUploadSuccess is true)
            return Results.Ok();

        try
        {
            await repository.Remove(fileId, ct);
            logger.LogInformation("Файл с id = {fileId} удалён из БД", fileId);

            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };
            await s3Client.DeleteObjectAsync(deleteObjectRequest);
            logger.LogInformation("Файл с id = {fileId} удалён из S3", fileId);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Results.BadRequest(ex.Message);
        }
    }
}
