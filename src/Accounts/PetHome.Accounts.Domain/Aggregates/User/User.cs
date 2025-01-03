using Microsoft.AspNetCore.Identity;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Core.ValueObjects;

namespace PetHome.Accounts.Domain.Aggregates.User;
public class User : IdentityUser<Guid>
{
    public IReadOnlyList<SocialNetwork>? SocialNetworks { get; private set; }
    public IReadOnlyList<Media>? Medias { get; private set; }
    public IReadOnlyList<PhoneNumber>? PhoneNumbers { get; private set; }
    public RoleId? RoleId { get; private set; }

    public User() { }
    private User(
        IReadOnlyList<SocialNetworkDto> socialNetworks,
        IReadOnlyList<MinioFileInfoDto> medias,
        IReadOnlyList<PhoneNumber> phoneNumbers,
        RoleId roleId)
    {
        PhoneNumbers = phoneNumbers;
        RoleId = roleId;
    }

    public static User Create(
        IEnumerable<SocialNetworkDto> socialNetworks,
        IEnumerable<MinioFileInfoDto> medias,
        IEnumerable<PhoneNumber> phoneNumbers,
        RoleId roleId)
    {
        return new User(
            socialNetworks.ToList(),
            medias.ToList(),
            phoneNumbers.ToList(),
            roleId);
    }
}
