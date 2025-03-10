using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.Contracts.UserManagment;
using PetHome.Accounts.Contracts.Dto;
using PetHome.Accounts.Domain.Aggregates;

namespace PetHome.Accounts.Application.Features.Contracts.UserManagment.GetUserMainInformation;
public class GetUserMainInformationUsingContract(IAuthenticationRepository repository)
    : IGetUserMainInformationContract
{
    public async Task<UserDto?> Execute(Guid userId, CancellationToken ct)
    {
        var getUserResult = await repository.GetUserById(userId, ct);
        if (getUserResult.IsFailure)
            return null;

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
