namespace FilesService.Core.Request;

public record StartMultipartUploadRequest(
       string BucketName,
       string FileName,
       string ContentType,
       long Size);