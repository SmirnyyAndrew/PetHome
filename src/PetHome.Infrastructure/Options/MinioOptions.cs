namespace PetHome.Infrastructure.Options;
public class MinioOptions
{
    public const string MINIO_NAME = "Minio";
    public string Endpoint { get; init; } = String.Empty;
    public string AccessKey { get; init; } = String.Empty;
    public string SecretKey { get; init; } = String.Empty;
    public bool IsWithSSL { get; init; } = false;
}
