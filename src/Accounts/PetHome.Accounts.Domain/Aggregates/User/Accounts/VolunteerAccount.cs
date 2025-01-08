using CSharpFunctionalExtensions;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Core.Interfaces;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;

namespace PetHome.Accounts.Domain.Aggregates.User.Accounts;
public class VolunteerAccount : SoftDeletableEntity
{
    public static RoleName ROLE = RoleName.Create("volunteer").Value;

    public UserId UserId { get; set; }
    public Date? StartVolunteeringDate { get; private set; }
    public IReadOnlyList<Requisites>? Requisites { get; private set; }
    public IReadOnlyList<Certificate>? Certificates { get; private set; }
    public IReadOnlyList<Pet>? Pets { get; private set; }


    private VolunteerAccount() { }

    private VolunteerAccount(UserId id)
    {
        UserId = id;
    }

    private VolunteerAccount(
            UserId userId,
            Date startVolunteeringDate,
            IReadOnlyList<Requisites> requisites,
            IReadOnlyList<Certificate> certificates)
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

    public override void SoftDelete()
    {
        base.SoftDelete();
        Pets?.ToList().ForEach(pet => pet.SoftDelete());
    }

    public override void SoftRestore()
    {
        base.SoftRestore();
        Pets?.ToList().ForEach(pet => pet.SoftRestore());
    }
}
