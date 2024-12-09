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


        foreach (var fileName in fileInfoDto.FileNames)
        {
            try
            {
                string fileExtension = Path.GetExtension(fileName);
                fileSavePath = $"{fileSavePath}{fileExtension}";
                var minioFileArgs = new GetObjectArgs()
                    .WithBucket(fileInfoDto.BucketName)
                    .WithObject(fileName)
                    .WithFile(fileSavePath);

                ObjectStat presignedUrl = await _minioClient.GetObjectAsync(minioFileArgs, ct)
                    .ConfigureAwait(false);

                _logger.LogInformation($"Файл {fileName} из bucket {fileInfoDto.BucketName} сохранён по пути = {fileSavePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Файл {fileName} в bucket {fileInfoDto.BucketName} не найден");
            }
        }
        string message = $"В bucket {fileInfoDto.BucketName} скачены {string.Join("\n\t\n\t", fileInfoDto.FileNames)}";
        return message;
    }
}
