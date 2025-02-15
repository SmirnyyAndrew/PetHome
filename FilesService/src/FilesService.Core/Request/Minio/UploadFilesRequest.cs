using FilesService.Core.Dto.File;

namespace FilesService.Core.Request.Minio;
public record UploadFilesRequest(MinioFilesInfoDto FileInfoDto, bool CreateBucketIfNotExist);
