using Microsoft.EntityFrameworkCore;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Domain.Others;
using PetHome.Accounts.Infrastructure.Database;
using System.Text.Json;

namespace PetHome.Accounts.Infrastructure.TransactionOutbox;
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
