using CSharpFunctionalExtensions;
using FilesService.Core.Models;
using FilesService.Core.Request.AmazonS3;
using FilesService.Core.Request.AmazonS3.MultipartUpload;
using FilesService.Core.Response;

namespace FilesService.Core.Interfaces;

public interface IFilesHttpClient
{
    public Task<Result<IReadOnlyList<FileData>?, string>> GetFilesDataByIds(
        IEnumerable<Guid> ids, CancellationToken ct);


    public Task<Result<FileUrlResponse, string>> UploadPresignedUrl(
        UploadPresignedUrlRequest request, CancellationToken ct);


    public Task<Result<FileLocationResponse, string>> CompleteMultipartUpload(
        string key, CompleteMultipartRequest request, CancellationToken ct);


    public Task<Result<FileUrlResponse, string>> GetPresignedUrl(
        string key, GetPresignedUrlRequest request, CancellationToken ct);


    public Task<Result<UploadPartFileResponse, string>> StartMultipartUpload(
      StartMultipartUploadRequest request, CancellationToken ct);


    public Task<Result<FileUrlResponse, string>> UploadPresignedPartUrl(
        string key, UploadPresignedPartUrlRequest request, CancellationToken ct);
}