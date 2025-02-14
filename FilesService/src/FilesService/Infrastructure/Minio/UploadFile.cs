using CSharpFunctionalExtensions;
using FilesService.Application.Interfaces;
using FilesService.Core.Dto.File;
using FilesService.Core.ErrorManagment;
using Minio.DataModel.Args;

namespace FilesService.Infrastructure.Minio;
public partial class MinioProvider : IFilesProvider
{
    //Загрузить файл
    public async Task<Result<MediaFile, Error>> UploadFile(
       Stream stream,
       MinioFileInfoDto fileInfo,
       bool createBucketIfNotExist,
       CancellationToken ct)
    {
        PutObjectArgs minioFileArgs = new PutObjectArgs()
        .WithBucket(fileInfo.BucketName.ToLower())
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithObject(fileInfo.FileName);

        var result = await _minioClient.PutObjectAsync(minioFileArgs, ct);
        string message = $"Файл {result.ObjectName} загружен в bucket = {fileInfo.BucketName}";
        _logger.LogInformation(message);

        return MediaFile.Create(fileInfo.BucketName, fileInfo.FileName);
    }
}
