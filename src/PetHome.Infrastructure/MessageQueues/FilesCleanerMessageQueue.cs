using PetHome.Infrastructure.Providers.Minio;
using System.Threading.Channels;

namespace PetHome.Infrastructure.MessageQueues;
public class FilesCleanerMessageQueue : IMessageQueue
{
    private readonly Channel<MinioFileInfoDto> _channel = Channel.CreateUnbounded<MinioFileInfoDto>();

    public async Task WriteAsync(MinioFileInfoDto filesInfoDto, CancellationToken ct)
    {
        await _channel.Writer.WriteAsync(filesInfoDto, ct);
    }

    public async Task<MinioFileInfoDto> ReadAsync(CancellationToken ct)
    {
        return await _channel.Reader.ReadAsync(ct);
    }
}
