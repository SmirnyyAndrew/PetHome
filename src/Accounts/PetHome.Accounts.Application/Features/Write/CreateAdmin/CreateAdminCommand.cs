using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.Write.CreateAdmin;
public record CreateAdminCommand(string Email, string UserName) : ICommand;