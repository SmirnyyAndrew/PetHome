using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Request.Minio;

namespace FilesService.Core.Interfaces;
public interface IMinioFilesHttpClient
{
    //Загрузить файл
    public Task<Result<MediaFile, string>> UploadFile( 
       UploadFileRequest request,
       CancellationToken ct);

    //Загрузить несколько файлов
    public Task<Result<IReadOnlyList<MediaFile>, string>> UploadFiles( 
        UploadFilesRequest request,
        CancellationToken ct);

    //Загрузить файл
    public Task<UnitResult<string>> UploadFileWithDataChecking( 
       UploadFileRequest request,
       CancellationToken ct);

    public Task<UnitResult<string>> DeleteFile(
         MinioFilesInfoDto fileInfoDto, CancellationToken ct);

    public Task<UnitResult<string>> DownloadFiles(
         DownloadFilesRequest request, CancellationToken ct);

    public Task<Result<List<string>, string>> GetFilePresignedPath(
        MinioFilesInfoDto fileInfoDto, CancellationToken ct);

    public Task<MinioFileName> InitName(string filePath);
}
