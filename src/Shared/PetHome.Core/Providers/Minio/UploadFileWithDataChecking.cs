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
       MinioFileInfoDto fileInfo,
       bool createBucketIfNotExist,
       CancellationToken ct)
    {
        var isExistBucketResult = await CheckIsExistBucket(fileInfo.BucketName, ct);
        if (isExistBucketResult.IsFailure && createBucketIfNotExist == false)
        {
            string message = "Bucket с именем {bucketName} не найден";
            _logger.LogError(message);
            return Errors.Failure(message);
        }
        else
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(fileInfo.BucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs, ct);
        }

        await UploadFile(stream, fileInfo, createBucketIfNotExist, ct);

        return Result.Success<Error>();
    }
}
