using CSharpFunctionalExtensions;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Interfaces;
using FilesService.Core.Request.Minio;
using Minio.DataModel.Args;

namespace FilesService.Infrastructure.Minio;
public partial class MinioProvider : IMinioFilesHttpClient
{
    //Загрузить файл
    public async Task<UnitResult<string>> UploadFileWithDataChecking( 
       UploadFileRequest request,
       CancellationToken ct)
    {
        var isExistBucketResult = await CheckIsExistBucket(request.FileInfo.BucketName, ct);
        if (isExistBucketResult.IsFailure && request.CreateBucketIfNotExist == false)
        {
            string message = "Bucket с именем {bucketName} не найден";
            _logger.LogError(message);
            return message;
        }
        else
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(request.FileInfo.BucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs, ct);
        }

        await UploadFile(request, ct);

        return Result.Success();
    }
}
