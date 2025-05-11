using AccountService.Domain.Others;

namespace AccountService.Application.Database.Repositories;
public interface IOutboxMessageRepository
{
    public Task Add<T>(T message, CancellationToken ct);
    public Task<IReadOnlyList<OutboxMessage>> Take(int count, CancellationToken ct);
}
