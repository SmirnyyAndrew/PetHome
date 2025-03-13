using CSharpFunctionalExtensions;
using MassTransit.Initializers;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.Dto;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Redis;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.Read.GetUsersInformation;
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
                var users = _repository.GetUsers(ct).Result
                .AsQueryable();

                switch (query.FilterType)
                {
                    case Core.Enums.UserFilter.email:
                        users = users.Where(u => u.Email.Contains(query.Filter));
                        break;
                    case Core.Enums.UserFilter.username:
                        users = users.Where(u => u.UserName.Contains(query.Filter));
                        break;
                    case Core.Enums.UserFilter.role_name:
                        users = users.Where(u => u.Role.Name.Contains(query.Filter));
                        break;
                    case Core.Enums.UserFilter.phone_number:
                        users = users.Where(u => u.PhoneNumbers.Any(p => p.Value.Contains(query.Filter)));
                        break;
                    default:
                        break;
                }

                return users
                .ToPagedList(query.PaginationSettings.PageNum, query.PaginationSettings.PageSize)
                .Select(u => (UserDto)u)
                .ToList();
            });

        return cachedUsersDto;
    }
}
