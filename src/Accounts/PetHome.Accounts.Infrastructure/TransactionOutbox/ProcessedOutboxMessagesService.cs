using MassTransit;
using Microsoft.Extensions.Logging;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts;
using PetHome.Accounts.Domain.Others;
using PetHome.Accounts.Infrastructure.Database;
using Polly;
using Polly.Retry;
using System.Text.Json;

namespace PetHome.Accounts.Infrastructure.TransactionOutbox;
public class ProcessedOutboxMessagesService
{
    private readonly IOutboxMessageRepository _repository;
    private readonly AuthorizationDbContext _dbContext;
    private readonly ILogger<ProcessedOutboxMessagesService> _logger;
    private readonly IPublishEndpoint _publisher;

    public ProcessedOutboxMessagesService(
        IOutboxMessageRepository repository,
        AuthorizationDbContext dbContext,
        ILogger<ProcessedOutboxMessagesService> logger,
        IPublishEndpoint publisher)
    {
        _repository = repository;
        _dbContext = dbContext;
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Execute(CancellationToken ct)
    {
        var messages = await _repository.Take(20, ct);
        if (messages.Count() == 0)
            return;

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromSeconds(1),
                ShouldHandle = new PredicateBuilder().Handle<Exception>(e => e is not NullReferenceException),
                OnRetry = args =>
                {
                    _logger.LogCritical(args.Outcome.Exception, "message");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        var processingTasks = messages.Select(message => ProcessMessageAsync(message, pipeline, ct));
        await Task.WhenAll(processingTasks);

        try
        {
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction outbox: failed to save data into the database"); 
        }  
    }


    private async Task ProcessMessageAsync(
        OutboxMessage message, ResiliencePipeline pipeline, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        try
        {
            var messageType = AssemblyReference.Assembly.GetType(message.Type)
                    ?? throw new ArgumentNullException("Message type wasn't found");

            var deserializedMessage = JsonSerializer.Deserialize(message.Payload, messageType)
                    ?? throw new ArgumentNullException("Message payload wasn't found");

            await pipeline.ExecuteAsync(async token =>
            {
                await _publisher.Publish(deserializedMessage, messageType, ct);
                message.ProcessedOn = DateTime.UtcNow;
            }, ct);
        }
        catch (Exception ex)
        {
            message.Error = ex.Message;
            message.ProcessedOn = DateTime.UtcNow;
            _logger.LogError("Failed to procees the message = {0}", message.Id);
        }

        return;
    }
}
