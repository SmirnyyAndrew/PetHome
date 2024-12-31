using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio.DataModel.Args;
using PetHome.Core.Interfaces;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects;

namespace PetHome.SharedKernel.Providers.Minio;
public partial class MinioProvider : IFilesProvider
{
    //Загрузить файл
    public async Task<Result<Media, Error>> UploadFile(
         Stream stream,
       MinioFileInfoDto fileInfo,
       bool createBucketIfNotExist,
       CancellationToken ct)
    {
        PutObjectArgs minioFileArgs = new PutObjectArgs()
        .WithBucket(fileInfo.BucketName.ToLower())
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithObject(fileInfo.FileName.Value);

        var result = await _minioClient.PutObjectAsync(minioFileArgs, ct);
        string message = $"Файл {result.ObjectName} загружен в bucket = {fileInfo.BucketName}";
        _logger.LogInformation(message);

        return Media.Create(fileInfo.BucketName, fileInfo.FileName.Value);
    }
}
