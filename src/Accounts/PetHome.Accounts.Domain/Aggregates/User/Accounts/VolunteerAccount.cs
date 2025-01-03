using PetHome.Core.Interfaces;
using PetHome.Core.ValueObjects;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;

namespace PetHome.Accounts.Domain.Aggregates.User.Accounts;
public class VolunteerAccount : SoftDeletableEntity
{
    public UserId? UserId { get; private set; }
    public Date? StartVolunteeringDate { get; private set; }
    public IReadOnlyList<Requisites>? Requisites { get; private set; }
    public IReadOnlyList<Certificate>? Certificates { get; private set; }
    public IReadOnlyList<Pet>? Pets { get; private set; } 
    private VolunteerAccount() { }
    private VolunteerAccount(
        UserId userId,
        Date startVolunteeringDate)
    {
        UserId = userId;
        StartVolunteeringDate = startVolunteeringDate;
    }

    public static VolunteerAccount Create(
        UserId? userId,
            Date startVolunteeringDate,
            ValueObjectList<Requisites> requisites,
            ValueObjectList<Certificate> certificates)
    {
        return new VolunteerAccount(userId, startVolunteeringDate);
    } 
}
