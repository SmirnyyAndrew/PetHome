using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.CreateAdmin;
public record CreateAdminCommand(string Email, string UserName) : ICommand;