using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using PetHome.Accounts.Application.Database.Dto;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.Read.GetUserInformation;
public class GetUserInformationUseCase
    : IQueryHandler<UserDto, GetUserQuery>
{
    private readonly IAuthenticationRepository _repository; 

    public GetUserInformationUseCase(IAuthenticationRepository repository)
    {
        _repository = repository; 
    }

    public async Task<Result<UserDto, ErrorList>> Execute(
        GetUserQuery query, CancellationToken ct)
    {
        var result = await _repository.GetUserById(query.UserId, ct);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        User user = result.Value;
        UserDto userDto = new UserDto(
            user.Id,
            user.UserName,
            user.Role?.Name,
            user.BirthDate);
        return userDto;
    }
}
