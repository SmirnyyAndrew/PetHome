using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Application.Features.Write.Registration.RegisterAdmin;
public record RegisterAdminCommand(Guid UserId) : ICommand;
