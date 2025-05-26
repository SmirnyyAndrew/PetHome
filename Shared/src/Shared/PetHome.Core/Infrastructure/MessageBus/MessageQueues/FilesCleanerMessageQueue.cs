using FilesService.Core.Dto.File;
using System.Threading.Channels;

namespace PetHome.Core.Infrastructure.MessageBus.MessageQueues;
public class FilesCleanerMessageQueue : IMessageQueue
{
    private readonly Channel<MinioFilesInfoDto> _channel = Channel.CreateUnbounded<MinioFilesInfoDto>();

    public async Task WriteAsync(MinioFilesInfoDto filesInfoDto, CancellationToken ct)
    {
        await _channel.Writer.WriteAsync(filesInfoDto, ct);
    }

    public async Task<MinioFilesInfoDto> ReadAsync(CancellationToken ct)
    {
        return await _channel.Reader.ReadAsync(ct);
    }
}
