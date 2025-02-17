using FilesService.Core.Dto.File;

namespace PetHome.Core.Response.Messaging;

public interface IMessageQueue
{
    public Task WriteAsync(MinioFilesInfoDto filesInfoDto, CancellationToken ct);
    public Task<MinioFilesInfoDto> ReadAsync(CancellationToken ct);
}