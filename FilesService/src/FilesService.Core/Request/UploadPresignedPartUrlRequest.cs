namespace FilesService.Core.Request;
public record UploadPresignedPartUrlRequest(
       string BucketName,
       string UploadId,
       int PartNumber);