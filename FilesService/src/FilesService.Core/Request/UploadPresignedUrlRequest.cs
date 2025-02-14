namespace FilesService.Core.Request;
public record UploadPresignedUrlRequest(
   string BucketName,
   string FileName,
   string ContentType,
   long Size);