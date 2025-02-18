using CSharpFunctionalExtensions;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Interfaces;
using FilesService.Core.Request.Minio;
using Minio.DataModel;
using Minio.DataModel.Args;

namespace FilesService.Infrastructure.Minio;
public partial class MinioProvider : IMinioFilesHttpClient
{
    //Скачать файл
    public async Task<UnitResult<string>> DownloadFiles(
         DownloadFilesRequest request, CancellationToken ct)
    {
        var isExistBucketResult = await CheckIsExistBucket(request.FileInfoDto.BucketName, ct);
        if (isExistBucketResult.IsFailure)
            return "Failed to download the file";

        var requestParams = new Dictionary<string, string>(StringComparer.Ordinal)
        {{"response-content-type","application/json"}};


        foreach (var fileName in request.FileInfoDto.FileNames)
        {
            try
            {
                string fileExtension = Path.GetExtension(fileName.Value);
                string fullFileName = $"{request.FileSavePath}{fileExtension}";
                var minioFileArgs = new GetObjectArgs()
                    .WithBucket(request.FileInfoDto.BucketName)
                    .WithObject(fileName.Value)
                    .WithFile(fullFileName);

                ObjectStat presignedUrl = await _minioClient.GetObjectAsync(minioFileArgs, ct)
                    .ConfigureAwait(false);

                _logger.LogInformation("Файл {0} из bucket {1} сохранён по пути = {2}",
                    fileName, request.FileInfoDto.BucketName, fullFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError("Файл {0} в bucket {1} не найден", fileName, request.FileInfoDto.BucketName);
            }
        }

        return Result.Success();
    }
}
