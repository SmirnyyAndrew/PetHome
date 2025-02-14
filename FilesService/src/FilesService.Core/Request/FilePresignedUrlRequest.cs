namespace FilesService.Core.Request;

public record FilePresignedUrlRequest(string Key, string BucketName);
