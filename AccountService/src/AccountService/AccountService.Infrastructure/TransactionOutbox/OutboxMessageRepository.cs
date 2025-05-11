using AccountService.Application.Database.Repositories;
using AccountService.Domain.Others;
using AccountService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AccountService.Infrastructure.TransactionOutbox;
public class OutboxMessageRepository(AuthorizationDbContext dbContext)
    : IOutboxMessageRepository
{
    public async Task Add<T>(T message, CancellationToken ct)
    {
        OutboxMessage outboxMessage = new OutboxMessage()
        {
            Id = Guid.NewGuid(),
            Payload = JsonSerializer.Serialize(message.ToString(), JsonSerializerOptions.Default),
            Type = typeof(T).FullName!,
            OccurredOn = DateTime.UtcNow,
            ProcessedOn = null,
            Error = string.Empty,
        };

        await dbContext.OutboxMessages.AddAsync(outboxMessage, ct);
        return;
    }

    public async Task<IReadOnlyList<OutboxMessage>> Take(int count, CancellationToken ct)
    {
        var messages = await dbContext.OutboxMessages
            .Where(m => m.ProcessedOn == null)
            .OrderBy(m => m.OccurredOn)
            .Take(count)
            .ToListAsync(ct);
        return messages;
    }
}
