namespace PetHome.Core.Response.Login;
public record LoginResponse(
    string AccessToken, 
    string RefreshToken,
    string UserId,
    string Email,
    string UserName);

