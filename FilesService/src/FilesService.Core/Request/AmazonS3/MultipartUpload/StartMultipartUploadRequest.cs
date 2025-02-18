namespace FilesService.Core.Request.AmazonS3.MultipartUpload;

public record StartMultipartUploadRequest(
       string BucketName,
       string FileName,
       string ContentType,
       long Size);