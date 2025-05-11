namespace PetHome.Core.Infrastructure.Auth;
public class UserScopedData
{
    public Guid UserId { get; set; } = Guid.Empty;
    public string Role { get; set; } = string.Empty; 
    public IReadOnlyList<string> Permissions { get; set; } = [];
    public string Email { get; set; } = string.Empty;
}
 