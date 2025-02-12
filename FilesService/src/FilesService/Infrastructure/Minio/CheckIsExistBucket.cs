using CSharpFunctionalExtensions;
using FilesService.Application.Interfaces;
using FilesService.Core.ErrorManagment;

namespace FilesService.Infrastructure.Minio;
public partial class MinioProvider : IFilesProvider
{
    // Проверить, существует ли bucket
    private async Task<Result<string, Error>> CheckIsExistBucket(string bucketName, CancellationToken ct)
    {
        //Получить бакеты
        var buckets = await _minioClient.ListBucketsAsync(ct);
        IReadOnlyList<string> bucketNames = buckets.Buckets.Select(x => x.Name.ToLower()).ToList();

        if (bucketNames.Any(x => x == bucketName) == false)
        {
            _logger.LogError("Bucket с именем {0} не существует", bucketName);
            return Errors.NotFound($"Bucket с именем {bucketName}");
        }
        return bucketName;
    }

}
