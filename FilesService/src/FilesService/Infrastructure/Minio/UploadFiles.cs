using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Interfaces;
using FilesService.Core.Request.Minio;

namespace FilesService.Infrastructure.Minio;
public partial class MinioProvider : IMinioFilesHttpClient
{
    /// <summary>
    /// Загрузить несколько файлов в minio
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<Result<IReadOnlyList<MediaFile>, string>> UploadFiles(
        UploadFilesRequest request,
        CancellationToken ct)
    {
        if (request.FileInfoDto.FileNames.Count() != request.Streams.Count())
        {
            string message = "Несовпадение количества файлов и их Dto";
            _logger.LogError(message);
            return message;
        }
        var bucketExistingCheck = await CheckIsExistBucket(request.FileInfoDto.BucketName, ct);
        if (request.CreateBucketIfNotExist == false
            && bucketExistingCheck.IsFailure)
        {
            string message = $"Bucket с именем {request.FileInfoDto.BucketName} не найден";
            _logger.LogError(message);
            return message;
        }

        var semaphoreSlim = new SemaphoreSlim(MAX_STREAMS_LENGHT);
        List<MediaFile> medias = new List<MediaFile>();
        int index = 0;
        IEnumerable<Task> uploadTasks = request.Streams.Select(async stream =>
        {
            try
            {
                await semaphoreSlim.WaitAsync(ct);

                MinioFileInfoDto fileInfo = new MinioFileInfoDto(
                    request.FileInfoDto.BucketName,
                    request.FileInfoDto.FileNames.ToList()[index++].Value);
                UploadFileRequest uploadFileRequest = new UploadFileRequest(stream, fileInfo, request.CreateBucketIfNotExist);
                var result = await UploadFile(
                                uploadFileRequest,
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
          request.FileInfoDto.BucketName, result);
        return medias;
    }
}
