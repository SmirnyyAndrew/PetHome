using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Interfaces;
using Minio.DataModel.Args;

namespace FilesService.Infrastructure.Minio;
public partial class MinioProvider : IMinioFilesHttpClient

{ 
    /// <summary>
    /// Получить ссылки для загрузки файлов в minio
    /// </summary>
    /// <param name="fileInfoDto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<Result<List<string>, string>> GetFilePresignedPath(
        MinioFilesInfoDto fileInfoDto, CancellationToken ct)
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
                    .WithObject(fileName.Value)
                    .WithExpiry(1000)
                    .WithHeaders(requestParams);
                var presignedUrl = await _minioClient
                    .PresignedGetObjectAsync(minioPresignedArgs)
                    .ConfigureAwait(false);

                _logger.LogInformation("Для файла {0} в bucket {1} получена временная ссылка для скачивание = {2}",
                    fileName, fileInfoDto.BucketName, presignedUrl);
                presignedPathes.Add(presignedUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError("{0}\n\t{1}\n\tФайл {2} в bucket {3} не найден",
                   ex.Source, ex.InnerException, fileName, fileInfoDto.BucketName);
                return ex.Message;
            }
        }
        return presignedPathes;
    }
}
