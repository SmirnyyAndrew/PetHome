using CSharpFunctionalExtensions;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using PetHome.Application.Interfaces;
using PetHome.Domain.Shared.Error;
using System.Security.AccessControl;

namespace PetHome.Infrastructure.Providers.Minio;
public class MinioProvider : IFilesProvider
{
    private IMinioClient _minioClient;

    public MinioProvider(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    //Удалить файл
    public async Task<Result<string, Error>> DeleteFile(
         MinioFileInfoDto fileInfoDto, CancellationToken ct)
    {
        var isExistBucketResult = await IsExistBucket(fileInfoDto.BucketName, ct);
        if (isExistBucketResult.IsFailure)
            return isExistBucketResult.Error;


        var minioFileArgs = new RemoveObjectArgs()
            .WithBucket(fileInfoDto.BucketName)
            .WithObject(fileInfoDto.FileName);
        await _minioClient.RemoveObjectAsync(minioFileArgs).ConfigureAwait(false);

        return $"Файл {fileInfoDto.FileName} успешно удалён";
    }

    //Скачать файл
    public async Task<Result<string, Error>> DownloadFile(
         MinioFileInfoDto fileInfoDto, string fileSavePath, CancellationToken ct)
    {
        var isExistBucketResult = await IsExistBucket(fileInfoDto.BucketName, ct);
        if (isExistBucketResult.IsFailure)
            return isExistBucketResult.Error;

        var requestParams = new Dictionary<string, string>(StringComparer.Ordinal)
        {{"response-content-type","application/json"}};


        try
        {
            string fileExtension = Path.GetExtension(fileInfoDto.FileName);
            var minioFileArgs = new GetObjectArgs()
                .WithBucket(fileInfoDto.BucketName)
                .WithObject(fileInfoDto.FileName)
                .WithFile($"{fileSavePath}{fileExtension}");

            ObjectStat presignedUrl = await _minioClient.GetObjectAsync(minioFileArgs, ct)
                .ConfigureAwait(false);

            return fileInfoDto.FileName;
        }
        catch (Exception ex)
        {
            return Errors.Failure($"Файл {fileInfoDto.FileName} не найден");
        }
    }

    //Загрузить файл
    public async Task<Result<string, Error>> UploadFile(
        Stream stream,
        string bucketName,
        string filename,
        bool createBucketIfNotExist,
        CancellationToken ct)
    {
        var isExistBucketResult = await IsExistBucket(bucketName, ct);
        if (isExistBucketResult.IsFailure && createBucketIfNotExist == true)
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs, ct);
        }

        //Расширение файла
        string fileExtension = Path.GetExtension(filename);

        Guid newFilePath = Guid.NewGuid();
        PutObjectArgs minioFileArgs = new PutObjectArgs()
            .WithBucket(bucketName.ToLower())
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithObject($"{newFilePath}{fileExtension}");

        var result = await _minioClient.PutObjectAsync(minioFileArgs, ct);

        return result.ObjectName;
    }

    //Получить ссылку на файл
    public async Task<Result<string, Error>> GetFilePresignedPath(
        MinioFileInfoDto fileInfoDto, CancellationToken ct)
    {
        var requestParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
             {"response-content-type", "application/json" },
             //Автоматическое скачивание файла после перехода по ссылке
             {"response-content-disposition", $"attachment; filename=\"{fileInfoDto.FileName}\"" }
        };

        try
        {
            var minioPresignedArgs = new PresignedGetObjectArgs()
                .WithBucket(fileInfoDto.BucketName)
                .WithObject(fileInfoDto.FileName)
                .WithExpiry(1000)
                .WithHeaders(requestParams);
            var presignedUrl = await _minioClient
                .PresignedGetObjectAsync(minioPresignedArgs)
                .ConfigureAwait(false);

            return presignedUrl;
        }
        catch (Exception)
        {
            return Errors.Failure($"Файл {fileInfoDto.FileName} не найден");
        }

    }

    // Проверить, существует ли bucket
    private async Task<Result<string, Error>> IsExistBucket(string bucketName, CancellationToken ct)
    {
        //Получить бакеты
        var buckets = await _minioClient.ListBucketsAsync(ct);
        IReadOnlyList<string> bucketNames = buckets.Buckets.Select(x => x.Name.ToLower()).ToList();

        if (bucketNames.Any(x => x == bucketName) == false)
            return Errors.NotFound($"Bucket с именем {bucketName}");

        return bucketName;
    }
}
