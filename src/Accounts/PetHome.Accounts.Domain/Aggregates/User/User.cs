using Microsoft.AspNetCore.Identity;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Core.ValueObjects;

namespace PetHome.Accounts.Domain.Aggregates.User;
public class User : IdentityUser<Guid>
{
    public Email Email { get; set; }
    public FullName FullName { get;  set; }
    public UserName UserName { get;  set; }
    public Login Login { get; set; }
    public Password Password { get; set; }
    public ValueObjectList<SocialNetwork>? SocialNetworks { get; set; }
    public ValueObjectList<Media>? Medias { get; set; }
    public ValueObjectList<PhoneNumber> PhoneNumbers { get; set; }
    public RoleId RoleId { get; set; }

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
