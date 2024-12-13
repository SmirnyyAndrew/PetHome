namespace PetHome.Infrastructure.Providers.Minio;
public record MinioFileInfoDto(string BucketName, IEnumerable<MinioFileName> FileNames);