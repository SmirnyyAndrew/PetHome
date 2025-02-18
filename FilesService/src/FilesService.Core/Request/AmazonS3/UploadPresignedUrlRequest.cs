namespace FilesService.Core.Request.AmazonS3;
public record UploadPresignedUrlRequest(
   string BucketName,
   string FileName,
   string ContentType,
   long Size);