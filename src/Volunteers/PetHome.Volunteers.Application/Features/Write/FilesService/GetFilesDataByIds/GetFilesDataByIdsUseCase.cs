using CSharpFunctionalExtensions;
using FilesService.Communication;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Volunteers.Application.Features.Write.FilesService.GetFilesDataByIds;
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
