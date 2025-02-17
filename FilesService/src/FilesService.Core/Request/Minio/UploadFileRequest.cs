using FilesService.Core.Dto.File;
using Microsoft.AspNetCore.Http;

namespace FilesService.Core.Request.Minio;
public record UploadFileRequest(Stream Stream, MinioFileInfoDto FileInfo, bool CreateBucketIfNotExist);
