using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;
using PetHome.Infrastructure.Providers.Minio;

namespace PetHome.Application.Interfaces;
public interface IFilesProvider
{
    public Task<Result<string, Error>> UploadFile(
        Stream stream, string bucketName, string filename, bool createBucketIfNotExist, CancellationToken ct);

    public Task<Result<string, Error>> DeleteFile(
         MinioFileInfoDto fileInfoDto, CancellationToken ct);

    public Task<Result<string, Error>> DownloadFile(
         MinioFileInfoDto fileInfoDto, string fileSavePath, CancellationToken ct);

    public Task<Result<string, Error>> GetFilePresignedPath(
        MinioFileInfoDto fileInfoDto, CancellationToken ct);
}
