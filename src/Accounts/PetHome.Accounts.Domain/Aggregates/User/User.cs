using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects;

namespace PetHome.Accounts.Domain.Aggregates.User;
public class User : IdentityUser<Guid>
{
    public static RoleName ROLE = RoleName.Create("user").Value;

    public IReadOnlyList<SocialNetwork>? SocialNetworks { get; private set; } = [];
    public IReadOnlyList<Media>? Medias { get; private set; } = [];
    public IReadOnlyList<PhoneNumber>? PhoneNumbers { get; private set; } = [];
    public RoleId? RoleId { get; set; }
    public Role? Role { get; set; }

    public User() { }

    private User(
            Email email,
            UserName userName,
            Role role)
    {
        Id = Guid.NewGuid();
        Email = email;
        UserName = userName;
        Role = role;
        RoleId = RoleId.Create(role.Id).Value;
    }

    public static Result<User, Error> Create(
            Email email,
            UserName userName,
            Role role)
    {
        if (role != null && email != null && role != null)
            return new User(email, userName, role);

        return Errors.Validation("User");
    }

    //private User(
    //    IReadOnlyList<SocialNetwork> socialNetworks,
    //    IReadOnlyList<Media> medias,
    //    IReadOnlyList<PhoneNumber> phoneNumbers,
    //    RoleId roleId)
    //{
    //    SocialNetworks = socialNetworks;
    //    Medias = medias;
    //    PhoneNumbers = phoneNumbers;
    //    RoleId = roleId;
    //}

    //public static User Create(
    //    IEnumerable<SocialNetwork> socialNetworks,
    //    IEnumerable<Media> medias,
    //    IEnumerable<PhoneNumber> phoneNumbers,
    //    RoleId roleId)
    //{
    //    return new User(
    //        socialNetworks.ToList(),
    //        medias.ToList(),
    //        phoneNumbers.ToList(),
    //    roleId);
    //}
}
