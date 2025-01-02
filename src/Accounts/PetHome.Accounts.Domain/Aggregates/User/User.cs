using Microsoft.AspNetCore.Identity;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Core.ValueObjects;

namespace PetHome.Accounts.Domain.Aggregates.User;
public class User : IdentityUser<Guid>
{
    private UserId Id { get; }
    private Email Email { get; }
    private FullName FullName { get; }
    private UserName UserName { get; }
    private Login Login { get; }
    private Password Password { get; }
    private ValueObjectList<SocialNetwork>? SocialNetworks { get; }
    private ValueObjectList<Media>? Medias { get; }
    private ValueObjectList<PhoneNumber> PhoneNumbers { get; }
    private RoleId RoleId { get; }

    public User() { }
    private User(
        Email email,
        FullName fullName,
        UserName userName,
        Login login,
        Password password,
        ValueObjectList<SocialNetwork>? socialNetworks,
        ValueObjectList<Media>? medias,
        ValueObjectList<PhoneNumber> phoneNumbers,
        RoleId roleId)
    {
        Id = UserId.Create().Value;
        Email = email;
        FullName = fullName;
        UserName = userName;
        Login = login;
        Password = password;
        SocialNetworks = socialNetworks;
        Medias = medias;
        PhoneNumbers = phoneNumbers;
        RoleId = roleId;
    }

    public static User Create(
        Email email,
        FullName fullName,
        UserName userName,
        Login login,
        Password password,
        ValueObjectList<SocialNetwork>? socialNetworks,
        ValueObjectList<Media>? medias,
        ValueObjectList<PhoneNumber> phoneNumbers,
        RoleId roleId)
    {
        return new User(
            email,
            fullName,
            userName,
            login,
            password,
            socialNetworks,
            medias,
            phoneNumbers,
            roleId);
    }
}
