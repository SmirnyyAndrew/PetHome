using Minio.DataModel;

namespace FilesService.Core.Response;
public record UploadPartFileResponse(string Key, string UploadId);

