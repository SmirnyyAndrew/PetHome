namespace FilesService.Core.Request.AmazonS3;

public record FilePresignedUrlRequest(string Key, string BucketName);
