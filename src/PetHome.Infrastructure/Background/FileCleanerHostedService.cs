using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetHome.Application.Interfaces;
using PetHome.Infrastructure.MessageQueues;

namespace PetHome.Infrastructure.Background;
public class FileCleanerHostedService : BackgroundService
{
    private readonly IMessageQueue _messagequeue;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FileCleanerHostedService(
        IMessageQueue messagequeue,
        IServiceScopeFactory serviceScopeFactory)
    {
        _messagequeue = messagequeue;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateAsyncScope();
        var fileProvider = scope.ServiceProvider.GetRequiredService<IFilesProvider>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var readResult = await _messagequeue.ReadAsync(stoppingToken);

            //do code
            readResult.Select(x => x);
        }
    }
}
