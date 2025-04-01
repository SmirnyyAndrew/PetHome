using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Application.Features.Read.GetUserInformation;
public record GetUserInformationQuery(Guid UserId) : IQuery;
