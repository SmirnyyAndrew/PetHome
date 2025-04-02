using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Contracts.HttpCommunication.Requests.UserManagement;
public record GetUserMainInformationQuery(Guid UserId) : IQuery;
