using CSharpFunctionalExtensions;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Models.File;

namespace FilesService.Application.Interfaces;
public interface IFilesProvider
{
    //Загрузить файл
    public Task<Result<MediaFile, Error>> UploadFile(
       Stream stream,
       MinioFileInfoDto fileInfo,
       bool createBucketIfNotExist,
       CancellationToken ct);

    //Загрузить несколько файлов
    public Task<Result<IReadOnlyList<MediaFile>, Error>> UploadFile(
        IEnumerable<Stream> streams,
        MinioFilesInfoDto fileInfoDto,
        bool createBucketIfNotExist,
        CancellationToken ct);

    

    //Загрузить файл
    public Task<UnitResult<Error>> UploadFileWithDataChecking(
       Stream stream,
       MinioFileInfoDto fileInfo,
       bool createBucketIfNotExist,
       CancellationToken ct);

    public Task<Result<string, Error>> DeleteFile(
         MinioFilesInfoDto fileInfoDto, CancellationToken ct);

    public Task<Result<string, Error>> DownloadFiles(
         MinioFilesInfoDto fileInfoDto, string fileSavePath, CancellationToken ct);

    public Task<Result<List<string>, Error>> GetFilePresignedPath(
        MinioFilesInfoDto fileInfoDto, CancellationToken ct);

    public MinioFileName InitName(string filePath);
}
