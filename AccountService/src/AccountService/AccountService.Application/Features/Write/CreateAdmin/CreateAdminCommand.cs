using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Application.Features.Write.CreateAdmin;
public record CreateAdminCommand(string Email, string UserName) : ICommand;