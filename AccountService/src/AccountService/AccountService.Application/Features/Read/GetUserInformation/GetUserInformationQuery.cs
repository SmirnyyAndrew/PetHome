using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Read.GetUserInformation;
public record GetUserInformationQuery(Guid UserId) : IQuery;
