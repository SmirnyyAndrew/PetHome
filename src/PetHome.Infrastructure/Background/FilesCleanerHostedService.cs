using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetHome.Application.Interfaces;
using PetHome.Application.Messaging;
using PetHome.Infrastructure.Providers.Minio;

namespace PetHome.Infrastructure.Background;
public class FilesCleanerHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<FilesCleanerHostedService> _logger;
    private readonly IFilesProvider _filesProvider;
    private readonly IMessageQueue _messageQueue;

    public FilesCleanerHostedService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<FilesCleanerHostedService> logger,
        IFilesProvider filesProvider,
        IMessageQueue messageQueue)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _filesProvider = filesProvider;
        _messageQueue = messageQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateAsyncScope();
        var fileProvider = scope.ServiceProvider.GetRequiredService<IFilesProvider>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var readResult = await _messageQueue.ReadAsync(stoppingToken); 
            MinioFilesInfoDto fileInfoDto = new MinioFilesInfoDto(
                readResult.BucketName,
                readResult.FileNames.Select(f => MinioFileName.Create(f.Value).Value));

            await _filesProvider.DeleteFile(fileInfoDto, stoppingToken);
        }
        await Task.CompletedTask;
    }
}
