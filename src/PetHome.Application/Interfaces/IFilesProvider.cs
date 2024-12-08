using CSharpFunctionalExtensions;
using PetHome.Application.Features.Dtos;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.Shared.Error;
using PetHome.Infrastructure.Providers.Minio;

namespace PetHome.Application.Interfaces;
public interface IFilesProvider
{
    //Загрузить файл
    public Task<Result<Media, Error>> UploadFile(
       Stream stream,
       string bucketName,
       string filename,
       bool createBucketIfNotExist,
       CancellationToken ct);

    //Загрузить несколько файлов
    public Task<Result<IReadOnlyList<Media>, Error>> UploadFile(
        IEnumerable<Stream> streams,
        string bucketName,
        IEnumerable<string> fileNames,
        bool createBucketIfNotExist,
        CancellationToken ct);

    //Загрузить файл
    public Task<UnitResult<Error>> UploadFileWithDataChecking(
       Stream stream,
       string bucketName,
       string filename,
       bool createBucketIfNotExist,
       CancellationToken ct);

    public Task<Result<string, Error>> DeleteFile(
         MinioFileInfoDto fileInfoDto, CancellationToken ct);

    public Task<Result<string, Error>> DownloadFile(
         MinioFileInfoDto fileInfoDto, string fileSavePath, CancellationToken ct);

    public Task<Result<string, Error>> GetFilePresignedPath(
        MinioFileInfoDto fileInfoDto, CancellationToken ct);
}
