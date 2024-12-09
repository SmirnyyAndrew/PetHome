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
            _logger.LogError($"Несовпадение количества файлов и их Dto");
            return Errors.Conflict($"Несовпадение количества файлов и их Dto");
        }
        var bucketExistingCheck = await CheckIsExistBucket(bucketName, ct);
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
