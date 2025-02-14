using CSharpFunctionalExtensions;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Models;
using FilesService.Core.Request;
using FilesService.Core.Response;
using System.Net;
using System.Net.Http.Json;

namespace FilesService.Communication;

public class FilesHttpClient(HttpClient httpClient)
{
    public async Task<Result<IReadOnlyList<FileData>?, Error>> GetFilesDataByIds(
        IEnumerable<Guid> ids, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync("files", ids, ct);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return Errors.NotFound("files");
        }
        IReadOnlyList<FileData>? files = await response.Content.ReadFromJsonAsync<IReadOnlyList<FileData>>(ct);
        return files?.ToList() ?? [];
    }


    public async Task<Result<FileUrlResponse, Error>> UploadPresignedUrl(
        UploadPresignedUrlRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync("files/presigned", request, ct);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return Errors.Conflict("files");
        }

        FileUrlResponse? file = await response.Content.ReadFromJsonAsync<FileUrlResponse>(ct);
        return file;
    }


    public async Task<Result<FileLocationResponse, Error>> CompleteMultipartUpload(
        string key, CompleteMultipartRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync($"files/{key}/complite-multipart/presigned", request, ct);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return Errors.NotFound("Key");
        }

        FileLocationResponse? fileLocation = await response.Content.ReadFromJsonAsync<FileLocationResponse>(ct);
        return fileLocation;
    }


    public async Task<Result<FileUrlResponse, Error>> GetPresignedUrl(
        string key, GetPresignedUrlRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync($"files/{key}/presigned", request, ct);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return Errors.NotFound("file");
        }
        FileUrlResponse? fileUrl = await response.Content.ReadFromJsonAsync<FileUrlResponse>(ct);
        return fileUrl;
    }


    public async Task<Result<UploadPartFileResponse, Error>> StartMultipartUpload(
      StartMultipartUploadRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync("files/multipart/presigned", request, ct);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return Errors.Conflict("file");
        }

        UploadPartFileResponse? uploadResponse = await response.Content.ReadFromJsonAsync<UploadPartFileResponse>(ct);
        return uploadResponse;
    }
}