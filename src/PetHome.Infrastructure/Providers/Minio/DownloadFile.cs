using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio.DataModel;
using Minio.DataModel.Args;
using PetHome.Application.Interfaces;
using PetHome.Domain.Shared.Error;

namespace PetHome.Infrastructure.Providers.Minio;
public partial class MinioProvider : IFilesProvider
{
    //Скачать файл
    public async Task<Result<string, Error>> DownloadFile(
         MinioFileInfoDto fileInfoDto, string fileSavePath, CancellationToken ct)
    {
        var isExistBucketResult = await CheckIsExistBucket(fileInfoDto.BucketName, ct);
        if (isExistBucketResult.IsFailure)
            return isExistBucketResult.Error;

        var requestParams = new Dictionary<string, string>(StringComparer.Ordinal)
        {{"response-content-type","application/json"}};


        try
        {
            string fileExtension = Path.GetExtension(fileInfoDto.FileName);
            fileSavePath = $"{fileSavePath}{fileExtension}";
            var minioFileArgs = new GetObjectArgs()
                .WithBucket(fileInfoDto.BucketName)
                .WithObject(fileInfoDto.FileName)
                .WithFile(fileSavePath);

            ObjectStat presignedUrl = await _minioClient.GetObjectAsync(minioFileArgs, ct)
                .ConfigureAwait(false);

            _logger.LogInformation($"Файл {fileInfoDto.FileName} из bucket {fileInfoDto.BucketName} сохранён по пути = {fileSavePath}");
            return fileSavePath;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Файл {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName} не найден");
            return Errors.Failure($"Файл {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName} не найден");
        }
    }
}
