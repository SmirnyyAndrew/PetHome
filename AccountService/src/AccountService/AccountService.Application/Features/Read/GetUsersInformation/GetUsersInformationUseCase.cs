using AccountService.Application.Database.Repositories;
using AccountService.Contracts.HttpCommunication.Dto;
using CSharpFunctionalExtensions;
using MassTransit.Initializers;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Redis;
using PetHome.Core.Response.Validation.Validator;

namespace AccountService.Application.Features.Read.GetUsersInformation;
public class GetUsersInformationUseCase
    : IQueryHandler<IReadOnlyList<UserDto>, GetUsersInformationQuery>
{
    private readonly IAuthenticationRepository _repository;
    private readonly ICacheService _cacheService;

    public GetUsersInformationUseCase(
        IAuthenticationRepository repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<Result<IReadOnlyList<UserDto>, ErrorList>> Execute(GetUsersInformationQuery query, CancellationToken ct)
    {
        var cachedUsersDto = await _cacheService.GetOrSetAsync(
            Constants.Redis.PAGED_USERS(query.PaginationSettings.PageSize, query.PaginationSettings.PageNum),
            async () =>
            {
                var pagedFiltredUsers = await _repository.GetPagedUsersWithFilter(
                    query.PaginationSettings, query.UserFilter, ct);

                var pagedFiltredUsersDto = pagedFiltredUsers.Select(u => (UserDto)u).ToList();
                return pagedFiltredUsersDto;
            });

        return cachedUsersDto ?? [];
    }
}
