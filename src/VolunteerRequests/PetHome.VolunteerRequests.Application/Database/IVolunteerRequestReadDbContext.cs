using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Database;
public interface IVolunteerRequestReadDbContext
{
    public IQueryable<VolunteerRequest> VolunteerRequests{ get; }
}
