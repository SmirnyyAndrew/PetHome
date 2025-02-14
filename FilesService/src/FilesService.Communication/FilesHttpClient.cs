using CSharpFunctionalExtensions;
using FilesService.Core.Response;

namespace FilesService.Communication;

public class FilesHttpClient(HttpClient httpClient)
{
    public async Task<Result<IReadOnlyList<FileResponse>>> GetFilesPresignedUrls(
        GetFilesPresignedUrlsRequest request)
    {
        return default;
    }
}

public record GetFilesPresignedUrlsRequest();