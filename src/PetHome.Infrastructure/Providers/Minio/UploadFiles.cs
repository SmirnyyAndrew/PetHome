using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Interfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.Shared.Error;

namespace PetHome.Infrastructure.Providers.Minio;
public partial class MinioProvider : IFilesProvider
{
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
            string message = "Несовпадение количества файлов и их Dto";
            _logger.LogError(message);
            return Errors.Conflict(message);
        }
        var bucketExistingCheck = await CheckIsExistBucket(bucketName, ct);
        if (createBucketIfNotExist == false
            && bucketExistingCheck.IsFailure)
        {
            string message = $"Bucket с именем {bucketName} не найден";
            _logger.LogError(message);
            return Errors.Failure(message);
        }

        var semaphoreSlim = new SemaphoreSlim(MAX_STREAMS_LENGHT);
        List<Media> medias = new List<Media>();
        int index = 0;
        IEnumerable<Task> uploadTasks = streams.Select(async stream =>
        {
            try
            {
                await semaphoreSlim.WaitAsync(ct);
                var result = await UploadFile(
                                stream,
                                bucketName,
                                fileNames.ToList()[index++],
                                createBucketIfNotExist,
                                ct);
                medias.Add(result.Value);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        });
        await Task.WhenAll(uploadTasks);

        string result = uploadTasks.Count(x => x.IsCompleted).ToString();
        _logger.LogError("В {0} было добавлено {1} медиа файла(-ов)",
          bucketName, result);
        return medias;
    }
}
