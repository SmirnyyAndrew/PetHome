using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.Dto;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Redis;

namespace PetHome.Accounts.Application.Features.Read.GetUsersInformation;
public class GetUsersUseCase
    : IQueryHandler<IReadOnlyList<UserDto>>
{
    private readonly IAuthenticationRepository _repository;
    private readonly ICacheService _cacheService;

    public GetUsersUseCase(
        IAuthenticationRepository repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<IReadOnlyList<UserDto>> Execute(CancellationToken ct)
    {
        await _cacheService.RemoveAsync(Constants.Redis.USERS, ct);
        IReadOnlyList<User>? users = await _repository.GetUsers(ct);

        IReadOnlyList<UserDto>? usersDto = await _cacheService.GetOrSetAsync<IReadOnlyList<UserDto>>(Constants.Redis.USERS,
            async () => users.Select(u => (UserDto)u).ToList());

        return usersDto ?? [];
    }
}
