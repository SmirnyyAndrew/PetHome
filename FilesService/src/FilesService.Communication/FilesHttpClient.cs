using CSharpFunctionalExtensions;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
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
} 