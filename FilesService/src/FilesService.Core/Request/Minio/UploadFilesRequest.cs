using FilesService.Core.Dto.File;

namespace FilesService.Core.Request.Minio;
public record UploadFilesRequest(IEnumerable<Stream> Streams, MinioFilesInfoDto FileInfoDto, bool CreateBucketIfNotExist);
