using PetHome.Infrastructure.Providers.Minio;
using System.Threading.Channels;

namespace PetHome.Infrastructure.MessageQueues;

public interface IMessageQueue
{
    public Task WriteAsync(MinioFileInfoDto filesInfoDto, CancellationToken ct);
    public Task<MinioFileInfoDto> ReadAsync(CancellationToken ct);
}