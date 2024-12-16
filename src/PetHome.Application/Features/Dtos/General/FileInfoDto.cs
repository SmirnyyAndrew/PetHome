namespace PetHome.Infrastructure.Providers.Minio;
public record FileInfoDto(string BucketName, IEnumerable<string> FileNames);