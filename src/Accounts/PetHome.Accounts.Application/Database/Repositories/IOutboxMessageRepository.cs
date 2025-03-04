using PetHome.Accounts.Domain.Others;

namespace PetHome.Accounts.Application.Database.Repositories;
public interface IOutboxMessageRepository
{
    public Task Add<T>(T message, CancellationToken ct);
    public Task<IReadOnlyList<OutboxMessage>> Take(int count, CancellationToken ct);
}
