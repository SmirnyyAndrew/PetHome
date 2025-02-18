namespace FilesService.Core.Request.AmazonS3.MultipartUpload;
public record UploadPresignedPartUrlRequest(
       string BucketName,
       string UploadId,
       int PartNumber);