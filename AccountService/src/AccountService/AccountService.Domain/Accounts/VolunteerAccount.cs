using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using PetHome.Core.Interfaces.Database;
using PetHome.Core.Models;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;

namespace AccountService.Domain.Accounts;
public class VolunteerAccount : DomainEntity<Guid>, ISoftDeletableEntity
{
    public static RoleName ROLE = RoleName.Create("volunteer").Value;

    public UserId UserId { get; private set; }
    public User User { get; private set; }
    public Date? StartVolunteeringDate { get; private set; }
    public IReadOnlyList<Requisites>? Requisites { get; private set; }
    public IReadOnlyList<Certificate>? Certificates { get; private set; }
    //public IReadOnlyList<Pet>? Pets { get; private set; } 
    public DateTime DeletionDate { get; set; }
    public bool IsDeleted { get; set; }

    private VolunteerAccount(UserId id) : base(id)
    {
        UserId = id;
    }

    private VolunteerAccount(
            UserId userId,
            Date startVolunteeringDate,
            IReadOnlyList<Requisites> requisites,
            IReadOnlyList<Certificate> certificates) : base(userId)
    {
        UserId = userId;
        StartVolunteeringDate = startVolunteeringDate;
        Requisites = requisites;
        Certificates = certificates;
    }


    public static Result<VolunteerAccount, Error> Create(UserId id) => new VolunteerAccount(id);
    public static Result<VolunteerAccount, Error> Create(
            User user,
            Date startVolunteeringDate,
            IReadOnlyList<Requisites> requisites,
            IReadOnlyList<Certificate> certificates)
    {
        Role role = user.Role;
        if (role is not null && role.Name.ToLower() == ROLE)
        {
            UserId userId = UserId.Create(user.Id).Value;
            return new VolunteerAccount(
                userId,
                startVolunteeringDate,
                requisites,
                certificates);
        }
        return Errors.Conflict($"пользователь с id = {user.Id}");
    }

    public UnitResult<Error> SetRequisites(IEnumerable<Requisites> requisites)
    {
        Requisites = requisites.ToList();
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> SetCertificates(IEnumerable<Certificate> certificates)
    {
        Certificates = certificates.ToList();
        return UnitResult.Success<Error>();
    } 

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
