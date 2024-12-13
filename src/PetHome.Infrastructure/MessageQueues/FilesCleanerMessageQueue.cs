using System.Threading.Channels;

namespace PetHome.Infrastructure.MessageQueues;
public class FilesCleanerMessageQueue : IMessageQueue
{
    private readonly Channel<IEnumerable<string>> _channel = Channel.CreateUnbounded<IEnumerable<string>>();

    public async Task WriteAsync(IEnumerable<string> files, CancellationToken ct)
    {
        await _channel.Writer.WriteAsync(files, ct);
    }

    public async Task<IEnumerable<string>> ReadAsync(CancellationToken ct)
    {
        return await _channel.Reader.ReadAsync(ct);
    }
}
