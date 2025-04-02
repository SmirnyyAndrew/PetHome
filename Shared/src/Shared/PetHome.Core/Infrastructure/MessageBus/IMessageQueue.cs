using FilesService.Core.Dto.File;

namespace PetHome.Core.Infrastructure.MessageBus;

public interface IMessageQueue
{
    public Task WriteAsync(MinioFilesInfoDto filesInfoDto, CancellationToken ct);
    public Task<MinioFilesInfoDto> ReadAsync(CancellationToken ct);
}