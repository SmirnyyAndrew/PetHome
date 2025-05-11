using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.Registration.RegisterAdmin;
public record RegisterAdminCommand(Guid UserId) : ICommand;
