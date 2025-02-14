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
    public async Task<Result<IReadOnlyList<FileData>?, string>> GetFilesDataByIds(
        IEnumerable<Guid> ids, CancellationToken ct)
    { 
        var response = await httpClient.PostAsJsonAsync("files", ids, ct);

        if (response.StatusCode != HttpStatusCode.OK) 
            return "Get files data by ids failed";  

        IReadOnlyList<FileData>? files = await response.Content.ReadFromJsonAsync<IReadOnlyList<FileData>>(ct);
        return files?.ToList() ?? [];
    }


    public async Task<Result<FileUrlResponse, string>> UploadPresignedUrl(
        UploadPresignedUrlRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync("files/presigned", request, ct);

        if (response.StatusCode != HttpStatusCode.OK) 
            return "Upload presigned url failed"; 

        FileUrlResponse? file = await response.Content.ReadFromJsonAsync<FileUrlResponse>(ct);
        return file;
    }


    public async Task<Result<FileLocationResponse, string>> CompleteMultipartUpload(
        string key, CompleteMultipartRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync($"files/{key}/complite-multipart/presigned", request, ct);
        if (response.StatusCode != HttpStatusCode.OK)
            return "Complete multipart upload failed";

        FileLocationResponse? fileLocation = await response.Content.ReadFromJsonAsync<FileLocationResponse>(ct);
        return fileLocation;
    }


    public async Task<Result<FileUrlResponse, string>> GetPresignedUrl(
        string key, GetPresignedUrlRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync($"files/{key}/presigned", request, ct);
        if (response.StatusCode != HttpStatusCode.OK)
            return "Get presigned url failed";

        FileUrlResponse? fileUrl = await response.Content.ReadFromJsonAsync<FileUrlResponse>(ct);
        return fileUrl;
    }


    public async Task<Result<UploadPartFileResponse, string>> StartMultipartUpload(
      StartMultipartUploadRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync("files/multipart/presigned", request, ct);
        if (response.StatusCode != HttpStatusCode.OK)
            return "Start multipart upload failed";

        UploadPartFileResponse? uploadResponse = await response.Content.ReadFromJsonAsync<UploadPartFileResponse>(ct);
        return uploadResponse;
    }


    public async Task<Result<FileUrlResponse, string>> UploadPresignedPartUrl(
        string key, UploadPresignedPartUrlRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync($"files/{key}/part-presigned", request, ct);
        if (response.StatusCode != HttpStatusCode.OK)
            return "Upload presigned part url failed";

        FileUrlResponse? fileUrl = await response.Content.ReadFromJsonAsync<FileUrlResponse>(ct);
        return fileUrl;
    }
}