using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetManagementService.Application.Features.Write.FilesService.GetFilesDataByIds;
public class GetFilesDataByIdsUseCase
    : ICommandHandler<IReadOnlyList<FileData>, GetFilesDataByIdsCommand>
{
    private readonly IAmazonFilesHttpClient _httpClient;

    public GetFilesDataByIdsUseCase(IAmazonFilesHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IReadOnlyList<FileData>, ErrorList>> Execute(
        GetFilesDataByIdsCommand command, CancellationToken ct)
    {
        var result = await _httpClient.GetFilesDataByIds(command.Ids, ct);
        if (result.IsFailure)
        { 
            return Errors.NotFound("files").ToErrorList();
        }

        return result.Value.ToList();
    }
}
