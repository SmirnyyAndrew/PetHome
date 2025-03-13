using PetHome.Core.Enums;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Dto;

namespace PetHome.Accounts.Application.Features.Read.GetUsersInformation;
public record GetUsersInformationQuery(PagedListDto PaginationSettings, UserFilter FilterType, string Filter) : IQuery;
