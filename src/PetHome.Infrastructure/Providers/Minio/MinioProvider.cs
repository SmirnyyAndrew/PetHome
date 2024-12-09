using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using PetHome.Application.Interfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.Shared.Error;

namespace PetHome.Infrastructure.Providers.Minio;
public class MinioProvider : IFilesProvider
{
    private readonly int MAX_STREAMS_LENGHT = 5;
    private IMinioClient _minioClient;
    private ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
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

        _logger.LogInformation($"Файл {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName}  успешно удалён");
        return $"Файл {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName} успешно удалён";
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
            fileSavePath = $"{fileSavePath}{fileExtension}";
            var minioFileArgs = new GetObjectArgs()
                .WithBucket(fileInfoDto.BucketName)
                .WithObject(fileInfoDto.FileName)
                .WithFile(fileSavePath);

            ObjectStat presignedUrl = await _minioClient.GetObjectAsync(minioFileArgs, ct)
                .ConfigureAwait(false);

            _logger.LogInformation($"Файл {fileInfoDto.FileName} из bucket {fileInfoDto.BucketName} сохранён по пути = {fileSavePath}");
            return fileSavePath;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Файл {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName} не найден");
            return Errors.Failure($"Файл {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName} не найден");
        }
    }




    //Загрузить файл
    public async Task<Result<Media, Error>> UploadFile(
        Stream stream,
        string bucketName,
        string filename,
        bool createBucketIfNotExist,
        CancellationToken ct)

    {
        //Расширение файла
        string fileExtension = Path.GetExtension(filename);

        Guid newFilePath = Guid.NewGuid();
        string fullName = $"{newFilePath}{fileExtension}";
        PutObjectArgs minioFileArgs = new PutObjectArgs()
        .WithBucket(bucketName.ToLower())
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithObject(fullName);

        var result = await _minioClient.PutObjectAsync(minioFileArgs, ct);
        string message = $"Файл {result.ObjectName} загружен в bucket = {bucketName}";
        _logger.LogInformation(message);

        return Media.Create(bucketName, fullName);
    }







    //Загрузить файл
    public async Task<UnitResult<Error>> UploadFileWithDataChecking(
        Stream stream,
        string bucketName,
        string filename,
        bool createBucketIfNotExist,
        CancellationToken ct)
    {
        var isExistBucketResult = await IsExistBucket(bucketName, ct);
        if (isExistBucketResult.IsFailure && createBucketIfNotExist == false)
        {
            _logger.LogError($"Bucket с именем {bucketName} не найден");
            return Errors.Failure($"Bucket с именем {bucketName} не найден");
        }
        else
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs, ct);
        }

        await UploadFile(stream, bucketName, filename, createBucketIfNotExist, ct);

        return Result.Success<Error>();
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

            _logger.LogInformation($"Для файла {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName} получена временная ссылка для скачивание = {presignedUrl}");
            return presignedUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Source}\n{ex.InnerException}\nФайл {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName} не найден");
            return Errors.Failure($"Файл {fileInfoDto.FileName} в bucket {fileInfoDto.BucketName} не найден");
        }

    }







    // Проверить, существует ли bucket
    private async Task<Result<string, Error>> IsExistBucket(string bucketName, CancellationToken ct)
    {
        //Получить бакеты
        var buckets = await _minioClient.ListBucketsAsync(ct);
        IReadOnlyList<string> bucketNames = buckets.Buckets.Select(x => x.Name.ToLower()).ToList();

        if (bucketNames.Any(x => x == bucketName) == false)
        {
            _logger.LogError($"Bucket с именем {bucketName} не существует");
            return Errors.NotFound($"Bucket с именем {bucketName}");
        }
        return bucketName;
    }







    //Загрузить несколько файлов
    public async Task<Result<IReadOnlyList<Media>, Error>> UploadFile(
        IEnumerable<Stream> streams,
        string bucketName,
        IEnumerable<string> fileNames,
        bool createBucketIfNotExist,
        CancellationToken ct)
    {
        if (fileNames.Count() != streams.Count())
        {
            _logger.LogError($"Несовпадение количества файлов и их Dto");
            return Errors.Conflict($"Несовпадение количества файлов и их Dto");
        }
        var bucketExistingCheck = await IsExistBucket(bucketName, ct);
        if (createBucketIfNotExist == false
            && bucketExistingCheck.IsFailure)
        {
            _logger.LogError($"Bucket с именем {bucketName} не найден");
            return Errors.Failure($"Bucket с именем {bucketName} не найден");
        }


        var semaphoreSlim = new SemaphoreSlim(MAX_STREAMS_LENGHT);
        List<Task> uploadTasks = new List<Task>();
        List<Media> medias = new List<Media>();


        for (int i = 0; i < streams.Count(); i++)
        {
            await semaphoreSlim.WaitAsync(ct);
            try
            {
                var taskResult = UploadFile(
                            streams.ToList()[i],
                            bucketName,
                            fileNames.ToList()[i],
                            createBucketIfNotExist,
                            ct);
                uploadTasks.Add(taskResult);


                medias.Add(taskResult.Result.Value);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        await Task.WhenAll(uploadTasks);

        string result = uploadTasks.Count(x => x.IsCompleted).ToString();
        _logger.LogError($"В {bucketName} было добавлено {result} медиа файла(-ов)");
        return medias;
    }
}
