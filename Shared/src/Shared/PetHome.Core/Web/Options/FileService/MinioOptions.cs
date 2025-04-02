namespace PetHome.Core.Web.Options.FileService;
public class MinioOptions
{
    public const string MINIO_NAME = "Minio";
    public string Endpoint { get; init; } = String.Empty;
    public string Accesskey { get; init; } = String.Empty;
    public string Secretkey { get; init; } = String.Empty;
    public bool IsWithSSL { get; init; } = false;
}
