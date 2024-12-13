using System.Threading.Channels;

namespace PetHome.Infrastructure.MessageQueues;

public interface IMessageQueue
{
    public Task WriteAsync(IEnumerable<string> files, CancellationToken ct);
    public Task<IEnumerable<string>> ReadAsync(CancellationToken ct);
}