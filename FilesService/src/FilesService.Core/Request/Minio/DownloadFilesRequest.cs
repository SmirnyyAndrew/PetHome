using FilesService.Core.Dto.File;

namespace FilesService.Core.Request.Minio;
public record DownloadFilesRequest(MinioFilesInfoDto FileInfoDto, string FileSavePath);
