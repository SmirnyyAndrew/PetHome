using PetHome.VolunteerRequests.Application.Database.Dto;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Database.Interfaces;
public interface IVolunteerRequestReadDbContext
{
    public IQueryable<VolunteerRequestDto> VolunteerRequests { get; }
}
