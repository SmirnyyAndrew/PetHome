using FilesService.Core.Interfaces;
using Minio;

namespace FilesService.Infrastructure.Minio;

/// <summary>
/// Provider для работы с minio
/// </summary>
public partial class MinioProvider : IMinioFilesHttpClient
{
    private readonly int MAX_STREAMS_LENGHT = 5;
    private IMinioClient _minioClient;
    private ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }
}
