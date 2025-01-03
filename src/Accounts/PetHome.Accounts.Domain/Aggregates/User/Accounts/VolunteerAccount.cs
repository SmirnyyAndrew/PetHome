using CSharpFunctionalExtensions;
using PetHome.Core.Interfaces;
using PetHome.Core.ValueObjects;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;

namespace PetHome.Accounts.Domain.Aggregates.User.Accounts;
public class VolunteerAccount : SoftDeletableEntity
{
    public UserId UserId { get; set; }
    public Date? StartVolunteeringDate { get; private set; }
    public IReadOnlyList<Requisites>? Requisites { get; private set; }
    public IReadOnlyList<Certificate>? Certificates { get; private set; }
    public IReadOnlyList<Pet>? Pets { get; private set; }

    private VolunteerAccount() { }
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

    public static Result<VolunteerAccount> Create(
            UserId userId,
            Date startVolunteeringDate,
            IReadOnlyList<Requisites> requisites,
            IReadOnlyList<Certificate> certificates)
    {
        return new VolunteerAccount(
            userId,
            startVolunteeringDate,
            requisites,
            certificates);
    }
}
