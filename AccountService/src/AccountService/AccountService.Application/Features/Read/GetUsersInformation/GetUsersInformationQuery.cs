using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Domain.Models;
using PetHome.SharedKernel.Responses.Dto;
using PetHome.SharedKernel.ValueObjects.User;

namespace AccountService.Application.Features.Read.GetUsersInformation;
public record GetUsersInformationQuery(PagedListDto PaginationSettings, UserFilterDto UserFilter) : IQuery;
