using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio.DataModel.Args;
using PetHome.Application.Interfaces;
using PetHome.Domain.Shared.Error;

namespace PetHome.Infrastructure.Providers.Minio;
public partial class MinioProvider : IFilesProvider

{ //Получить ссылку на файл
    public async Task<Result<List<string>, Error>> GetFilePresignedPath(
        MinioFileInfoDto fileInfoDto, CancellationToken ct)
    {
        List<string> presignedPathes = new List<string>();

        foreach (var fileName in fileInfoDto.FileNames)
        {
            var requestParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                 {"response-content-type", "application/json" },
                 //Автоматическое скачивание файла после перехода по ссылке
                 {"response-content-disposition", $"attachment; filename=\"{fileName}\"" }
            };

            try
            {
                var minioPresignedArgs = new PresignedGetObjectArgs()
                    .WithBucket(fileInfoDto.BucketName)
                    .WithObject(fileName)
                    .WithExpiry(1000)
                    .WithHeaders(requestParams);
                var presignedUrl = await _minioClient
                    .PresignedGetObjectAsync(minioPresignedArgs)
                    .ConfigureAwait(false);

                _logger.LogInformation($"Для файла {fileName} в bucket {fileInfoDto.BucketName} получена временная ссылка для скачивание = {presignedUrl}");
                presignedPathes.Add(presignedUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source}\n{ex.InnerException}\nФайл {fileName} в bucket {fileInfoDto.BucketName} не найден");
            }
        }
        return presignedPathes;
    }
}
