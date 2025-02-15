using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Request.Minio;

namespace FilesService.Application.Interfaces;
public interface IFilesProvider
{
    //Загрузить файл
    public Task<Result<MediaFile, Error>> UploadFile(
       Stream stream,
       UploadFileRequest request,
       CancellationToken ct);

    //Загрузить несколько файлов
    public Task<Result<IReadOnlyList<MediaFile>, Error>> UploadFiles(
        IEnumerable<Stream> streams,
        UploadFilesRequest request,
        CancellationToken ct);

    //Загрузить файл
    public Task<UnitResult<Error>> UploadFileWithDataChecking(
       Stream stream,
       UploadFileRequest request,
       CancellationToken ct);

    public Task<Result<string, Error>> DeleteFile(
         MinioFilesInfoDto fileInfoDto, CancellationToken ct);

    public Task<Result<string, Error>> DownloadFiles(
         DownloadFilesRequest request, CancellationToken ct);

    public Task<Result<List<string>, Error>> GetFilePresignedPath(
        MinioFilesInfoDto fileInfoDto, CancellationToken ct);

    public MinioFileName InitName(string filePath);
}
