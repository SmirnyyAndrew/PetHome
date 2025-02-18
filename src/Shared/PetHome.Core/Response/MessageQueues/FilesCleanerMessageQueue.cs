using FilesService.Core.Dto.File;
using PetHome.Core.Response.Messaging;
using System.Threading.Channels;

namespace PetHome.Core.Response.MessageQueues;
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
