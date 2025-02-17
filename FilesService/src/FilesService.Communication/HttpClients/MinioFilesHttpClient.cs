using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;
using FilesService.Core.Request.Minio;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;

namespace FilesService.Communication.HttpClients;

public class MinioFilesHttpClient(HttpClient httpClient) : IMinioFilesHttpClient
{
    public async Task<UnitResult<string>> DeleteFile(
        MinioFilesInfoDto fileInfoDto, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync("minio/files", fileInfoDto, ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return "Failed to delete the file";

        return Result.Success(); ;
    }

    public async Task<UnitResult<string>> DownloadFiles(
        DownloadFilesRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync("minio/download-files", request, ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return "Failed to download files";

        return Result.Success();
    }

    public async Task<Result<List<string>, string>> GetFilePresignedPath(
        MinioFilesInfoDto fileInfoDto, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync("minio/file-presigned-path", fileInfoDto, ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return "Failed to download files";

        string? result = await response.Content.ReadFromJsonAsync<string>(ct);
        return result;
    }


    public async Task<Result<MediaFile, string>> UploadFile(
        UploadFileRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync("minio/file-presigned-path", request, ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return "Failed to upload the file";

        MediaFile? result = await response.Content.ReadFromJsonAsync<MediaFile>(ct);
        return result;
    }


    public Task<Result<IReadOnlyList<MediaFile>, string>> UploadFiles(
        UploadFilesRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }


    public async Task<UnitResult<string>> UploadFileWithDataChecking(
        UploadFileRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync("minio/upload-file-with-data-checking", request, ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return "Failed to upload file with data checking";
         
        return Result.Success();
    }
}