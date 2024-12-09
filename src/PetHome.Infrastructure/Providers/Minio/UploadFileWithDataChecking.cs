using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio.DataModel.Args;
using PetHome.Application.Interfaces;
using PetHome.Domain.Shared.Error;

namespace PetHome.Infrastructure.Providers.Minio;
public partial class MinioProvider : IFilesProvider
{
    //Загрузить файл
    public async Task<UnitResult<Error>> UploadFileWithDataChecking(
        Stream stream,
        string bucketName,
        string filename,
        bool createBucketIfNotExist,
        CancellationToken ct)
    {
        var isExistBucketResult = await CheckIsExistBucket(bucketName, ct);
        if (isExistBucketResult.IsFailure && createBucketIfNotExist == false)
        {
            _logger.LogError($"Bucket с именем {bucketName} не найден");
            return Errors.Failure($"Bucket с именем {bucketName} не найден");
        }
        else
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs, ct);
        }

        await UploadFile(stream, bucketName, filename, createBucketIfNotExist, ct);

        return Result.Success<Error>();
    }
}
