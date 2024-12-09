using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio.DataModel.Args;
using PetHome.Application.Interfaces;
using PetHome.Domain.Shared.Error;

namespace PetHome.Infrastructure.Providers.Minio;
public partial class MinioProvider : IFilesProvider
{
    //Удалить файл
    public async Task<Result<string, Error>> DeleteFile(
         MinioFileInfoDto fileInfoDto, CancellationToken ct)
    {
        var isExistBucketResult = await CheckIsExistBucket(fileInfoDto.BucketName, ct);
        if (isExistBucketResult.IsFailure)
            return isExistBucketResult.Error;


        var minioFileArgs = new RemoveObjectArgs()
            .WithBucket(fileInfoDto.BucketName)
            .WithObject(fileInfoDto.FileName);
        await _minioClient.RemoveObjectAsync(minioFileArgs).ConfigureAwait(false);

        _logger.LogInformation($"Файл {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName}  успешно удалён");
        return $"Файл {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName} успешно удалён";
    }
}
