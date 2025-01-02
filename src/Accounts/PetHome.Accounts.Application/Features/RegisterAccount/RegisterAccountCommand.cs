using Microsoft.AspNetCore.Identity;
using PetHome.Accounts.Domain;
using PetHome.Core.Interfaces.FeatureManagment; 

namespace PetHome.Accounts.Application.Features.RegisterAccount;
public record RegisterAccountCommand(string Login, string Password) : ICommand;
