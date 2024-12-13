using Microsoft.Extensions.Logging;
using Minio;
using PetHome.Application.Interfaces;

namespace PetHome.Infrastructure.Providers.Minio;

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
