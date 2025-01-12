using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHome.Accounts.Domain.Accounts;
using PetHome.Core.Interfaces.Database;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Domain.Aggregates;
public class User : IdentityUser<Guid>, ISoftDeletableEntity
{
    public static RoleName ROLE = RoleName.Create("user").Value;
    public IReadOnlyList<SocialNetwork>? SocialNetworks { get; private set; } = [];
    public IReadOnlyList<Media>? Medias { get; private set; } = [];
    public IReadOnlyList<PhoneNumber>? PhoneNumbers { get; private set; } = [];
    public RoleId? RoleId { get; private set; }
    public Role? Role { get; set; }
    public Date? BirthDate { get; set; }
    public DateTime DeletionDate { get; set; }
    public bool IsDeleted { get; set; } = false;
    public AdminAccount? Admin { get; private set; }
    public ParticipantAccount? Participant { get; private set; }
    public VolunteerAccount? Volunteer { get; private set; }

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

    private User(
        IReadOnlyList<SocialNetwork> socialNetworks,
        IReadOnlyList<Media> medias,
        IReadOnlyList<PhoneNumber> phoneNumbers,
        RoleId roleId)
    {
        SocialNetworks = socialNetworks;
        Medias = medias;
        PhoneNumbers = phoneNumbers;
        RoleId = roleId;
    }

    public static User Create(
        IEnumerable<SocialNetwork> socialNetworks,
        IEnumerable<Media> medias,
        IEnumerable<PhoneNumber> phoneNumbers,
        RoleId roleId)
    {
        return new User(
            socialNetworks.ToList(),
            medias.ToList(),
            phoneNumbers.ToList(),
            roleId);
    }

    public UnitResult<Error> SetMainInfo(UserName userName, Email email, Date birthDate)
    {
        UserName = userName;
        Email = email;
        BirthDate = birthDate;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> SetSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks.ToList();
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> SetMedia(IEnumerable<Media> medias)
    {
        Medias = medias.ToList();
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> SetPhoneNumbers(IEnumerable<PhoneNumber> phoneNumbers)
    {
        PhoneNumbers = phoneNumbers.ToList();
        return UnitResult.Success<Error>();
    }

    //Класс не может наследоваться от двух абстрактных классов одновременно,
    //поэтому реализация интерфейса ISoftDeletableEntity
    public void SoftDelete()
    {
        DeletionDate = DateTime.UtcNow;
        IsDeleted = true;
    }

    public void SoftRestore()
    {
        DeletionDate = default;
        IsDeleted = false;
    }
}
