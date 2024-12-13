using Microsoft.AspNetCore.Mvc;

namespace PetHome.Infrastructure.Providers.Minio;
public record MinioProviderDto(string BucketName, string FileName);