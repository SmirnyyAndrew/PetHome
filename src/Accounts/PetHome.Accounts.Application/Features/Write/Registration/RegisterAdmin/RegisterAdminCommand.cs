using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.Write.Registration.RegisterAdmin;
public record RegisterAdminCommand(Guid UserId) : ICommand;
