using CSharpFunctionalExtensions;
using FilesService.Application.Interfaces;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Models.File;

namespace FilesService.Infrastructure.Minio;
public partial class MinioProvider : IFilesProvider
{
    //Загрузить несколько файлов
    public async Task<Result<IReadOnlyList<MediaFile>, Error>> UploadFile(
        IEnumerable<Stream> streams,
        MinioFilesInfoDto fileInfoDto,
        bool createBucketIfNotExist,
        CancellationToken ct)
    {
        if (fileInfoDto.FileNames.Count() != streams.Count())
        {
            string message = "Несовпадение количества файлов и их Dto";
            _logger.LogError(message);
            return Errors.Conflict(message);
        }
        var bucketExistingCheck = await CheckIsExistBucket(fileInfoDto.BucketName, ct);
        if (createBucketIfNotExist == false
            && bucketExistingCheck.IsFailure)
        {
            string message = $"Bucket с именем {fileInfoDto.BucketName} не найден";
            _logger.LogError(message);
            return Errors.Failure(message);
        }

        var semaphoreSlim = new SemaphoreSlim(MAX_STREAMS_LENGHT);
        List<MediaFile> medias = new List<MediaFile>();
        int index = 0;
        IEnumerable<Task> uploadTasks = streams.Select(async stream =>
        {
            try
            {
                await semaphoreSlim.WaitAsync(ct);

                MinioFileInfoDto fileInfo = new MinioFileInfoDto(
                    fileInfoDto.BucketName,
                    fileInfoDto.FileNames.ToList()[index++].Value);
                var result = await UploadFile(
                                stream,
                                fileInfo,
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
        _logger.LogInformation("В {0} было добавлено {1} медиа файла(-ов)",
          fileInfoDto.BucketName, result);
        return medias;
    }
}
