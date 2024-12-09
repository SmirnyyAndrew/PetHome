using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio.DataModel.Args;
using PetHome.Application.Interfaces;
using PetHome.Domain.Shared.Error;

namespace PetHome.Infrastructure.Providers.Minio;
public partial class MinioProvider : IFilesProvider

{ //Получить ссылку на файл
    public async Task<Result<string, Error>> GetFilePresignedPath(
        MinioFileInfoDto fileInfoDto, CancellationToken ct)
    {
        var requestParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
             {"response-content-type", "application/json" },
             //Автоматическое скачивание файла после перехода по ссылке
             {"response-content-disposition", $"attachment; filename=\"{fileInfoDto.FileName}\"" }
        };

        try
        {
            var minioPresignedArgs = new PresignedGetObjectArgs()
                .WithBucket(fileInfoDto.BucketName)
                .WithObject(fileInfoDto.FileName)
                .WithExpiry(1000)
                .WithHeaders(requestParams);
            var presignedUrl = await _minioClient
                .PresignedGetObjectAsync(minioPresignedArgs)
                .ConfigureAwait(false);

            _logger.LogInformation($"Для файла {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName} получена временная ссылка для скачивание = {presignedUrl}");
            return presignedUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Source}\n{ex.InnerException}\nФайл {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName} не найден");
            return Errors.Failure($"Файл {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName} не найден");
        }

    }
}
