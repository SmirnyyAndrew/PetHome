using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Read.VolunteerManegment.GetVolunteerById;
public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;
