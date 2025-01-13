using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Database.Interfaces;
public interface IVolunteerRequestReadDbContext
{
    public IQueryable<VolunteerRequest> VolunteerRequests { get; }
}
