using FilesService.Application.Interfaces;
using Minio;

namespace FilesService.Infrastructure.Minio;

//Этот класс является partial - реализация методов лежит в отдельных классах
public partial class MinioProvider : IFilesProvider
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
