using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.LoginUser;
public record LoginUserQuery(string Email, string Password) : IQuery;
