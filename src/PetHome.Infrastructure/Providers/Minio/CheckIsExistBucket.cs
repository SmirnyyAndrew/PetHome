using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Interfaces;
using PetHome.Domain.Shared.Error;

namespace PetHome.Infrastructure.Providers.Minio;
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
            _logger.LogError($"Bucket с именем {bucketName} не существует");
            return Errors.NotFound($"Bucket с именем {bucketName}");
        }
        return bucketName;
    }

}
