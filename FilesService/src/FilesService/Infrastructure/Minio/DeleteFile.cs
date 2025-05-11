using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using PetHome.SharedKernel.Responses.ErrorManagement;
using FilesService.Core.Interfaces;
using Minio.DataModel.Args;

namespace FilesService.Infrastructure.Minio;
public partial class MinioProvider : IMinioFilesHttpClient
{
    /// <summary>
    /// Удалить файлы из minio
    /// </summary>
    /// <param name="fileInfoDto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<UnitResult<string>> DeleteFile(
         MinioFilesInfoDto fileInfoDto, CancellationToken ct)
    {
        var isExistBucketResult = await CheckIsExistBucket(fileInfoDto.BucketName, ct);
        if (isExistBucketResult.IsFailure)
            return isExistBucketResult.Error.Message;

        foreach (var fileName in fileInfoDto.FileNames)
        {
            var minioFileArgs = new RemoveObjectArgs()
                .WithBucket(fileInfoDto.BucketName)
                .WithObject(fileName.Value);
            await _minioClient.RemoveObjectAsync(minioFileArgs).ConfigureAwait(false);

            _logger.LogInformation("Файл {0} в bucket {1}  успешно удалён", fileInfoDto.FileNames, fileInfoDto.BucketName);
        }
        string message = $"В bucket {fileInfoDto.BucketName} успешно удалены удалены:\n\t {string.Join("\n\t", fileInfoDto.FileNames)}";
        _logger.LogInformation(message);
        return message;
    }
}
