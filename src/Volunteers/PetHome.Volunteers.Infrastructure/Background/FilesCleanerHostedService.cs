using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetHome.Core.Response.Messaging;

namespace PetHome.Volunteers.Infrastructure.Background;
public class FilesCleanerHostedService : BackgroundService
{  
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<FilesCleanerHostedService> _logger;
    private readonly IMessageQueue _messageQueue;

    public FilesCleanerHostedService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<FilesCleanerHostedService> logger,
        IMinioFilesHttpClient FilesHttpClient,
        IMessageQueue messageQueue)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _messageQueue = messageQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateAsyncScope();
        var fileProvider = scope.ServiceProvider.GetRequiredService<IMinioFilesHttpClient>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var readResult = await _messageQueue.ReadAsync(stoppingToken);
            MinioFilesInfoDto fileInfoDto = new MinioFilesInfoDto(
                readResult.BucketName,
                readResult.FileNames.Select(f => MinioFileName.Create(f.Value).Value));

            await fileProvider.DeleteFile(fileInfoDto, stoppingToken);
        }
        await Task.CompletedTask;
    } 
}
