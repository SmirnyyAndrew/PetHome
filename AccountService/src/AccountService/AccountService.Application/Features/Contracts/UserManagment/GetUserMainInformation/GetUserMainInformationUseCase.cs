using AccountService.Application.Database.Repositories;
using AccountService.Contracts.HttpCommunication.Dto;
using AccountService.Contracts.HttpCommunication.Requests.UserManagement;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace AccountService.Application.Features.Contracts.UserManagment.GetUserMainInformation;
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
