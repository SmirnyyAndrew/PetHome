using AccountService.Application.Database.Repositories;
using AccountService.Contracts.HttpCommunication.Dto;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Redis;
using PetHome.SharedKernel.Constants;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace AccountService.Application.Features.Read.GetUserInformation;
public class GetUserInformationUseCase
    : IQueryHandler<UserDto?, GetUserInformationQuery>
{
    private readonly IAuthenticationRepository _repository;
    private readonly ICacheService _cacheService;

    public GetUserInformationUseCase(
        IAuthenticationRepository repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<Result<UserDto?, ErrorList>> Execute(
        GetUserInformationQuery query, CancellationToken ct)
    {
        UserDto? userDto = await _cacheService.GetOrSetAsync<UserDto>(Constants.Redis.USER(query.UserId), async () =>
        {
            var getUserResult = await _repository.GetUserById(query.UserId, ct);
            if (getUserResult.IsFailure)
                return null;

            User user = getUserResult.Value;

            string roleName = user.Role is null ? string.Empty : user.Role.Name;
            DateTime birthDate = user.BirthDate is null ? default : user.BirthDate.Value;
            userDto = new UserDto(
                user.Id,
                user.UserName,
                user.Email,
                roleName,
                birthDate);

            return userDto;
        }, options: null, ct);

        return userDto;
    }
}
