using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Application.Features.Write.LoginUser;
public record LoginUserQuery(string Email, string Password) : IQuery;
