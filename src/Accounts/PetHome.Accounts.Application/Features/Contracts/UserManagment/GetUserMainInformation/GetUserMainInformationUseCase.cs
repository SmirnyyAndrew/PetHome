using CSharpFunctionalExtensions;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.HttpCommunication.Dto;
using PetHome.Accounts.Contracts.HttpCommunication.Requests.UserManagement;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.Contracts.UserManagment.GetUserMainInformation;
public class GetUserMainInformationUseCase(IAuthenticationRepository repository)
    : IQueryHandler<UserDto, GetUserMainInformationQuery>
{
    public async Task<Result<UserDto, ErrorList>> Execute(GetUserMainInformationQuery query, CancellationToken ct)
    {
        var getUserResult = await repository.GetUserById(query.UserId, ct);
        if (getUserResult.IsFailure)
            return default;

        User user = getUserResult.Value;
        DateTime birhDate = user.BirthDate is null
            ? default
            : user.BirthDate.Value;
        UserDto? userDto = new UserDto(
            user.Id,
            user.UserName,
            user.Email,
            user.Role?.Name,
            birhDate);
        return userDto;
    } 
}
