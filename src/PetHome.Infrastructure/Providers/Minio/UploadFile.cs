using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio.DataModel.Args;
using PetHome.Application.Interfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.Shared.Error;

namespace PetHome.Infrastructure.Providers.Minio;
public partial class MinioProvider : IFilesProvider
{
    //Загрузить файл
    public async Task<Result<Media, Error>> UploadFile(
        Stream stream,
        string bucketName,
        MinioFileName filename,
        bool createBucketIfNotExist,
        CancellationToken ct) 
    {   
        PutObjectArgs minioFileArgs = new PutObjectArgs()
        .WithBucket(bucketName.ToLower())
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithObject(filename.Value);

        var result = await _minioClient.PutObjectAsync(minioFileArgs, ct);
        string message = $"Файл {result.ObjectName} загружен в bucket = {bucketName}";
        _logger.LogInformation(message);

        return Media.Create(bucketName, filename.Value);
    }
}
